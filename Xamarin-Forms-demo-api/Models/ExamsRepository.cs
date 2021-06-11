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

        public async Task<IEnumerable<Exams>> GetList(int maxId = 0, int limit = 5)
        {
            string sql = "SELECT * FROM exams WHERE id > @maxId LIMIT @limit";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Exams>(sql, new { maxId, limit });
            });
        }

        public async Task<Exams> Get(int id)
        {
            string sql = "SELECT * FROM exams WHERE id = @id ";
            return await WithConnection(async conn =>
            {
                return await conn.QueryFirstAsync<Exams>(sql, new { id });
            });
        }

    }
}
