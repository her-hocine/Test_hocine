using System;
using System.Collections.Generic;

#nullable disable

namespace Database_First.Models
{ 
    public partial class Livre
    {
        public Livre()
        {
            Emprunters = new HashSet<Emprunter>();
        }

        public int IdL { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Categories { get; set; }
        public int? NbExemlpaire { get; set; }
        public int? NbExemlpaireTotal { get; set; }

        public virtual ICollection<Emprunter> Emprunters { get; set; }
    }
}
