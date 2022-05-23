using Rotas.entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace Rotas.data
{
    public class Manager
    {
        const string arquivoCsv = @"C:\dados\tb_rota.csv";

        #region se for usar BD
        private HttpContext _mContext;

        public Manager()
        {
        }

        #region SqlStatment Aqui são os metodos caso fosse salvar no BD
        public TableControl SqlQuery(string pQuery)
        {
            return new TableControl(DataConnection.SqlQuery(pQuery));
        }
        public TableControl SqlQuery(string commandtext, string sortexpression, int skip, int take)
        {
            return new TableControl(DataConnection.SqlQuery(commandtext, sortexpression, skip, take));
        }
        public TableControl SqlQuery(string commandtext, int take)
        {
            return new TableControl(DataConnection.SqlQuery(commandtext, take));
        }
        public string SqlQueryUniq(string pQuery)
        {
            return DataConnection.SqlQueryUniq(pQuery);
        }

        public int SqlExecute(string pQuery)
        {
            try
            {
                return DataConnection.SqlExecute(pQuery);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException(ex.Message);
            }
        }
        #endregion

        #region quando for usar BD
        public bool ValidaDigitoVerificador(string numDado, int numDig, int limMult)
        {
            try
            {
                string dado = "", digito = "";
                int mult, soma, n;

                dado = numDado.Substring(0, 12);
                for (n = 0; n < numDig; n++)
                {
                    soma = 0;
                    mult = 2 - n;
                    for (var i = dado.Length - 1; i >= 1; i--)
                    {
                        soma = soma + mult * Convert.ToInt32(dado.Substring(i, 1));
                        mult += 1;
                        if (mult > limMult) mult = 2;
                    }
                    digito = (((soma * 10) % 11) % 10).ToString();
                    dado += (digito == "0" ? "1" : digito);
                }
                return (dado == numDado);
            }
            catch { return false; }
        }

        public DataConnection DataConnection
        {
            get { return (new DataConnection()); }
        }
        public entities.Rota Rota
        {
            get { return ((entities.Rota)_mContext.Session["Rota"]); }
            set { _mContext.Session["Rota"] = value; }
        }
        public System.Web.HttpContext Context
        {
            get { return _mContext; }
            set { _mContext = value; }
        }

        #endregion

        #endregion

    }
}
