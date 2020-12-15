using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class PostsRepository : BaseRepository
    {
        public PostsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Posts>> GetList(int uid, int page = 1, int limit = 5)
        {
            var sql = "SELECT * FROM posts WHERE uid = @uid LIMIT @limit OFFSET @from";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Posts>(sql, new { from = (page - 1) * limit, limit, uid });
            });
        }

        public async Task<int> Post(Posts post)
        {
            var sql = "INSERT INTO posts SET content = @content, uid = @uid";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sql, new { post.content, post.uid });
            });
        }
    }
}
