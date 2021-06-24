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
        private const string DBNAME = "ChatSessions.db3";
        protected static readonly SQLiteAsyncConnection db;

        static BaseStore()
        {
            db = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME));
        }
    }
}
