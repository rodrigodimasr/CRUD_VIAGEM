using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Rotas.data
{
    public class DataConnection
    {
        string connectionS = "Server=TI-1266\\SQLEXPRESS;Database=Cadastro;Trusted_connection=True;MultipleActiveResultSets=true";
        #region constructor
        protected virtual void Dispose(bool disposing)
        {
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Properties
        public string WorkStationId { get; set; }
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserPassword { get; set; }
        public string UserId { get; set; }
        #endregion

        public virtual bool IsValid()
        {
            var isValid = false;
            var connection = new SqlConnection(connectionS);
            try
            {
                connection.Open();
                isValid = true;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return isValid;
        }

        public virtual int SqlExecute(string pQuery)
        {
            var value = 0;
            var connection = new SqlConnection(connectionS);

            var command = new SqlCommand(pQuery, connection);
            command.CommandTimeout = 1000 * 60 * 2;

            command.Connection.Open();
            try { value = command.ExecuteNonQuery(); }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
            }

            return value;
        }
        public virtual string SqlQueryUniq(string pQuery)
        {
            var value = (string)null;

            var command = new SqlCommand(pQuery, new SqlConnection(connectionS));
            command.CommandTimeout = 1000 * 60 * 2;
            command.Connection.Open();

            try { value = command.ExecuteScalar()?.ToString(); }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
            }
            return value;
        }
        public virtual DataTable SqlQuery(string pQuery)
        {
            var table = new DataTable();
            var command = new SqlCommand(pQuery, new SqlConnection(connectionS))
            {
                CommandTimeout = 1000 * 60 * 2
            };
            try
            {
                command.Connection.Open();

                var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
            }
            return table;
        }
        public virtual DataTable SqlQuery(string commandtext, int take)
        {
            var q = $@"
                select *
                    from (
                        select top {take} *
                        from ({commandtext}) as innercur
                        ) as cur
                    ";

            return SqlQuery(q);
        }
        public virtual DataTable SqlQuery(string commandtext, string sortexpression, int skip, int take)
        {
            if (string.IsNullOrWhiteSpace(sortexpression))
                throw new ApplicationException("A ordem para pesquisa paginada é obrigatória");

            // emissao desc, DATA_PARA_TRANSFERENCIA desc
            var q = $@"
                select *
                    from (
                        select row_number() over (order by {sortexpression}) as [row_number], *
                        from ({commandtext}) as innercur
                        ) as cur
                    where [row_number] between {skip} + 1 and {skip} + {take}
                        order by [row_number]
                ";

            return SqlQuery(q);
        }
    }
}
