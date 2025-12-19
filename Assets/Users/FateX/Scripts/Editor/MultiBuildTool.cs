using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using System.Linq;
using UnityEditor.Build.Profile;

public class MultiBuildTool : EditorWindow
{
    // Настройки сборки
    private string buildName = "MyGame";
    private string buildVersion = "1.0.0";
    private bool autoIncrementVersion = true;
    private bool createArchive = true;
    
    // Платформы
    private Dictionary<BuildTarget, PlatformBuildSettings> platformSettings = new Dictionary<BuildTarget, PlatformBuildSettings>();
    private Vector2 scrollPosition;
    
    // Ключи для сохранения настроек
    private const string PREF_BUILD_NAME = "MultiBuild_BuildName";
    private const string PREF_BUILD_VERSION = "MultiBuild_BuildVersion";
    private const string PREF_AUTO_INCREMENT = "MultiBuild_AutoIncrement";
    private const string PREF_CREATE_ARCHIVE = "MultiBuild_CreateArchive";
    
    [Serializable]
    private class PlatformBuildSettings
    {
        public bool enabled = false;
        public string buildProfile = "Default";
        public string customPath = "";
        
#if UNITY_6000_0_OR_NEWER
        public BuildProfile buildProfileAsset = null;
#endif
        
        public PlatformBuildSettings(bool enabled = false)
        {
            this.enabled = enabled;
        }
    }
    
    // Кэш для Build Profiles
    private List<string> availableBuildProfiles = new List<string>();
#if UNITY_6000_0_OR_NEWER
    private Dictionary<string, BuildProfile> buildProfileAssets = new Dictionary<string, BuildProfile>();
#endif
    
    [MenuItem("Tools/Multi-Platform Build Tool")]
    public static void ShowWindow()
    {
        MultiBuildTool window = GetWindow<MultiBuildTool>("Build Tool");
        window.minSize = new Vector2(500, 600);
    }
    
    private void OnEnable()
    {
        LoadSettingsFromPlayerSettings();
        LoadSettings();
        InitializePlatforms();
        LoadBuildProfiles();
    }
    
    private void LoadSettingsFromPlayerSettings()
    {
        // Загружаем название и версию из Player Settings
        buildName = PlayerSettings.productName;
        buildVersion = PlayerSettings.bundleVersion;
    }
    
    private void LoadBuildProfiles()
    {
        availableBuildProfiles.Clear();
        buildProfilePaths.Clear();
        
        // Ищем все .asset файлы в папке Build Profiles
        string buildProfilesPath = "Assets/Settings/Build Profiles";
        
        if (Directory.Exists(buildProfilesPath))
        {
            string[] files = Directory.GetFiles(buildProfilesPath, "*.asset", SearchOption.AllDirectories);
            
            foreach (string file in files)
            {
                string relativePath = file.Replace("\\", "/");
                string fileName = Path.GetFileNameWithoutExtension(file);
                
                availableBuildProfiles.Add(fileName);
                buildProfilePaths[fileName] = relativePath;
            }
        }
        
        // Также ищем во всем проекте на случай если путь другой
        if (availableBuildProfiles.Count == 0)
        {
            string[] allAssets = AssetDatabase.FindAssets("t:Object", new[] { "Assets" });
            
            foreach (string guid in allAssets)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                if (path.Contains("Build Profile") && path.EndsWith(".asset"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(path);
                    
                    if (!availableBuildProfiles.Contains(fileName))
                    {
                        availableBuildProfiles.Add(fileName);
                        buildProfilePaths[fileName] = path;
                    }
                }
            }
        }
        
        if (availableBuildProfiles.Count == 0)
        {
            availableBuildProfiles.Add("Default");
        }
        
        Debug.Log($"Found {availableBuildProfiles.Count} build profiles");
        foreach (var profile in availableBuildProfiles)
        {
            if (buildProfilePaths.ContainsKey(profile))
            {
                Debug.Log($"  - {profile}: {buildProfilePaths[profile]}");
            }
        }
    }
    
    private void InitializePlatforms()
    {
        if (platformSettings.Count == 0)
        {
            platformSettings[BuildTarget.StandaloneWindows64] = new PlatformBuildSettings();
            platformSettings[BuildTarget.StandaloneOSX] = new PlatformBuildSettings();
            platformSettings[BuildTarget.StandaloneLinux64] = new PlatformBuildSettings();
            platformSettings[BuildTarget.Android] = new PlatformBuildSettings();
            platformSettings[BuildTarget.iOS] = new PlatformBuildSettings();
            platformSettings[BuildTarget.WebGL] = new PlatformBuildSettings();
            
            LoadPlatformSettings();
        }
    }
    
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Build Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Основные настройки
        DrawMainSettings();
        
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Platform Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Настройки платформ
        DrawPlatformSettings();
        
        GUILayout.Space(20);
        
        // Кнопки действий
        DrawActionButtons();
        
        EditorGUILayout.EndScrollView();
    }
    
    private void DrawMainSettings()
    {
        EditorGUILayout.BeginVertical("box");
        
        EditorGUI.BeginChangeCheck();
        
        buildName = EditorGUILayout.TextField("Build Name", buildName);
        buildVersion = EditorGUILayout.TextField("Version", buildVersion);
        
        if (EditorGUI.EndChangeCheck())
        {
            // Синхронизируем с Player Settings
            PlayerSettings.productName = buildName;
            PlayerSettings.bundleVersion = buildVersion;
            SaveSettings();
        }
        
        EditorGUILayout.Space();
        
        // Кнопка синхронизации
        if (GUILayout.Button("↻ Sync from Player Settings", GUILayout.Height(25)))
        {
            LoadSettingsFromPlayerSettings();
        }
        
        EditorGUILayout.Space();
        
        autoIncrementVersion = EditorGUILayout.Toggle("Auto Increment Version", autoIncrementVersion);
        createArchive = EditorGUILayout.Toggle("Create Archive (ZIP)", createArchive);
        
        EditorGUILayout.Space();
        
        // Информация о текущей версии
        EditorGUILayout.HelpBox($"Current Version: {buildVersion}\nNext Version: {GetNextVersion()}", MessageType.Info);
        
        EditorGUILayout.EndVertical();
    }
    
    private void DrawPlatformSettings()
    {
        EditorGUILayout.BeginVertical("box");
        
        foreach (var platform in platformSettings)
        {
            DrawPlatformRow(platform.Key, platform.Value);
        }
        
        EditorGUILayout.EndVertical();
    }
    
    private void DrawPlatformRow(BuildTarget target, PlatformBuildSettings settings)
    {
        EditorGUILayout.BeginHorizontal("box");
        
        EditorGUI.BeginChangeCheck();
        
        // Галочка включения платформы
        settings.enabled = EditorGUILayout.Toggle(settings.enabled, GUILayout.Width(20));
        
        // Название платформы
        EditorGUILayout.LabelField(GetPlatformDisplayName(target), GUILayout.Width(150));
        
        // Build Profile
        EditorGUI.BeginDisabledGroup(!settings.enabled);
        
        EditorGUILayout.LabelField("Profile:", GUILayout.Width(50));
        
        // Dropdown с доступными профилями
        if (availableBuildProfiles.Count > 0)
        {
            int currentIndex = availableBuildProfiles.IndexOf(settings.buildProfile);
            if (currentIndex == -1) currentIndex = 0;
            
            int newIndex = EditorGUILayout.Popup(currentIndex, availableBuildProfiles.ToArray(), GUILayout.Width(120));
            
            if (newIndex >= 0 && newIndex < availableBuildProfiles.Count)
            {
                settings.buildProfile = availableBuildProfiles[newIndex];
                
                // Сохраняем путь к профилю
                if (buildProfilePaths.ContainsKey(settings.buildProfile))
                {
                    settings.buildProfilePath = buildProfilePaths[settings.buildProfile];
                }
            }
        }
        else
        {
            settings.buildProfile = EditorGUILayout.TextField(settings.buildProfile, GUILayout.Width(120));
        }
        
        // Кнопка обновления списка профилей
        if (GUILayout.Button("↻", GUILayout.Width(25)))
        {
            LoadBuildProfiles();
        }
        
        // Кастомный путь (опционально)
        if (GUILayout.Button("Path...", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Build Folder", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                settings.customPath = path;
            }
        }
        
        if (!string.IsNullOrEmpty(settings.customPath))
        {
            EditorGUILayout.LabelField("✓", GUILayout.Width(20));
        }
        
        EditorGUI.EndDisabledGroup();
        
        if (EditorGUI.EndChangeCheck())
        {
            SavePlatformSettings();
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawActionButtons()
    {
        EditorGUILayout.BeginVertical("box");
        
        // Кнопка сборки
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("BUILD SELECTED PLATFORMS", GUILayout.Height(40)))
        {
            BuildSelectedPlatforms();
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Select All"))
        {
            foreach (var platform in platformSettings)
            {
                platform.Value.enabled = true;
            }
            SavePlatformSettings();
        }
        
        if (GUILayout.Button("Deselect All"))
        {
            foreach (var platform in platformSettings)
            {
                platform.Value.enabled = false;
            }
            SavePlatformSettings();
        }
        
        if (GUILayout.Button("Open Build Folder"))
        {
            string path = Path.Combine(Application.dataPath, "..", "Builds");
            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Build folder doesn't exist yet.", "OK");
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }
    
    private void BuildSelectedPlatforms()
    {
        if (string.IsNullOrEmpty(buildName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a build name!", "OK");
            return;
        }
        
        bool anySelected = false;
        foreach (var platform in platformSettings)
        {
            if (platform.Value.enabled)
            {
                anySelected = true;
                break;
            }
        }
        
        if (!anySelected)
        {
            EditorUtility.DisplayDialog("Error", "Please select at least one platform!", "OK");
            return;
        }
        
        if (!EditorUtility.DisplayDialog("Confirm Build", 
            $"Build {GetSelectedPlatformsCount()} platform(s)?\n\nVersion: {buildVersion}", 
            "Build", "Cancel"))
        {
            return;
        }
        
        // Начинаем сборку
        DateTime startTime = DateTime.Now;
        int successCount = 0;
        int failCount = 0;
        
        foreach (var platform in platformSettings)
        {
            if (platform.Value.enabled)
            {
                EditorUtility.DisplayProgressBar("Building", 
                    $"Building for {GetPlatformDisplayName(platform.Key)}...", 
                    (float)successCount / GetSelectedPlatformsCount());
                
                if (BuildForPlatform(platform.Key, platform.Value))
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }
        }
        
        EditorUtility.ClearProgressBar();
        
        // Автоматическое увеличение версии
        if (autoIncrementVersion && successCount > 0)
        {
            buildVersion = GetNextVersion();
            PlayerSettings.bundleVersion = buildVersion; // Обновляем в Player Settings
            SaveSettings();
        }
        
        // Показываем результат
        TimeSpan duration = DateTime.Now - startTime;
        string message = $"Build completed!\n\n" +
                        $"Success: {successCount}\n" +
                        $"Failed: {failCount}\n" +
                        $"Time: {duration.Minutes}m {duration.Seconds}s\n\n" +
                        $"New version: {buildVersion}";
        
        EditorUtility.DisplayDialog("Build Complete", message, "OK");
    }
    
    private bool BuildForPlatform(BuildTarget target, PlatformBuildSettings settings)
    {
        try
        {
            string platformName = GetPlatformFolderName(target);
            string basePath = string.IsNullOrEmpty(settings.customPath) 
                ? Path.Combine(Application.dataPath, "..", "Builds", platformName)
                : Path.Combine(settings.customPath, platformName);
            
            string versionedPath = Path.Combine(basePath, $"v{buildVersion}");
            Directory.CreateDirectory(versionedPath);
            
            string buildPath = Path.Combine(versionedPath, GetBuildFileName(target));
            
            // Применяем настройки из Build Profile если он указан
            if (!string.IsNullOrEmpty(settings.buildProfilePath) && File.Exists(settings.buildProfilePath))
            {
                Debug.Log($"Using build profile: {settings.buildProfile} ({settings.buildProfilePath})");
                
                // Загружаем настройки профиля
                UnityEngine.Object profileAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(settings.buildProfilePath);
                
                if (profileAsset != null)
                {
                    // Можно читать настройки из профиля через SerializedObject
                    SerializedObject serializedProfile = new SerializedObject(profileAsset);
                    
                    // Здесь можно применить настройки из профиля к PlayerSettings
                    // Например: компрессия, оптимизация, define symbols и т.д.
                    ApplyProfileSettings(serializedProfile, target);
                }
            }
            
            // Стандартная сборка
            BuildPlayerOptions buildOptions = GetBuildPlayerOptions(target, buildPath, settings);
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded for {platformName}: {buildPath}");
                
                if (createArchive && ShouldArchivePlatform(target))
                {
                    CreateZipArchive(versionedPath, basePath, platformName);
                }
                
                return true;
            }
            else
            {
                Debug.LogError($"Build failed for {platformName}");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Build error for {target}: {e.Message}");
            return false;
        }
    }
    
    private void ApplyProfileSettings(SerializedObject profileObject, BuildTarget target)
    {
        // Здесь можно применять настройки из профиля
        // Например, читать свойства и применять их к PlayerSettings
        
        // Пример: чтение define symbols
        SerializedProperty definesProp = profileObject.FindProperty("scriptingDefines");
        if (definesProp != null && definesProp.isArray)
        {
            List<string> defines = new List<string>();
            for (int i = 0; i < definesProp.arraySize; i++)
            {
                defines.Add(definesProp.GetArrayElementAtIndex(i).stringValue);
            }
            
            if (defines.Count > 0)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildPipeline.GetBuildTargetGroup(target),
                    string.Join(";", defines)
                );
            }
        }
        
        // Можно добавить больше настроек по необходимости
    }
    
    private BuildPlayerOptions GetBuildPlayerOptions(BuildTarget target, string buildPath, PlatformBuildSettings settings)
    {
        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = GetScenePaths(),
            locationPathName = buildPath,
            target = target,
            options = BuildOptions.None
        };
        
        Debug.Log($"Building for {target} with profile: {settings.buildProfile}");
        
        return buildOptions;
    }
    
    private void CreateZipArchive(string sourceFolder, string baseFolder, string platformName)
    {
        try
        {
            string zipPath = Path.Combine(baseFolder, $"{buildName}_{platformName}_v{buildVersion}.zip");
            
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            
            ZipFile.CreateFromDirectory(sourceFolder, zipPath, System.IO.Compression.CompressionLevel.Optimal, false);
            Debug.Log($"Archive created: {zipPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create archive: {e.Message}");
        }
    }
    
    private bool ShouldArchivePlatform(BuildTarget target)
    {
        // Платформы с множеством файлов
        return target == BuildTarget.StandaloneWindows64 ||
               target == BuildTarget.StandaloneOSX ||
               target == BuildTarget.StandaloneLinux64 ||
               target == BuildTarget.WebGL;
    }
    
    private string GetBuildFileName(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                return $"{buildName}.exe";
            case BuildTarget.StandaloneOSX:
                return $"{buildName}.app";
            case BuildTarget.StandaloneLinux64:
                return $"{buildName}.x86_64";
            case BuildTarget.Android:
                return $"{buildName}.apk";
            case BuildTarget.iOS:
                return buildName;
            case BuildTarget.WebGL:
                return buildName;
            default:
                return buildName;
        }
    }
    
    private string GetPlatformDisplayName(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64: return "Windows 64-bit";
            case BuildTarget.StandaloneOSX: return "macOS";
            case BuildTarget.StandaloneLinux64: return "Linux 64-bit";
            case BuildTarget.Android: return "Android";
            case BuildTarget.iOS: return "iOS";
            case BuildTarget.WebGL: return "WebGL";
            default: return target.ToString();
        }
    }
    
    private string GetPlatformFolderName(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64: return "Windows";
            case BuildTarget.StandaloneOSX: return "macOS";
            case BuildTarget.StandaloneLinux64: return "Linux";
            case BuildTarget.Android: return "Android";
            case BuildTarget.iOS: return "iOS";
            case BuildTarget.WebGL: return "WebGL";
            default: return target.ToString();
        }
    }
    
    private string[] GetScenePaths()
    {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        return scenes.ToArray();
    }
    
    private int GetSelectedPlatformsCount()
    {
        int count = 0;
        foreach (var platform in platformSettings)
        {
            if (platform.Value.enabled) count++;
        }
        return count;
    }
    
    private string GetNextVersion()
    {
        string[] parts = buildVersion.Split('.');
        if (parts.Length == 3)
        {
            if (int.TryParse(parts[2], out int patch))
            {
                return $"{parts[0]}.{parts[1]}.{patch + 1}";
            }
        }
        return buildVersion;
    }
    
    private void SaveSettings()
    {
        // Сохраняем в Player Settings
        PlayerSettings.productName = buildName;
        PlayerSettings.bundleVersion = buildVersion;
        
        // Сохраняем дополнительные настройки в EditorPrefs
        EditorPrefs.SetString(PREF_BUILD_NAME, buildName);
        EditorPrefs.SetString(PREF_BUILD_VERSION, buildVersion);
        EditorPrefs.SetBool(PREF_AUTO_INCREMENT, autoIncrementVersion);
        EditorPrefs.SetBool(PREF_CREATE_ARCHIVE, createArchive);
    }
    
    private void LoadSettings()
    {
        // Загружаем дополнительные настройки
        autoIncrementVersion = EditorPrefs.GetBool(PREF_AUTO_INCREMENT, true);
        createArchive = EditorPrefs.GetBool(PREF_CREATE_ARCHIVE, true);
    }
    
    private void SavePlatformSettings()
    {
        foreach (var platform in platformSettings)
        {
            string key = $"MultiBuild_Platform_{platform.Key}";
            EditorPrefs.SetBool($"{key}_Enabled", platform.Value.enabled);
            EditorPrefs.SetString($"{key}_Profile", platform.Value.buildProfile);
            EditorPrefs.SetString($"{key}_Path", platform.Value.customPath);
        }
    }
    
    private void LoadPlatformSettings()
    {
        foreach (var platform in platformSettings.Keys)
        {
            string key = $"MultiBuild_Platform_{platform}";
            var settings = platformSettings[platform];
            settings.enabled = EditorPrefs.GetBool($"{key}_Enabled", false);
            settings.buildProfile = EditorPrefs.GetString($"{key}_Profile", "Default");
            settings.customPath = EditorPrefs.GetString($"{key}_Path", "");
        }
    }
}