namespace SurveyBasket.Errors
{
    public class QuestionErrors
    {
        public static Error QuestionNotFound => new("Question.NotFound", $"Question with id not found");
        public static Error DublicateContent => new("Question.DublicateContent", $"Another Question With This Content");
    }
   
}
