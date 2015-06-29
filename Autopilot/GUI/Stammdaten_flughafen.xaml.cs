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
using System.IO;
using System.Data;
using System.Net;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Stammdaten-flughafen.xaml
    /// </summary>
    public partial class Stammdaten_flughafen : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string urlQuelle = @"https://raw.githubusercontent.com/jpatokal/openflights/master/data/airports.dat";

        public Stammdaten_flughafen()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tb_urlQuelle.Text = urlQuelle;
            DataGrid.ItemsSource = GetList();
        }

        private ObservableCollection<flughafen> GetList()
        {
            var list = from e in content.flughafen select e;
            return new ObservableCollection<flughafen>(list);
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Sollen der Import jetzt durchgeführt werden?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                // CSV downloaden
                WebClient client = new WebClient();
                string DownloadOK = "";
                try
                {
                    client.DownloadFile(new Uri(urlQuelle), "airports.csv");
                    DownloadOK = "1";
                }
                catch
                {
                    DownloadOK = "0";
                }

                if (DownloadOK == "1")
                {
                    //Tabelle leeren
                    string SQLcmd = "DELETE flughafen";

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
                    }
                    catch (System.Exception err)
                    {
                        MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conn.Close();

                    //csv einlesen
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[12]{ 
                    new DataColumn("flh_id", typeof(int)),
                    new DataColumn("flh_name", typeof(string)),
                    new DataColumn("flh_stadt",typeof(string)),
                    new DataColumn("flh_land", typeof(string)),
                    new DataColumn("flh_iatacode", typeof(string)),
                    new DataColumn("flh_icaocode", typeof(string)),
                    new DataColumn("flh_latitude", typeof(string)),
                    new DataColumn("flh_longitude", typeof(string)),
                    new DataColumn("flh_altitude", typeof(string)),
                    new DataColumn("flh_zeitzone", typeof(string)),
                    new DataColumn("flh_dst", typeof(string)),
                    new DataColumn("flh_zeitzone_base", typeof(string)),
                });

                    string csvData = File.ReadAllText("airports.csv");
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (string cell in row.Split(','))
                            {
                                if (i < 12)
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "");
                                    i++;
                                }
                                else
                                {
                                    MessageBox.Show("Achtung, mehr als 12 Spalten in Zeile " + dt.Rows.Count.ToString() + "!\n\n" + row.ToString() + "\n\nZeile wird nicht importiert.", "Importfehler", MessageBoxButton.OK, MessageBoxImage.Error);
                                    dt.Rows[dt.Rows.Count - 1][1] = "löschen";
                                }
                            }
                        }
                    }

                    //Fehlimporte löschen
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr[1] == "löschen")
                            dr.Delete();
                    }

                    //Daten in DB schreiben
                    using (SqlConnection con = new SqlConnection(DBconnStrg))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.DestinationTableName = "flughafen";
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }

                    //CSV löschen
                    File.Delete("airports.csv");

                    MessageBox.Show("Import abgeschlossen!\n\nAktualisieren der Ansicht noch nicht programmiert.");
                }
            }
        }
    }
}
