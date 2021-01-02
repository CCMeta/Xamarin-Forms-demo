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
        private readonly ExamAnswersRepository _repository;

        public ExamAnswersController(ExamAnswersRepository ExamAnswersRepository, IHttpContextAccessor context) : base(context)
        {
            _repository = ExamAnswersRepository;
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
            IEnumerable<int> exam_ids = examAnswers.Select((a) => a.questionId);
            if (await _repository.Post(examAnswers, uid: _uid))
                return Ok(examAnswers);
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
