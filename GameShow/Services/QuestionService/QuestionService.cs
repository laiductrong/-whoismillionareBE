using AutoMapper;
using GameShow.Controllers;
using GameShow.Data;
using GameShow.DTO.Question;
using GameShow.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShow.Services.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly IMapper _imapper;
        private readonly DataContext _dataContext;
        public QuestionService( IMapper mapper, DataContext dataContext)
        {
            _imapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServiceReponse<List<GetQuestion>>> AddQuestion(AddQuestion addQuestion)
        {
            var serviceReposive = new ServiceReponse<List<GetQuestion>>();
            Question question= new Question();
            question.Ques = addQuestion.Ques;
            question.AnswerA=addQuestion.AnswerA;
            question.AnswerB = addQuestion.AnswerB;
            question.AnswerC=addQuestion.AnswerC;
            question.AnswerD=addQuestion.AnswerD;
            question.CorrectAnswer=addQuestion.CorrectAnswer;
            try 
            {
                await _dataContext.Questions.AddAsync(question);
                await _dataContext.SaveChangesAsync();
                serviceReposive.Data = (await _dataContext.Questions.ToListAsync()).Select(x=> _imapper.Map<GetQuestion>(x)).ToList();
                serviceReposive.Success = true;
                serviceReposive.Message = "Add Question success";
            }
            catch(Exception ex)
            {
                serviceReposive.Data = null;
                serviceReposive.Success = false;
                serviceReposive.Success = true;
                serviceReposive.Message = ex.Message;
            }
            return serviceReposive;
        }

        public async Task<ServiceReponse<List<GetQuestion>>> DeleteQuestion(int id)
        {
            var serviceReposive = new ServiceReponse<List<GetQuestion>>();
            var question = new Question();
            question = await _dataContext.Questions.FirstOrDefaultAsync(x=> x.Id==id);
            if(question != null)
            {
                _dataContext.Remove(question);
                await _dataContext.SaveChangesAsync();
                serviceReposive.Data = (await _dataContext.Questions.ToListAsync()).Select(x => _imapper.Map<GetQuestion>(x)).ToList();
                serviceReposive.Success = true;
                serviceReposive.Message = "Add Question success";
            }
            else
            {
                serviceReposive.Data = null;
                serviceReposive.Success = false;
                serviceReposive.Message = "Delete Question fail";
            }
            return serviceReposive;
        }

        public async Task<ServiceReponse<List<GetQuestion>>> GetQuestions()
        {
            var serviceReposive = new ServiceReponse<List<GetQuestion>>();
            try
            {
                serviceReposive.Data = (await _dataContext.Questions.ToListAsync()).Select(x => _imapper.Map<GetQuestion>(x)).ToList();
                serviceReposive.Success = true;
                serviceReposive.Message = "Get Questions success";
            }
            catch (Exception ex)
            {
                serviceReposive.Data = null;
                serviceReposive.Success = false;
                serviceReposive.Success = true;
                serviceReposive.Message = ex.Message;
            }
            return serviceReposive;
        }

        public async Task<ServiceReponse<GetQuestion>> GetQuestionById(int id)
        {
            var serviceReponsive = new ServiceReponse<GetQuestion>();
            var question = _imapper.Map<GetQuestion>(await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id == id));
            if(question is not null)
            {
                serviceReponsive.Data = question;
                serviceReponsive.Success = true;
                serviceReponsive.Message = "Find question success";
            }
            else
            {
                serviceReponsive.Data = null;
                serviceReponsive.Success = false;
                serviceReponsive.Message = "Find question fails";
            }
            return serviceReponsive;
        }

        public async Task<ServiceReponse<GetQuestion>> UpdateQuestion(UpdateQuestion updateQuestion)
        {
            var serviceReponsive = new ServiceReponse<GetQuestion>();
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x=> x.Id == updateQuestion.Id);
            if (question is not null)
            {
                question.Ques = updateQuestion.Ques;
                question.AnswerA = updateQuestion.AnswerA;
                question.AnswerB = updateQuestion.AnswerB;
                question.AnswerC = updateQuestion.AnswerC;
                question.AnswerD = updateQuestion.AnswerD;
                question.CorrectAnswer = updateQuestion.CorrectAnswer;

                // Lưu thay đổi vào database
                await _dataContext.SaveChangesAsync();

                serviceReponsive.Data = _imapper.Map<GetQuestion>(question);
                serviceReponsive.Success = true;
                serviceReponsive.Message = "Update question successfully.";
            }
            else
            {
                serviceReponsive.Data = null;
                serviceReponsive.Success = false;
                serviceReponsive.Message = "update question fails";
            }
            return serviceReponsive;
        }
    }
}
