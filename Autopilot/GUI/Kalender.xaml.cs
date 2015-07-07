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
    /// Interaktionslogik für Kalender.xaml
    /// </summary>
    public partial class Kalender : Page
    {
        public Kalender()
        {
            InitializeComponent();
            Inhaltsanzeige.Navigate(new Uri("GUI/TerminUebersicht.xaml", UriKind.Relative));
        }

        private void bt_NeuerTermin_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/TerminNeu.xaml", UriKind.Relative));
        }

        private void bt_Terminuebersicht_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/TerminUebersicht.xaml", UriKind.Relative));
        }
    }
}
