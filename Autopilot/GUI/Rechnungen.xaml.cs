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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Rechnungen.xaml
    /// </summary>
    public partial class Rechnungen : Page
    {
        public Rechnungen()
        {
            InitializeComponent();
            Inhaltsanzeige.Navigate(new Uri("GUI/Rechnungen_Uebersicht.xaml", UriKind.Relative));
        }

        private void bt_UEbersicht_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Rechnungen_Uebersicht.xaml", UriKind.Relative));
        }

        private void bt_Mahnlauf_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Rechnungen_Mahnlauf.xaml", UriKind.Relative));
        }
    }
}
