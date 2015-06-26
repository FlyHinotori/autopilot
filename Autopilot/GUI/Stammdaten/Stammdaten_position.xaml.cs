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

namespace Autopilot.Gui
{
    /// <summary>
    /// Interaktionslogik für Stammdaten-position.xaml
    /// </summary>
    public partial class Stammdaten_position : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        bool isInsertMode = false;
        bool isBeingEdited = false;

        public Stammdaten_position()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = GetList();
        }

        private ObservableCollection<position> GetList()
        {
            var list = from e in content.position select e;
            return new ObservableCollection<position>(list);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            position position = new position();
            position data = e.Row.DataContext as position;
            if (isInsertMode)
            {
                var InsertRecord = MessageBox.Show("Möchten Sie " + data.pos_bez + " als neue Position zufügen?", "Bestätigen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    position.pos_bez = data.pos_bez;
                    position.pos_id = data.pos_id;
                    position.pos_gehalt_pa = data.pos_gehalt_pa;
                    position.part_id = data.part_id;
                    content.position.Add(position);
                    content.SaveChanges();
                    DataGrid.ItemsSource = GetList();
                    MessageBox.Show(data.pos_bez + " wurde zugefügt!", "Eintrag gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    var Res = MessageBox.Show("Möchten Sie wirklich " + grid.SelectedItems.Count + " Position(en) löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            position position = row as position;
                            content.position.Remove(position);
                        }
                        content.SaveChanges();
                        MessageBox.Show(grid.SelectedItems.Count + " Position(en) wurden gelöscht!");
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
