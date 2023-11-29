using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesArticles
{
    internal class DataAccessLayer
    {
       private static bool CheckEntityUnicity(int EntityKey)
        {
            int NbOccs = 0;
            using (OleDbConnection Cnn = DatabaseConnection.GetConnection())
            {
                string StrSQl = $"SELECT COUNT(*) FROM Article WHERE reference={EntityKey}";
                OleDbCommand Cmd = new OleDbCommand(StrSQl, Cnn);
                Cmd.Parameters.AddWithValue("@EntityKey", EntityKey);
                NbOccs = (int)DatabaseAccessUtilities.ScalarRequest(Cmd);
            }
            if (NbOccs == 0)
                return true;
            else 
                return false;  
        }
        private static Article GetEntityFromDataRow(DataRow dr) 
        {
        Article a = new Article();
            a.Reference = (int)dr["reference"];
            a.Designation = dr["designation"] == System.DBNull.Value ? "" : (string)dr["designation"];
            a.Categorie = dr["categorie"] == System.DBNull.Value ? "" : (string)dr["categorie"];
            if (dr["prix"] == System.DBNull.Value)
                a.Prix = null;
            else
                a.Prix = (float)dr["prix"];
            a.DateFabrication = (DateTime)dr["dateFabrication"];
            a.Promo = (bool)dr["promo"];
            return a;
        }
        private static List<Article> GetListFromDataTable(DataTable dt)
        {
            if(dt !=null)
            {
                List<Article> L = new List<Article>(dt.Rows.Count);
                foreach(DataRow dr in dt.Rows)
                {
                    L.Add(GetEntityFromDataRow(dr));
                }
                return L;
            }
            else
            {
                return null;
            }

        }
        public static  void Add(Article a)
        {   
            using(OleDbConnection cnn = DatabaseConnection.GetConnection())
            {
                if(CheckEntityUnicity(a.Reference)==true)
                {
                    string StrSQL = "INSERT INTO Article(Reference,Designation,Categorie,Prix,DateFabrication,Promo) VALUES(@Reference,@Designation,@Categorie,@Prix,@DateFabrication,@Promo)";
                    OleDbCommand Cmd=new OleDbCommand(StrSQL, cnn);
                    Cmd.Parameters.Add("@Reference", OleDbType.Integer).Value = a.Reference;
                    Cmd.Parameters.Add("@Designation",OleDbType.VarChar).Value = a.Designation;
                    Cmd.Parameters.Add("@Categorie", OleDbType.VarChar).Value = a.Categorie;
                    Cmd.Parameters.Add("@Prix", OleDbType.Single).Value = a.Prix;
                    Cmd.Parameters.Add("@DateFabrication", OleDbType.DBDate).Value = a.DateFabrication;
                    Cmd.Parameters.Add("@Promo",OleDbType.Boolean).Value = a.Promo;
                    DatabaseAccessUtilities.NonQueryRequest(Cmd);
                    Console.WriteLine("Executing the query");

                }
                else
                {
                    throw new MyException("There has been Error with adding your Article","The reference is Already in use","Data Access Layer");

                }
            }
        }
    public static void Delete(int EntityKey) 
        {
            using (OleDbConnection cnn = DatabaseConnection.GetConnection())
            {
                string StrSQl = "DELETE * FROM Article WHERE reference=@EntityKey";
                OleDbCommand Cmd = new OleDbCommand( StrSQl, cnn);
                Cmd.Parameters.Add("@EnityKey",OleDbType.UnsignedBigInt).Value = EntityKey;
                DatabaseAccessUtilities.NonQueryRequest(Cmd);
            }
        }
        public static void Update(Article CurArticle,Article NewArticle)
        {
            using(OleDbConnection Cnn = DatabaseConnection.GetConnection())
            {
                if (CurArticle.Reference != NewArticle.Reference)
                {
                    throw new MyException("Error occurs in modifying non existing Article", "New Refernce is already in use", "Data Access Layer");

                }
                else
                {
                    string StrSQL = "UPDATE Article SET reference=@Reference, designation= @Designation, categorie = @Categorie, prix = @Prix, dateFabrication = @DateFabrication, promo=@Promo WHERE reference = @CurReference";
                    OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                    Cmd.Parameters.Add("@reference", OleDbType.Integer).Value = NewArticle.Reference;
                    Cmd.Parameters.Add("@designation", OleDbType.VarChar).Value = NewArticle.Designation;
                    Cmd.Parameters.Add("@categorie", OleDbType.VarChar).Value = NewArticle.Categorie;
                    Cmd.Parameters.Add("@prix", OleDbType.Single).Value = NewArticle.Prix;
                    Cmd.Parameters.Add("@dateFabrication", OleDbType.Date).Value = NewArticle.DateFabrication;
                    Cmd.Parameters.Add("@promo", OleDbType.Boolean).Value = NewArticle.Promo;
                    Cmd.Parameters.Add("@curReference", OleDbType.Integer).Value = CurArticle.Reference;

                    DatabaseAccessUtilities.NonQueryRequest(Cmd);
                }
            }

        }

        public static Article SelectById(int EntityKey)
        {
            using (OleDbConnection Cnn = DatabaseConnection.GetConnection())
            {
                string StrSQL = "SELECT * FROM Article WHERE reference=@EntityKey";
                OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn); Cmd.Parameters.AddWithValue("@EntityKey", EntityKey);
                DataTable dt = DatabaseAccessUtilities.DisconnectedSelectRequest(Cmd);
                if (dt != null && dt.Rows.Count != 0)
                    return DataAccessLayer.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }


        }
        public static List<Article> SelectAll()
        {
            List<Article> ListeArticles = new List<Article>();
            Article p;

            using (OleDbConnection Cnn = DatabaseConnection.GetConnection())
            {
                try
                {
                    string StrSQL = "SELECT * FROM Article ";
                    OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
                    Cnn.Open();
                    OleDbDataReader dr = Cmd.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            p = new Article();
                            p.Reference = dr.GetInt32(0);
                            p.Designation = dr.GetString(1);
                            p.Categorie = dr.GetString(2);
                            p.Prix = dr.GetFloat(3);
                            p.DateFabrication = dr.GetDateTime(4);
                            p.Promo = dr.GetBoolean(5);
                            ListeArticles.Add(p);
                        }
                    }
                    return ListeArticles;
                }
                catch (OleDbException e)
                {
                    //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                    throw new MyException(e, "DataBase Errors", e.Message, "Data access Layer");
                }
                finally
                {
                    Cnn.Close();
                }

            }
            //public static List<Article> DisconnectedSelectAll()
            //{
            //    DataTable dt;
            //    using (OleDbConnection Cnn = DbConnection.GetConnection())
            //    {
            //        string StrSQL = "SELECT * FROM Article ";
            //        OleDbCommand Cmd = new OleDbCommand(StrSQL, Cnn);
            //        dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Cmd);
            //    }
            //    return GetListFromDataTable(dt);
            //}

        }
    }


    }

