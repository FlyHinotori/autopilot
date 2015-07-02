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
    /// Interaktionslogik für Stammdaten-personal.xaml
    /// </summary>
    public partial class Stammdaten_personal : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        int per_id;
        int pos_id;
        int sta_id;
        string PfadPersonalBild;
        
        public Stammdaten_personal()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();
            cb_Position.ItemsSource = FillPosition();
            cb_Status.ItemsSource = FillStatus();
            bt_Speichern.IsEnabled = false;
            bt_BildUpload.IsEnabled = false;
            bt_FileDialog.IsEnabled = false;
        }

        private ObservableCollection<personalliste> GetList()
        {
            var list = from e in content.personalliste select e;
            return new ObservableCollection<personalliste>(list);
        }

        private ObservableCollection<position> FillPosition()
        {
            var list = from e in content.position select e;
            return new ObservableCollection<position>(list);
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
                var ID = content.personal.SingleOrDefault(c => c.per_id == per_id);
                ID.per_name = tb_Name.Text.ToString();
                ID.per_vorname = tb_Vorname.Text.ToString();
                ID.sta_id = Convert.ToInt32(cb_Status.SelectedValue.ToString());
                ID.pos_id = Convert.ToInt32(cb_Position.SelectedValue.ToString());

                content.SaveChanges();
                MessageBox.Show("Update des DataGrid-Updates noch nicht gebaut.");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count != 0 && DataGrid.ItemsSource != null)
            {
                DataRowView row = DataGrid.SelectedItems as DataRowView;
                per_id = ((personalliste)DataGrid.SelectedItem).per_id;
                pos_id = ((personalliste)DataGrid.SelectedItem).pos_id;
                sta_id = ((personalliste)DataGrid.SelectedItem).sta_id;

                var ID = content.personal.SingleOrDefault(c => c.per_id == per_id);
                tb_Name.Text = Convert.ToString(ID.per_name);
                tb_Vorname.Text = Convert.ToString(ID.per_vorname);

                if (ID.per_bild != null)
                {
                    Stream StreamObj = new MemoryStream(ID.per_bild);
                    BitmapImage BitObj = new BitmapImage();
                    BitObj.BeginInit();
                    BitObj.StreamSource = StreamObj;
                    BitObj.EndInit();
                    this.img_Personal.Source = BitObj;
                }
                else
                {
                    img_Personal.Source = null;
                }

                cb_Position.SelectedValue = Convert.ToString(pos_id);
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
            PfadPersonalBild = openFileDialog.FileName;
            ImageSource imageSource = new BitmapImage(new Uri(PfadPersonalBild));
            img_Personal.Source = imageSource;
        }    
   
        private void bt_BildUpload_Click(object sender, RoutedEventArgs e)
        {
            if (PfadPersonalBild == null || PfadPersonalBild == "")
            {
                MessageBox.Show("Bitte vorher ein Bild auswählen!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ImageClass images = new ImageClass();
                images.ImagePath = PfadPersonalBild;
                images.ImageToByte = File.ReadAllBytes(PfadPersonalBild);

                var ID = content.personal.SingleOrDefault(c => c.per_id == per_id);
                ID.per_bild = images.ImageToByte;

                content.SaveChanges();
            }

            PfadPersonalBild = "";
        }

    }
}
