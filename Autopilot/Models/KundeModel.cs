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
        //members of table "kunde"
        string FName = String.Empty;
        string FVorname = String.Empty;
        string FStrasse = String.Empty;
        string FOrt = String.Empty;
        string FPostleitzahl = String.Empty;
        string FLand = String.Empty;
        string FEMail = String.Empty;
        string FTelefon = String.Empty;
        //members of table "kundengruppe"
        string FGruppe = String.Empty;
        //members of table "anrede"
        string FAnrede = String.Empty;
        //members of table "titel"
        string FTitel = String.Empty;

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
        public string Vorname
        {
            get { return FVorname; }
            set
            {
                FVorname = value;
                NotifyPropertyChanged("Vorname");
            }
        }
        public string Strasse
        {
            get { return FStrasse; }
            set
            {
                FStrasse = value;
                NotifyPropertyChanged("Strasse");
            }
        }
        public string Ort
        {
            get { return FOrt; }
            set
            {
                FOrt = value;
                NotifyPropertyChanged("Ort");
            }
        }
        public string Postleitzahl
        {
            get { return FPostleitzahl; }
            set
            {
                FPostleitzahl = value;
                NotifyPropertyChanged("Postleitzahl");
            }
        }
        public string Land
        {
            get { return FLand; }
            set
            {
                FLand = value;
                NotifyPropertyChanged("Land");
            }
        }
        public string EMail
        {
            get { return FEMail; }
            set
            {
                FEMail = value;
                NotifyPropertyChanged("EMail");
            }
        }
        public string Telefon
        {
            get { return FTelefon; }
            set
            {
                FTelefon = value;
                NotifyPropertyChanged("Telefon");
            }
        }
        public string Gruppe
        {
            get { return FGruppe; }
            set
            {
                FGruppe = value;
                NotifyPropertyChanged("Gruppe");
            }
        }
        public string Anrede
        {
            get { return FAnrede; }
            set
            {
                FAnrede = value;
                NotifyPropertyChanged("Anrede");
            }
        }
        public string Titel
        {
            get { return FTitel; }
            set
            {
                FTitel = value;
                NotifyPropertyChanged("Titel");
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
