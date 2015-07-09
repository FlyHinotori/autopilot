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
    /// Interaktionslogik für Kunden.xaml
    /// </summary>
    public partial class Kunden : Page
    {
        public Kunden()
        {
            InitializeComponent();
            Inhaltsanzeige.Navigate(new Uri("GUI/KundenUebersicht.xaml", UriKind.Relative));
        }

        private void bt_NeuerKunde_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/KundenNeu.xaml", UriKind.Relative));
        }

        private void bt_Kundenuebersicht_Click(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/KundenUebersicht.xaml", UriKind.Relative));
        }
    }
}
