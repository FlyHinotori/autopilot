using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Aufträge.xaml
    /// </summary>
    public partial class Aufträge : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable TableAuftraege = new DataTable("Auftraege");
        int FAuftragsID = 0;
        int FFlugzeit = 0;
        string FAnrede;
        string FAuftragsArt;
        string FStartDate;
        string FEndDate;
        string FPaxAnzahl;
        string FVonOrt;
        string FBisOrt;
        string FKundengruppe;
        AutopilotEntities FContent;
        
        Font NormalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
        Font BoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

        public Aufträge()
        {
            InitializeComponent();
            FContent = new AutopilotEntities();
        }

        private void LoadAuftraege()
        {
            TableAuftraege.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);
            string SQLcmd = "SELECT a.auf_id, a.knd_id, a.auf_panzahl, anr.anr_bez, k.knd_vorname, k.knd_name, t.ter_beginn, t.ter_ende, s.sta_bez, aa.aart_bez,"
                + " kg.kng_bez, CAST( (t.ter_ende - t.ter_beginn + 1) AS Int) AS flugzeit, fh1.flh_name AS VonOrt, fh2.flh_name AS BisOrt"
                + " FROM auftrag a LEFT JOIN kunde k ON (k.knd_id = a.knd_id) LEFT JOIN anrede anr ON (anr.anr_id = k.anr_id) LEFT JOIN kundengruppe kg ON (kg.kng_id = k.knd_id)"
                + " LEFT JOIN status s ON (s.sta_id = a.sta_id) LEFT JOIN termin_auftrag ta ON (ta.auf_id = a.auf_id) LEFT JOIN termin t ON (t.ter_id = ta.ter_id) "
                + " LEFT JOIN auftragsart aa ON (a.aart_id = aa.aart_id) LEFT JOIN flughafen fh1 ON (fh1.flh_id = a.flh_id_beginn) LEFT JOIN flughafen fh2 ON (fh2.flh_id = a.flh_id_ende)"
                + " ORDER BY a.sta_id ASC";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(TableAuftraege);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAuftraege();
            GridAuftraege.ItemsSource = TableAuftraege.DefaultView;
        }

        /*!
         If Auftragsstatus is empty, all buttons will be disabled
         */
        private void SetButtons(string Auftragsstatus)
        { 
            BtnAngebotErstellen.IsEnabled = Auftragsstatus == "Aufnahme";
            BtnAuftragStornieren.IsEnabled = (Auftragsstatus == "Angebot") || Auftragsstatus == "Vertrag";
            BtnVertragErstellen.IsEnabled = Auftragsstatus == "Angebot";
            BtnVertragUnterschrieben.IsEnabled = Auftragsstatus == "Vertrag";
            BtnRechnungErstellen.IsEnabled = ((Auftragsstatus == "Durchführung") && (FKundengruppe == "PRE")) || 
                ((Auftragsstatus == "Beendet") && (FKundengruppe != "PRE"));
            BtnFlugdatenErfassen.IsEnabled = Auftragsstatus == "Durchführung";
            BtnFeedbackErfassen.IsEnabled = (Auftragsstatus != "Aufnahme") && (Auftragsstatus != "Angebot") &&
                (Auftragsstatus != "Vertrag") && (Auftragsstatus != "");
        }

        private void GridAuftraege_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridAuftraege.SelectedCells.Count != 0 && GridAuftraege.ItemsSource != null && GridAuftraege.SelectedItem != null)
            {
                DataRowView row = GridAuftraege.SelectedItems as DataRowView;
                FKundengruppe = ((DataRowView)GridAuftraege.SelectedItem).Row["kng_bez"].ToString();
                string Auftragsstatus = ((DataRowView)GridAuftraege.SelectedItem).Row["sta_bez"].ToString();
                SetButtons(Auftragsstatus);
                FAuftragsID = Convert.ToInt32(((DataRowView)GridAuftraege.SelectedItem).Row["auf_id"].ToString());
                FFlugzeit = Convert.ToInt32(((DataRowView)GridAuftraege.SelectedItem).Row["flugzeit"].ToString());
                FAnrede = ((DataRowView)GridAuftraege.SelectedItem).Row["anr_bez"].ToString()
                    + " " + ((DataRowView)GridAuftraege.SelectedItem).Row["knd_vorname"].ToString()
                    + " " + ((DataRowView)GridAuftraege.SelectedItem).Row["knd_name"].ToString();
                FAuftragsArt = ((DataRowView)GridAuftraege.SelectedItem).Row["aart_bez"].ToString();
                FStartDate = Convert.ToDateTime(((DataRowView)GridAuftraege.SelectedItem).Row["ter_beginn"]).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                FEndDate = Convert.ToDateTime(((DataRowView)GridAuftraege.SelectedItem).Row["ter_ende"]).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                FPaxAnzahl = ((DataRowView)GridAuftraege.SelectedItem).Row["auf_panzahl"].ToString();
                FVonOrt = ((DataRowView)GridAuftraege.SelectedItem).Row["VonOrt"].ToString();
                FBisOrt = ((DataRowView)GridAuftraege.SelectedItem).Row["BisOrt"].ToString();
            }
            else
                //disable all buttons
                SetButtons("");
        }

        private void ChangeStatusTo(string NeuerStatus)
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE auftrag SET sta_id = (SELECT sta_id FROM status WHERE sta_bez = '" + NeuerStatus + "') WHERE auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();            
        }

        private void SetAblehnungsgrund(int AblehnungsgrundID)
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE auftrag SET ablg_id = " + AblehnungsgrundID.ToString() + " WHERE auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close(); 
        }

        private void BtnAngebotErstellen_Click(object sender, RoutedEventArgs e)
        {
            Document Angebot = CreatePDF("Angebot");
            Angebot.Open();
            Angebot.Add(GetAngebotHeader());
            iTextSharp.text.Image FlugzeugPic = GetFlugzeugImage();
            if (FlugzeugPic != null)
                Angebot.Add(FlugzeugPic);
            Angebot.Add(GetAngebotText());
            Angebot.Close();
            ChangeStatusTo("Angebot");
            LoadAuftraege();
        }

        private void BtnAuftragStornieren_Click(object sender, RoutedEventArgs e)
        {
            Autopilot.ablehnungsgrund grund = new Autopilot.ablehnungsgrund();
            grund.ablg_bez = Microsoft.VisualBasic.Interaction.InputBox("Aus welchem Grund wurde der Auftrag abgelehnt?", "Ablehnungsgrund", "kein Grund bekannt", 500, 500);
            FContent.ablehnungsgrund.Add(grund);
            FContent.SaveChanges();
            SetAblehnungsgrund(grund.ablg_id);
            ChangeStatusTo("Storno");
            LoadAuftraege();
        }

        private void BtnVertragErstellen_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusTo("Vertrag");
        }

        private void BtnVertragUnterschrieben_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusTo("Durchführung");
        }

        private void BtnRechnungErstellen_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusTo("erstellt");
        }

        private void BtnFlugdatenErfassen_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusTo("Beendet");
        }

        #region PDFErstellung
        private Document CreatePDF(string Name)
        {
            Directory.CreateDirectory("pdf");
            FileStream fs = new FileStream("pdf\\" + Name + Guid.NewGuid().ToString() + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document(new iTextSharp.text.Rectangle(PageSize.A4));
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            return doc;
        }

        private PdfPTable GetAngebotHeader()
        {
            PdfPTable HeaderTable = new PdfPTable(1);
            HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            var Header = new Phrase();
            Header.Add(new Chunk("Charterangebot", BoldFont));
            Header.Add(new Chunk("\n\nder", NormalFont));
            Header.Add(new Chunk("\n\nHINOTORI Executive AG \n\n", BoldFont));
            HeaderTable.AddCell(Header);
            return HeaderTable;
        }

        private Phrase GetAngebotText()
        {
            double Preis = CalculateCosts();
            var BriefText = new Phrase();
            BriefText.Add(new Chunk("\n\nSehr geehrte/r " + FAnrede + ",", NormalFont));
            BriefText.Add(new Chunk("\n\nfür Ihre Anfrage bedanken wir uns ganz herzlich. Gern machen wir Ihnen ein Angebot über den Charterauftrag.", NormalFont));
            BriefText.Add(new Chunk("\n\n" + GetCharterDescription() + " für " + Preis.ToString("F2") + " €", NormalFont));
            BriefText.Add(new Chunk("\n\nWir würden uns freuen Ihren Auftrag zu erhalten. Bei Fragen zögern Sie nicht uns zu kontaktieren.", NormalFont));
            BriefText.Add(new Chunk("\n\nMit freundlichen Grüße", NormalFont));
            BriefText.Add(new Chunk("\n\nHINOTORI Executive AG", NormalFont));
            return BriefText;
        }

        private iTextSharp.text.Image GetFlugzeugImage()
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get flugzeug pic
            cmd.CommandText = "SELECT f.flz_bild FROM termin_auftrag ta LEFT JOIN termin t ON (ta.ter_id = t.ter_id) LEFT JOIN termin_flugzeug tf ON (tf.ter_id = t.ter_id)"
                + " LEFT JOIN flugzeug f ON (f.flz_id = tf.flz_id)"
                + " WHERE ta.auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            SqlDataReader ResultSet = cmd.ExecuteReader();
            BitmapImage BitObj = new BitmapImage();
            byte[] a = null;
            if (ResultSet.Read())
            {
                a = (byte[])ResultSet["flz_bild"];
            }
            conn.Close();

            if (a == null)
                return null;
            iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(a);

            if (pic.Height > pic.Width)
            {
                //Maximum height is 50 pixels.
                float percentage = 0.0f;
                percentage = 50 / pic.Height;
                pic.ScalePercent(percentage * 100);
            }
            else
            {
                //Maximum width is 50 pixels.
                float percentage = 0.0f;
                percentage = 50 / pic.Width;
                pic.ScalePercent(percentage * 100);
            }

            pic.Border = iTextSharp.text.Rectangle.BOX;
            pic.BorderColor = iTextSharp.text.BaseColor.BLACK;
            pic.BorderWidth = 1f;
            return pic;
        }

        private string GetCharterDescription()
        {
            if (FAuftragsArt == "Zeitcharter")
                return GetZeitcharterDescription();
            else if (FAuftragsArt == "Einzelflug")
                return GetEinzelflugDescription();
            else if (FAuftragsArt == "Flug mit Zwischenaufenthalt")
                return GetFlugMitZwischenstoppDescription();
            else
                return "";
        }

        private string GetZeitcharterDescription()
        {
            return "Im Zeitraum von " + FStartDate + " bis " + FEndDate;
        }

        private string GetFlugMitZwischenstoppDescription()
        {
            return "Passagierzahl: " + FPaxAnzahl + " \n" + "Im Zeitraum von " + FStartDate + " bis " + FEndDate + "\n"
                + "Von " + FVonOrt + " nach " + FBisOrt + " über " + GetZwischenstationen();
        }

        private string GetEinzelflugDescription()
        {
            return "Passagierzahl: " + FPaxAnzahl + " \n" + "Im Zeitraum von " + FStartDate + " bis " + FEndDate + "\n" + "Von " + FVonOrt + " nach " + FBisOrt;
        }

        private string GetZwischenstationen()
        {
            string Zwischenstationen = "";
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get all Zwischenstationen
            cmd.CommandText = "SELECT fh.flh_name FROM zwischenlandung z LEFT JOIN flughafen fh ON (fh.flh_id = z.flh_id) "
                + " WHERE z.auf_id = " + FAuftragsID.ToString()
                + " ORDER BY z.zwl_reihenfolge ASC";
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            while (ResultSet.Read())
            {
                string Station = ResultSet["flh_name"].ToString();
                Zwischenstationen += Station;
                Zwischenstationen += ", ";
            }
            conn.Close();
            //remove last comma
            if (Zwischenstationen.Length > 2)
                Zwischenstationen = Zwischenstationen.Remove(Zwischenstationen.Length - 2, 2);
            return Zwischenstationen;
        }
        #endregion

        private double CalculateCosts()
        {
            double Fixkosten = GetFixkosten();
            double Personalkosten = GetPersonalkosten();
            double Flugkosten = GetFlugkosten();
            return Fixkosten + Personalkosten + Flugkosten;
        }

        private double GetFlugkosten()
        {
            double Flugkosten = 0;
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get hourly costs
            cmd.CommandText = "SELECT CAST(ft.ftyp_vkosten_ph AS int) AS costperhour FROM termin_auftrag ta LEFT JOIN termin t ON (ta.ter_id = t.ter_id)"
                + " LEFT JOIN termin_flugzeug tf ON (tf.ter_id = t.ter_id) LEFT JOIN flugzeug f ON (f.flz_id = tf.flz_id) LEFT JOIN flugzeugtyp ft ON (ft.ftyp_id = f.ftyp_id)"
                + " WHERE ta.auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            if (ResultSet.Read())
            {
                int CostPerHour = (int)ResultSet["costperhour"];
                Flugkosten = CostPerHour * FFlugzeit * 24;
            }
            conn.Close();
            return Flugkosten;
        }

        private double GetPersonalkosten()
        {
            double Personalkosten = 0;
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get annual salary
            cmd.CommandText = "SELECT CAST(pos.pos_gehalt_pa AS int) AS gehalt FROM termin_auftrag ta LEFT JOIN termin t ON (ta.ter_id = t.ter_id) LEFT JOIN termin_personal tp ON (tp.ter_id = t.ter_id)"
                + " LEFT JOIN personal p ON (p.per_id = tp.per_id) LEFT JOIN position pos ON (pos.pos_id = p.pos_id)"
                + " WHERE ta.auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            while (ResultSet.Read())
            {
                if (ResultSet["gehalt"] != DBNull.Value)
                { 
                    int Jahresgehalt = (int)ResultSet["gehalt"];
                    Personalkosten = Personalkosten + ((Jahresgehalt / 365) * FFlugzeit * 1.2);
                }
            }
            conn.Close();
            return Personalkosten;
        }

        private double GetFixkosten()
        {
            double Fixkosten = 0;
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get annual fix costs
            cmd.CommandText = "SELECT CAST(ft.ftyp_fkosten_pa AS int) AS costperanno FROM termin_auftrag ta LEFT JOIN termin t ON (ta.ter_id = t.ter_id)"
                + " LEFT JOIN termin_flugzeug tf ON (tf.ter_id = t.ter_id) LEFT JOIN flugzeug f ON (f.flz_id = tf.flz_id) LEFT JOIN flugzeugtyp ft ON (ft.ftyp_id = f.ftyp_id)"
                + " WHERE ta.auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            if (ResultSet.Read())
            {
                int CostPerAnno = (int)ResultSet["costperanno"];
                Fixkosten = (CostPerAnno / 365) * FFlugzeit;
            }
            conn.Close();
            return Fixkosten;
        }
    }
}
