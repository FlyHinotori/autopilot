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
    /// Interaktionslogik für Stammdaten-kundengruppe.xaml
    /// </summary>
    public partial class Stammdaten_kundengruppe : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        bool isInsertMode = false;
        bool isBeingEdited = false;

        public Stammdaten_kundengruppe()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();            
        }

        private ObservableCollection<kundengruppe> GetList()
        {
            var list = from e in content.kundengruppe select e;
            return new ObservableCollection<kundengruppe>(list);             
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            kundengruppe kundengruppe = new kundengruppe();
            kundengruppe data = e.Row.DataContext as kundengruppe;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + data.kng_bez + " als neue Kundengruppe zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    kundengruppe.kng_bez = data.kng_bez;
                    kundengruppe.kng_id = data.kng_id;
                    content.kundengruppe.Add(kundengruppe);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show(data.kng_bez + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Kundengruppen löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            kundengruppe kundengruppe = row as kundengruppe;
                            content.kundengruppe.Remove(kundengruppe);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Kundengruppen wurden gelöscht!");
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
