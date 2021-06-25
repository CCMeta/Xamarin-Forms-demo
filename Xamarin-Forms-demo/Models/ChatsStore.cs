using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class ChatsStore : BaseStore
    {

        public ChatsStore()
        {
            Task.Run(async () =>
            {
                //if (db.TableMappings.Count(i => i.TableName == "ChatSessions") < 1)
                //await db.DropTableAsync<Chats>();
                await db.CreateTableAsync<Chats>();

                //var result = await ListAsync();
            }).Wait();
        }

        public Task<List<Chats>> ListAsync(int partner)
        {
            //Get all notes.
            return db.Table<Chats>().Where(i => i.uid == partner || i.partner_id == partner).ToListAsync();
        }

        public Task<List<Chats>> ListUnreadAsync()
        {
            // Get a specific note.
            return db.Table<Chats>().Where(i => i.isRead == 0).ToListAsync();
        }

        public Task<int> SaveAsync(Chats item)
        {
            if (item.local_id != 0)
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

        public Task<int> DeleteAsync(Chats item)
        {
            // Delete a note.
            return db.DeleteAsync(item);
        }
    }
}
