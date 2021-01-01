using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ExamsRepository : BaseRepository
    {
        public ExamsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Exams>> GetList(int page = 1, int limit = 5)
        {
            var sql = "SELECT * FROM exams LIMIT @limit OFFSET @from";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Exams>(sql, new { from = (page - 1) * limit, limit });
            });
        }

    }
}
