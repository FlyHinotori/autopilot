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
            //System.Windows.Data.CollectionViewSource autopilotEntitiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("autopilotEntitiesViewSource")));
            //content.flugzeugtyp.Load();
            //autopilotEntitiesViewSource.Source = content.flugzeugtyp.Local;
            
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
    }
}
