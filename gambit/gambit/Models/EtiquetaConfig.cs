using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambit.Models
{
    public class EtiquetaConfig
    {
        public int Altura { get; set; }
        public string TipoPPL { get; set; }
        public int Codigo { get; set; }
        public int Modelo { get; set; }
        public string Descricao { get; set; }
        public int Largura { get; set; }
        public int Colunas { get; set; }
        public int Gap { get; set; }

        public Eixo Eixo { get; set; }
    }
}
