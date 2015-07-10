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
            FContent = new AutopilotEntities();
            FKunde = new KundeModel();
            FZwischenHalte = new ObservableCollection<flughafen>();
            FCabinCrew = new ObservableCollection<personal>();
            FPilotenCrew = new ObservableCollection<personal>();
            FStatus = AuftragStatus.Aufnahme;
            FStartDate = DateTime.Now.Date;
            FEndDate = DateTime.Now.Date;
        }

        //members
        AutopilotEntities FContent;
        KundeModel FKunde;
        AuftragStatus FStatus;
        int FID;
        int FArtID;
        int FStartFlughafenID;
        int FZielFlughafenID;
        int FPassengerCount;
        int FCharterDauer;
        ObservableCollection<flughafen> FZwischenHalte;
        ObservableCollection<personal> FCabinCrew;
        ObservableCollection<personal> FPilotenCrew;
        int FFlugzeugTypID;
        string FWuensche;
        DateTime FStartDate;
        DateTime FEndDate;

        #region properties
        public int ID
        {
            get { return FID; }
            set
            {
                FID = value;
                NotifyPropertyChanged("ID");
            }
        }
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
        public int FlugzeugTypID
        {
            get { return FFlugzeugTypID; }
            set
            {
                FFlugzeugTypID = value;
                NotifyPropertyChanged("FlugzeugTypID");
            }
        }
        public string Wuensche
        {
            get { return FWuensche; }
            set
            {
                FWuensche = value;
                NotifyPropertyChanged("Wuensche");
            }
        }
        public DateTime StartDate
        {
            get { return FStartDate; }
            set
            {
                FStartDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }
        public DateTime EndDate
        {
            get { return FEndDate; }
            set
            {
                FEndDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private int GetIDFromStatus()
        {
            //Turn StatusID to StatusString
            string StatusString;
            switch (FStatus)
            {
                case AuftragStatus.Aufnahme:
                    StatusString = "Aufnahme";
                    break;
                default:
                    throw new AuftragDatenFehlerhaftException("Status nicht implementiert!");
            }

            //Fetch StatusgruppenID
            statusgruppe Statusgruppe = FContent.statusgruppe.Where(sg => sg.stg_bez == "Auftrag").FirstOrDefault();
            if (Statusgruppe == null)
                throw new AuftragDatenFehlerhaftException("Statusgruppe nicht gefunden!");

            //Fetch StatusID
            status Status = FContent.status.Where(s => s.sta_bez == StatusString && s.stg_id == Statusgruppe.stg_id).FirstOrDefault();
            if (Status != null)
                return Status.sta_id;
            else
                throw new AuftragDatenFehlerhaftException("Status nicht gefunden!");
        }
        private int GetAvailableFlugzeugID()
        {
            Autopilot.flugzeug EinFlugzeug = FContent.flugzeug.Where(f => f.ftyp_id == FFlugzeugTypID).FirstOrDefault();
            if (EinFlugzeug == null)
                throw new AuftragDatenFehlerhaftException("Kein Flugzeug dieses Typs");
            return EinFlugzeug.flz_id;
        }

        #region private save methods
        private void SaveAuftrag()
        {
            Autopilot.auftrag DerAuftrag = new Autopilot.auftrag();
            DerAuftrag.knd_id = FKunde.ID;
            DerAuftrag.sta_id = GetIDFromStatus();
            DerAuftrag.aart_id = FArtID;
            DerAuftrag.flh_id_beginn = FStartFlughafenID;
            DerAuftrag.flh_id_ende = FZielFlughafenID;
            DerAuftrag.auf_panzahl = FPassengerCount;
            DerAuftrag.auf_dauer_h = FCharterDauer;
            DerAuftrag.auf_wuensche = FWuensche;
            FContent.auftrag.Add(DerAuftrag);
            FContent.SaveChanges();
            ID = DerAuftrag.auf_id;
        }
        private void SaveTermin()
        {
            //Fetch TerminartID
            terminart Terminart = FContent.terminart.Where(ta => ta.tart_bez == "Charter").FirstOrDefault();
            if (Terminart == null)
                throw new AuftragDatenFehlerhaftException("Terminart Charter nicht gefunden");

            //Save general Termin
            Autopilot.termin Termin = new Autopilot.termin();
            Termin.tart_id = Terminart.tart_id;
            Termin.ter_beginn = FStartDate;
            Termin.ter_ende = FEndDate;
            FContent.termin.Add(Termin);
            FContent.SaveChanges();

            //Link Auftrag with Termin
            Autopilot.termin_auftrag AuftragTermin = new Autopilot.termin_auftrag();
            AuftragTermin.auf_id = FID;
            AuftragTermin.ter_id = Termin.ter_id;
            FContent.termin_auftrag.Add(AuftragTermin);
            FContent.SaveChanges();
            SavePersonalTermin(Termin.ter_id);
            SaveFlugzeugTermin(Termin.ter_id);
        }
        private void SavePersonTermin(int PersonID, int TerminID)
        {
            Autopilot.termin_personal PersonTermin = new Autopilot.termin_personal();
            PersonTermin.ter_id = TerminID;
            PersonTermin.per_id = PersonID;
            FContent.termin_personal.Add(PersonTermin);
            FContent.SaveChanges();
        }
        private void SavePersonalTermin(int TerminID)
        {
            if (FCabinCrew.Count > 0)
            {
                foreach (Autopilot.personal Person in FCabinCrew)
                    SavePersonTermin(Person.per_id, TerminID);
            }
            if (FPilotenCrew.Count > 0)
            {
                foreach (Autopilot.personal Person in FPilotenCrew)
                    SavePersonTermin(Person.per_id, TerminID);
            }
        }
        private void SaveZwischenhalt(int FlughafenID, int Reihenfolge)
        {        
            Autopilot.zwischenlandung DieZwischenlandung = new Autopilot.zwischenlandung();
            DieZwischenlandung.auf_id = FID;
            DieZwischenlandung.flh_id = FlughafenID;
            DieZwischenlandung.zwl_reihenfolge = Reihenfolge;
            FContent.zwischenlandung.Add(DieZwischenlandung);
            FContent.SaveChanges();
        }
        private void SaveFlughaefen()
        {
            if (FZwischenHalte.Count > 0)
            {
                int ReihenfolgeCount = 0;
                foreach (Autopilot.flughafen Flughafen in FZwischenHalte)
                {
                    SaveZwischenhalt(Flughafen.flh_id, ReihenfolgeCount);
                    ReihenfolgeCount++;
                }
            }
        }
        private void SaveFlugzeugTermin(int TerminID)
        {
            Autopilot.termin_flugzeug FlugzeugTermin = new Autopilot.termin_flugzeug();
            FlugzeugTermin.ter_id = TerminID;
            if (FFlugzeugTypID > 0)
                FlugzeugTermin.flz_id = GetAvailableFlugzeugID();
            else
                throw new AuftragDatenUnvollstaendigException("Kein Flugzeugtyp ausgewählt!");
            FContent.termin_flugzeug.Add(FlugzeugTermin);
            FContent.SaveChanges();       
        }
        #endregion

        public void Save()
        {
            FKunde.Save();
            SaveAuftrag();
            SaveTermin();
            SaveFlughaefen();
        }
    }
}