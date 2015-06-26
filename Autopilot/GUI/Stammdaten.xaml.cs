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

namespace Autopilot.Gui
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
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_ablehnungsgrund.xaml", UriKind.Relative));
        }

        private void Anreden_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_anrede.xaml", UriKind.Relative));
        }

        private void Auftragsarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_auftragsart.xaml", UriKind.Relative));
        }

        private void Firmendaten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_firmendaten.xaml", UriKind.Relative));
        }

        private void Flughaefen_anzeigen(object sender, RoutedEventArgs e)
        {
            //Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten_flughafen.xaml", UriKind.Relative));
            MessageBox.Show("\nLeider noch ohne Funktion!\n");
        }

        private void Flugzeuge_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_flugzeug.xaml", UriKind.Relative));
        }

        private void Flugzeugtypen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_flugzeugtyp.xaml", UriKind.Relative));
        }

        private void Hersteller_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_hersteller.xaml", UriKind.Relative));
        }

        private void Kundengruppen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_kundengruppe.xaml", UriKind.Relative));
        }

        private void Mahnstufen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_mahnstufe.xaml", UriKind.Relative));
        }

        private void Personal_anzeigen(object sender, RoutedEventArgs e)
        {
            //Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten_personal.xaml", UriKind.Relative));
            MessageBox.Show("\nLeider noch ohne Funktion!\n");
        }

        private void Positionen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_position.xaml", UriKind.Relative));
        }

        private void Positionsarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_positionsart.xaml", UriKind.Relative));
        }

        private void Status_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_status.xaml", UriKind.Relative));
        }

        private void Statusgruppen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_statusgruppe.xaml", UriKind.Relative));
        }

        private void Terminarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_terminart.xaml", UriKind.Relative));
        }

        private void Titel_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_titel.xaml", UriKind.Relative));
        }

        private void Triebwerksarten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten/Stammdaten_triebwerksart.xaml", UriKind.Relative));
        }
    }
}
