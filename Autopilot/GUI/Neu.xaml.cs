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
using Autopilot.Models;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Neu.xaml
    /// </summary>
    public partial class Neu : Page
    {
        private AuftragModel FAuftrag;
        public Neu()
        {
            InitializeComponent();
            FAuftrag = new AuftragModel();
            TabsNeuerAuftrag.DataContext = FAuftrag;
        }

        private void AuftragSpeichern()
        {
            using(new WaitCursor())
            {
                FAuftrag.Save();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.TabsNeuerAuftrag.SelectedItem = TabRoute;
        }

        private void btnZurueck_Click(object sender, RoutedEventArgs e)
        {
            AuftragSpeichern(); // <- debug    
        }

    }
}
