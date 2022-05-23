using Rotas.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotas.data.Data
{
    public class PairColumnValue
    {
        public PairColumnValue(string pColumn, Object pValue, TypeCode pType, bool pk)
        {
            try
            {
                Column = pColumn?.Trim();
                PK = pk;
                if (pValue == null)
                    Type = TypeCode.DBNull;
                else
                {
                    if (System.Type.GetTypeCode(pValue.GetType()) != pType)
                        Value = Convert.ChangeType(pValue, pType);
                    Type = pType;
                }
            }
            catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
        }
        public PairColumnValue(string pColumn, Object pValue, TypeCode pType)
        {
            try
            {
                Column = pColumn?.Trim();

                if (pValue == null)
                    Type = TypeCode.DBNull;
                else
                {
                    if (System.Type.GetTypeCode(pValue.GetType()) != pType)
                        Value = Convert.ChangeType(pValue, pType);
                    Type = pType;
                }
            }
            catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
        }
        public PairColumnValue(string pColumn, Object pValue, bool pk)
        {
            try
            {
                Column = pColumn?.Trim();
                Value = pValue;
                PK = pk;

                if (pValue == null)
                    Type = TypeCode.DBNull;
                else
                    Type = System.Type.GetTypeCode(pValue.GetType());
            }
            catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
        }
        public PairColumnValue(string pColumn, Object pValue)
        {
            try
            {
                Column = pColumn?.Trim();
                Value = pValue;

                if (pValue == null)
                    Type = TypeCode.DBNull;
                else
                    Type = System.Type.GetTypeCode(pValue.GetType());
            }
            catch (Exception ex) { throw new DataAccessLayerException(ex.Message); }
        }

        #region Properties
        public TypeCode Type { get; set; }
        public Object Value { get; set; }
        public string Column { get; set; }
        public bool PK { get; set; }
        #endregion
    }
}
