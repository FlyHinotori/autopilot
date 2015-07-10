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
            CBStartFlughafen.ItemsSource = FillFlughafen();
            CBZielFlughafen.ItemsSource = FillFlughafen();
            CBZwischenhalt.ItemsSource = FillFlughafen();
            CBCabinCrew.ItemsSource = FillStewardess();
            CBPiloten.ItemsSource = FillPiloten();
            CBFlugzeugTyp.ItemsSource = FillFlugzeugTyp();
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
        private ObservableCollection<flughafen> FillFlughafen()
        {
            var list = from e in FContent.flughafen select e;
            return new ObservableCollection<flughafen>(list);
        }
        private ObservableCollection<personal> FillPersonal()
        {
            var list = from e in FContent.personal select e;
            return new ObservableCollection<personal>(list);
        }
        private ObservableCollection<personal> FillStewardess()
        {
            position Stewardess = FContent.position.Where(p => p.pos_bez == "Stewardess").FirstOrDefault();
            ObservableCollection<personal> PersonList = FillPersonal();
            ObservableCollection<personal> Stewardesses = new ObservableCollection<personal>();

            foreach (personal Person in PersonList)
            {
                if (Person.pos_id == Stewardess.pos_id)
                    Stewardesses.Add(Person);
            }
            return Stewardesses;
        }
        private ObservableCollection<personal> FillPiloten()
        {
            position Copilot = FContent.position.Where(p => p.pos_bez == "Copilot").FirstOrDefault();
            position Pilot = FContent.position.Where(p => p.pos_bez == "Pilot").FirstOrDefault();
            ObservableCollection<personal> PersonList = FillPersonal();
            ObservableCollection<personal> Piloten = new ObservableCollection<personal>();

            foreach (personal Person in PersonList)
            {
                if ((Person.pos_id == Copilot.pos_id) || (Person.pos_id == Pilot.pos_id))
                    Piloten.Add(Person);
            }
            return Piloten;
        }
        private ObservableCollection<flugzeugtyp> FillFlugzeugTyp()
        {
            var list = from e in FContent.flugzeugtyp select e;
            return new ObservableCollection<flugzeugtyp>(list);
        }
        #endregion

        private void AuftragSpeichern()
        {
            using(new WaitCursor())
            {
                try
                {
                    FAuftrag.Save();
                    MessageBox.Show("Auftrag wurde angelegt");
                }
                catch (GeneralModelsException e)
                {
                    if (e is KundeDatenUnvollstaendigException)
                        MessageBox.Show("Problem mit den Kundendaten: " + e.Message);
                    else if (e is AuftragDatenFehlerhaftException)
                        MessageBox.Show("Fehler in den Stammdaten: " + e.Message);
                    else if (e is AuftragDatenUnvollstaendigException)
                        MessageBox.Show("Unvollständiger Auftrag: " + e.Message);
                    else
                        throw;
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

        private void IntegerInputField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //just handle integer input
            int i;
            e.Handled = !int.TryParse(e.Text, out i);
        }

        private void btnAddZwischenhalt_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.ZwischenHalte.Add((flughafen)CBZwischenhalt.SelectedItem);
        }

        private void btnRemoveZwischenhalt_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.ZwischenHalte.Remove((flughafen)LBZwischenhalte.SelectedItem);
        }

        private void btnAddCabinCrew_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.CabinCrew.Add((personal)CBCabinCrew.SelectedItem);
        }

        private void btnRemoveCabinCrew_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.CabinCrew.Remove((personal)LBCabinCrew.SelectedItem);
        }

        private void btnAddPilot_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.PilotenCrew.Add((personal)CBPiloten.SelectedItem);
        }

        private void btnRemovePilot_Click(object sender, RoutedEventArgs e)
        {
            FAuftrag.PilotenCrew.Remove((personal)LBPiloten.SelectedItem);
        }

        private void CBAuftragsArt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            auftragsart NewAuftragsart = ((sender as ComboBox).SelectedItem as auftragsart);
            if (NewAuftragsart.aart_bez == "Einzelflug")
                SetEinzelflugView();
            else if (NewAuftragsart.aart_bez == "Flug mit Zwischenaufenthalt")
                SetZwischenhaltView();
            else if (NewAuftragsart.aart_bez == "Zeitcharter")
                SetCharterView();
            else
                throw new Exception("Auftragsart nicht bekannt!");
        }

        // Summary:
        //     Shows the CharterDauer controls
        private void SetCharterView()
        {
            LStartFlughafen.Visibility = Visibility.Hidden;
            CBStartFlughafen.Visibility = Visibility.Hidden;
            LZielFlughafen.Visibility = Visibility.Hidden;
            CBZielFlughafen.Visibility = Visibility.Hidden;
            LPassengerCount.Visibility = Visibility.Hidden;
            TBPassengerCount.Visibility = Visibility.Hidden;
            LZwischenhalt.Visibility = Visibility.Hidden;
            CBZwischenhalt.Visibility = Visibility.Hidden;
            btnAddZwischenhalt.Visibility = Visibility.Hidden;
            LBZwischenhalte.Visibility = Visibility.Hidden;
            btnRemoveZwischenhalt.Visibility = Visibility.Hidden;
            LCharterDauer.Visibility = Visibility.Visible;
            TBCharterDauer.Visibility = Visibility.Visible;
        }

        // Summary:
        //     Shows the Startflughafen, Zielflughafen, Zwischenhalte and PassengerCount controls
        private void SetZwischenhaltView()
        {
            LStartFlughafen.Visibility = Visibility.Visible;
            CBStartFlughafen.Visibility = Visibility.Visible;
            LZielFlughafen.Visibility = Visibility.Visible;
            CBZielFlughafen.Visibility = Visibility.Visible;
            LPassengerCount.Visibility = Visibility.Visible;
            TBPassengerCount.Visibility = Visibility.Visible;
            LZwischenhalt.Visibility = Visibility.Visible;
            CBZwischenhalt.Visibility = Visibility.Visible;
            btnAddZwischenhalt.Visibility = Visibility.Visible;
            LBZwischenhalte.Visibility = Visibility.Visible;
            btnRemoveZwischenhalt.Visibility = Visibility.Visible;
            LCharterDauer.Visibility = Visibility.Hidden;
            TBCharterDauer.Visibility = Visibility.Hidden;
        }

        // Summary:
        //     Shows the Startflughafen, Zielflughafen and PassengerCount controls
        private void SetEinzelflugView()
        {
            LStartFlughafen.Visibility = Visibility.Visible;
            CBStartFlughafen.Visibility = Visibility.Visible;
            LZielFlughafen.Visibility = Visibility.Visible;
            CBZielFlughafen.Visibility = Visibility.Visible;
            LPassengerCount.Visibility = Visibility.Visible;
            TBPassengerCount.Visibility = Visibility.Visible;
            LZwischenhalt.Visibility = Visibility.Hidden;
            CBZwischenhalt.Visibility = Visibility.Hidden;
            btnAddZwischenhalt.Visibility = Visibility.Hidden;
            LBZwischenhalte.Visibility = Visibility.Hidden;
            btnRemoveZwischenhalt.Visibility = Visibility.Hidden;
            LCharterDauer.Visibility = Visibility.Hidden;
            TBCharterDauer.Visibility = Visibility.Hidden;
        }

    }
}
