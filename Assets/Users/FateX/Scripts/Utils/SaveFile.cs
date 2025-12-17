using UnityEngine;

namespace Users.FateX.Scripts.Utils
{
	/// <summary>Methods that allow creation and retrival of serialized Objects</summary>
	public class SaveFile
	{
		//WebGL: Add this config item to your index.html https://docs.unity3d.com/Manual/web-templates-build-configuration.html
		//		 Or do this https://docs.unity3d.com/Manual/web-interacting-code-example.html
		//						--> [System.Runtime.InteropServices.DllImport("__Internal")] private static extern void JS_FileSystem_Sync();
		//						--> call JS_FileSystem_Sync(); somewhere (i haven't tried)


	
		public static string GetSavePath( string foldername, string filename )
		{
#if UNITY_WEBGL
			var	path = System.IO.Path.Combine("idbfs", foldername);  //	Path: "/idbfs/<foldername>"
#else         
		var	path = System.IO.Path.Combine(Application.persistentDataPath, foldername); 
#endif
					
			if (!System.IO.Directory.Exists(path)) {
				//Console.WriteLine("Creating save directory: " + path);
				System.IO.Directory.CreateDirectory(path);
			}
			var result = System.IO.Path.Combine(path, filename);	//	File Path: "/idbfs/<foldername>/<filename>"
			return result;
		}
	
		// EXAMPLE:
		// Creating a property like this in your gameState script makes usage of below Load/Save easier
		private static string PathExample => SaveFile.GetSavePath("MyGame","MyGameState.save");
		//	Save<MyStateClass>( myStateObjectVariable, PathExample);
		//	Load<MyStateClass>( PathExample);


		/// <summary> Will store a serializable object to a file </summary>
		public static void Save<T>( T gameState, string path ) where T: class
		{	//Console.WriteLine($"Saving to Save File:\n{Path}");	//\n\n{json}");
			string json = UnityEngine.JsonUtility.ToJson(gameState, true);				
			System.IO.File.WriteAllText(path, json);
		}


		/// <summary> Will try to Load a previously serialized object from a file </summary>
		public static T Load<T>( string path ) where T: class, new()
		{
			T loadedGameState;
		
			if (!System.IO.File.Exists(path)) {
				//Console.WriteLine($"Creating new Save File\n{path}");				
				loadedGameState = new T();
			}else{
				//Console.WriteLine($"Loading existing Save File from:\n{path}");	//\n\n{json}");
				string json = System.IO.File.ReadAllText(path);				
				loadedGameState = UnityEngine.JsonUtility.FromJson<T>(json);
			}
			return loadedGameState;
		}

		/// <summary>Delete a file, as always, use with caution</summary>
		public static void DeleteSaveFile(string path)
		{
			if (System.IO.File.Exists(path)) {
				System.IO.File.Delete(path);
			}
		}
	}
}