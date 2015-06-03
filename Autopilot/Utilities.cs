using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;

namespace Autopilot
{
    class Utilities
    {
        public string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString; //zentraler DB-ConnectionStrg
        
        public void SQLCmd(string SQLCmd_txt)
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = SQLCmd_txt;
            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();
        }

        public void FehlerMsgBox(string FehlerMsgBoxText, string FehlerMsgBoxTitel)
        {
            MessageBox.Show(FehlerMsgBoxText, FehlerMsgBoxTitel, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void HinweisMsgBox(string HinweisMsgBoxText, string HinweisMsgBoxTitel)
        {
            MessageBox.Show(HinweisMsgBoxText, HinweisMsgBoxTitel, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
