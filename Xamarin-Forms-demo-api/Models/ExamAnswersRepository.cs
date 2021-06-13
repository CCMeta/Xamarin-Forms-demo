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

        public async Task<IEnumerable<ExamAnswers>> GetListByTranscriptsId(int uid, int transcriptId)
        {
            var sql = "SELECT exam_answers.*, exam_questions.* FROM exam_answers LEFT JOIN exam_questions ON exam_answers.questionId = exam_questions.id WHERE uid = @uid AND exam_answers.transcriptId = @transcriptId";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<ExamAnswers>(sql, new { uid, transcriptId });
            });
        }

        public async Task<bool> Post(ExamAnswers[] itemList, int uid)
        {
            string sql = "INSERT INTO exam_answers SET transcriptId = @transcriptId, point = @point, questionId = @questionId, answer = @answer, uid = @uid";
            return await WithConnection(async conn =>
            {
                var transaction = conn.BeginTransaction();
                foreach (var item in itemList)
                {
                    if (await conn.ExecuteAsync(sql, new { item.transcriptId, item.point, item.answer, item.questionId, uid }, transaction) != 1)
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
