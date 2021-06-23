using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class BaseRepository
    {
        readonly SQLiteAsyncConnection database;

        public BaseRepository()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChatSessions.db3"));
            Task.Run(async () =>
            {
                await database.CreateTableAsync<ChatSessions>();
                await SaveAsync(new ChatSessions() { Date = DateTime.Now, Text = "我是一个session" });
                var result = await GetNotesAsync();
            });
        }

        public Task<List<ChatSessions>> GetNotesAsync()
        {
            //Get all notes.
            return database.Table<ChatSessions>().ToListAsync();
        }

        public Task<ChatSessions> GetNoteAsync(int id)
        {
            // Get a specific note.
            return database.Table<ChatSessions>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(ChatSessions item)
        {
            if (item.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(item);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteAsync(ChatSessions item)
        {
            // Delete a note.
            return database.DeleteAsync(item);
        }
    }
}
