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
        DataTable datatableInfo = new DataTable("Termine");
        Int32 ter_id;
        Int32 tart_id;
        DateTime ter_beginn;
        DateTime ter_ende;
        string FlugzeugPersonal;
        string FlugzeugPersonalID;

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
            string SQLcmd = "SELECT termin.ter_id, flugzeug.flz_id as ID_spez, termin.tart_id, 'f' as f_p, tart_bez, ter_beginn, ter_ende, ftyp_bez + ' - ' + flz_kennzeichen as ter_txt from termin, terminart, termin_flugzeug, flugzeugtyp, flugzeug where termin.tart_id = terminart.tart_id and termin.ter_id = termin_flugzeug.ter_id and termin_flugzeug.flz_id = flugzeug.flz_id and flugzeug.ftyp_id = flugzeugtyp.ftyp_id";
            SqlCommand cmd_f = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_f = new SqlDataAdapter(cmd_f);            
            adapter_f.Fill(datatableTermine);

            //Termine Personal laden
            SQLcmd = "SELECT termin.ter_id, personal.per_id as ID_spez, termin.tart_id, 'p' as f_p, tart_bez, ter_beginn, ter_ende, per_name + ', ' + per_vorname + '(' + pos_bez + ')' as ter_txt from termin, terminart, termin_personal, position, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.pos_id = position.pos_id";
            SqlCommand cmd_p = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_p = new SqlDataAdapter(cmd_p);
            adapter_p.Fill(datatableTermine);

            DataGridFilter.ItemsSource = datatableTermine.DefaultView;

            cb_Terminart.IsEnabled = false;
            dp_BeginnAuswahl.IsEnabled = false;
            dp_EndeAuswahl.IsEnabled = false;
            bt_Speichern.IsEnabled = false;
            lb_Termin.Content = null;
        }

        private ObservableCollection<terminart> FillFTyp()
        {
            var list = from e in content.terminart select e;
            return new ObservableCollection<terminart>(list);
        }

        private void bt_NeuerTermin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridFilter.SelectedCells.Count != 0 && DataGridFilter.ItemsSource != null && DataGridFilter.SelectedItem != null)
            {
                DataRowView row = DataGridFilter.SelectedItems as DataRowView;
                ter_id = Convert.ToInt32(((DataRowView)DataGridFilter.SelectedItem).Row["ter_id"].ToString());
                tart_id = Convert.ToInt32(((DataRowView)DataGridFilter.SelectedItem).Row["tart_id"].ToString());
                ter_beginn = Convert.ToDateTime(((DataRowView)DataGridFilter.SelectedItem).Row["ter_beginn"].ToString());
                ter_ende = Convert.ToDateTime(((DataRowView)DataGridFilter.SelectedItem).Row["ter_ende"].ToString());
                FlugzeugPersonal = ((DataRowView)DataGridFilter.SelectedItem).Row["f_p"].ToString();
                FlugzeugPersonalID = ((DataRowView)DataGridFilter.SelectedItem).Row["ID_spez"].ToString();
                lb_Termin.Content = ((DataRowView)DataGridFilter.SelectedItem).Row["ter_txt"].ToString();

                cb_Terminart.SelectedValue = Convert.ToString(tart_id);
                dp_BeginnAuswahl.SelectedDate = ter_beginn;
                dp_EndeAuswahl.SelectedDate = ter_ende;

                //Termine der Art "Charter" dürfen nicht verändert werden
                if (Convert.ToString(((DataRowView)DataGridFilter.SelectedItem).Row["tart_bez"].ToString()) != "Charter")
                {
                    cb_Terminart.IsEnabled = true;
                    dp_BeginnAuswahl.IsEnabled = true;
                    dp_EndeAuswahl.IsEnabled = true;
                    bt_Speichern.IsEnabled = true;
                }
                else
                {
                    cb_Terminart.IsEnabled = false;
                    dp_BeginnAuswahl.IsEnabled = false;
                    dp_EndeAuswahl.IsEnabled = false;
                    bt_Speichern.IsEnabled = false;
                }

                fuelleDataGridInfo();
            }
        }

        private void fuelleDataGridInfo()
        {
            datatableInfo.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);
            string SQLcmd = "";

            if (FlugzeugPersonal == "f")
            {
                //Termine Flugzeuge laden
                SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_flugzeug, flugzeug where termin.tart_id = terminart.tart_id and termin.ter_id = termin_flugzeug.ter_id and termin_flugzeug.flz_id = flugzeug.flz_id and flugzeug.flz_id = " + FlugzeugPersonalID + " and ((ter_beginn between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)) or (ter_ende between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)))";
                SqlCommand cmd_f = new SqlCommand(SQLcmd, conn);
                SqlDataAdapter adapter_f = new SqlDataAdapter(cmd_f);
                adapter_f.Fill(datatableInfo);
            }
            else
            {
                //Termine Personal laden
                SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_personal, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.per_id = " + FlugzeugPersonalID + " and ((ter_beginn between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)) or (ter_ende between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)))";
                SqlCommand cmd_p = new SqlCommand(SQLcmd, conn);
                SqlDataAdapter adapter_p = new SqlDataAdapter(cmd_p);
                adapter_p.Fill(datatableInfo);
            }

            DataGridInfo.ItemsSource = datatableInfo.DefaultView;
        }

        private void dp_BeginnEndeAuswahl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridFilter.SelectedCells.Count != 0 && DataGridFilter.ItemsSource != null && dp_BeginnAuswahl.SelectedDate != null && dp_EndeAuswahl.SelectedDate != null)
            {
                fuelleDataGridInfo();
            }
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Terminart.Text != "Charter")
            {
                var res = MessageBox.Show("Sollen die Änderungen gespeichert werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    SqlConnection conn = new SqlConnection(DBconnStrg);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE termin SET tart_id = " + cb_Terminart.SelectedValue + ", ter_beginn = CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.ToString() + "\',103), ter_ende = CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.ToString() + "\',103) WHERE ter_id =" + ter_id;
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

                    fuelleDataGridFilter();
                    DataGridInfo.ItemsSource = null;
                }
            }
            else
            {
                MessageBox.Show("In die Terminart \"Charter\" kann hier nicht gewechselt werden.\n\nZum Speichern bitte die Auswahl ändern!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tb_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtereDataGridFilter();
        }

        private void tb_Filter_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_Filter.Clear();
        }

        private void dp_BeginnEndeFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filtereDataGridFilter();
        }

        private void filtereDataGridFilter()
        {
            if (DataGridFilter != null)
            {
                string suchbegriff = Convert.ToString(tb_Filter.Text);

                if (suchbegriff != null && dp_BeginnFilter.SelectedDate != null && dp_EndeFilter.SelectedDate != null)
                {
                    var dv = datatableTermine.DefaultView;
                    dv.RowFilter = "ter_txt LIKE \'%" + suchbegriff + "%\' and ter_beginn >= #" + dp_BeginnFilter.SelectedDate.Value.Year + "-" + dp_BeginnFilter.SelectedDate.Value.Month + "-" + dp_BeginnFilter.SelectedDate.Value.Day + "#" + "and ter_ende <= #" + dp_EndeFilter.SelectedDate.Value.Year + "-" + dp_EndeFilter.SelectedDate.Value.Month + "-" + dp_EndeFilter.SelectedDate.Value.Day + "#";
                }
                if (suchbegriff != null && (dp_BeginnFilter.SelectedDate == null || dp_EndeFilter.SelectedDate == null))
                {
                    var dv = datatableTermine.DefaultView;
                    dv.RowFilter = "ter_txt LIKE \'%" + suchbegriff + "%\'";
                }
                if (suchbegriff == null && dp_BeginnFilter.SelectedDate != null && dp_EndeFilter.SelectedDate != null)
                {
                    var dv = datatableTermine.DefaultView;
                    dv.RowFilter = "ter_beginn >= #" + dp_BeginnFilter.SelectedDate.Value.Year + "-" + dp_BeginnFilter.SelectedDate.Value.Month + "-" + dp_BeginnFilter.SelectedDate.Value.Day + "#" + "and ter_ende <= #" + dp_EndeFilter.SelectedDate.Value.Year + "-" + dp_EndeFilter.SelectedDate.Value.Month + "-" + dp_EndeFilter.SelectedDate.Value.Day + "#";
                }
            }
        }
    }
}
