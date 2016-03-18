using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace DbDictExport.WPF.Common
{
    public class AuthHistoryService
    {
        private static Dictionary<string, SqlServerAuth> _history;

        private const string FILENAME = "sqlserver_history.json";

        static AuthHistoryService()
        {
            _history = new Dictionary<string, SqlServerAuth>();
            LoadHistory();
        }

        /// <summary>
        /// load history to dictionary from temp file
        /// </summary>
        /// <returns></returns>
        private static void LoadHistory()
        {
            if (File.Exists(FILENAME))
            {
                var historyContent = File.ReadAllText(FILENAME);
                var auths = JsonConvert.DeserializeObject<List<SqlServerAuth>>(historyContent);
                if (auths != null && auths.Count > 0)
                {
                    foreach (var auth in auths)
                    {
                        if (_history.ContainsKey(auth.Server)) continue;
                        _history[auth.Server] = auth;
                    }
                }
            }
            else
            {
                AppendDefaultAuth();
                using (var sw = new StreamWriter(File.Open(FILENAME, FileMode.Create)))
                {
                    var content = JsonConvert.SerializeObject(_history.Values);
                    sw.Write(content);
                    sw.Flush();
                }
            }
        }

        private static void AppendDefaultAuth()
        {
            _history["."] = new SqlServerAuth
            {
                Server = ".",
                AuthType = SqlServerAuthType.Windows,
            };
            _history["(local)"] = new SqlServerAuth
            {
                Server = "(local)",
                AuthType = SqlServerAuthType.Windows,
            };
        }

        public static IList<SqlServerAuth> GetHistories()
        {
            return _history.Values.ToList();
        }

        public static SqlServerAuth GetHistory(string server)
        {
            if (string.IsNullOrEmpty(server)) return null;
            SqlServerAuth auth;
            return _history.TryGetValue(server, out auth) ? auth : null;
        }

        public static Dictionary<string, SqlServerAuth> GetHistoryDictionary()
        {
            return _history;
        }

        public static bool Exists(string server)
        {
            return _history != null && _history.ContainsKey(server);
        }

        public static void PersistHistory(SqlServerAuth auth)
        {
            if (string.IsNullOrEmpty(auth?.Server)) return;
            _history[auth.Server] = auth;

            using (var sw = new StreamWriter(File.Open(FILENAME, FileMode.Create)))
            {
                var content = JsonConvert.SerializeObject(_history.Values);
                sw.Write(content);
                sw.Flush();
            }
        }

        public static void ClearHistory()
        {
            _history = new Dictionary<string, SqlServerAuth>();
            AppendDefaultAuth();
        }
    }
}