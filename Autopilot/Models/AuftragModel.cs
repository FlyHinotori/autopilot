using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Autopilot.Models
{
    public enum AuftragStatus { Aufnahme, Angebot, Vertrag, Durchführung, Beendet, Storno };

    public class AuftragModel : INotifyPropertyChanged
    {
        public AuftragModel()
        {
            FKunde = new KundeModel();
            FZwischenHalte = new ObservableCollection<flughafen>();
            FCabinCrew = new ObservableCollection<personal>();
            FPilotenCrew = new ObservableCollection<personal>();
            FStatus = AuftragStatus.Aufnahme;
        }

        //members
        KundeModel FKunde;
        AuftragStatus FStatus;
        int FArtID;
        int FStartFlughafenID;
        int FZielFlughafenID;
        int FPassengerCount;
        int FCharterDauer;
        ObservableCollection<flughafen> FZwischenHalte;
        ObservableCollection<personal> FCabinCrew;
        ObservableCollection<personal> FPilotenCrew;

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
        public int ArtID
        {
            get { return FArtID; }
            set
            {
                FArtID = value;
                NotifyPropertyChanged("ArtID");
            }
        }
        public int StartFlughafenID
        {
            get { return FStartFlughafenID; }
            set
            {
                FStartFlughafenID = value;
                NotifyPropertyChanged("StartFlughafenID");
            }
        }
        public int ZielFlughafenID
        {
            get { return FZielFlughafenID; }
            set
            {
                FZielFlughafenID = value;
                NotifyPropertyChanged("ZielFlughafenID");
            }
        }
        public int PassengerCount
        {
            get { return FPassengerCount; }
            set
            {
                FPassengerCount = value;
                NotifyPropertyChanged("PassengerCount");
            }
        }
        public ObservableCollection<flughafen> ZwischenHalte
        {
            get { return FZwischenHalte; }
            set
            {
                FZwischenHalte = value;
                NotifyPropertyChanged("ZwischenHalte");
            }
        }
        public int CharterDauer
        {
            get { return FCharterDauer; }
            set
            {
                FCharterDauer = value;
                NotifyPropertyChanged("CharterDauer");
            }
        }
        public ObservableCollection<personal> CabinCrew
        {
            get { return FCabinCrew; }
            set
            {
                FCabinCrew = value;
                NotifyPropertyChanged("CabinCrew");
            }
        }
        public ObservableCollection<personal> PilotenCrew
        {
            get { return FPilotenCrew; }
            set
            {
                FPilotenCrew = value;
                NotifyPropertyChanged("PilotenCrew");
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
