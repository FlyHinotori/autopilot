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
            string SQLcmd = "SELECT auftrag.auf_id, status.sta_id, auftragsart.aart_id, kunde.knd_id, sta_bez, aart_bez, knd_name + \', \' + knd_vorname FROM auftrag, status, auftragsart, kunde WHERE auftrag.sta_id = status.sta_id AND auftrag.aart_id = auftragsart.aart_id AND auftrag.knd_id = kunde.knd_id";
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

                if (suchbegriff != null)
                {
                    var dv = datatableUebersicht.DefaultView;
                    //dv.RowFilter = "auf_id LIKE \'%" + suchbegriff + "%\'";
                }
            }
        }
    }
}
