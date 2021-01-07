using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class KnowledgesRepository : BaseRepository
    {
        public KnowledgesRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Knowledges>> GetList(int maxId = 0, int limit = 5)
        {
            var sql = "SELECT * FROM knowledges WHERE id > @maxId LIMIT @limit";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Knowledges>(sql, new { maxId, limit });
            });

        }
    }
}
