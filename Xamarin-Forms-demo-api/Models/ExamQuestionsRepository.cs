using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class ExamQuestionsRepository : BaseRepository
    {
        public ExamQuestionsRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<ExamQuestions>> GetList(int exams_id)
        {
            var sql = "SELECT * FROM exam_questions WHERE exam_id = @exams_id";
            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<ExamQuestions>(sql, new { exams_id });
            });
        }

    }
}
