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
using Microsoft.Win32;
using System.IO;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Stammdaten-flugzeug.xaml
    /// </summary>
    public partial class Stammdaten_flugzeug : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        int flz_id;
        int ftyp_id;
        int sta_id;
        string PfadFlugzeugBild;
        
        public Stammdaten_flugzeug()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();
            cb_Flugzeugtyp.ItemsSource = FillFTyp();
            cb_Status.ItemsSource = FillStatus();
            bt_Speichern.IsEnabled = false;
            bt_BildUpload.IsEnabled = false;
            bt_FileDialog.IsEnabled = false;
        }

        private ObservableCollection<flugzeugliste> GetList()
        {
            var list = from e in content.flugzeugliste select e;
            return new ObservableCollection<flugzeugliste>(list);
        }

        private ObservableCollection<flugzeugtyp> FillFTyp()
        {
            var list = from e in content.flugzeugtyp select e;
            return new ObservableCollection<flugzeugtyp>(list);
        }

        private ObservableCollection<status> FillStatus()
        {
            var list = from e in content.status select e;
            return new ObservableCollection<status>(list);
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {            
            var res = MessageBox.Show("Sollen die Änderungen gespeichert werden?","Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                var ID = content.flugzeug.SingleOrDefault(c => c.flz_id == flz_id);
                ID.flz_kennzeichen = tb_Kennzeichen.Text.ToString();
                ID.sta_id = Convert.ToInt32(cb_Status.SelectedValue.ToString());
                ID.ftyp_id = Convert.ToInt32(cb_Flugzeugtyp.SelectedValue.ToString());

                content.SaveChanges();

                content = new AutopilotEntities();
                DataGrid.ItemsSource = GetList();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count != 0 && DataGrid.ItemsSource != null && DataGrid.CurrentItem != null)
            {
                DataRowView row = DataGrid.SelectedItems as DataRowView;
                flz_id = ((flugzeugliste)DataGrid.SelectedItem).flz_id;
                ftyp_id = ((flugzeugliste)DataGrid.SelectedItem).ftyp_id;
                sta_id = ((flugzeugliste)DataGrid.SelectedItem).sta_id;

                var ID = content.flugzeug.SingleOrDefault(c => c.flz_id == flz_id);
                tb_Kennzeichen.Text = Convert.ToString(ID.flz_kennzeichen);

                if (ID.flz_bild != null)
                {
                    Stream StreamObj = new MemoryStream(ID.flz_bild);
                    BitmapImage BitObj = new BitmapImage();
                    BitObj.BeginInit();
                    BitObj.StreamSource = StreamObj;
                    BitObj.EndInit();
                    this.img_Flugzeug.Source = BitObj;
                }
                else
                {
                    img_Flugzeug.Source = null;
                }

                cb_Flugzeugtyp.SelectedValue = Convert.ToString(ftyp_id);
                cb_Status.SelectedValue = Convert.ToString(sta_id);
                bt_Speichern.IsEnabled = true;
                bt_FileDialog.IsEnabled = true;
                bt_BildUpload.IsEnabled = true;
            }
        }

        public class ImageClass
        {
            public int Id { get; set; }
            public string ImagePath { get; set; }
            public byte[] ImageToByte { get; set; }
        }

        private void bt_FileDialog_Click(object sender, RoutedEventArgs e)
        {
            ImageClass images = new ImageClass();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            //openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files(*.png)|*.png|JPG";
            //openFileDialog.DefaultExt = ".jpeg";
            PfadFlugzeugBild = openFileDialog.FileName;
            ImageSource imageSource = new BitmapImage(new Uri(PfadFlugzeugBild));
            img_Flugzeug.Source = imageSource;
        }    
   
        private void bt_BildUpload_Click(object sender, RoutedEventArgs e)
        {
            if (PfadFlugzeugBild == null || PfadFlugzeugBild == "")
            {
                MessageBox.Show("Bitte vorher ein Bild auswählen!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ImageClass images = new ImageClass();
                images.ImagePath = PfadFlugzeugBild;
                images.ImageToByte = File.ReadAllBytes(PfadFlugzeugBild);

                var ID = content.flugzeug.SingleOrDefault(c => c.flz_id == flz_id);
                ID.flz_bild = images.ImageToByte;

                content.SaveChanges();
            }

            PfadFlugzeugBild = "";
        }

        private void bt_NeuesFlz_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Soll ein neues Flugzeug angelegt werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                SqlConnection conn = new SqlConnection(DBconnStrg);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO flugzeug (sta_id, ftyp_id,flz_kennzeichen) VALUES (40, 1, \'<<Dummy>>\')";
                cmd.CommandType = CommandType.Text;

                try
                {
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Ein Flugzeug mit dem Kennzeichen <<Dummy>> wurde angelegt und kann nun bearbeitet werden.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                    DataGrid.ItemsSource = GetList();
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
