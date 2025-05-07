using SurveyBasket.Model;

namespace SurveyBasket.Services.NewFolder
{
    public interface IPollServices
    {
        public IEnumerable<Poll> GetAll();

        public Poll? Get(int id);

        public Poll Add(Poll poll);

        public bool Update(int id, Poll poll);

        public bool Delete(int id);

    }
}
