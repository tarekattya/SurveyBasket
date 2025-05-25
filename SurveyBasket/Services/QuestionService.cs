using SurveyBasket.Presistence.DbContextt;

namespace SurveyBasket.Services
{
    public class QuestionService(ApplicationDbContext context) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;



        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int Pollid, CancellationToken cancellationToken = default)
        {

            var PollIsExist = _context.Polls.Any(P => P.Id == Pollid);
            if (!PollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            var questions = await _context.Questions
                .Where(P => P.PollId == Pollid)
                .Include(Q => Q.Answers)
                .ProjectToType<QuestionResponse>()
                .ToListAsync
                (cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);


        }

        public async Task<Result<QuestionResponse>> GetAsync(int Pollid, int id, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _context.Polls.AnyAsync(P => P.Id == Pollid);
            if (!PollIsExist)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            var question = await _context.Questions
                .Where(Q => Q.Id == id && Q.PollId == Pollid)
                .Include(Q => Q.Answers)
                .ProjectToType<QuestionResponse>()
                .SingleOrDefaultAsync(cancellationToken);
            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            return Result.Success(question);
        }



        public async Task<Result<QuestionResponse>> AddAsync(int Pollid, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _context.Polls.AnyAsync(P => P.Id == Pollid);
            if (!PollIsExist)
               return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            var questionIsExist = await _context.Questions.AnyAsync(Q => Q.Content == request.Content && Q.PollId == Pollid);
            if (questionIsExist)
                return Result.Failure<QuestionResponse>(QuestionErrors.DublicateContent);

            var question = request.Adapt<Question>();
            question.PollId = Pollid;
           await _context.Questions.AddAsync(question , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(question.Adapt<QuestionResponse>());


        }
        public async Task<Result> UpdateAsync(int Pollid, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var QuestionIsExists = await _context.Questions.AnyAsync(Q => Q.PollId == Pollid
            && Q.Id != id
            && Q.Content == request.Content, cancellationToken);

            if (QuestionIsExists)
                return Result.Failure(QuestionErrors.DublicateContent);

            var Question = await _context.Questions.Include(Q => Q.Answers).SingleOrDefaultAsync
                (Q => Q.Id == id && Q.PollId == Pollid, cancellationToken);
            if (Question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);
            Question.Content = request.Content;

            var CurrentAnswers = Question.Answers.Select(A => A.Content).ToList();

            var NewAnswers = request.Answers.Except(CurrentAnswers).ToList();

            NewAnswers.ForEach(answer =>
            {
                Question.Answers.Add(new Answer { Content = answer });
            });


            Question.Answers.ToList().ForEach(answer =>

            answer.IsActive = request.Answers.Contains(answer.Content)
            );
                     

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> ToggleStatus(int Pollid, int id, CancellationToken cancellationToken = default)
        {
            var Question = await _context.Questions.SingleOrDefaultAsync(Q => Q.Id == id && Q.PollId == Pollid, cancellationToken);
            if (Question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);
            Question.IsActive = !Question.IsActive;
            _context.Questions.Update(Question);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }
   
}
