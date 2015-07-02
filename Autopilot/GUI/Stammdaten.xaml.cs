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
    /// Interaktionslogik für Stammdaten.xaml
    /// </summary>
    public partial class Stammdaten : Page
    {
        public Stammdaten()
        {
            InitializeComponent();
        }

        private void Ablehnungsgruende_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_ablehnungsgrund.xaml", UriKind.Relative));
        }

        private void Anreden_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_anrede.xaml", UriKind.Relative));
        }

        private void Auftragsarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_auftragsart.xaml", UriKind.Relative));
        }

        private void Firmendaten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_firmendaten.xaml", UriKind.Relative));
        }

        private void Flughaefen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_flughafen.xaml", UriKind.Relative));
        }

        private void Flugzeuge_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_flugzeug.xaml", UriKind.Relative));
        }

        private void Flugzeugtypen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_flugzeugtyp.xaml", UriKind.Relative));
        }

        private void Hersteller_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_hersteller.xaml", UriKind.Relative));
        }

        private void Kundengruppen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_kundengruppe.xaml", UriKind.Relative));
        }

        private void Mahnstufen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_mahnstufe.xaml", UriKind.Relative));
        }

        private void Personal_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_personal.xaml", UriKind.Relative));            
        }

        private void Positionen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_position.xaml", UriKind.Relative));
        }

        private void Positionsarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_positionsart.xaml", UriKind.Relative));
        }

        private void Status_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_status.xaml", UriKind.Relative));
        }

        private void Statusgruppen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_statusgruppe.xaml", UriKind.Relative));
        }

        private void Terminarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_terminart.xaml", UriKind.Relative));
        }

        private void Titel_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_titel.xaml", UriKind.Relative));
        }

        private void Triebwerksarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Stammdaten_triebwerksart.xaml", UriKind.Relative));
        }
    }
}
