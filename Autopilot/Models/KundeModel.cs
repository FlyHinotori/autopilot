using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autopilot.Models
{
    public class KundeModel : INotifyPropertyChanged
    {
        //members
        string FName = String.Empty;
        string FAdresse = String.Empty;

        //properties
        public string Name
        {
            get { return FName; }
            set
            {
                FName = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string Adresse
        {
            get { return FAdresse; }
            set
            {
                FAdresse = value;
                NotifyPropertyChanged("Adresse");
            }
        }

        //INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }        
    }
}
