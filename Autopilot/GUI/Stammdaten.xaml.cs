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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Stammdaten.xaml
    /// </summary>
    public partial class Stammdaten : Page
    {
        private AutopilotEntities content = new AutopilotEntities();
        
        public Stammdaten()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource autopilotEntitiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("autopilotEntitiesViewSource")));
            content.flugzeugtyp.Load();
            autopilotEntitiesViewSource.Source = content.flugzeugtyp.Local;
        }
    }
}
