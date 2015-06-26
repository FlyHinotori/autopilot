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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Stammdaten-flugzeugtyp.xaml
    /// </summary>
    public partial class Stammdaten_flugzeugtyp : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        bool isInsertMode = false;
        bool isBeingEdited = false;
                        
        public Stammdaten_flugzeugtyp()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();            
        }

        private ObservableCollection<flugzeugtyp> GetList()
        {            
            var list = from e in content.flugzeugtyp select e;
            return new ObservableCollection<flugzeugtyp>(list);             
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            flugzeugtyp flugzeugtyp = new flugzeugtyp();
            flugzeugtyp data = e.Row.DataContext as flugzeugtyp;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + data.ftyp_bez + " als neuen Flugzeugtyp zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    flugzeugtyp.ftyp_anz_ccrew = data.ftyp_anz_ccrew;
                    flugzeugtyp.ftyp_anz_fcrew = data.ftyp_anz_fcrew;
                    flugzeugtyp.ftyp_anz_pass = data.ftyp_anz_pass;
                    flugzeugtyp.ftyp_anz_triebwerke = data.ftyp_anz_triebwerke;
                    flugzeugtyp.ftyp_bez = data.ftyp_bez;
                    flugzeugtyp.ftyp_fkosten_pa = data.ftyp_fkosten_pa;
                    flugzeugtyp.ftyp_id = data.ftyp_id;
                    flugzeugtyp.ftyp_reichweite_km = data.ftyp_reichweite_km;
                    flugzeugtyp.ftyp_speed = data.ftyp_speed;
                    flugzeugtyp.ftyp_vkosten_ph = data.ftyp_vkosten_ph;
                    flugzeugtyp.her_id = data.her_id;
                    flugzeugtyp.twa_id = data.twa_id;
                    content.flugzeugtyp.Add(flugzeugtyp);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show(data.ftyp_bez + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Flugzeugtyp(en) löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            flugzeugtyp flugzeugtyp = row as flugzeugtyp;
                            content.flugzeugtyp.Remove(flugzeugtyp);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Flugzeugtyp(en) wurden gelöscht!");
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

    }
}
