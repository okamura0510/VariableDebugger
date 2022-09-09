using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace G2U4S
{
    public static class G2U4SUtil
    {
        public static readonly string[] BoolTrueValues = { "true", "1", "○" };

        public static Action OnInit;
        public static Action OnScriptCreated;
        public static Action OnAssetCreated;
        public static Action OnAssetUpdated;

        public static string GetRelativePath(string fullPath)
        {
            var unifiedFullPath = Path.GetFullPath(fullPath);
            var unifiedDataPath = Path.GetFullPath(Application.dataPath);
            return unifiedFullPath.Replace(unifiedDataPath, "Assets");
        }

        public static string SearchDirectory(string dirName, string dirPath)
        {
			var di = new DirectoryInfo(dirPath);
			if(di.Name == dirName) return di.FullName;

            foreach(var child in di.GetDirectories())
            {
				var childDirPath = SearchDirectory(dirName, child.FullName);
				if(childDirPath != null) return childDirPath;
            }
            return null;
        }

        public static List<string> GetFilesInDirectory(
            string dir, string searchPattern, params string[] ignorePrefixes)
        {
            return GetFilesInDirectory(new List<string>(), dir, searchPattern, ignorePrefixes);
        }
        
        public static List<string> GetFilesInDirectory(
            List<string> files, string dir, string searchPattern, params string[] ignorePrefixes)
		{
			var di = new DirectoryInfo(dir);
			foreach(var fi in di.GetFiles(searchPattern))
			{
				if(!Array.Exists(ignorePrefixes, ignorePrefix => fi.Name.StartsWith(ignorePrefix, StringComparison.Ordinal)))
				{
					files.Add(fi.FullName);
				}
			}

			foreach(var childDi in di.GetDirectories())
			{
				if(!Array.Exists(ignorePrefixes, ignorePrefix => childDi.Name.StartsWith(ignorePrefix, StringComparison.Ordinal)))
				{
					files = GetFilesInDirectory(files, childDi.FullName, searchPattern, ignorePrefixes);
				}
			}
			return files;
		}
        
        public static bool IsMultipleValue(Type type)
        {
            if( type == typeof(Vector2)    || 
                type == typeof(Vector3)    ||
                type == typeof(Vector4)    || 
                type == typeof(Rect)       ||
                type == typeof(Vector2Int) || 
                type == typeof(Vector3Int) || 
                type == typeof(RectInt)    || 
                type == typeof(Quaternion) || 
                type == typeof(Color)      || 
                type == typeof(Color32))
            {
                return true;
            }
            return false;
        }

        public static T Parse<T>(string str, string assetDir = "")
        {
            return (T)Parse(typeof(T), str, assetDir);
        }

        public static object Parse(Type type, string str, string assetDir = "")
        {
            if(type.IsArray)
            {
                return Parse(type.GetElementType(), str, true, assetDir);
            }
            else
            {
                return Parse(type, str, false, assetDir);
            }
        }

        public static object Parse(Type type, string str, bool isArray, string assetDir = "")
        {
            if(isArray)
            {
                return ParseArray(type, str, assetDir);
            }
            else if(type == typeof(sbyte)) 
            {
                sbyte.TryParse(str, out var value); return value;
            }
            else if(type == typeof(byte)) 
            {
                byte.TryParse(str, out var value); return value;
            }
            else if(type == typeof(short)) 
            {
                short.TryParse(str, out var value); return value;
            }
            else if(type == typeof(ushort)) 
            {
                ushort.TryParse(str, out var value); return value;
            }
            else if(type == typeof(int)) 
            {
                int.TryParse(str, out var value); return value; 
            }
            else if(type == typeof(uint)) 
            {
                uint.TryParse(str, out var value); return value;
            }
            else if(type == typeof(long)) 
            {
                long.TryParse(str, out var value); return value;
            }
            else if(type == typeof(ulong)) 
            {
                ulong.TryParse(str, out var value); return value;
            }
            else if(type == typeof(char)) 
            {
                char.TryParse(str, out var value); return value;
            }
            else if(type == typeof(float)) 
            {
                float.TryParse(str, out var value); return value;
            }
            else if(type == typeof(double)) 
            { 
                double.TryParse(str, out var value); return value;
            }
            else if(type == typeof(bool))
            {
                return (str != null && BoolTrueValues.Contains(str.ToLower())); 
            }
            else if(type == typeof(string)) 
            {
                return str; 
            }
            else if(type.IsEnum) 
            {
                return ParseEnum(type, str); 
            }
            else if(type.IsSubclassOf(typeof(ScriptableObject)))
            {
                return ParseScriptableObject(type, str, assetDir);
            }
            else if(type == typeof(SerializableDateTime))
            {
                return new SerializableDateTime(DateTime.Parse(str));
            }
            else if(type == typeof(Vector2))
            {
                return ParseVector2(str);
            }
            else if(type == typeof(Vector3))
            {
                return ParseVector3(str);
            }
            else if(type == typeof(Vector4))
            {
                return ParseVector4(str);
            }
            else if(type == typeof(Rect))
            {
                return ParseRect(str);
            }
            else if(type == typeof(Vector2Int))
            {
                return ParseVector2Int(str);
            }
            else if(type == typeof(Vector3Int))
            {
                return ParseVector3Int(str);
            }
            else if(type == typeof(RectInt))
            {
                return ParseRectInt(str);
            }
            else if(type == typeof(Quaternion))
            {
                return ParseQuaternion(str);
            }
            else if(type == typeof(Color))
            {
                return ParseColor(str);
            }
            else if(type == typeof(Color32))
            {
                return ParseColor32(str);
            }
            return null;
        }

        public static string ParseString(object value)
        {
			if(value == null) return "";

            var type = value.GetType();
			if(type.IsArray)
			{
				var array = (Array)value;
				var str   = "";
                foreach(var elementValue in array)
				{
                    var isMultiple = IsMultipleValue(elementValue.GetType());

                    if(str != "")  str += ",";
                    if(isMultiple) str += "(";
                    str += ParseString(elementValue);
                    if(isMultiple) str += ")";

				}
                return str;
			}
            else if(type == typeof(Vector2))
            {
                var v = (Vector2)value;
                return $"{v.x},{v.y}";
            }
            else if(type == typeof(Vector3))
            {
                var v = (Vector3)value;
                return $"{v.x},{v.y},{v.z}";
            }
            else if(type == typeof(Vector4))
            {
                var v = (Vector4)value;
                return $"{v.x},{v.y},{v.z},{v.w}";
            }
            else if(type == typeof(Rect))
            {
                var r = (Rect)value;
                return $"{r.x},{r.y},{r.width},{r.height}";
            }
            else if(type == typeof(Vector2Int))
            {
                var v = (Vector2Int)value;
                return $"{v.x},{v.y}";
            }
            else if(type == typeof(Vector3Int))
            {
                var v = (Vector3Int)value;
                return $"{v.x},{v.y},{v.z}";
            }
            else if(type == typeof(RectInt))
            {
                var r = (RectInt)value;
                return $"{r.x},{r.y},{r.width},{r.height}";
            }
            else if(type == typeof(Quaternion))
            {
                var q = (Quaternion)value;
                return $"{q.x},{q.y},{q.z},{q.w}";
            }
            else if(type == typeof(Color))
            {
                var c = (Color)value;
                return $"{c.r},{c.g},{c.b},{c.a}";
            }
            else if(type == typeof(Color32))
            {
                var c = (Color32)value;
                return $"{c.r},{c.g},{c.b},{c.a}";
            }
			return value.ToString();
        }

        public static Type ParseType(string str, Type[] types = null)
        {
            // 組み込み型のTypeから取得出来る文字列がショート形式でないので個別判定してる
            if(str == "sbyte")       return typeof(sbyte);
            else if(str == "byte")   return typeof(byte);
            else if(str == "short")  return typeof(short);
            else if(str == "ushort") return typeof(ushort);
            else if(str == "int")    return typeof(int);
            else if(str == "uint")   return typeof(uint);
            else if(str == "long")   return typeof(long);
            else if(str == "ulong")  return typeof(ulong);
            else if(str == "char")   return typeof(char);
            else if(str == "float")  return typeof(float);
            else if(str == "double") return typeof(double);
            else if(str == "bool")   return typeof(bool);
            else if(str == "string") return typeof(string);
            else                     return types.FirstOrDefault(t => t.Name == str);
        }

        public static Array ParseArray<T>(string str, string assetDir = "")
        {
            return ParseArray(typeof(T), str, assetDir);
        }

        public static Array ParseArray(Type type, string str, string assetDir = "")
        {
            var strs = new List<string>();
            if(!string.IsNullOrEmpty(str))
            {
                if(IsMultipleValue(type))
                {
                    if(!str.Contains('(') && !str.Contains(')'))
                    {
                        strs.Add(str);
                    }
                    else
                    {
                        foreach(var values in str.Split('(').Skip(1))
                        {
                            var s = values.Split(')').First();
                            strs.Add(s);
                        }
                    }
                }
                else
                {
                    foreach(var s in str.Split(','))
                    {
                        strs.Add(s);
                    }
                }
            }

            var array = Array.CreateInstance(type, strs.Count);
            for(var i = 0; i < strs.Count; i++)
            {
                var value = Parse(type, strs[i].Trim(), assetDir);
                if(type.IsClass || value != null)
                {
                    array.SetValue(value, i);
                }
            }
            return array;
        }
        
        public static T ParseEnum<T>(string str)
        {
            return (T)ParseEnum(typeof(T), str);
        }

        public static object ParseEnum(Type type, string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return Enum.GetValues(type).GetValue(0);
            }
            else
            {
                return Enum.Parse(type, str);
            }
        }

        public static T ParseScriptableObject<T>(string assetDir) where T : ScriptableObject
        {
            return ParseScriptableObject<T>(null, assetDir);
        }

        public static T ParseScriptableObject<T>(string key, string assetDir) where T : ScriptableObject
        {
            return (T)ParseScriptableObject(typeof(T), key, assetDir);
        }

        public static UnityEngine.Object ParseScriptableObject(Type type, string key, string assetDir)
        {
#if UNITY_EDITOR
            var suffix = string.IsNullOrEmpty(key) ? "" : $"_{key}";
            var path   = $"{assetDir}/{type.Name}{suffix}.asset";
            return UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
#else
            return null;
#endif
        }

        public static Vector2 ParseVector2(string str)
        {
            var x = 0f;
            var y = 0f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out y);
            }
            return new Vector2(x, y);
        }

        public static Vector3 ParseVector3(string str)
        {
            var x = 0f;
            var y = 0f;
            var z = 0f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) float.TryParse(strs[2].Trim(), out z);
            }
            return new Vector3(x, y, z);
        }

        public static Vector4 ParseVector4(string str)
        {
            var x = 0f;
            var y = 0f;
            var z = 0f;
            var w = 0f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) float.TryParse(strs[2].Trim(), out z);
                if(strs.Length >= 4) float.TryParse(strs[3].Trim(), out w);
            }
            return new Vector4(x, y, z, w);
        }

        public static Rect ParseRect(string str)
        {
            var x = 0f;
            var y = 0f;
            var w = 0f;
            var h = 0f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) float.TryParse(strs[2].Trim(), out w);
                if(strs.Length >= 4) float.TryParse(strs[3].Trim(), out h);
            }
            return new Rect(x, y, w, h);
        }

        public static Vector2Int ParseVector2Int(string str)
        {
            var x = 0;
            var y = 0;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) int.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) int.TryParse(strs[1].Trim(), out y);
            }
            return new Vector2Int(x, y);
        }

        public static Vector3Int ParseVector3Int(string str)
        {
            var x = 0;
            var y = 0;
            var z = 0;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) int.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) int.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) int.TryParse(strs[2].Trim(), out z);
            }
            return new Vector3Int(x, y, z);
        }

        public static RectInt ParseRectInt(string str)
        {
            var x = 0;
            var y = 0;
            var w = 0;
            var h = 0;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) int.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) int.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) int.TryParse(strs[2].Trim(), out w);
                if(strs.Length >= 4) int.TryParse(strs[3].Trim(), out h);
            }
            return new RectInt(x, y, w, h);
        }

        public static Quaternion ParseQuaternion(string str)
        {
            var x = 0f;
            var y = 0f;
            var z = 0f;
            var w = 0f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out x);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out y);
                if(strs.Length >= 3) float.TryParse(strs[2].Trim(), out z);
                if(strs.Length >= 4) float.TryParse(strs[3].Trim(), out w);
            }
            return new Quaternion(x, y, z, w);
        }

        public static Color ParseColor(string str)
        {
            var r = 0f;
            var g = 0f;
            var b = 0f;
            var a = 1f;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) float.TryParse(strs[0].Trim(), out r);
                if(strs.Length >= 2) float.TryParse(strs[1].Trim(), out g);
                if(strs.Length >= 3) float.TryParse(strs[2].Trim(), out b);
                if(strs.Length >= 4) float.TryParse(strs[3].Trim(), out a);
            }
            return new Color(r, g, b, a);
        }

        public static Color32 ParseColor32(string str)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            byte a = 255;
            if(!string.IsNullOrEmpty(str))
            {
                var strs = str.Replace("(", "").Replace(")", "").Split(',');
                if(strs.Length >= 1) byte.TryParse(strs[0].Trim(), out r);
                if(strs.Length >= 2) byte.TryParse(strs[1].Trim(), out g);
                if(strs.Length >= 3) byte.TryParse(strs[2].Trim(), out b);
                if(strs.Length >= 4) byte.TryParse(strs[3].Trim(), out a);
            }
            return new Color32(r, g, b, a);
        }
    }
}