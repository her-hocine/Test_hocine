using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Database_First.Models
{
    public partial class Emprunter
    { 
        public int IdE { get; set; }
        public int? IdM { get; set; }
        public int? IdL { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateEmp { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateRetour { get; set; }

        public virtual Livre IdLNavigation { get; set; }
        public virtual Membre IdMNavigation { get; set; }
    }
}
