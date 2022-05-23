using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotas.entities
{
    public class Rota
    {
        public int Id { get; set; }
        public string Origem { get; set; }
        public string Parada_1 { get; set; }
        public string Parada_2 { get; set; }
        public string Parada_3 { get; set; }
        public string Parada_4 { get; set; }
        public string DestinoFinal { get; set; }
        public decimal Valor { get; set; }
        
    }
}
