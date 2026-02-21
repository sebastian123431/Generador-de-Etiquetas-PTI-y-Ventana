using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Script.Serialization;

namespace WindowsFormsApplication1.Data
{
    public static class JsonMini
    {
        private static readonly JavaScriptSerializer _js = new JavaScriptSerializer();

        public static object Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return _js.DeserializeObject(json);
        }

        public static Dictionary<string, object> AsDict(object obj) => obj as Dictionary<string, object>;
        public static object[] AsArray(object obj) => obj as object[];

        public static string GetString(Dictionary<string, object> dict, params string[] keys)
        {
            if (dict == null || keys == null) return string.Empty;
            foreach (var k in keys)
            {
                if (string.IsNullOrWhiteSpace(k)) continue;
                if (dict.TryGetValue(k, out var v) && v != null) return v.ToString();
            }
            return string.Empty;
        }

        public static bool GetBool(Dictionary<string, object> dict, bool defaultValue, params string[] keys)
        {
            if (dict == null || keys == null) return defaultValue;
            foreach (var k in keys)
            {
                if (string.IsNullOrWhiteSpace(k)) continue;
                if (!dict.TryGetValue(k, out var v) || v == null) continue;

                if (v is bool b) return b;
                var s = v.ToString();
                if (bool.TryParse(s, out var parsed)) return parsed;
                if (s == "1") return true;
                if (s == "0") return false;
            }
            return defaultValue;
        }

        public static int GetInt(Dictionary<string, object> dict, int defaultValue, params string[] keys)
        {
            if (dict == null || keys == null) return defaultValue;
            foreach (var k in keys)
            {
                if (string.IsNullOrWhiteSpace(k)) continue;
                if (!dict.TryGetValue(k, out var v) || v == null) continue;

                if (v is int i) return i;
                if (v is long l) return (int)l;
                if (v is double d) return (int)d;

                if (int.TryParse(v.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                    return parsed;
                if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedD))
                    return (int)parsedD;
            }
            return defaultValue;
        }
    }
}
