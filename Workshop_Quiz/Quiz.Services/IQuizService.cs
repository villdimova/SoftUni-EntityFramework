using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IQuizService
    {
        int Add(string Title);

        QuizViewModel GetQuizById(int quizId);

    }
}
