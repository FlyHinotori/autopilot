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
    /// Interaktionslogik für TerminNeu.xaml
    /// </summary>
    public partial class TerminNeu : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable datatableInfo = new DataTable("Termine");
        DataTable datatableRessource = new DataTable("Ressource");
        DataTable datatableTerminart = new DataTable("Terminart");
        Int32 ter_id;
        string FlugzeugPersonal;
        string FlugzeugPersonalID;

        public TerminNeu()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleRessource();
            fuelleTerminart();
        }

        private void fuelleRessource()
        {
            datatableRessource.Clear();
            cb_Ressource.ItemsSource = null;
            cb_Ressource.Items.Clear();

            SqlConnection conn = new SqlConnection(DBconnStrg);

            string SQLcmd_txt = "SELECT \'f\' + convert(char,flugzeug.flz_id) as id, ftyp_bez + ' - ' + flz_kennzeichen as txt from flugzeugtyp, flugzeug where flugzeug.ftyp_id = flugzeugtyp.ftyp_id";
            SqlDataAdapter adapter_f = new SqlDataAdapter(SQLcmd_txt, conn);
            adapter_f.Fill(datatableRessource);

            SQLcmd_txt = "SELECT \'p\' + convert(char,personal.per_id) as id, per_name + ', ' + per_vorname + '(' + pos_bez + ')' as txt from position, personal where personal.pos_id = position.pos_id";
            SqlDataAdapter adapter_p = new SqlDataAdapter(SQLcmd_txt, conn);
            adapter_p.Fill(datatableRessource);

            cb_Ressource.ItemsSource = datatableRessource.DefaultView;
            
            cb_Ressource.SelectedIndex = -1;
        }
        
        private void fuelleTerminart()
        {
            datatableTerminart.Clear();
            cb_Terminart.ItemsSource = null;
            cb_Terminart.Items.Clear();

            SqlConnection conn = new SqlConnection(DBconnStrg);

            string SQLcmd_txt = "SELECT tart_id as id, tart_bez as txt from terminart where tart_bez not like \'Charter\'";
            SqlDataAdapter adapter = new SqlDataAdapter(SQLcmd_txt, conn);
            adapter.Fill(datatableTerminart);

            cb_Terminart.ItemsSource = datatableTerminart.DefaultView;

            cb_Terminart.SelectedIndex = -1;
        }

        private void fuelleDataGridInfo()
        {
            datatableInfo.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);
            string SQLcmd = "";

            if (FlugzeugPersonal == "f")
            {
                if (dp_BeginnAuswahl.SelectedDate != null && dp_EndeAuswahl.SelectedDate != null)
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
                SQLcmd = "SELECT tart_bez, ter_beginn, ter_ende from termin, terminart, termin_personal, personal where termin.tart_id = terminart.tart_id and termin.ter_id = termin_personal.ter_id and termin_personal.per_id = personal.per_id and personal.per_id = " + FlugzeugPersonalID;
                
                SqlCommand cmd_p = new SqlCommand(SQLcmd, conn);
                SqlDataAdapter adapter_p = new SqlDataAdapter(cmd_p);
                adapter_p.Fill(datatableInfo);
            }

            DataGridInfo.ItemsSource = datatableInfo.DefaultView;
        }

        private void cb_Ressource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlugzeugPersonalID = cb_Ressource.SelectedValue.ToString();  
          
            if(FlugzeugPersonalID.Contains("f"))
            {
                //Flugzeug
                FlugzeugPersonalID = FlugzeugPersonalID.Replace("f", "");
                FlugzeugPersonalID = FlugzeugPersonalID.Trim();
                FlugzeugPersonal = "f";
            }
            else
            {
                //Personal
                FlugzeugPersonalID = FlugzeugPersonalID.Replace("p", "");
                FlugzeugPersonalID = FlugzeugPersonalID.Trim();
                FlugzeugPersonal = "p";
            }


            fuelleDataGridInfo();
        }

        private void dp_BeginnEndeAuswahl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_BeginnAuswahl.SelectedDate != null && dp_EndeAuswahl.SelectedDate != null)
            {
                fuelleDataGridInfo();
            }
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Terminart.Text != "Charter" && dp_BeginnAuswahl.SelectedDate != null && dp_EndeAuswahl.SelectedDate != null)
            {
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
                    }
                    catch (System.Exception err)
                    {
                        MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conn.Close();
                }

            }
            else
            {
                MessageBox.Show("Bitte prüfen Sie Ihre Eingaben.\n\nZum Speichern bitte die Auswahl ändern!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
