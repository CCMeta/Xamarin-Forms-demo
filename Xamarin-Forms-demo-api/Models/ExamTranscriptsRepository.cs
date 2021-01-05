using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ExamTranscriptsRepository : BaseRepository
    {
        public ExamTranscriptsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<ExamTranscripts>> GetList(int uid, int questionId)
        {
            var sql = "SELECT * FROM exam_transcripts WHERE uid = @uid ";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<ExamTranscripts>(sql, new { uid, questionId });
            });
        }

        public async Task<int> Post(ExamTranscripts item)
        {
            var sql = "INSERT INTO exam_transcripts SET " +
                "uid = @uid, duration = @duration, major = @major, score = @score, title = @title;" +
                " SELECT @@IDENTITY ";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sql, new { item.uid, item.duration, item.major, item.score, item.title });
            });
        }

    }
}
