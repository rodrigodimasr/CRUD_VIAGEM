using Rotas.data;
using Rotas.data.Data;
using Rotas.entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rotas.negocio
{
    public class Rota : IRota
    {
        private readonly Manager _manager;
        public Rota()
        {
            _manager = new Manager();
        }

        public bool Delete(int Id)
        {
            //verificando se existe ID
            ValidarID(Id);

            var query = $@" DELETE TB_VIAGEM WHERE ID = '{Id}'";

            try { _manager.SqlExecute(query); }
            catch (Exception ex) { throw new DataAccessLayerException($@"Não foi possível cadastrar a viagem.", ex); }

            return true;
        }

        public entities.Rota Find(string origem, string destino)
        {
            if (string.IsNullOrEmpty(origem))
                throw new DataAccessLayerException("É preciso informar uma origem.");
            if (string.IsNullOrEmpty(origem))
                throw new DataAccessLayerException("É preciso informar um destinfo final.");

            var query = $@"SELECT * FROM TB_VIAGEM WHERE Origem = '{origem}' and DestinoFinal = '{destino}' and
                            valor = (select min(valor) from tb_viagem where Origem = '{origem}' and DestinoFinal = '{destino}')";
            return ConvertData(_manager.SqlQuery(query)).FirstOrDefault();

            //throw new NotImplementedException();
        }

        public List<entities.Rota> FindAll(string origem, string destino)
        {
            if (string.IsNullOrEmpty(origem))
                throw new DataAccessLayerException("É preciso informar uma origem.");
            if (string.IsNullOrEmpty(origem))
                throw new DataAccessLayerException("É preciso informar um destinfo final.");

            var query = $@"SELECT * FROM TB_VIAGEM WHERE Origem = '{origem}' and DestinoFinal = '{destino}'";
            return ConvertData(_manager.SqlQuery(query));
        }

        public List<entities.Rota> GetAll()
        {
            var query = "SELECT * FROM TB_VIAGEM";
            return ConvertData(_manager.SqlQuery(query));
        }

        public bool Save(entities.Rota item)
        {

            #region tratamentos origem e destino

            if (string.IsNullOrEmpty(item.Origem))
                throw new DataAccessLayerException("É preciso informar uma origem.");

            if (string.IsNullOrEmpty(item.DestinoFinal))
                throw new DataAccessLayerException("É preciso informar um destino final.");

            if (item.Origem == item.DestinoFinal)
                throw new DataAccessLayerException("Destino não pode ser igual a origem!");

            if (item.Valor <= 0)
                throw new DataAccessLayerException($@"Valor da viagem não pode ser: '{item.Valor}'");

            string q = $@"SELECT Id FROM TB_VIAGEM where Id='{item.Id}'";
            var result = _manager.SqlQueryUniq(q);

            if (result != null)
                throw new DataAccessLayerException($@"Já exite um registro de viagem com esse Id: '{item.Id}'");

            string qq = $@"SELECT ID FROM TB_VIAGEM where Origem = '{item.Origem}' and Parada_1 = '{item.Parada_1}' and Parada_2 = '{item.Parada_2}'
                        and Parada_3 = '{item.Parada_3}'  and Parada_4 = '{item.Parada_4}' and DestinoFinal = '{item.DestinoFinal}'";
            var existe = _manager.SqlQueryUniq(q);

            if (!string.IsNullOrEmpty(existe))
                throw new DataAccessLayerException($@"Já existe uma viagem cadastrada com essa origem e destino e as mesmas paradas!");


            #endregion fim tratamentos


            string query = $@"begin try
                          begin transaction
                          INSERT INTO TB_VIAGEM(ID, ORIGEM, PARADA_1, PARADA_2, PARADA_3, PARADA_4, DESTINOFINAL, VALOR) 
                          VALUES('{item.Id}', '{item.Origem}', '{item.Parada_1}', '{item.Parada_2}', '{item.Parada_3}', '{item.Parada_4}',
                            '{item.DestinoFinal}', '{item.Valor.ToSqlString()}')
                          commit transaction
                          end try
                          begin catch
                              rollback transaction
            declare @ErrorMessage varchar(max), @ErrorSeverity int, @ErrorState int
            select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
            raiserror(@ErrorMessage, @ErrorSeverity, @ErrorState)
                          end catch";

            try { _manager.SqlExecute(query); }
            catch (Exception ex) { throw new DataAccessLayerException($@"Não foi possível cadastrar a viagem.", ex); }

            return true;
        }

        public bool Update(entities.Rota item)
        {
            //verificando se existe ID
            ValidarID(item.Id);

            var qViagem = new DataQuery("TB_VIAGEM");

            qViagem.AddColumnItem(new PairColumnValue("ID", item.Id, true));

            if (!string.IsNullOrEmpty(item.Origem))
                qViagem.AddColumnItem(new PairColumnValue("Origem", item.Origem));

            if (!string.IsNullOrEmpty(item.Parada_1))
                qViagem.AddColumnItem(new PairColumnValue("Parada_1", item.Parada_1));

            if (!string.IsNullOrEmpty(item.Parada_2))
                qViagem.AddColumnItem(new PairColumnValue("Parada_2", item.Parada_2));

            if (!string.IsNullOrEmpty(item.Parada_3))
                qViagem.AddColumnItem(new PairColumnValue("Parada_3", item.Parada_3));

            if (!string.IsNullOrEmpty(item.Parada_4))
                qViagem.AddColumnItem(new PairColumnValue("Parada_4", item.Parada_4));

            if (!string.IsNullOrEmpty(item.DestinoFinal))
                qViagem.AddColumnItem(new PairColumnValue("DestinoFinal", item.DestinoFinal));

            if (item.Valor > 0)
                qViagem.AddColumnItem(new PairColumnValue("Valor", item.Valor.ToSqlString()));


            string query = $@"begin try
                        begin transaction
                        {qViagem.UpdateString()}
                        commit transaction
                    end try
                    begin catch
                        rollback transaction
						declare @ErrorMessage varchar(max), @ErrorSeverity int, @ErrorState int
						select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
						raiserror(@ErrorMessage, @ErrorSeverity, @ErrorState)
                    end catch";

            try { _manager.SqlExecute(query); }
            catch (Exception ex) { throw new DataAccessLayerException($@"Não foi possível atualizar o cadastro da viagem!", ex); }

            return true;
        }

        private List<entities.Rota> ConvertData(TableControl data)
        {
            var list = data.DataTable.Select().ToList().Select(item =>
                new entities.Rota()
                {
                    Id = Convert.ToInt32(item["ID"]?.ToString()),
                    Origem = item["ORIGEM"]?.ToString()?.TrimEnd(),
                    Parada_1 = item["PARADA_1"]?.ToString()?.TrimEnd(),
                    Parada_2 = item["PARADA_2"]?.ToString()?.TrimEnd(),
                    Parada_3 = item["PARADA_3"]?.ToString()?.TrimEnd(),
                    Parada_4 = item["PARADA_4"]?.ToString()?.TrimEnd(),
                    DestinoFinal = item["DESTINOFINAL"]?.ToString()?.TrimEnd(),
                    Valor = item["VALOR"].ToDecimal(0),
                }).ToList();

            return list;
        }

        private void ValidarID(int Id)
        {
            string q = $@"SELECT Id FROM TB_VIAGEM where Id='{Id}'";
            var result = _manager.SqlQueryUniq(q);

            if (result == null)
                throw new DataAccessLayerException($@"Id informado não existe");
        }

    }
}
