using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ChatsRepository : BaseRepository
    {
        public ChatsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Contacts>> GetList(int uid)
        {
            string sql = "SELECT * FROM chats LEFT JOIN users ON users.id = chats.partner_id WHERE chats.uid = @uid";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid });
            });

        }
    }
}
