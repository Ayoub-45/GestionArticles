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
     public double? Prix { get; set; }
     public string Desigination { get; set; }    
     public DateTime DateFabrication { get; set; }  
     public bool Promo { get; set; }
    public Article() { }
    public Article(int Reference,string designation, string categorie, double? prix,DateTime dateFabrication ,bool promo )
        {
            this.Reference = Reference;
            this.Desigination = designation;
            this.Prix = prix;  
            this.DateFabrication = dateFabrication;
            this.Promo = promo;
            Console.WriteLine("Article created");
        }
        
    }
}
