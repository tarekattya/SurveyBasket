using SurveyBasket.Model;

namespace SurveyBasket.Repositories.IDEMO
{
    public interface IPollRepo
    {


        public  static List<Poll> Polls { get; set; } 

        public IEnumerable<Poll> Getall();

        public Poll? Get(int id);

        public Poll Add(Poll poll);

        bool Update(int id, Poll poll);

        bool Deleted(int id);


    }
}
