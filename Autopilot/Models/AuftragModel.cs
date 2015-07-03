using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autopilot.Models
{
    public class AuftragModel : INotifyPropertyChanged
    {
        public AuftragModel()
        {
            FKunde = new KundeModel();
        }

        //members
        KundeModel FKunde;

        #region properties
        public KundeModel Kunde
        {
            get { return FKunde; }
            set
            {
                FKunde = value;
                NotifyPropertyChanged("Kunde");
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        //save methods
        public void Save()
        {
            FKunde.Save();
        }

    }
}
