using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class ChatSessionsStore
    {
        readonly SQLiteAsyncConnection db;

        public ChatSessionsStore()
        {
            db = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChatSessions.db3"));
            Task.Run(async () =>
            {
                if (db.TableMappings.Count(i => i.TableName == "ChatSessions") < 1)
                    await db.CreateTableAsync<ChatSessions>();

                await SaveAsync(new ChatSessions() { Partner = 3, Unread = 2 });
                var result = await ListAsync();
            });
        }

        public Task<List<ChatSessions>> ListAsync()
        {
            //Get all notes.
            return db.Table<ChatSessions>().ToListAsync();
        }

        public Task<ChatSessions> GetAsync(int id)
        {
            // Get a specific note.
            return db.Table<ChatSessions>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(ChatSessions item)
        {
            if (item.ID != 0)
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

        public Task<int> DeleteAsync(ChatSessions item)
        {
            // Delete a note.
            return db.DeleteAsync(item);
        }
    }
}
