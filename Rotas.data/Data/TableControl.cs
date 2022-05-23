using Rotas.entities;
using System;
using System.Data;
using System.Linq;

namespace Rotas.data
{
    public class TableControl
    {
		DataTable _mCurrentTable;

		public TableControl()
		{
			_mCurrentTable = null;
		}
		public TableControl(DataTable pTable)
		{
			_mCurrentTable = pTable;
		}

		public string GetColumn(string pColumn)
		{
			return GetColumn(pColumn, 0);
		}

		public string GetColumn(string pColumn, int pIndex)
		{
			try
			{
				if (_mCurrentTable.Columns[pColumn].ToString() != string.Empty && _mCurrentTable.Columns[pColumn].ToString() != null)
					return _mCurrentTable.Rows[pIndex][pColumn].ToString().Trim();
				else
					return "";
			}
			catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
		}

		public int RecCount
		{
			get { return _mCurrentTable.Rows.Count; }
		}

		public bool HasRows
		{
			get { return (RecCount > 0 ? true : false); }
		}

		public System.Data.DataTable DataTable
		{
			get { return _mCurrentTable; }
			set { _mCurrentTable = value; }
		}
	}
}
