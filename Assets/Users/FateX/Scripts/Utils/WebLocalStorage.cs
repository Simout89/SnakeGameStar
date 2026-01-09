using System;
using System.Runtime.InteropServices;

namespace Users.FateX.Scripts.Utils
{
    public static class WebLocalStorage
    {
        // ❗ ЖЁСТКО ФИКСИРОВАННЫЙ ПРЕФИКС ИГРЫ
        // МЕНЯТЬ НЕЛЬЗЯ ПОСЛЕ РЕЛИЗА
        private const string Prefix = "jormungand";

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void LS_Set(string key, string value);
        [DllImport("__Internal")] private static extern IntPtr LS_Get(string key);
        [DllImport("__Internal")] private static extern void LS_Delete(string key);
#endif

        private static string WithPrefix(string key)
        {
            return Prefix + key;
        }

        public static void Set(string key, string value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LS_Set(WithPrefix(key), value);
#endif
        }

        public static string Get(string key)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IntPtr ptr = LS_Get(WithPrefix(key));
            if (ptr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStringUTF8(ptr);
#else
            return null;
#endif
        }

        public static void Delete(string key)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LS_Delete(WithPrefix(key));
#endif
        }
    }
}