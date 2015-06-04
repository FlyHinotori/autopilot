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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Autopilot
{
    /// <summary>
    /// Interaktionslogik für TestArea.xaml
    /// </summary>
    public partial class TestArea : Window
    {
        //Einbinden der UtilitieKlasse
        Utilities utilities = new Utilities();

        public TestArea()
        {
            InitializeComponent();
            fülleLabel();
        }
        
        void fülleLabel()
        {
            string anr_id = "1";
            
            SqlConnection conn = new SqlConnection(utilities.DBconnStrg);
            conn.Open(); 

            SqlCommand cmd_pers = new SqlCommand();
            cmd_pers.Connection = conn;
            cmd_pers.CommandText = "SELECT anr_id, anr_bez FROM anrede WHERE anr_id = " + anr_id;
            cmd_pers.CommandType = System.Data.CommandType.Text;

            SqlDataReader dr_pers = cmd_pers.ExecuteReader();
            if (dr_pers.Read())
            {
                label_ID.Content = Convert.ToString(dr_pers.GetValue(0));
                label_BEZ.Content = Convert.ToString(dr_pers.GetValue(1));
            }
            conn.Close();            
        }
    }
}
