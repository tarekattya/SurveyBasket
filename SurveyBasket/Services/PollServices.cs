using SurveyBasket.Model;
using SurveyBasket.Repositories.IDEMO;
using SurveyBasket.Services.NewFolder;

namespace SurveyBasket.Services
{
    public class PollServices(IPollRepo pollRepo) : IPollServices
    {
        private readonly IPollRepo _pollRepo = pollRepo;

        public IEnumerable<Poll> GetAll()
        {
            return _pollRepo.Getall();
        }

        public Poll? Get(int id)
        {
            return _pollRepo.Get(id);
        }

       public Poll Add(Poll poll)
        {
            return _pollRepo.Add(poll);
        }

        public bool Update(int id, Poll poll)
        {
            return (_pollRepo.Update(id, poll));
        }

        public bool Delete(int id)
        {
            return (_pollRepo.Deleted(id));
        }
    }


}
