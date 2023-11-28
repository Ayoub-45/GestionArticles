using System;
using System.Collections.Generic;
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
        public static object ScalarRequest()
        {

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
