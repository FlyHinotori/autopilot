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
    /// Interaktionslogik für Stammdaten-flugzeug.xaml
    /// </summary>
    public partial class Stammdaten_flugzeug : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        int flz_id;
        int ftyp_id;
        int sta_id;
        
        public Stammdaten_flugzeug()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();
            cb_Flugzeugtyp.ItemsSource = FillFTyp();
            cb_Status.ItemsSource = FillStatus();
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
                MessageBox.Show("Update des DataGrid-Updates noch nicht gebaut.");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count != 0 && DataGrid.ItemsSource != null)
            {
                DataRowView row = DataGrid.SelectedItems as DataRowView;
                flz_id = ((flugzeugliste)DataGrid.SelectedItem).flz_id;
                ftyp_id = ((flugzeugliste)DataGrid.SelectedItem).ftyp_id;
                sta_id = ((flugzeugliste)DataGrid.SelectedItem).sta_id;

                var ID = content.flugzeug.SingleOrDefault(c => c.flz_id == flz_id);
                tb_Kennzeichen.Text = Convert.ToString(ID.flz_kennzeichen);

                cb_Flugzeugtyp.SelectedValue = Convert.ToString(ftyp_id);
                cb_Status.SelectedValue = Convert.ToString(sta_id);
            }
        }       

    }
}
