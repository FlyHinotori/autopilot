using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Autopilot.DataModel
{
    class Flugzeuge
    {
        //Eindeutige FlugzeugID
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int flz_id { get; set; }

        //unterschiedlichen IDs des Flugzeuges
        public int sta_id { get; set; }
        public int her_id { get; set; }
    }
}
