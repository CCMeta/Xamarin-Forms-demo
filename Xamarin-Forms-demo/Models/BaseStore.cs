using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public abstract class BaseStore
    {
        private readonly string DBNAME;
        protected readonly SQLiteAsyncConnection db;

        public BaseStore(int uid)
        {
            DBNAME = $"{uid}.db3";
            db = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME));
        }
    }
}
