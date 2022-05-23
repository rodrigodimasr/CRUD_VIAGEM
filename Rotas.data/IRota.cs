using Rotas.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rotas.data
{
    public interface IRota
    {
        List<entities.Rota> GetAll();
        entities.Rota Find(string origem,  string destino);
        List<entities.Rota> FindAll(string origem, string destino);
        bool Save(entities.Rota item);
        bool Update(entities.Rota item);
        bool Delete(int Id);
    }
}
