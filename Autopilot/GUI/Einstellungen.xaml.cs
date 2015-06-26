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

namespace Autopilot.Gui
{
    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Page
    {
        public Einstellungen()
        {
            InitializeComponent();
        }

        private void bt_SQLdo_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("SQL-Anweisung wirklich ausführen?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                string SQLcmd = Convert.ToString(tb_SQLcmd.Text);

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
                    MessageBox.Show("SQL-Anweisung erfolgreich ausgeführt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
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
