using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class CoursesRepository : BaseRepository
    {
        public CoursesRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Courses>> GetList(int page = 1, int limit = 5)
        {
            var sql = "SELECT * FROM courses LIMIT @limit OFFSET @from";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Courses>(sql, new { from = (page - 1) * limit, limit });
            });

        }
    }
}
