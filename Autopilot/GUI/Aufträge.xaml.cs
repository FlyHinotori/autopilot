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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Aufträge.xaml
    /// </summary>
    public partial class Aufträge : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable TableAuftraege = new DataTable("Auftraege");

        public Aufträge()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TableAuftraege.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT a.auf_id, a.knd_id, anr.anr_bez, k.knd_vorname, k.knd_name, t.ter_beginn, t.ter_ende, s.sta_bez"
                + " FROM auftrag a LEFT JOIN kunde k ON (k.knd_id = a.knd_id) LEFT JOIN anrede anr ON (anr.anr_id = k.anr_id) LEFT JOIN status s ON (s.sta_id = a.sta_id)"
                + " LEFT JOIN termin_auftrag ta ON (ta.auf_id = a.auf_id) LEFT JOIN termin t ON (t.ter_id = ta.ter_id)";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(TableAuftraege);

            GridAuftraege.ItemsSource = TableAuftraege.DefaultView;
        }
    }
}
