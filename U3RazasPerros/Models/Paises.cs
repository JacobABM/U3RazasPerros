using System;
using System.Collections.Generic;

#nullable disable

namespace U3RazasPerros.Models
{
    public partial class Paises
    {
        public Paises()
        {
            Razas = new HashSet<Razas>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Razas> Razas { get; set; }
    }
}
