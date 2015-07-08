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
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Rechnungen_Uebersicht.xaml
    /// </summary>
    public partial class Rechnungen_Uebersicht : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable datatableUebersicht = new DataTable("Uebersicht");

        public Rechnungen_Uebersicht()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleDataGridUebersicht();
        }

        private void fuelleDataGridUebersicht()
        {
            datatableUebersicht.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT auftrag.auf_id, status.sta_id, auftragsart.aart_id, kunde.knd_id, sta_bez, case when ((select sum(buc_haben) - sum(buc_soll) from buchung where buchung.auf_id = auftrag.auf_id) is null) then 0 else (select sum(buc_haben) - sum(buc_soll) from buchung where buchung.auf_id = auftrag.auf_id) end - auf_preis - auf_zusatzkosten as saldo, (select mst_reihenfolge from mahnstufe where mahnstufe.mst_id = auftrag.mst_id) as mahnstufe, aart_bez, ter_beginn, ter_ende, knd_name + \', \' + knd_vorname as kunde_bez, (select flh_name + \'(\' + flh_stadt + \')\' as abflug from flughafen where flughafen.flh_id = auftrag.flh_id_beginn) as abflughafen, (select flh_name + '(' + flh_stadt + ')' as zielflug from flughafen where flughafen.flh_id = auftrag.flh_id_ende) as zielflughafen FROM auftrag, status, auftragsart, kunde, termin_auftrag, termin WHERE auftrag.sta_id = status.sta_id AND auftrag.aart_id = auftragsart.aart_id AND auftrag.knd_id = kunde.knd_id AND auftrag.auf_id = termin_auftrag.auf_id AND termin_auftrag.ter_id = termin.ter_id";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(datatableUebersicht);

            DataGridUebersicht.ItemsSource = datatableUebersicht.DefaultView;
        }

        private void tb_FilterUebersicht_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridUebersicht != null)
            {
                string suchbegriff = Convert.ToString(tb_FilterUebersicht.Text);

                if (suchbegriff != null && suchbegriff != "")
                {
                    var dv = datatableUebersicht.DefaultView;
                    dv.RowFilter = "auf_id = \'" + suchbegriff + "\' OR kunde_bez like \'%" + suchbegriff + "%\' OR abflughafen like \'%" + suchbegriff + "%\' OR zielflughafen like \'%" + suchbegriff + "%\'";
                }
                else
                {
                    var dv = datatableUebersicht.DefaultView;
                    dv.RowFilter = "kunde_bez like \'%\'";
                }
            }
        }
    }
}
