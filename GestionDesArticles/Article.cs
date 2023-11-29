using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesArticles
{
    internal class Article
    {
     public int Reference { get; set; }
     public float? Prix { get; set; }
     public string Designation { get; set; }    
     public DateTime DateFabrication { get; set; }  
     public bool Promo { get; set; }
    public string Categorie { get; set; }
    public Article() { }
    public Article(int reference,string designation, string categorie, float? prix,DateTime dateFabrication ,bool promo )
        {
            this.Reference = reference;
            this.Designation = designation;
            this.Prix = prix;  
            this.DateFabrication = dateFabrication;
            this.Promo = promo;
            this.Categorie = categorie;
            
            Console.WriteLine("Article created");
        }
        
    }
}
