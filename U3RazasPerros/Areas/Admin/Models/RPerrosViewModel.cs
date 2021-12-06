using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Models
{
    public class RPerrosViewModel
    {
        public IEnumerable<Paises> Paises { get; set; }
        public Razas Raza { get; set; }

    }
}
