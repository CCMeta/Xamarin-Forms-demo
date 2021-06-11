using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/Exams/{exams_id}/Answers")]
    [ApiController]
    public class ExamAnswersController : DefaultController
    {
        private readonly ExamsRepository _examsRepository;
        private readonly ExamAnswersRepository _repository;
        private readonly ExamQuestionsRepository _examQuestionsRepository;
        private readonly ExamTranscriptsRepository _examTranscriptsRepository;

        public ExamAnswersController(
            ExamsRepository ExamsRepository,
            ExamAnswersRepository ExamAnswersRepository,
            ExamQuestionsRepository ExamQuestionsRepository,
            ExamTranscriptsRepository ExamTranscriptsRepository
            , IHttpContextAccessor context) : base(context)
        {
            _examsRepository = ExamsRepository;
            _repository = ExamAnswersRepository;
            _examQuestionsRepository = ExamQuestionsRepository;
            _examTranscriptsRepository = ExamTranscriptsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ExamAnswers>> GetAsync(int questionId)
        {
            return await _repository.GetList(uid: _uid, questionId: questionId);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ExamAnswers[] examAnswers)
        {
            int[] questionIdList = examAnswers.Select(examAnswer => examAnswer.questionId).ToArray();
            var examQuestions = await _examQuestionsRepository.GetListByQuestionIdList(questionIdList);

            //count&&set each item score
            foreach (ExamAnswers examAnswer in examAnswers)
            {
                string regularAnswer = examQuestions.First(examQuestion => examQuestion.id == examAnswer.questionId).answer;
                examAnswer.point = (examAnswer.answer == regularAnswer) ? 5 : 0;
            }

            //
            int exam_id = examQuestions.First().exam_id;
            var exam = await _examsRepository.Get(exam_id);
            var examTranscript = new ExamTranscripts
            {
                uid = _uid,
                duration = 2223,//to be continue
                score = examAnswers.Sum((examAnswer) => examAnswer.point),
                major = exam.major,
                title = exam.title,
            };
            int examTranscriptId = await _examTranscriptsRepository.Post(examTranscript);

            //load each item examTranscriptId
            foreach (ExamAnswers examAnswer in examAnswers)
            {
                examAnswer.transcriptId = examTranscriptId;
            }

            //exam_ids select all of questions take the regular answers and compute points , then take points to examAnswers model
            if (await _repository.Post(examAnswers, uid: _uid))
                return Ok(examAnswers);
            //this place need to handle if failed then how to do with examTranscript already added
            return BadRequest(examAnswers);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
