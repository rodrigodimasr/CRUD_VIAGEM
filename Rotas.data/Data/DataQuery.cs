using Rotas.entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotas.data.Data
{
	public class DataQuery
	{

		private ArrayList _mStringTypes;
		private ArrayList _mIntTypes;
		private ArrayList _mDateTypes;
		private ArrayList _mBitTypes;
		private ArrayList _mDecimalTypes;
		private ArrayList _mAnotherTypes;

		private ArrayList _mSqlColumns;
		private string _mSqlWhere;
		private string _mSqlTable;

		public DataQuery(string pSqlTable)
		{
			SqlTable = pSqlTable;
			SqlWhere = string.Empty;
			_mSqlColumns = new ArrayList();
			SqlWhereColumns = new ArrayList();

			#region Groups of Types
			//----- Groups of Strings types
			_mStringTypes = new ArrayList();
			_mStringTypes.Add(TypeCode.Char);
			_mStringTypes.Add(TypeCode.String);
			_mStringTypes.Add(TypeCode.Empty);
			_mStringTypes.Add(TypeCode.DBNull);

			//----- Group of Integers
			_mIntTypes = new ArrayList();
			_mIntTypes.Add(TypeCode.Int16);
			_mIntTypes.Add(TypeCode.Int32);
			_mIntTypes.Add(TypeCode.Int64);
			_mIntTypes.Add(TypeCode.UInt16);
			_mIntTypes.Add(TypeCode.UInt32);
			_mIntTypes.Add(TypeCode.UInt64);

			//----- Group of Dates
			_mDateTypes = new ArrayList();
			_mDateTypes.Add(TypeCode.DateTime);

			//----- Group of Bits
			_mBitTypes = new ArrayList();
			_mBitTypes.Add(TypeCode.Boolean);

			//----- Group of Decimals
			_mDecimalTypes = new ArrayList();
			_mDecimalTypes.Add(TypeCode.Decimal);
			_mDecimalTypes.Add(TypeCode.Double);
			_mDecimalTypes.Add(TypeCode.Single);

			//----- Group of Miscelaneos
			_mAnotherTypes = new ArrayList();
			_mAnotherTypes.Add(TypeCode.SByte);
			_mAnotherTypes.Add(TypeCode.Object);
			_mAnotherTypes.Add(TypeCode.Byte);
			#endregion
		}

		public void AddColumnItem(PairColumnValue pColumn)
		{
			SqlColumns.Add(pColumn);
		}

		#region AddWhereItem
		public void AddWhereItem(PairColumnValue pWhere, Conditional pConditional, Logical pLogical)
		{
			AddWhereItem(pWhere, pConditional);

			switch (pLogical)
			{
				case Logical.And:
					SqlWhere += " And ";
					break;
				case Logical.Or:
					SqlWhere += " Or ";
					break;
			}
		}
		public void AddWhereItem(PairColumnValue pWhere, Conditional pConditional)
		{
			SqlWhereColumns.Add(pWhere);

			SqlWhere += "(" + pWhere.Column.Trim();

			switch (pConditional)
			{
				case Conditional.Equal:
					SqlWhere += " = " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.NotEqual:
					SqlWhere += " != " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.Bigger:
					SqlWhere += " > " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.BiggerEqual:
					SqlWhere += " >= " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.Smaller:
					SqlWhere += " < " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.SmallerEQual:
					SqlWhere += " <= " + GetDataTyped(pWhere) + ")";
					break;
				case Conditional.Null:
					SqlWhere += " is null )";
					break;
				case Conditional.NotNull:
					SqlWhere += " is Not null )";
					break;
			}

		}
		#endregion

		#region InsertString
		public string InsertString()
		{
			var strInsert = "";

			strInsert = "Insert into " + SqlTable + " (";

			for (var x = 0; x < ColumnCount; x++)
				strInsert += ((PairColumnValue)SqlColumns[x]).Column.Trim() + (x < ColumnCount - 1 ? ", " : "");

			strInsert += ") Values ( ";

			for (var x = 0; x < ColumnCount; x++)
			{
				strInsert += GetDataTyped((PairColumnValue)SqlColumns[x]);
				strInsert += (x < ColumnCount - 1 ? ", " : ")");
			}

			return strInsert;
		}
		#endregion

		#region UpdateString
		public string UpdateString()
		{
			var strUpdate = "";

			strUpdate = "Update " + SqlTable + " Set ";

			for (var x = 0; x < ColumnCount; x++)
			{
				var column = ((PairColumnValue)SqlColumns[x]);
				/* não faz update em PK */
				if (column.PK)
					continue;

				strUpdate += column.Column.Trim();
				strUpdate += " = " + GetDataTyped(column);
				strUpdate += (x < ColumnCount - 1 ? ", " : "");
			}

			return strUpdate + (SqlWhere == string.Empty ? "" : " Where " + SqlWhere);
		}
		#endregion

		#region GetDataType
		private string GetDataTyped(PairColumnValue pData)
		{
			var strReturn = "";

			var dbType = pData.Type;
			var strValue = pData.Value;

			try
			{

				if (_mStringTypes.Contains(dbType) || _mAnotherTypes.Contains(dbType))
				{
					if (strValue == null)
						strReturn = " null ";
					else
						strReturn = "'" + strValue.ToString().Trim().Replace("'", "''") + "'";
				}

				if (_mIntTypes.IndexOf(dbType) >= 0)
				{
					strReturn = strValue.ToString().Trim();
				}

				if (_mDecimalTypes.Contains(dbType))
				{
					strReturn = strValue.ToString().Replace(",", ".");
				}

				if (_mDateTypes.Contains(dbType))
				{
					strReturn = "'" + ((DateTime)strValue).ToString("yyyyMMdd HH:mm:ss.fff") + "'";
				}

				if (_mBitTypes.Contains(dbType))
				{
					strReturn = ((Boolean)strValue ? "1" : "0");
				}

				return strReturn;
			}
			catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
		}
		#endregion

		#region Properties
		public string SqlTable
		{
			get { return _mSqlTable; }
			set { _mSqlTable = value; }
		}
		private System.Collections.ArrayList SqlColumns
		{
			get { return _mSqlColumns; }
			set { _mSqlColumns = value; }
		}
		public ArrayList SqlWhereColumns { get; set; }
		private string SqlWhere
		{
			get { return _mSqlWhere; }
			set { _mSqlWhere = value; }
		}
		public int ColumnCount
		{
			get { return SqlColumns.Count; }
		}
		#endregion

	}
}
