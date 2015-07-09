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
    /// Interaktionslogik für KundenUebersicht.xaml
    /// </summary>
    public partial class KundenUebersicht : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        int knd_id;
        int kng_id;
        int anr_id;
        int tit_id;
        DataTable datatableInfo = new DataTable("Info");

        public KundenUebersicht()
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
            if (tb_Filter.Text == "")
            {
                var list = from e in content.Kundenliste
                           select e;
                return new ObservableCollection<Kundenliste>(list);
            }
            else
            {
                var list = from e in content.Kundenliste
                           where e.knd_name.Contains(tb_Filter.Text) || e.knd_vorname.Contains(tb_Filter.Text) || e.anschrift.Contains(tb_Filter.Text)
                           select e;
                return new ObservableCollection<Kundenliste>(list);
            }
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
                ID.kng_id = Convert.ToInt32(cb_Kundengruppe.SelectedValue.ToString());
                ID.anr_id = Convert.ToInt32(cb_Anrede.SelectedValue.ToString());
                ID.tit_id = Convert.ToInt32(cb_Titel.SelectedValue.ToString());
                ID.knd_land = Convert.ToString(tb_Land.Text);
                ID.knd_mail = Convert.ToString(tb_Mail.Text);
                ID.knd_name = Convert.ToString(tb_Name.Text);
                ID.knd_ort = Convert.ToString(tb_Ort.Text);
                ID.knd_plz = Convert.ToString(tb_PLZ.Text);
                ID.knd_strasse = Convert.ToString(tb_Strasse.Text);
                ID.knd_telefon = Convert.ToString(tb_Telefon.Text);
                ID.knd_vorname = Convert.ToString(tb_Vorname.Text);

                content.SaveChanges();

                tb_Land.IsEnabled = false;
                tb_Mail.IsEnabled = false;
                tb_Name.IsEnabled = false;
                tb_Ort.IsEnabled = false;
                tb_PLZ.IsEnabled = false;
                tb_Strasse.IsEnabled = false;
                tb_Telefon.IsEnabled = false;
                tb_Vorname.IsEnabled = false;
                cb_Kundengruppe.IsEnabled = false;
                cb_Anrede.IsEnabled = false;
                cb_Titel.IsEnabled = false;
                bt_Speichern.IsEnabled = false;
                
                content = new AutopilotEntities();
                DataGridKunden.ItemsSource = GetList();
                DataGridInfo.ItemsSource = null;
            }
        }

        private void DataGridKunden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridKunden.SelectedCells.Count != 0 && DataGridKunden.ItemsSource != null && Convert.ToString(DataGridKunden.Items.Count.ToString()) != "0" && DataGridKunden.CurrentItem != null)
            {
                DataRowView row = DataGridKunden.SelectedItems as DataRowView;
                knd_id = ((Kundenliste)DataGridKunden.SelectedItem).knd_id;
                kng_id = ((Kundenliste)DataGridKunden.SelectedItem).kng_id;
                anr_id = ((Kundenliste)DataGridKunden.SelectedItem).anr_id;
                tit_id = ((Kundenliste)DataGridKunden.SelectedItem).tit_id;

                var ID = content.kunde.SingleOrDefault(c => c.knd_id == knd_id);
                tb_Land.Text = Convert.ToString(ID.knd_land);
                tb_Mail.Text = Convert.ToString(ID.knd_mail);
                tb_Name.Text = Convert.ToString(ID.knd_name);
                tb_Ort.Text = Convert.ToString(ID.knd_ort);
                tb_PLZ.Text = Convert.ToString(ID.knd_plz);
                tb_Strasse.Text = Convert.ToString(ID.knd_strasse);
                tb_Telefon.Text = Convert.ToString(ID.knd_telefon);
                tb_Vorname.Text = Convert.ToString(ID.knd_vorname);                

                cb_Kundengruppe.SelectedValue = Convert.ToString(kng_id);
                cb_Anrede.SelectedValue = Convert.ToString(anr_id);
                cb_Titel.SelectedValue = Convert.ToString(tit_id);

                tb_Land.IsEnabled = true;
                tb_Mail.IsEnabled = true;
                tb_Name.IsEnabled = true;
                tb_Ort.IsEnabled = true;
                tb_PLZ.IsEnabled = true;
                tb_Strasse.IsEnabled = true;
                tb_Telefon.IsEnabled = true;
                tb_Vorname.IsEnabled = true;
                cb_Kundengruppe.IsEnabled = true;
                cb_Anrede.IsEnabled = true;
                cb_Titel.IsEnabled = true;
                bt_Speichern.IsEnabled = true;

                fuelleDataGridInfo();
            }
        }

        private void tb_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridKunden.ItemsSource = GetList();            
        }

        private void tb_Filter_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_Filter.Clear();
            DataGridInfo.ItemsSource = null;
        }

        private void fuelleDataGridInfo()
        {
            datatableInfo.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);
            string SQLcmd = "SELECT auftragsart.aart_id, sta_bez, auf_faellig_am, (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) as saldo, (select mst_reihenfolge from mahnstufe where mahnstufe.mst_id = auftrag.mst_id) as mahnstufe, aart_bez, ter_beginn, ter_ende, (select flh_name + \'(\' + flh_stadt + \')\' as abflug from flughafen where flughafen.flh_id = auftrag.flh_id_beginn) as abflughafen, (select flh_name + '(' + flh_stadt + ')' as zielflug from flughafen where flughafen.flh_id = auftrag.flh_id_ende) as zielflughafen FROM auftrag, status, auftragsart, kunde, termin_auftrag, termin WHERE auftrag.sta_id = status.sta_id AND auftrag.aart_id = auftragsart.aart_id AND auftrag.knd_id = kunde.knd_id AND auftrag.auf_id = termin_auftrag.auf_id AND termin_auftrag.ter_id = termin.ter_id AND kunde.knd_id = " + knd_id;

            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(datatableInfo);

            DataGridInfo.ItemsSource = datatableInfo.DefaultView;
        }
    }
}
