namespace DbDictExport.WPF.Common
{
    public class SqlServerAuth
    {
        public string Server { get; set; }

        public SqlServerAuthType AuthType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public enum SqlServerAuthType
    {
        SqlServer,
        Windows
    }
}