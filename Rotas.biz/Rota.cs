using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rotas.biz
{
    public class Rota
    {
        public static List<entities.Rota> GetAll()
        {
            var data = Interface.CreateRota();
            var itens = data.GetAll();
            return itens;
        }

        public static entities.Rota Find(string origem, string destino)
        {
            var data = Interface.CreateRota();
            var itens = data.Find(origem, destino);
            return itens;
        }

        public static List<entities.Rota> FindAll(string origem, string destino)
        {
            var data = Interface.CreateRota();
            var itens = data.FindAll(origem, destino);
            return itens;
        }

        public static bool Save(entities.Rota item)
        {
            var data = Interface.CreateRota();
            return data.Save(item);
        }

        public static bool Update(entities.Rota item)
        {
            var data = Interface.CreateRota();
            return data.Update(item);
        }

        public static bool Delete(int Id)
        {
            var data = Interface.CreateRota();
            return data.Delete(Id);
        }
    }
}
