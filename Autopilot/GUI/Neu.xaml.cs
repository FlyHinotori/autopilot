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
using System.Collections.ObjectModel;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Neu.xaml
    /// </summary>
    public partial class Neu : Page
    {
        AutopilotEntities FContent;
        private AuftragModel FAuftrag;
        public Neu()
        {
            InitializeComponent();
            FContent = new AutopilotEntities();
            FAuftrag = new AuftragModel();
            TabsNeuerAuftrag.DataContext = FAuftrag;
            CBGruppe.ItemsSource = FillKundengruppe();
            CBAnrede.ItemsSource = FillAnrede();
            CBTitel.ItemsSource = FillTitel();
            CBAuftragsArt.ItemsSource = FillAuftragsart();
        }

        #region context list fillers
        private ObservableCollection<kundengruppe> FillKundengruppe()
        {
            var list = from e in FContent.kundengruppe select e;
            return new ObservableCollection<kundengruppe>(list);
        }
        private ObservableCollection<anrede> FillAnrede()
        {
            var list = from e in FContent.anrede select e;
            return new ObservableCollection<anrede>(list);
        }
        private ObservableCollection<titel> FillTitel()
        {
            var list = from e in FContent.titel select e;
            return new ObservableCollection<titel>(list);
        }
        private ObservableCollection<auftragsart> FillAuftragsart()
        {
            var list = from e in FContent.auftragsart select e;
            return new ObservableCollection<auftragsart>(list);
        }
        #endregion

        private void AuftragSpeichern()
        {
            using(new WaitCursor())
            {
                try
                {
                    FAuftrag.Save();
                }
                catch (KundeDatenUnvollstaendigException e)
                {
                    MessageBox.Show("Problem mit den Kundendaten: " + e.Message);
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int HighestTabIndex = TabsNeuerAuftrag.Items.Count - 1;
            if (TabsNeuerAuftrag.SelectedIndex < HighestTabIndex)
                TabsNeuerAuftrag.SelectedIndex += 1;
        }

        private void btnZurueck_Click(object sender, RoutedEventArgs e)
        {
            if (TabsNeuerAuftrag.SelectedIndex > 0)
                TabsNeuerAuftrag.SelectedIndex -= 1;
        }

        private void TabsNeuerAuftrag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int HighestTabIndex = TabsNeuerAuftrag.Items.Count - 1;
            if (TabsNeuerAuftrag.SelectedIndex != HighestTabIndex)
            {
                btnWeiter.Visibility = Visibility.Visible;
                btnSpeichern.Visibility = Visibility.Hidden;
            }
            else
            {
                btnWeiter.Visibility = Visibility.Hidden;
                btnSpeichern.Visibility = Visibility.Visible;
            }

            btnZurueck.IsEnabled = (TabsNeuerAuftrag.SelectedIndex != 0);
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            AuftragSpeichern();
        }

    }
}
