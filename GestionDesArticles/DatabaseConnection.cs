using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesArticles
{
    internal class DatabaseConnection
    {
        static string path = "C:\\Users\\Dell\\Desktop\\ArticleDB.accdb";
        static string StrCnn = $"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {path}";
        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(StrCnn);
        }
    }
}
