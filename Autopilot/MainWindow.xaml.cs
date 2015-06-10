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
    }
}
