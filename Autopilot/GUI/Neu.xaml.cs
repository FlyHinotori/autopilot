﻿using System;
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
        }

        #region context list fillers
        private ObservableCollection<kundengruppe> FillKundengruppe()
        {
            var list = from e in FContent.kundengruppe select e;
            return new ObservableCollection<kundengruppe>(list);
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
            this.TabsNeuerAuftrag.SelectedItem = TabRoute;
        }

        private void btnZurueck_Click(object sender, RoutedEventArgs e)
        {
            AuftragSpeichern(); // <- debug    
        }

    }
}
