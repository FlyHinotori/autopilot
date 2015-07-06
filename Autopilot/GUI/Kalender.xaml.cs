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
        DataTable datatableTermine = new DataTable("Termine");
        DataTable datatableInfo = new DataTable("Termine");
        DataTable datatableAuswahl = new DataTable("Auswahl");
        Int32 ter_id;
        Int32 tart_id;
        DateTime ter_beginn;
        DateTime ter_ende;
        string FlugzeugPersonal;
        string FlugzeugPersonalID;
        string NeuerTermin ="0";

        public Kalender()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleDataGridFilter();
            cb_Terminart.ItemsSource = FillFTyp();
            DataGridAuswahl.Visibility = Visibility.Hidden;
            DataGridFilter.Visibility = Visibility.Visible;
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
            bt_Abbrechen.IsEnabled = false;
            bt_Loeschen.IsEnabled = false;
            lb_Termin.Content = null;
        }

        private ObservableCollection<terminart> FillFTyp()
        {
            var list = from e in content.terminart select e;
            return new ObservableCollection<terminart>(list);
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
                    bt_Abbrechen.IsEnabled = true;
                    bt_Loeschen.IsEnabled = true;
                }
                else
                {
                    cb_Terminart.IsEnabled = false;
                    dp_BeginnAuswahl.IsEnabled = false;
                    dp_EndeAuswahl.IsEnabled = false;
                    bt_Speichern.IsEnabled = false;
                    bt_Abbrechen.IsEnabled = false;
                    bt_Loeschen.IsEnabled = false;
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
                SQLcmd = "";
                if (NeuerTermin == "0")
                {
                    SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_flugzeug, flugzeug where termin.tart_id = terminart.tart_id and termin.ter_id = termin_flugzeug.ter_id and termin_flugzeug.flz_id = flugzeug.flz_id and flugzeug.flz_id = " + FlugzeugPersonalID + " and ((ter_beginn between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)) or (ter_ende between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)))";
                }
                else
                {
                    SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_flugzeug, flugzeug where termin.tart_id = terminart.tart_id and termin.ter_id = termin_flugzeug.ter_id and termin_flugzeug.flz_id = flugzeug.flz_id and flugzeug.flz_id = " + FlugzeugPersonalID;
                }
                SqlCommand cmd_f = new SqlCommand(SQLcmd, conn);
                SqlDataAdapter adapter_f = new SqlDataAdapter(cmd_f);
                adapter_f.Fill(datatableInfo);
            }
            else
            {
                //Termine Personal laden
                SQLcmd = "";
                if (NeuerTermin == "0")
                {
                    SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_personal, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.per_id = " + FlugzeugPersonalID + " and ((ter_beginn between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)) or (ter_ende between CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.Value.AddDays(-1).ToShortDateString() + "\',103) and CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.Value.AddDays(1).ToShortDateString() + "\',103)))";
                }
                else
                {
                    SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_personal, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.per_id = " + FlugzeugPersonalID;
                }
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
                if (NeuerTermin == "0")
                {
                    //Termin bearbeiten
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
                    //Neuer Termin
                    var res = MessageBox.Show("Soll ein neuer Termin angelegt werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        SqlConnection conn = new SqlConnection(DBconnStrg);
                        conn.Open();
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = conn;
                        cmd1.CommandText = "INSERT INTO termin (tart_id, ter_beginn, ter_ende) VALUES (" + cb_Terminart.SelectedValue.ToString() + ",CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.ToString() + "\',103), CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.ToString() + "\',103))";
                        cmd1.CommandType = CommandType.Text;

                        try
                        {
                            cmd1.ExecuteNonQuery();
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        conn.Close();

                        //ter_id holen
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = conn;
                        cmd2.CommandText = "SELECT MAX(ter_id) FROM termin WHERE tart_id = " + cb_Terminart.SelectedValue.ToString() + " AND ter_beginn = CONVERT(date,\'" + dp_BeginnAuswahl.SelectedDate.ToString() + "\',103) and ter_ende = CONVERT(date,\'" + dp_EndeAuswahl.SelectedDate.ToString() + "\',103)";
                        cmd2.CommandType = CommandType.Text;

                        conn.Open();

                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        if (dr2.Read())
                        {
                            ter_id = Convert.ToInt32(dr2.GetValue(0));
                        }

                        conn.Close();

                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.Connection = conn;
                        string SQLcmd3 = "";
                        if (FlugzeugPersonal == "f")
                        {
                            SQLcmd3 = "INSERT INTO termin_flugzeug (ter_id, flz_id) VALUES (" + ter_id + "," + FlugzeugPersonalID + ")";
                        }
                        else
                        {
                            SQLcmd3 = "INSERT INTO termin_personal (ter_id, per_id) VALUES (" + ter_id + "," + FlugzeugPersonalID + ")";
                        }
                        cmd3.CommandText = SQLcmd3;
                        cmd3.CommandType = CommandType.Text;

                        conn.Open();

                        try
                        {
                            cmd3.ExecuteNonQuery();
                            NeuerTermin = "0";
                            DataGridAuswahl.Visibility = Visibility.Hidden;
                            DataGridFilter.Visibility = Visibility.Visible;
                            dp_BeginnFilter.Visibility = Visibility.Visible;
                            dp_EndeFilter.Visibility = Visibility.Visible;
                            lb_von.Visibility = Visibility.Visible;
                            lb_bis.Visibility = Visibility.Visible;
                            tb_Filter.Clear();
                            dp_BeginnFilter.SelectedDate = null;
                            dp_EndeFilter.SelectedDate = null;

                            fuelleDataGridFilter();
                            filtereDataGridFilter();
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("In die Terminart \"Charter\" kann hier nicht gewechselt werden.\n\nZum Speichern bitte die Auswahl ändern!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tb_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NeuerTermin == "0")
            {
                filtereDataGridFilter();
            }
            else
            {
                filtereDataGridAuswahl();
            }
        }

        private void tb_Filter_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_Filter.Clear();
        }

        private void dp_BeginnEndeFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeuerTermin == "0")
            {
                filtereDataGridFilter();
            }
            else
            {
                filtereDataGridAuswahl();
            }
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

        private void bt_NeuerTermin_Click(object sender, RoutedEventArgs e)
        {
            NeuerTermin = "1";
            DataGridAuswahl.Visibility = Visibility.Visible;
            DataGridFilter.Visibility = Visibility.Hidden;
            dp_BeginnFilter.Visibility = Visibility.Hidden;
            dp_EndeFilter.Visibility = Visibility.Hidden;
            lb_von.Visibility = Visibility.Hidden;
            lb_bis.Visibility = Visibility.Hidden;
            tb_Filter.Clear();

            fuelleDataGridAuswahl();
            filtereDataGridAuswahl();
        }

        private void fuelleDataGridAuswahl()
        {
            datatableAuswahl.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Flugzeuge laden
            string SQLcmd = "SELECT flugzeug.flz_id as auswahl_id, 'f' as f_p, ftyp_bez + ' - ' + flz_kennzeichen as auswahl_txt from flugzeugtyp, flugzeug where flugzeug.ftyp_id = flugzeugtyp.ftyp_id";
            SqlCommand cmd_f = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_f = new SqlDataAdapter(cmd_f);
            adapter_f.Fill(datatableAuswahl);

            //Personal laden
            SQLcmd = "SELECT personal.per_id as auswahl_id, 'p' as f_p, per_name + ', ' + per_vorname + '(' + pos_bez + ')' as auswahl_txt from position, personal where personal.pos_id = position.pos_id";
            SqlCommand cmd_p = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter_p = new SqlDataAdapter(cmd_p);
            adapter_p.Fill(datatableAuswahl);

            DataGridAuswahl.ItemsSource = datatableAuswahl.DefaultView;

            cb_Terminart.IsEnabled = false;
            dp_BeginnAuswahl.IsEnabled = false;
            dp_EndeAuswahl.IsEnabled = false;
            bt_Speichern.IsEnabled = false;
            bt_Abbrechen.IsEnabled = false;
            bt_Loeschen.IsEnabled = false;
            lb_Termin.Content = null;
        }

        private void DataGridAuswahl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAuswahl.SelectedCells.Count != 0 && DataGridAuswahl.ItemsSource != null && DataGridAuswahl.SelectedItem != null)
            {
                DataRowView row = DataGridAuswahl.SelectedItems as DataRowView;
                FlugzeugPersonalID = ((DataRowView)DataGridAuswahl.SelectedItem).Row["auswahl_id"].ToString();
                FlugzeugPersonal = ((DataRowView)DataGridAuswahl.SelectedItem).Row["f_p"].ToString();
                lb_Termin.Content = ((DataRowView)DataGridAuswahl.SelectedItem).Row["auswahl_txt"].ToString();

                cb_Terminart.SelectedValue = null;
                dp_BeginnAuswahl.SelectedDate = null;
                dp_EndeAuswahl.SelectedDate = null;

                cb_Terminart.IsEnabled = true;
                dp_BeginnAuswahl.IsEnabled = true;
                dp_EndeAuswahl.IsEnabled = true;
                bt_Speichern.IsEnabled = true;
                bt_Abbrechen.IsEnabled = true;
                bt_Loeschen.IsEnabled = false;
            
                fuelleDataGridInfo();
            }
        }

        private void filtereDataGridAuswahl()
        {
            if (DataGridAuswahl != null)
            {
                string suchbegriff = Convert.ToString(tb_Filter.Text);

                if (suchbegriff != null && suchbegriff != "")
                {
                    var dv = datatableAuswahl.DefaultView;
                    dv.RowFilter = "auswahl_txt LIKE \'%" + suchbegriff + "%\'";
                }                
            }
        }

        private void bt_Abbrechen_Click(object sender, RoutedEventArgs e)
        {
            NeuerTermin = "0";
            DataGridAuswahl.Visibility = Visibility.Hidden;
            DataGridFilter.Visibility = Visibility.Visible;
            dp_BeginnFilter.Visibility = Visibility.Visible;
            dp_EndeFilter.Visibility = Visibility.Visible;
            lb_von.Visibility = Visibility.Visible;
            lb_bis.Visibility = Visibility.Visible;
            tb_Filter.Clear();
            dp_BeginnFilter.SelectedDate = null;
            dp_EndeFilter.SelectedDate = null;

            fuelleDataGridFilter();
            filtereDataGridFilter();
        }

        private void bt_Loeschen_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Soll der Termin wirklich gelöscht werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                SqlConnection conn = new SqlConnection(DBconnStrg);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "DELETE FROM termin where ter_id = " + ter_id;
                cmd1.CommandType = CommandType.Text;

                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn;
                string SQLcmd2 = "";
                if (FlugzeugPersonal == "f")
                {
                    SQLcmd2 = "DELETE FROM termin_flugzeug WHERE ter_id = " + ter_id + " and flz_id = " + FlugzeugPersonalID;
                }
                else
                {
                    SQLcmd2 = "DELETE FROM termin_personal WHERE ter_id = " + ter_id + " and per_id = " + FlugzeugPersonalID;
                }
                cmd2.CommandText = SQLcmd2;
                cmd2.CommandType = CommandType.Text;

                try
                {
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    NeuerTermin = "0";
                    DataGridAuswahl.Visibility = Visibility.Hidden;
                    DataGridFilter.Visibility = Visibility.Visible;
                    dp_BeginnFilter.Visibility = Visibility.Visible;
                    dp_EndeFilter.Visibility = Visibility.Visible;
                    lb_von.Visibility = Visibility.Visible;
                    lb_bis.Visibility = Visibility.Visible;
                    tb_Filter.Clear();
                    dp_BeginnFilter.SelectedDate = null;
                    dp_EndeFilter.SelectedDate = null;

                    fuelleDataGridFilter();
                    filtereDataGridFilter();
                    DataGridInfo.ItemsSource = null;
                }
                catch (System.Exception err)
                {
                    MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                conn.Close();
            }
        }
    }
}
