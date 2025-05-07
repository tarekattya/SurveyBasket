
//using SurveyBasket.Contracts.Requests;
//using SurveyBasket.Contracts.Responses;

//namespace SurveyBasket.Mapping
//{
//    public static class MappingContract
//    {


//        public static PollResponse MapToResponse(this Poll poll)
//        {
//            return new()
//            {
//                Id = poll.Id,
//                Title = poll.Title,
//                Notes = poll.Description
//            };
//        }
//        public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
//        {
//            return polls.Select(MapToResponse);
//        }

//        public static Poll MapToPoll(this CreatePollRequest Request)
//        {
//            return new()
//            {
//                Title = Request.Title,
//                Description = Request.Description
//            };
//        }
//    }
//}
