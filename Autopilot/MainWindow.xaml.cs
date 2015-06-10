using System;
using System.Windows;

namespace Autopilot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Rechnungen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Rechnungen.xaml", UriKind.Relative));
        }

        private void Aufträge_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Aufträge.xaml", UriKind.Relative));
        }

        private void Kalender_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Kalender.xaml", UriKind.Relative));
        }

        private void Einstellungen_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Einstellungen.xaml", UriKind.Relative));
        }

        private void Hilfe_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Hilfe.xaml", UriKind.Relative));
        }

        private void Neu_anzeigen(object sender, RoutedEventArgs e)
        {
            Inhaltsanzeige.Navigate(new Uri("GUI/Neu.xaml", UriKind.Relative));
        }
    }
}
