using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autopilot.Models
{
    #region General Exception
    public class GeneralModelsException : Exception
    {
        public GeneralModelsException()
        {
        }
        public GeneralModelsException(string message)
            : base(message)
        {
        }
        public GeneralModelsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

    #region Kunden Exceptions
    public class KundeDatenUnvollstaendigException : GeneralModelsException
    {
        public KundeDatenUnvollstaendigException(string message)
            : base(message)
        {
        }
        public KundeDatenUnvollstaendigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

    #region Auftrag
    public class AuftragDatenUnvollstaendigException : GeneralModelsException
    {
        public AuftragDatenUnvollstaendigException(string message)
            : base(message)
        {
        }
        public AuftragDatenUnvollstaendigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class AuftragDatenFehlerhaftException : GeneralModelsException
    {
        public AuftragDatenFehlerhaftException(string message)
            : base(message)
        {
        }
        public AuftragDatenFehlerhaftException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class AuftragRessourcenNichtFreiException : GeneralModelsException
    {
        public AuftragRessourcenNichtFreiException(string message)
            : base(message)
        {
        }
        public AuftragRessourcenNichtFreiException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion
}
