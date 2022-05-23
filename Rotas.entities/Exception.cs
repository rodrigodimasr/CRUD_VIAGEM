using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotas.entities
{
    public class AlreadyExistsException : System.ApplicationException
    {
        public AlreadyExistsException() : base() { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
        public AlreadyExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class BusinessException : System.ApplicationException
    {
        public BusinessException() : base() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
        public BusinessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class DataAccessLayerException : System.ApplicationException
    {
        public DataAccessLayerException() : base() { }
        public DataAccessLayerException(string message) : base(message) { }
        public DataAccessLayerException(string message, Exception innerException) : base(message, innerException) { }
        public DataAccessLayerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class LicenseException : System.ApplicationException
    {
        public LicenseException() : base() { }
        public LicenseException(string message) : base(message) { }
        public LicenseException(string message, Exception innerException) : base(message, innerException) { }
        public LicenseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class LicenseWarningException : System.ApplicationException
    {
        public LicenseWarningException() : base() { }
        public LicenseWarningException(string message) : base(message) { }
        public LicenseWarningException(string message, Exception innerException) : base(message, innerException) { }
        public LicenseWarningException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
