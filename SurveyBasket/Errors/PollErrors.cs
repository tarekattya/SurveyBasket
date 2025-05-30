﻿namespace SurveyBasket.Errors
{
    public class PollErrors
    {
        public static Error PollNotFound => new("Poll.NotFound", $"Poll with id not found" , StatusCodes.Status400BadRequest);
        public static Error DublicateTitles => new("Poll.DublicateTitles", $"Another Poll With This Title", StatusCodes.Status409Conflict);

    }
}
