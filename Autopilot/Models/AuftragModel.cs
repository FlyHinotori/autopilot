﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

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
        int FFlugzeugID;
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
                if (FStartDate <= value)
                    FEndDate = value;
                else
                    FEndDate = FStartDate;
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
                FlugzeugTermin.flz_id = FFlugzeugID;
            else
                throw new AuftragDatenUnvollstaendigException("Kein Flugzeugtyp ausgewählt!");
            FContent.termin_flugzeug.Add(FlugzeugTermin);
            FContent.SaveChanges();       
        }
        #endregion

        #region AvailabilityChecks
        private void CheckPersonAvailability()
        {
            string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;

            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get all persons having an appointment between FStartDate and FEndDate (blocked persons). Charter appointments without contract are not included.
            cmd.CommandText = "SELECT tp.per_id FROM termin_personal tp LEFT JOIN termin t ON (t.ter_id = tp.ter_id) LEFT JOIN terminart ta ON (t.tart_id = ta.tart_id)"
                + " LEFT JOIN termin_auftrag tauf ON (tauf.ter_id = t.ter_id) LEFT JOIN auftrag auf ON (auf.auf_id = tauf.auf_id) LEFT JOIN status s ON (s.sta_id = auf.sta_id)"
                + " WHERE s.sta_bez IS NULL"
                + " OR NOT (s.sta_bez = 'Aufnahme' OR s.sta_bez = 'Angebot')"
                + " AND ((t.ter_beginn >= CAST('" + FStartDate.ToString("yyyy-MM-dd") +"' AS date) AND t.ter_beginn <= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date))"
                + " OR (t.ter_ende >= CAST('" + FStartDate.ToString("yyyy-MM-dd") + "' AS date) AND t.ter_ende <= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date))"
                + " OR (t.ter_beginn <= CAST('" + FStartDate.ToString("yyyy-MM-dd") + "' AS date) AND t.ter_ende >= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date)))";
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            //loop through each row and check for conflicts
            try
            {
                while (ResultSet.Read())
                {
                    int PersonID = (int)ResultSet["per_id"];

                    if (FCabinCrew.Count > 0)
                    {
                        foreach (Autopilot.personal Person in FCabinCrew)
                        {
                            if (Person.per_id == PersonID)
                                throw new AuftragRessourcenNichtFreiException(Person.per_vorname + " " + Person.per_name + " ist bereits verplant!");
                        }
                    }
                    if (FPilotenCrew.Count > 0)
                    {
                        foreach (Autopilot.personal Person in FPilotenCrew)
                        {
                            if (Person.per_id == PersonID)
                                throw new AuftragRessourcenNichtFreiException(Person.per_vorname + " " + Person.per_name + " ist bereits verplant!");
                        }
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private void CheckPlaneAvailability()
        {
            string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;

            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get all planes that are not blocked 
            cmd.CommandText = "SELECT f.flz_id FROM termin_flugzeug tf LEFT JOIN flugzeug f ON (tf.flz_id = f.flz_id) LEFT JOIN termin t ON (t.ter_id = tf.ter_id)"
                + " WHERE f.ftyp_id = " + FFlugzeugTypID.ToString()
                + " AND NOT ((t.ter_beginn >= CAST('" + FStartDate.ToString("yyyy-MM-dd") + "' AS date) AND t.ter_beginn <= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date))"
                + " OR (t.ter_ende >= CAST('" + FStartDate.ToString("yyyy-MM-dd") + "' AS date) AND t.ter_ende <= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date))"
                + " OR (t.ter_beginn <= CAST('" + FStartDate.ToString("yyyy-MM-dd") + "' AS date) AND t.ter_ende >= CAST('" + FEndDate.ToString("yyyy-MM-dd") + "' AS date)))";
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            //take the first plane
            try
            {
                if (ResultSet.Read())
                    FFlugzeugID = (int)ResultSet["flz_id"];
                else
                    throw new AuftragRessourcenNichtFreiException("Kein Flugzeug dieses Typs verfügbar");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        private void CheckMandatorySelections()
        {
            //Wurde ein Flugzeugtyp ausgewählt?
            if (!(FFlugzeugTypID > 0))
                throw new AuftragDatenUnvollstaendigException("Kein Flugzeugtyp ausgewählt!");

            //Wurden genug Flugbegleiter ausgewählt?
            int FlugbegleiterAnzahl = (int)FContent.flugzeugtyp.Where(ft => ft.ftyp_id == FFlugzeugTypID).FirstOrDefault().ftyp_anz_ccrew;
            if (FlugbegleiterAnzahl > FCabinCrew.Count)
                throw new AuftragDatenUnvollstaendigException("Nicht genügend Flugbegleiter ausgewählt!");
		}
		
		private void CheckDoability()
        {
            //Check the distance
            DistanceChecker DC = new DistanceChecker();
            switch (FArtID)
            {
                //Einzelflug
                case 1:
                    DC.AddFlughafen(FContent.flughafen.Where(fh => fh.flh_id == FStartFlughafenID).FirstOrDefault());
                    DC.AddFlughafen(FContent.flughafen.Where(fh => fh.flh_id == FZielFlughafenID).FirstOrDefault());
                    break;                      
                //Einzelflug + Zwischenhalt
                case 2:
                    DC.AddFlughafen(FContent.flughafen.Where(fh => fh.flh_id == FStartFlughafenID).FirstOrDefault());
                    foreach (Autopilot.flughafen Flughafen in FZwischenHalte)
                        DC.AddFlughafen(Flughafen);
                    DC.AddFlughafen(FContent.flughafen.Where(fh => fh.flh_id == FZielFlughafenID).FirstOrDefault());
                    break;
                //Zeitcharter - Distanz egal
                case 3:
                default:
                    return;
            }
            if (DC.CanFlugzeugDoTheFlight(FContent.flugzeugtyp.Where(ft => ft.ftyp_id == FFlugzeugTypID).FirstOrDefault()))
                return;
            else
                throw new AuftragDatenFehlerhaftException("Dieser Flugzeugtyp ist nicht für diese Distanz geeignet.");
        }

        public void Save()
        {
            CheckMandatorySelections();
            CheckDoability();
            CheckPersonAvailability();
            CheckPlaneAvailability();
            FKunde.Save();
            SaveAuftrag();
            SaveTermin();
            SaveFlughaefen();
        }
    }
}