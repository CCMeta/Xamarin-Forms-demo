using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class UsersRepository : BaseRepository
    {
        public UsersRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<Users>> Get(Users user)
        {
            var sql = "SELECT * FROM users WHERE username = @username AND password = @password";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Users>(sql, user);
            });
        }

        public async Task<IEnumerable<Users>> ListAsync()
        {
            var sql = "SELECT * FROM users";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Users>(sql);
            });
        }

    }
}
