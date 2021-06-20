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

        public async Task<IEnumerable<Chats>> GetList(int uid, int partner_id)
        {
            //select two guys send messages, so we use 2 condition for where 
            string sql = "SELECT * FROM chats LEFT JOIN users ON users.id = chats.uid WHERE (chats.uid = @uid AND chats.partner_id = @partner_id) OR (chats.uid = @partner_id AND chats.partner_id = @uid)";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Chats>(sql, new { uid, partner_id });
            });

        }
    }
}
