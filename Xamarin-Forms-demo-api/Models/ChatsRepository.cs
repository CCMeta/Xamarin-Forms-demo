﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ChatsRepository : BaseRepository
    {
        public ChatsRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<Chats>> GetListByPartner(int uid, int partner_id, int max_id)
        {
            //select two guys send messages, so we use 2 condition for where 
            string sql = "( SELECT chats.*,users.nickname,users.avatar,users.intro FROM chats LEFT JOIN users ON users.id = chats.uid WHERE chats.id > @max_id AND (( chats.uid = @uid AND chats.partner_id = @partner_id ) OR ( chats.uid = @partner_id AND chats.partner_id = @uid )) ORDER BY id DESC LIMIT 10 ) ORDER BY id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Chats>(sql, new { uid, partner_id, max_id });
            });
        }

        public async Task<IEnumerable<Chats>> GetListByMaxId(int uid, int max_id)
        {
            //select two guys send messages, so we use 2 condition for where 
            string sql = "( SELECT chats.*,users.nickname,users.avatar,users.intro FROM chats LEFT JOIN users ON users.id = chats.uid WHERE chats.id > @max_id AND (( chats.uid = @uid ) OR ( chats.partner_id = @uid )) ORDER BY id DESC) ORDER BY id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Chats>(sql, new { uid, max_id });
            });
        }

        public async Task<Chats> Get(int uid, int id)
        {
            //select two guys send messages, so we use 2 condition for where 
            string sql = "SELECT * FROM chats LEFT JOIN users ON users.id = chats.uid WHERE chats.id = @id AND chats.uid = @uid";
            return await WithConnection(async conn =>
            {
                return await conn.QueryFirstAsync<Chats>(sql, new { uid, id });
            });
        }

        public async Task<int> Post(Chats chat)
        {
            var sql = "INSERT INTO chats SET content = @content, uid = @uid, partner_id = @partner_id;"
                + " SELECT @@IDENTITY ";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sql, new { chat.content, chat.uid, chat.partner_id });
            });
        }
    }
}
