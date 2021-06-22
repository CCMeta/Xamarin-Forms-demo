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
            string sql = "SELECT * FROM contacts LEFT JOIN users ON users.id = contacts.partner_id WHERE contacts.uid = @uid";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid });
            });
        }

        public async Task<IEnumerable<Contacts>> GetByPartnerId(int uid, int partner_id)
        {
            string sql = "SELECT * FROM contacts LEFT JOIN users ON users.id = contacts.partner_id WHERE contacts.uid = @uid AND contacts.partner_id = @partner_id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid, partner_id });
            });
        }
    }
}
