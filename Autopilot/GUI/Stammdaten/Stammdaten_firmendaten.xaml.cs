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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Stammdaten-firmendaten.xaml
    /// </summary>
    public partial class Stammdaten_firmendaten : Page
    {
        AutopilotEntities content = new AutopilotEntities();

        public Stammdaten_firmendaten()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Textboxen_fuellen();
        }

        private void Textboxen_fuellen()
        {
            string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;

            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT fir_name, fir_strasse, fir_ort, fir_land FROM firmenstammdaten";
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader dr_pwd = cmd.ExecuteReader();
            if (dr_pwd.Read())
            {
                tb_Firmenname.Text = Convert.ToString(dr_pwd.GetValue(0));
                tb_Strasse.Text = Convert.ToString(dr_pwd.GetValue(1));
                tb_Ort.Text = Convert.ToString(dr_pwd.GetValue(2));
                tb_Land.Text = Convert.ToString(dr_pwd.GetValue(3));
            }

            conn.Close();
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Sollen die Änderungen gespeichert werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                string SQLcmd = "UPDATE firmenstammdaten SET fir_name = \'" + Convert.ToString(tb_Firmenname.Text) + "\', fir_strasse = \'" + Convert.ToString(tb_Strasse.Text) + "\', fir_ort = \'" + Convert.ToString(tb_Ort.Text) + "\', fir_land = \'" + Convert.ToString(tb_Land.Text) + "\'";

                string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;

                SqlConnection conn = new SqlConnection(DBconnStrg);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = SQLcmd;
                cmd.CommandType = System.Data.CommandType.Text;

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Daten erfolgreich gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
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
