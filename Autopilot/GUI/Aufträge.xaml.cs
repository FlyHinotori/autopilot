﻿using System;
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

        public Aufträge()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TableAuftraege.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT a.auf_id, a.knd_id, anr.anr_bez, k.knd_vorname, k.knd_name, t.ter_beginn, t.ter_ende, s.sta_bez, kg.kng_bez, CAST( (t.ter_ende - t.ter_beginn + 1) AS Int) AS flugzeit"
                + " FROM auftrag a LEFT JOIN kunde k ON (k.knd_id = a.knd_id) LEFT JOIN anrede anr ON (anr.anr_id = k.anr_id) LEFT JOIN kundengruppe kg ON (kg.kng_id = k.knd_id) " 
                + " LEFT JOIN status s ON (s.sta_id = a.sta_id) LEFT JOIN termin_auftrag ta ON (ta.auf_id = a.auf_id) LEFT JOIN termin t ON (t.ter_id = ta.ter_id) ORDER BY a.sta_id ASC";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(TableAuftraege);

            GridAuftraege.ItemsSource = TableAuftraege.DefaultView;
        }

        private void SetButtons(string Kundengruppe, string Auftragsstatus)
        { 
            BtnAngebotErstellen.IsEnabled = Auftragsstatus == "Aufnahme";
            BtnAuftragStornieren.IsEnabled = (Auftragsstatus == "Angebot") || Auftragsstatus == "Vertrag";
            BtnVertragErstellen.IsEnabled = Auftragsstatus == "Angebot";
            BtnVertragUnterschrieben.IsEnabled = Auftragsstatus == "Vertrag";
            BtnRechnungErstellen.IsEnabled = ((Auftragsstatus == "Durchführung") && (Kundengruppe == "PRE")) || 
                ((Auftragsstatus == "Beendet") && (Kundengruppe != "PRE"));
            BtnFlugdatenErfassen.IsEnabled = Auftragsstatus == "Durchführung";
            BtnFeedbackErfassen.IsEnabled = (Auftragsstatus != "Aufnahme") && (Auftragsstatus != "Angebot") &&
                (Auftragsstatus != "Vertrag");
        }

        private void GridAuftraege_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridAuftraege.SelectedCells.Count != 0 && GridAuftraege.ItemsSource != null && GridAuftraege.SelectedItem != null)
            {
                DataRowView row = GridAuftraege.SelectedItems as DataRowView;
                string Kundengruppe = ((DataRowView)GridAuftraege.SelectedItem).Row["kng_bez"].ToString();
                string Auftragsstatus = ((DataRowView)GridAuftraege.SelectedItem).Row["sta_bez"].ToString();
                SetButtons(Kundengruppe, Auftragsstatus);
                FAuftragsID = Convert.ToInt32(((DataRowView)GridAuftraege.SelectedItem).Row["auf_id"].ToString());
                FFlugzeit = Convert.ToInt32(((DataRowView)GridAuftraege.SelectedItem).Row["flugzeit"].ToString());
            }  
        }

        private void BtnAngebotErstellen_Click(object sender, RoutedEventArgs e)
        {
            CalculateCosts();
            CreatePDF();
        }

        private void CreatePDF()
        {
            FileStream fs = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document(new iTextSharp.text.Rectangle(PageSize.A4));
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            PdfPTable table = new PdfPTable(1);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase = new Phrase();
            phrase.Add(new Chunk("Charterangebot", boldFont));
            phrase.Add(new Chunk("\n\nder", normalFont));
            phrase.Add(new Chunk("\n\nHINOTORI Executive AG ", boldFont));
            table.AddCell(phrase);
            var phrase2 = new Phrase();
            phrase2.Add(new Chunk("\n\nSehr geehrte/r Herr Wurstwasser,", normalFont));
            phrase2.Add(new Chunk("\n\nfür Ihre Anfrage bedanken wir uns ganz herzlich. Gern machen wir Ihnen ein Angebot über den Charterauftrag.", normalFont));
            phrase2.Add(new Chunk("\n\nFlug von A nach B für 1000€", normalFont));
            phrase2.Add(new Chunk("\n\nWir würden uns freuen Ihren Auftrag zu erhalten. Bei Fragen zögern Sie nicht uns zu kontaktieren.", normalFont));
            phrase2.Add(new Chunk("\n\nMit freundlichen Grüße", normalFont));
            phrase2.Add(new Chunk("\n\nHINOTORI Executive AG", normalFont));
            doc.Add(table);
            doc.Add(phrase2);
            doc.Close();
        }

        private void CalculateCosts()
        {
            double Fixkosten = GetFixkosten();
            double Personalkosten = GetPersonalkosten();
            double Flugkosten = GetFlugkosten();
            double Gesamtkosten = Fixkosten + Personalkosten + Flugkosten;
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
                int Jahresgehalt = (int)ResultSet["gehalt"];
                Personalkosten = Personalkosten + ((Jahresgehalt / 365) * FFlugzeit * 1.2);
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
