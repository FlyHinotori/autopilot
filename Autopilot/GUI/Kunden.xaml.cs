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
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Kunden.xaml
    /// </summary>
    public partial class Kunden : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        int knd_id;
        int kng_id;
        int anr_id;
        int tit_id;

        public Kunden()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridKunden.ItemsSource = GetList();
            cb_Kundengruppe.ItemsSource = FillKGruppe();
            cb_Anrede.ItemsSource = FillAnrede();
            cb_Titel.ItemsSource = FillTitel();
            bt_Speichern.IsEnabled = false;
        }

        private ObservableCollection<Kundenliste> GetList()
        {
            var list = from e in content.Kundenliste select e;
            return new ObservableCollection<Kundenliste>(list);
        }

        private ObservableCollection<kundengruppe> FillKGruppe()
        {
            var list = from e in content.kundengruppe select e;
            return new ObservableCollection<kundengruppe>(list);
        }

        private ObservableCollection<anrede> FillAnrede()
        {
            var list = from e in content.anrede select e;
            return new ObservableCollection<anrede>(list);
        }

        private ObservableCollection<titel> FillTitel()
        {
            var list = from e in content.titel select e;
            return new ObservableCollection<titel>(list);
        }

        private void bt_Speichern_Click(object sender, RoutedEventArgs e)
        {            
            var res = MessageBox.Show("Sollen die Änderungen gespeichert werden?","Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                var ID = content.kunde.SingleOrDefault(c => c.knd_id == knd_id);
                //ID.flz_kennzeichen = tb_Kennzeichen.Text.ToString();
                ID.kng_id = Convert.ToInt32(cb_Kundengruppe.SelectedValue.ToString());
                ID.anr_id = Convert.ToInt32(cb_Anrede.SelectedValue.ToString());
                ID.tit_id = Convert.ToInt32(cb_Titel.SelectedValue.ToString());

                content.SaveChanges();
                MessageBox.Show("Update des DataGrid-Updates noch nicht gebaut.");
            }
        }

        private void DataGridKunden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridKunden.SelectedCells.Count != 0 && DataGridKunden.ItemsSource != null)
            {
                DataRowView row = DataGridKunden.SelectedItems as DataRowView;
                knd_id = ((Kundenliste)DataGridKunden.SelectedItem).knd_id;
                kng_id = ((Kundenliste)DataGridKunden.SelectedItem).kng_id;
                anr_id = ((Kundenliste)DataGridKunden.SelectedItem).anr_id;
                tit_id = ((Kundenliste)DataGridKunden.SelectedItem).tit_id;

                //tb_Kennzeichen.Text = Convert.ToString(ID.flz_kennzeichen);

                cb_Kundengruppe.SelectedValue = Convert.ToString(kng_id);
                cb_Anrede.SelectedValue = Convert.ToString(anr_id);
                cb_Titel.SelectedValue = Convert.ToString(tit_id);
                bt_Speichern.IsEnabled = true;
            }
        }
    }
}
