using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ExamAnswersRepository : BaseRepository
    {
        public ExamAnswersRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<ExamAnswers>> GetList(int uid, int questionId)
        {
            var sql = "SELECT * FROM exam_answers WHERE uid = @uid AND questionId = @questionId";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<ExamAnswers>(sql, new { uid, questionId });
            });
        }

        public async Task<bool> Post(ExamAnswers[] itemList, int uid)
        {
            var sql = "INSERT INTO exam_answers SET questionId = @questionId, answer = @answer, uid = @uid";
            return await WithConnection(async conn =>
            {
                var transaction = conn.BeginTransaction();
                foreach (var item in itemList)
                {
                    if (await conn.ExecuteAsync(sql, new { item.answer, item.questionId, uid }, transaction) != 1)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
            });
        }
    }
}
