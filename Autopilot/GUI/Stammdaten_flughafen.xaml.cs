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
        bool isInsertMode = false;
        bool isBeingEdited = false;

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

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            flughafen flughafen = new flughafen();
            flughafen data = e.Row.DataContext as flughafen;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + data.flh_name + " als neuen Flughafen zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    flughafen.flh_altitude = flughafen.flh_altitude;
                    flughafen.flh_dst = data.flh_dst;
                    flughafen.flh_iatacode = data.flh_iatacode;
                    flughafen.flh_icaocode = data.flh_icaocode;
                    flughafen.flh_id = data.flh_id;
                    flughafen.flh_land = data.flh_land;
                    flughafen.flh_latitude = data.flh_latitude;
                    flughafen.flh_longitude = data.flh_longitude;
                    flughafen.flh_name = data.flh_name;
                    flughafen.flh_stadt = data.flh_stadt;
                    flughafen.flh_zeitzone = data.flh_zeitzone;
                    flughafen.flh_zeitzone_base = data.flh_zeitzone_base;
                    content.flughafen.Add(flughafen);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show(data.flh_name + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    DataGrid.ItemsSource = GetList();
            }

            content.SaveChanges();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && !isBeingEdited)
            {
                var grid = (DataGrid)sender;
                if (grid.SelectedItems.Count > 0)
                {
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Flughafen löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            flughafen flughafen = row as flughafen;
                            content.flughafen.Remove(flughafen);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Flughafen wurde gelöscht!");
                    }
                    else
                        DataGrid.ItemsSource = GetList();
                }
            }
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funktion wurde deaktiviert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            /// <summary>
            /// Funktion des Import vorerst deaktiviert
            /// </summary>
            /*
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
            */
        }
    }
}
