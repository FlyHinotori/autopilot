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
            flugzeugtypDataGrid.ItemsSource = GetList();            
        }

        private ObservableCollection<flugzeugtyp> GetList()
        {            
            var list = from e in content.flugzeugtyp select e;
            return new ObservableCollection<flugzeugtyp>(list);             
        }

        private void flugzeugtypDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            flugzeugtyp flugzeugtyp = new flugzeugtyp();
            flugzeugtyp flzt = e.Row.DataContext as flugzeugtyp;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + flzt.ftyp_bez + " als neuen Flugzeugtyp zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    flugzeugtyp.ftyp_anz_ccrew = flzt.ftyp_anz_ccrew;
                    flugzeugtyp.ftyp_anz_fcrew = flzt.ftyp_anz_fcrew;
                    flugzeugtyp.ftyp_anz_pass = flzt.ftyp_anz_pass;
                    flugzeugtyp.ftyp_anz_triebwerke = flzt.ftyp_anz_triebwerke;
                    flugzeugtyp.ftyp_bez = flzt.ftyp_bez;
                    flugzeugtyp.ftyp_fkosten_pa = flzt.ftyp_fkosten_pa;
                    flugzeugtyp.ftyp_id = flzt.ftyp_id;
                    flugzeugtyp.ftyp_reichweite_km = flzt.ftyp_reichweite_km;
                    flugzeugtyp.ftyp_speed = flzt.ftyp_speed;
                    flugzeugtyp.ftyp_vkosten_ph = flzt.ftyp_vkosten_ph;
                    flugzeugtyp.her_id = flzt.her_id;
                    flugzeugtyp.twa_id = flzt.twa_id;
                    content.flugzeugtyp.Add(flugzeugtyp);
                    content.SaveChanges();
                    flugzeugtypDataGrid.ItemsSource = GetList();
                    MessageBox.Show(flzt.ftyp_bez + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    flugzeugtypDataGrid.ItemsSource = GetList();
            }

            content.SaveChanges();
        }

        private void flugzeugtypDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
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
                        flugzeugtypDataGrid.ItemsSource = GetList();
                }
            }
        }

        private void flugzeugtypDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }

        private void flugzeugtypDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

    }
}
