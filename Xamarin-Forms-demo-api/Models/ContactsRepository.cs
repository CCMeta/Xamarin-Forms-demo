using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ContactsRepository : BaseRepository
    {
        public ContactsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Contacts>> GetList(int uid)
        {
            string sql = "SELECT contacts.*, users.*,COUNT(chats.uid) as unread FROM `contacts` LEFT JOIN users ON contacts.partner_id = users.id left join chats on chats.uid = contacts.partner_id WHERE contacts.uid = @uid GROUP BY contacts.partner_id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid });
            });
        }

        public async Task<IEnumerable<Contacts>> GetByPartnerId(int uid, int partner_id)
        {
            string sql = "SELECT contacts.*, users.*,COUNT(chats.uid) as unread FROM `contacts` LEFT JOIN users ON contacts.partner_id = users.id left join chats on chats.uid = contacts.partner_id WHERE contacts.partner_id=@partner_id AND contacts.uid = @uid GROUP BY contacts.partner_id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid, partner_id });
            });
        }
    }
}
