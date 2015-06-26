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
    /// Interaktionslogik für Stammdaten-mahnstufe.xaml
    /// </summary>
    public partial class Stammdaten_mahnstufe : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        bool isInsertMode = false;
        bool isBeingEdited = false;

        public Stammdaten_mahnstufe()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();
        }

        private ObservableCollection<mahnstufe> GetList()
        {
            var list = from e in content.mahnstufe select e;
            return new ObservableCollection<mahnstufe>(list);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            mahnstufe mahnstufe = new mahnstufe();
            mahnstufe data = e.Row.DataContext as mahnstufe;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie die neue Mahnstufe zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    mahnstufe.mst_id = data.mst_id;
                    mahnstufe.mst_reihenfolge = data.mst_reihenfolge;
                    mahnstufe.mst_tage = data.mst_tage;
                    mahnstufe.mst_zuschlag = data.mst_zuschlag;
                    content.mahnstufe.Add(mahnstufe);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show("Eine neue Mahnstufe wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Mahnstufe(n) löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            mahnstufe mahnstufe = row as mahnstufe;
                            content.mahnstufe.Remove(mahnstufe);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Mahnstufe(n) wurden gelöscht!");
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
