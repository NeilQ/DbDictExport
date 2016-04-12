using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using DbDictExport.Core;
using DbDictExport.Core.Dal;

namespace DbDictExport.WPF.Pages
{
    public class HomeViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly SqlConnectionStringBuilder _connStringBuilder;

        public List<DbCategory> DbCategories { get; set; } = new List<DbCategory>();
        public HomeViewModel()
        {
            _connStringBuilder = Global.ConnectionBuilder as SqlConnectionStringBuilder;

            if (_connStringBuilder != null)
            {
                var dbNames = DataAccess.GetDbNameList(_connStringBuilder);
                if (dbNames != null)
                {
                    foreach (var dbName in dbNames)
                    {
                        DbCategories.Add(new DbCategory()
                        {
                            Name = dbName
                        });
                    }
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}