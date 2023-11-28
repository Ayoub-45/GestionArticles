using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesArticles
{
    internal class DatabaseAccessUtilities
    {  
       public static int NonQueryRequest(OleDbCommand command)
        {
            try
            {
                try
                {
                command.Connection.Open();
                }
                catch(OleDbException e)
                {
                    throw new MyException("Database connection Error","Failed to connect to database","Data access Layer");
                }
                return command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new MyException(e, "Database error", e.Message, "Data access Layer");
            }
            finally {
                command.Connection.Close();
                    }
            
        }
        public static int NonQueryRequest(string StrRequest,OleDbConnection connection)
        {
            try
            {
                try
                {
                    connection.Open();

                }
                catch(OleDbException e)
                {
                    throw new MyException("Connection to database error", "Failed to connect to database", "Data Access Layer");
                }
                OleDbCommand cmd=new OleDbCommand(StrRequest,connection);
                return cmd.ExecuteNonQuery();
            }
            catch(Exception e) {
                throw new MyException(e, "Database error", e.Message, "data access Layer");
            }
            finally
            {
                connection.Close();
            }
        
        }
        public static object ScalarRequest(OleDbCommand cmd)
        {
            try
            {
                try
                {
                    cmd.Connection.Open();
                    
                }
                catch(OleDbException e)
                {
                    throw new MyException(e, "Connection to database error", "Failed to connect to the database", "Data access Layer");

                }
                return cmd.ExecuteScalar();
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "Database Error", "Failed to execute the query", "Data Access Layer");
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        public static DataTable DisconnectedSelectRequest(OleDbCommand command)
        {
            try
            {
                DataTable table;
                OleDbDataAdapter selectAdapter = new OleDbDataAdapter(command);
                table = new DataTable();
                selectAdapter.Fill(table);
                return table;

            }
            catch(OleDbException e)
            {
                throw new MyException(e, "Database Error", e.Message, "Data Access Layer");
            }
        }
        public static DataTable DisconnectedSelectRequest(string StrSelectRequest, OleDbConnection MyConnection)
        {
            try
            {
                DataTable Table;
                OleDbCommand SelectCommand = new OleDbCommand(StrSelectRequest, MyConnection);
                OleDbDataAdapter SelectAdapter = new OleDbDataAdapter(SelectCommand);
                Table = new DataTable();
                SelectAdapter.Fill(Table);
                return Table;
            }
            catch (OleDbException e)
            {

                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");

            }
            finally
            {
                MyConnection.Close();
            }
        }
        public static OleDbDataReader ConnectedSelectRequest(OleDbCommand MyCommand)
        {
            try
            {
                MyCommand.Connection.Open();
                OleDbDataReader dr = MyCommand.ExecuteReader();
                return dr;

            }
            catch (OleDbException e)
            {
                //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                throw new MyException(e, "DataBase Errors", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
        public static bool CheckFieldValueExistence(string TableName, string FieldName, OleDbType FieldType, object FieldValue, OleDbConnection MyConnection)
        {
            try
            {
                string StrRequest = "SELECT COUNT(" + FieldName + ") FROM " + TableName + " WHERE ((" + FieldName + " = @" + FieldName + ")";
                StrRequest += "OR ( (@" + (FieldName + 1).ToString() + " IS NULL)AND (" + FieldName + " IS NULL)))";
                OleDbCommand Command = new OleDbCommand(StrRequest, MyConnection);
                Command.Parameters.Add("@" + FieldName, FieldType).Value = FieldValue;
                Command.Parameters.Add("@" + FieldName + 1, FieldType).Value = FieldValue;
                return ((int)DatabaseAccessUtilities.ScalarRequest(Command) != 0);
            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "There has been an error of Execution of the query : \n", "Data access Layer");
            }
            finally
            {
                MyConnection.Close();
            }

        }
        public static object GetMaxFieldValue(OleDbConnection MyConnection, string TableName, string FieldName)
        {
            try
            {
                string StrMaxRequest = "SELECT MAX(" + FieldName + ") FROM " + TableName;

                OleDbCommand Command = new OleDbCommand(StrMaxRequest, MyConnection);
                return (DatabaseAccessUtilities.ScalarRequest(Command));

            }
            catch (OleDbException e)
            {
                throw new MyException(e, "DataBase Error", "Execution error of verification of the existence of the value : \n", "Data Access Layer");
            }
            finally
            {
                MyConnection.Close();
            }
        }

    }




    //This is an inheritance of the class Exception which we will be using later
    public class MyException : Exception
        {
                string _Level;
                string _MyExceptionTitle;
                string _MyExceptionMessage;
            public  string MyExceptionTitle
            {  get { return this._MyExceptionTitle; } }
            public string MyExceptionMessage
            { get { return this._MyExceptionMessage.ToString(); } } 
            public string Level 
            {  get { return this._Level; } }

        public MyException(string MyExceptionTitle,string MyExceptionMessage, string lev)
        {
                this._Level = lev;
                this._MyExceptionMessage = MyExceptionMessage;
                this._MyExceptionTitle = MyExceptionTitle;

        }
        public MyException( Exception e, string MyExceptionTitle, string MyExceptionMessage,string lev):base(e.Message)
            {
                this._Level = lev;
                this._MyExceptionMessage=MyExceptionMessage;
                this._MyExceptionTitle = MyExceptionTitle;

            }    
        }
    }
    //////////////////////
    
}
