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
    /// Interaktionslogik für Kalender.xaml
    /// </summary>
    public partial class Kalender : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        string TerminNeu = "0";
        DataTable datatableTermine = new DataTable("Termine");

        public Kalender()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleDataGridFilter();
            cb_Terminart.ItemsSource = FillFTyp();
        }

        private void fuelleDataGridFilter()
        {
            datatableTermine.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Termine Flugzeuge laden
            string SQLcmd = "SELECT termin.ter_id, flugzeug.flz_id as ID_spez, termin.tart_id, tart_bez, ter_beginn, ter_ende, ftyp_bez + ' - ' + flz_kennzeichen as ter_txt from termin, terminart, termin_flugzeug, flugzeugtyp, flugzeug where termin.tart_id = terminart.tart_id and termin.ter_id = termin_flugzeug.ter_id and termin_flugzeug.flz_id = flugzeug.flz_id and flugzeug.ftyp_id = flugzeugtyp.ftyp_id";
            SqlCommand cmd_f = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_f = new SqlDataAdapter(cmd_f);            
            adapter_f.Fill(datatableTermine);

            //Termine Personal laden
            SQLcmd = "SELECT termin.ter_id, personal.per_id as ID_spez, termin.tart_id, tart_bez, ter_beginn, ter_ende, per_name + ', ' + per_vorname + '(' + pos_bez + ')' as ter_txt from termin, terminart, termin_personal, position, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.pos_id = position.pos_id";
            SqlCommand cmd_p = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_p = new SqlDataAdapter(cmd_p);
            adapter_p.Fill(datatableTermine);

            DataGridFilter.ItemsSource = datatableTermine.DefaultView;
        }

        private ObservableCollection<terminart> FillFTyp()
        {
            var list = from e in content.terminart select e;
            return new ObservableCollection<terminart>(list);
        }

        private void bt_NeuerTermin_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
