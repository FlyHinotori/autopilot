using System;
using System.Windows;

namespace Autopilot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Einbinden der Utilities-Klasse
        /// </summary>
        Utilities utilities = new Utilities();
        /// <summary>
        /// Beispiel für das Ausführen einer SQL-Anweisung
        /// utilities.SQLCmd("SQL-Anweisung");
        /// 
        /// Beispiele für das Aufrufen von Messageboxen  
        /// utilities.HinweisMsgBox("Hier kommt der Hinweistext rein", "Titel der Box");  
        /// utilities.FehlerMsgBox("Hier kommt der Fehlertext rein", "Titel der Box");  
        /// </summary>
        
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
