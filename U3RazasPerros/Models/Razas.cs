using System;
using System.Collections.Generic;

#nullable disable

namespace U3RazasPerros.Models
{
    public partial class Razas
    {
        public uint Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string OtrosNombres { get; set; }
        public int IdPais { get; set; }
        public float PesoMin { get; set; }
        public float PesoMax { get; set; }
        public float AlturaMin { get; set; }
        public float AlturaMax { get; set; }
        public int EsperanzaVida { get; set; }

        public virtual Paises IdPaisNavigation { get; set; }
        public virtual Caracteristicasfisicas Caracteristicasfisicas { get; set; }
    }
}
