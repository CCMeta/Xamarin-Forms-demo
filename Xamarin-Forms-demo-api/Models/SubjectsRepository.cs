using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class SubjectsRepository : BaseRepository
    {
        public SubjectsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Subjects>> GetSubjects(int limit = 6)
        {
            var sql = "SELECT * FROM subjects LIMIT @limit";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Subjects>(sql, new { limit });
            });

        }
    }
}
