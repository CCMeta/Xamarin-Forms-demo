﻿using Dapper;
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

        public async Task<IEnumerable<Contacts>> GetList(int uid, int maxId = 0, int limit = 5)
        {
            string sql = "SELECT * FROM contacts WHERE uid = @uid";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Contacts>(sql, new { uid, maxId, limit });
            });

        }
    }
}
