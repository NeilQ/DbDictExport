using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.js
{
    public abstract class AbstractCodeFactory
    {
        public string ModuleName { get; set; }

        public string EntityName { get; set; }

        public Table Table { get; set; }

        public abstract StringBuilder GenerateCodes();

        protected string MapCSharpType(string dbtype)
        {
            if (dbtype.ToLower().Contains("char")) return "string";
            if (dbtype.ToLower().Contains("bit")) return "bool";
            if (dbtype.ToLower().Contains("bigint")) return "long";
            if (dbtype.ToLower().Contains("int")) return "int";
            if (dbtype.ToLower().Contains("decimal")) return "decimal";
            if (dbtype.ToLower().Contains("float")) return "float";
            if (dbtype.ToLower().Contains("datetime")) return "DateTime";
            if (dbtype.ToLower().Contains("date")) return "DateTime";
            return "string";
        }

        protected string GetIndentStr(int indent)
        {
            return new string(' ', indent * 4);
        }

        /// <summary>
        /// Convert value to Pascal case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPascalCase(string value)
        {
            // If there are 0 or 1 characters, just return the string.
            if (value == null) return null;
            if (value.Length < 2) return value.ToUpper();

            // Split the string into words.
            var words = value.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = "";
            foreach (var word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }

        /// <summary>
        /// Convert to camel case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var flag = NoLowerCase(value);
            var builder = new StringBuilder();
            if (!IsSeparatorChar(value[0]))
            {
                builder.Append(char.ToLower(value[0]));
            }
            for (var i = 1; i < value.Length; i++)
            {
                if (IsSeparatorChar(value[i])) continue;
                if (IsSeparatorChar(value[i - 1]))
                {
                    builder.Append(char.ToUpper(value[i]));
                }
                else if (flag)
                {
                    builder.Append(char.ToLower(value[i]));
                }
                else
                {
                    builder.Append(value[i]);
                }
            }
            return builder.ToString();
        }

        private static bool IsSeparatorChar(char value)
        {
            return !char.IsLetterOrDigit(value);
        }

        private static bool NoLowerCase(string value)
        {
            return value.All(ch => !char.IsLower(ch));
        }
    }

}