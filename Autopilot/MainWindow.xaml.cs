using System;
using System.Windows;

namespace Autopilot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AutopilotEntities Inhalt = new AutopilotEntities();
        
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Rechnungen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Rechnungen.xaml", UriKind.Relative));
        }

        private void Aufträge_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Aufträge.xaml", UriKind.Relative));
        }

        private void Kalender_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Kalender.xaml", UriKind.Relative));
        }

        private void Einstellungen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Einstellungen.xaml", UriKind.Relative));
        }

        private void Hilfe_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Hilfe.xaml", UriKind.Relative));
        }

        private void Neu_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Neu.xaml", UriKind.Relative));
        }

        private void Stammdaten_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten.xaml", UriKind.Relative));
        }

        private void Kunden_anzeigen(object sender, RoutedEventArgs e)
        {
            //Inhaltsanzeige.Navigate(new Uri("Gui/Stammdaten_kunde.xaml", UriKind.Relative));
            MessageBox.Show("\nLeider noch ohne Funktion!\n");
        }
    }
}
