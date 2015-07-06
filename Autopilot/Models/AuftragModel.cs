using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autopilot.Models
{
    public enum AuftragStatus { Aufnahme, Angebot, Vertrag, Durchführung, Beendet, Storno };

    public class AuftragModel : INotifyPropertyChanged
    {
        public AuftragModel()
        {
            FKunde = new KundeModel();
            FStatus = AuftragStatus.Aufnahme;
        }

        //members
        KundeModel FKunde;
        AuftragStatus FStatus;
        string FArt; //TODO make this an enum, but mind to provide a converter to be able to bind the respective property in XAML (that's tricky)...

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
        public AuftragStatus Status
        {
            get { return FStatus; }
            set
            {
                FStatus = value;
                NotifyPropertyChanged("Status");
            }
        }
        public string Art
        {
            get { return FArt; }
            set
            {
                FArt = value;
                NotifyPropertyChanged("Art");
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
