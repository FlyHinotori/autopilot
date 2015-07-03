using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autopilot.Models
{
    public class KundeDatenUnvollstaendigException: Exception
    {
        public KundeDatenUnvollstaendigException(string message)
            : base(message)
        {
        }
    }

    public class KundeModel : INotifyPropertyChanged
    {
        AutopilotEntities FContent;
        public KundeModel()
        {
            FContent = new AutopilotEntities();
        }

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

        #region properties
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

        #region foreign key getters
        private int GetTitleID()
        {
            //Find current title in database
            Autopilot.titel DerTitel = FContent.titel.Where(t => t.tit_bez == FTitel).FirstOrDefault();
            //No title found?
            if (DerTitel == null)
            {
                //Create the new title
                Autopilot.titel NeuerTitel = new Autopilot.titel();
                NeuerTitel.tit_bez = FTitel;
                FContent.titel.Add(NeuerTitel);
                FContent.SaveChanges();
                //Assign the new title
                DerTitel = FContent.titel.Where(t => t.tit_bez == FTitel).First();
            }
            return DerTitel.tit_id;
        }

        private int GetAnredeID()
        {
            //Find current anrede in database
            Autopilot.anrede DieAnrede = FContent.anrede.Where(a => a.anr_bez == FAnrede).FirstOrDefault();
            //No anrede found?
            if (DieAnrede == null)
            {
                //Create the new anrede
                Autopilot.anrede NeueAnrede = new Autopilot.anrede();
                NeueAnrede.anr_bez = FAnrede;
                FContent.anrede.Add(NeueAnrede);
                FContent.SaveChanges();
                //Assign the new anrede
                DieAnrede = FContent.anrede.Where(a => a.anr_bez == FAnrede).First();
            }
            return DieAnrede.anr_id;
        }

        private int GetKundengruppeID()
        {
            //Find current kundengruppe in database
            Autopilot.kundengruppe DieGruppe = FContent.kundengruppe.Where(kg => kg.kng_bez == FGruppe).FirstOrDefault();
            //No kundengruppe found?
            if (DieGruppe == null)
            {
                //Create the new kundengruppe
                Autopilot.kundengruppe NeueGruppe = new Autopilot.kundengruppe();
                NeueGruppe.kng_bez = FGruppe;
                FContent.kundengruppe.Add(NeueGruppe);
                FContent.SaveChanges();
                //Assign the new kundengruppe
                DieGruppe = FContent.kundengruppe.Where(kg => kg.kng_bez == FGruppe).First();
            }
            return DieGruppe.kng_id;
        }
        #endregion

        private Autopilot.kunde GetKundeDBSet()
        {
            //Get kunde from database
            Autopilot.kunde Result = FContent.kunde.Where(k => k.knd_name == FName && k.knd_vorname == FVorname).FirstOrDefault();
            //No kunde found?
            if (Result == null)
            {
                //Create and add new kunde
                Result = new Autopilot.kunde();
                FContent.kunde.Add(Result);
            }
            return Result;
        }
        
        public void Save()
        {
            if ((FName.Length == 0) && (FVorname.Length == 0))
            {
                throw new KundeDatenUnvollstaendigException("Name oder Vorname fehlt!");
            }
            //store information in table "kunde"
            Autopilot.kunde DerKunde = GetKundeDBSet();
            DerKunde.knd_name = FName;
            DerKunde.knd_vorname = FVorname;
            DerKunde.knd_strasse = FStrasse;
            DerKunde.knd_ort = FOrt;
            DerKunde.knd_plz = FPostleitzahl;
            DerKunde.knd_land = FLand;
            DerKunde.knd_mail = FEMail;
            DerKunde.knd_telefon = FTelefon;
            if (FTitel.Length > 0)
            {
                DerKunde.tit_id = GetTitleID();
            }
            if (FAnrede.Length > 0)
            {
                DerKunde.anr_id = GetAnredeID();
            }
            if (FGruppe.Length > 0)
            {
                DerKunde.kng_id = GetKundengruppeID();
            }
            FContent.SaveChanges();
        }
    }
}
