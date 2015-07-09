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


namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für KundenNeu.xaml
    /// </summary>
    public partial class KundenNeu : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        
        public KundenNeu()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cb_Kundengruppe.ItemsSource = FillKGruppe();
            cb_Anrede.ItemsSource = FillAnrede();
            cb_Titel.ItemsSource = FillTitel();            
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
            if (cb_Kundengruppe.SelectedValue != null && cb_Anrede.SelectedValue != null && Convert.ToString(tb_Name.Text) != "" && Convert.ToString(tb_Strasse.Text) != "" && Convert.ToString(tb_PLZ.Text) != "" && Convert.ToString(tb_Ort.Text) != "")
            {
                var res = MessageBox.Show("Soll ein neuer Kunde angelegt werden?", "Speichern", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    string tit_id;
                    if(cb_Titel.SelectedValue == null)
                    {
                        tit_id = "1";
                    }
                    else
                    {
                        tit_id = cb_Titel.SelectedValue.ToString();
                    }
                    
                    SqlConnection conn = new SqlConnection(DBconnStrg);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO kunde (kng_id, anr_id, tit_id, knd_name, knd_vorname, knd_strasse, knd_ort, knd_plz, knd_land, knd_mail, knd_telefon) VALUES (" + Convert.ToString(cb_Kundengruppe.SelectedValue.ToString()) + "," + Convert.ToString(cb_Anrede.SelectedValue.ToString()) + "," + tit_id + ",\'" + Convert.ToString(tb_Name.Text) + "\',\'" + Convert.ToString(tb_Vorname.Text) + "\',\'" + Convert.ToString(tb_Strasse.Text) + "\',\'" + Convert.ToString(tb_Ort.Text) + "\',\'" + Convert.ToString(tb_PLZ.Text) + "\',\'" + Convert.ToString(tb_Land.Text) + "\',\'" + Convert.ToString(tb_Mail.Text) + "\',\'" + Convert.ToString(tb_Telefon.Text) + "\')";
                    cmd.CommandType = CommandType.Text;

                    try
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Der neue Kunde wurde angelegt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                        cb_Kundengruppe.SelectedIndex = -1;
                        cb_Anrede.SelectedIndex = -1;
                        cb_Titel.SelectedIndex = -1;
                        tb_Land.Clear();
                        tb_Mail.Clear();
                        tb_Name.Clear();
                        tb_Ort.Clear();
                        tb_PLZ.Clear();
                        tb_Strasse.Clear();
                        tb_Telefon.Clear();
                        tb_Vorname.Clear();
                    }
                    catch (System.Exception err)
                    {
                        MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Bitte prüfen Sie Ihre Eingaben.\n\nZum Speichern bitte die Auswahl ändern!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Bitte prüfen Sie Ihre Eingaben.\n\nEs müssen alle Pflichtfelder ausgefüllt sein!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
    }
}
