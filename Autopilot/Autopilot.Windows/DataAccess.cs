//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
using SQLite;
using System;

namespace Autopilot
{
    public class SQLiteDb
    {
        string _path;
        public SQLiteDb(string path)
        {
            _path = path;
        }
        
         public void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
            }
        }
    }
    
}
