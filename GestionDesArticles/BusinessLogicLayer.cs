using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GestionDesArticles.BusinessLogicLayer;

namespace GestionDesArticles
{
    internal class BusinessLogicLayer
    {

            public static void Add(Article p)
            {
                DataAccessLayer.Add(p);
            }

            public static void Update(Article CurArticle, Article NewArticle)
            {
                DataAccessLayer.Update(CurArticle, NewArticle);
            }

            public static void Delete(int pRef)
            {
                DataAccessLayer.Delete(pRef);
            }

            public static Article GetById(int pRef)
            {
                return DataAccessLayer.SelectById(pRef);
            }

            public static List<Article> GetAll()
            {
                return DataAccessLayer.SelectAll();
            }
        }
    }
