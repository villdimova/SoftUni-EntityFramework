using Quiz.Models.Quiz.Models;
using System.Collections.Generic;

namespace Quiz.Models
{
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
           
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int QuizId { get; set; }
        public QuizEntity QuizModel { get; set; }

        public ICollection<Answer> Answers { get; set; }

       
    }
}