using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagensRepetidas
{
    public class ImagemAnalizada
    {
        public string Hash { get; set; }
        public string PathCompleto { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public bool Selecionada { get; set; }
    }
}
