using System.Collections.Generic;

namespace U3RazasPerros.Models.ViewModels
{
    public class IndexViewModel
    {
        public char Inicial { get; set; }
        public IEnumerable<Razas> Perros { get; set; }
        public IEnumerable<char> Iniciales { get; set; }
    }
}
