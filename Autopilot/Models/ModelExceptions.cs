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
}
