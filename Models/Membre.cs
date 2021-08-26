using System;
using System.Collections.Generic;

#nullable disable

namespace Database_First.Models
{
    public partial class Membre
    { 
        public Membre()
        {
            Emprunters = new HashSet<Emprunter>();
        }

        public int IdM { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Courriel { get; set; }
        public string MotPasse { get; set; }

        public virtual ICollection<Emprunter> Emprunters { get; set; }
    }
}
