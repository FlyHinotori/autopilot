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
            Utilities utilities = new Utilities();

            /// <summary>
            /// Beispiel für das Ausführen einer SQL-Anweisung
            /// utilities.SQLCmd("SQL-Anweisung");
            /// </summary>
            
            /// <summary>
            /// Beispiele für das Aufrufen von Messageboxen
            /// utilities.HinweisMsgBox("Hier kommt der Hinweistext rein", "Titel der Box");
            /// utilities.FehlerMsgBox("Hier kommt der Fehlertext rein", "Titel der Box");
            /// </summary>
        }

        private void button_TestArea_Click(object sender, RoutedEventArgs e)
        {
            TestArea ShowForm = new TestArea();
            ShowForm.ShowDialog();
        }
    }
}
 