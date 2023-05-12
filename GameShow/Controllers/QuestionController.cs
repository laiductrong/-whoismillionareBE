using GameShow.DTO.Question;
using GameShow.Services.QuestionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("GetList")]
        public async Task<ActionResult<ServiceReponse<List<GetQuestion>>>> GetList()
        {
            return Ok(await _questionService.GetQuestions());
        }
        [HttpPost("AddQuestion"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<List<GetQuestion>>>> AddQuestion(AddQuestion question)
        {
            return Ok(await _questionService.AddQuestion(question));
        }
        [HttpDelete("Delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<List<GetQuestion>>>> DeleteQuestion(int id)
        {
            return Ok(await _questionService.DeleteQuestion(id));
        }
        [HttpGet("GetById/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<GetQuestion>>> GetQuestionById(int id)
        {
            return Ok(await _questionService.GetQuestionById(id));
        }
        [HttpPut("Update"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<GetQuestion>>> UpdateQuestion(UpdateQuestion updateQuestion)
        {
            return Ok(await _questionService.UpdateQuestion(updateQuestion));
        }
    }
}
