using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class ChatSessionsStore : BaseStore
    {

        public ChatSessionsStore()
        {
            Task.Run(async () =>
            {
                //if (db.TableMappings.Count(i => i.TableName == "ChatSessions") < 1)
                //await db.DropTableAsync<Contacts>();
                await db.CreateTableAsync<Contacts>();

                //var result = await ListAsync();
            }).Wait();
        }

        public Task<List<Contacts>> ListAsync()
        {
            //Get all notes.
            return db.Table<Contacts>().ToListAsync();
        }

        public Task<Contacts> GetAsync(int partner)
        {
            // Get a specific note.
            return db.Table<Contacts>().Where(i => i.partner_id == partner).FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(Contacts item)
        {
            if (item.id != 0)
            {
                // Update an existing note.
                return db.UpdateAsync(item);
            }
            else
            {
                // Save a new note.
                return db.InsertAsync(item);
            }
        }

        public Task<int> DeleteAsync(Contacts item)
        {
            // Delete a note.
            return db.DeleteAsync(item);
        }
    }
}
