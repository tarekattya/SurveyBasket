using SurveyBasket.Model;
using SurveyBasket.Repositories.IDEMO;

namespace SurveyBasket.Repositories
{
    public class PollRepo : IPollRepo
    {



        private static List<Poll> _polls = new List<Poll> {
    new Poll { Id = 1, Title = "poll 1", Description = "pollllllllllllllllllllllllllll" },
    new Poll { Id = 2, Title = "poll 2", Description = "pollllllllllllllllllllllllllll" },
    new Poll { Id = 3, Title = "poll 3", Description = "pollllllllllllllllllllllllllll" }
};

        public static List<Poll> Polls => _polls;
         

       public Poll? Get(int id) => Polls.Find(p => p.Id == id);


        IEnumerable<Poll> IPollRepo.Getall() => Polls;

        public Poll Add(Poll poll)
        {
            poll.Id = Polls.Count + 1;
            Polls.Add(poll);
            return poll;
        }

        public bool Update(int id, Poll poll)
        {
            var currentpoll = Get(id);
            if (currentpoll is null)
                return false;
            currentpoll.Title = poll.Title;
            currentpoll.Description = poll.Description;
            return true;
        }

        public bool Deleted(int id )
        {
            var currentpoll = Get(id);
            if(currentpoll is null) return false;
            _polls.Remove(currentpoll);
            return true;
        }
    }
}
