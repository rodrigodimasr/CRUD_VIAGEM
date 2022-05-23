using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotas.data.Data
{
    public enum Logical { And = 0, Or };
    public enum Conditional { Equal = 0, NotEqual, Bigger, BiggerEqual, Smaller, SmallerEQual, Null, NotNull }
    public enum PrinterLengthLine { Daruma = 30, Sweda = 30, Diebold = 42 };
    public enum OrdenadorNotaFiscal { DataCrescente = 0, DataDecrescente = 1, NumeroCrescente = 2, NumeroDecrescente = 3 };
}
