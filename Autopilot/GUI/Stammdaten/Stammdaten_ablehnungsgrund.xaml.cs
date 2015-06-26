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
    /// Interaktionslogik für Stammdaten-ablehnungsgrund.xaml
    /// </summary>
    public partial class Stammdaten_ablehnungsgrund : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        bool isInsertMode = false;
        bool isBeingEdited = false;

        public Stammdaten_ablehnungsgrund()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();            
        }

        private ObservableCollection<Ablehnungsgrund> GetList()
        {
            var list = from e in content.ablehnungsgrund select e;
            return new ObservableCollection<Ablehnungsgrund>(list);             
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Ablehnungsgrund ablehnungsgrund = new Ablehnungsgrund();
            Ablehnungsgrund data = e.Row.DataContext as Ablehnungsgrund;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + data.Bezeichnung + " als neuen Ablehnungsgrund zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    ablehnungsgrund.Bezeichnung = data.Bezeichnung;
                    ablehnungsgrund.Id = data.Id;
                    content.ablehnungsgrund.Add(ablehnungsgrund);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show(data.Bezeichnung + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Ablehnungsgründe löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            Ablehnungsgrund ablehnungsgrund = row as Ablehnungsgrund;
                            content.ablehnungsgrund.Remove(ablehnungsgrund);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Ablehnungsgründe wurden gelöscht!");
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
