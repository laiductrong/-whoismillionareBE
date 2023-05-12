using GameShow.Controllers;
using GameShow.DTO.Question;

namespace GameShow.Services.QuestionService
{
    public interface IQuestionService
    {
        Task<ServiceReponse<List<GetQuestion>>> GetQuestions();
        Task<ServiceReponse<GetQuestion>> GetQuestionById(int id);
        Task<ServiceReponse<List<GetQuestion>>> AddQuestion(AddQuestion addQuestion);
        Task<ServiceReponse<GetQuestion>> UpdateQuestion(UpdateQuestion updateQuestion);
        Task<ServiceReponse<List<GetQuestion>>> DeleteQuestion(int id);
    }
}
