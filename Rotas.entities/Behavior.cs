using System.Collections.Generic;
using System.Linq;


namespace System
{
    public static class ConversionExtensionMethods
    {
        /// <summary>
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="defaultValue">Se não for possível efetuar a conversão retorna este valor.</param>
        /// <returns></returns>
        public static Guid ToGuid(this object objectID, Guid defaultValue)
        {
            try { return new Guid(Convert.ToString(objectID)); }
            catch { return defaultValue; }
        }
        public static Guid? ToGuidNullable(this object objectID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(objectID)))
                    return null;

                return new Guid(objectID.ToString());
            }
            catch { return null; }
        }
        /// <summary>
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object objectID)
        {
            return objectID.ToGuid(Guid.Empty);
        }
        /// <summary>
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("O tipo passado deve ser um enumerador (System.Enum).");

            var list = new Dictionary<string, object>();
            foreach (var value in System.Enum.GetValues(enumType))
                list.Add(enumType.GetEnumName(value).ReplaceFromEnum(), value);
            return list;
        }
        /// <summary>
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="extraKey">Chave para item extra.</param>
        /// <param name="extraValue">Valor para item extra.</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this Type enumType, string extraKey, object extraValue)
        {
            var list = new Dictionary<string, object>();
            list.Add(extraKey, extraValue);
            foreach (var item in enumType.ToDictionary())
                list.Add(item.Key, item.Value);
            return list;
        }
        /// <summary>
        /// </summary>
        /// <param name="param">Parametro a ser verificado</param>
        /// <returns>Parâmetro ou coringa</returns>
        public static string ToSqlField(this string value)
        {
            return string.IsNullOrEmpty(value) ? "%" : value;
        }
        public static string ToSqlWildcard(this string value)
        {
            return string.IsNullOrEmpty(value) ? "%" : value.Replace('*', '%').Replace('+', '%').Replace('?', '_');
        }
        /// <summary>
        /// </summary>
        /// <param name="searchKey">Chave de busca</param>
        /// <returns>Nova chave de busca compatível com SQL.</returns>
        public static string ToSqlSearchKey(this string searchKey)
        {
            string newSearchKey = string.Empty;

            if (string.IsNullOrEmpty(searchKey))
            {
                newSearchKey = "%";
                return newSearchKey;
            }

            searchKey = searchKey.ToLower();
            searchKey = searchKey.Replace("*", "%");
            searchKey = searchKey.Replace("+", "%");
            searchKey = searchKey.Replace("\"", "'");
            string[] buffer = new string[] { "?", "á", "é", "í", "ó", "ú", "â", "ê", "ç", "ã", "õ", "ü" };
            foreach (string key in buffer)
                searchKey = searchKey.Replace(key, "_");

            ///Preserva espaços em branco entre apóstrofes. Do contrário substitui por coringa (%). 
            if (searchKey.Contains("'"))
            {
                string[] split = searchKey.Split(new char[] { '\'' });
                for (int i = 0; i < split.Length; i++)
                    if (i % 2 == 0)
                        newSearchKey += split[i].Replace(" ", "%");
                    else
                        newSearchKey += split[i];
            }
            else
                newSearchKey = searchKey.Replace(" ", "%");

            newSearchKey = "%" + newSearchKey + "%";
            //newSearchKey = "%" + searchKey + "%";

            return newSearchKey;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string[] Split(this string self)
        {
            self = self.Replace(',', ';').Replace('|', ';');
            if (!self.EndsWith(";"))
                self += ";";

            var list = self.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToDefault(this string self, string defaultvalue)
        {
            return string.IsNullOrWhiteSpace(self) ? defaultvalue : self;
        }
        /// <summary>
        /// Troca "__" por ", " e "_" por " ".
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ReplaceFromEnum(this string self)
        {
            return self.Replace("__", ", ").Replace("_", " ");
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string self, DateTime defaultvalue)
        {
            if (string.IsNullOrWhiteSpace(self))
                return defaultvalue;

            var val = new DateTime(1900, 1, 1);

            if (!DateTime.TryParse(self, out val))
                return defaultvalue;

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNullable(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return null;
            var val = new DateTime(1900, 1, 1);
            DateTime.TryParse(self, out val);
            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int ToInt32(this string self, int defaultvalue)
        {
            if (string.IsNullOrWhiteSpace(self))
                return defaultvalue;

            var val = 0d;
            if (!double.TryParse(self, out val))
                return defaultvalue;

            return (int)val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int? ToInt32Nullable(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return null;

            var val = 0d;
            double.TryParse(self, out val);

            return (int)val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return false;

            return ((new string[] { "1", "true", "active", "checked", "selected", "sim", "verdadeiro", ".t." }).Contains(self.ToLower().Trim()));
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float ToSingle(this string self, int defaultvalue)
        {
            if (string.IsNullOrWhiteSpace(self))
                return defaultvalue;

            var val = 0f;
            if (!float.TryParse(self, out val))
                return defaultvalue;

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float? ToSingleNullable(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return null;

            var val = 0f;
            float.TryParse(self, out val);

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string self, decimal defaultvalue)
        {
            if (string.IsNullOrWhiteSpace(self))
                return defaultvalue;

            var val = 0m;
            if (!decimal.TryParse(self, out val))
                return defaultvalue;

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
                return null;

            var val = 0m;
            decimal.TryParse(self, out val);

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string self) where T : struct
        {
            T val;
            if (!Enum.TryParse<T>(self, out val))
                throw new InvalidCastException(string.Format("Não é possível converter '{0}' em um tipo '{1}' válido.", self, typeof(T).FullName));

            return val;
        }
        public static T ToEnum<T>(this object self) where T : struct
        {
            var s = Convert.ToString(self ?? string.Empty);
            return s.ToEnum<T>();
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string self, T defaultvalue) where T : struct
        {
            T val;
            if (!Enum.TryParse<T>(self, out val))
                return defaultvalue;

            return val;
        }
        public static T ToEnum<T>(this object self, T defaultvalue) where T : struct
        {
            var s = Convert.ToString(self ?? string.Empty);
            return s.ToEnum<T>(defaultvalue);
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Nullable<T> ToEnumNullable<T>(this string self) where T : struct
        {
            if (string.IsNullOrWhiteSpace(self))
                return null;

            T val;

            if (!Enum.TryParse<T>(self, out val))
                return null;

            return val;
        }
        public static Nullable<T> ToEnumNullable<T>(this object self) where T : struct
        {
            var s = Convert.ToString(self ?? string.Empty);
            return s.ToEnumNullable<T>();
        }
        /// <summary>
        /// Ex.: foreach(var key in searchkey.SplitSqlKeys) q = q.Where(a => a.name.Contains(key);
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string[] SplitSqlKeys(this string self)
        {
            self = self.Trim().Replace('"', '\'');
            if (self.StartsWith("'") && self.EndsWith("'"))
                return new string[] { self };

            self = self.Replace(' ', '%').Replace('*', '%').Replace('?', '_').Replace("%%", "%");

            return self.Split('%');
        }
        /// <summary>
        /// </summary>
        /// <param name="size">Tamanho total desejado para a string</param>
        /// <param name="finalconcat">Concatenação para o final da descrição. Ex: ...</param>
        /// <returns>String tratada com quantidade de caracteres definido.</returns>
        public static string ToShortString(this string description, int size, string finalconcat)
        {
            try
            {
                if (description.Length > size)
                    return (description.PadRight(size).Substring(0, size) + finalconcat);
                else
                    return description;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object self)
        {
            return self.ToDateTime(DateTime.Today);
        }
        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object self, DateTime defaultvalue)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return defaultvalue;

            var val = new DateTime(1900, 1, 1);

            if (!DateTime.TryParse(s, out val))
                return defaultvalue;

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNullable(this object self)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return null;
            var val = new DateTime(1900, 1, 1);
            DateTime.TryParse(s, out val);
            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this object self, int defaultvalue)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return defaultvalue;

            var val = 0d;
            if (!double.TryParse(s, out val))
                return defaultvalue;

            return (int)val;
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int? ToInt32Nullable(this object self)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return null;

            var val = 0d;
            if (!double.TryParse(s, out val))
                return null;

            return (int)val;
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object self)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return false;

            return ((new string[] { "1", "true", "active", "checked", "selected", "sim", "verdadeiro", ".t." }).Contains(s.ToLower().Trim()));
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool? ToBooleanNullable(this object self)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return null;

            return ((new string[] { "1", "true", "active", "checked", "selected", "sim", "verdadeiro", ".t." }).Contains(s.ToLower().Trim()));
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object self, decimal defaultvalue)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return defaultvalue;

            var val = 0m;
            if (!decimal.TryParse(s, out val))
                return defaultvalue;

            return val;
        }
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this object self)
        {
            var s = Convert.ToString(self);

            if (string.IsNullOrWhiteSpace(s))
                return null;

            var val = 0m;
            decimal.TryParse(s, out val);

            return val;
        }
     
        public static string ToSqlString(this DateTime self)
        {
            var s = self.ToString("yyyyMMdd HH:mm:ss");
            return s;
        }
        public static string ToSqlString(this DateTime? self)
        {
            var s = (self.HasValue ? self.Value.ToString("yyyyMMdd HH:mm:ss") : null);
            return s;
        }
        public static string ToSqlString(this decimal self)
        {
            var s = self.ToString(Globalization.CultureInfo.InvariantCulture);
            return s;
        }
        public static string ToSqlString(this decimal? self)
        {
            var s = (self.HasValue ? self.Value.ToString(Globalization.CultureInfo.InvariantCulture) : null);
            return s;
        }
        public static string ToSqlString(this double self)
        {
            var s = self.ToString(Globalization.CultureInfo.InvariantCulture);
            return s;
        }
        public static string ToSqlString(this double? self)
        {
            var s = (self.HasValue ? self.Value.ToString(Globalization.CultureInfo.InvariantCulture) : null);
            return s;
        }

    }

}
