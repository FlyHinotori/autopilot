using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Autopilot.GUI
{
    /// <summary>Für die Zeit, in der eine Instanz dieses Objekts existiert, ist der Maus-Cursor der "Wait"-Cursor. (Beispiel beachten!)</summary>
    /// <example>
    /// <code>
    /// using(new WaitCursor())
    /// {
    ///     //very long task
    /// }
    /// </code>
    /// </example>
    public class WaitCursor : IDisposable
    {
        private Cursor FOriginalCursor;

        public WaitCursor()
        {
            FOriginalCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = FOriginalCursor;
        }
    }
}
