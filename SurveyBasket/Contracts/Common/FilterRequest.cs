using Microsoft.Extensions.Options;

namespace SurveyBasket.Contracts.Common
{
    public record FilterRequest
    {
       public int pageNumber { get; init; } = 1;
       public int pageSize { get; init; } = 10;

        public string? SearchValue { get; init; }
        public string? SortColumn { get; init; }
        public string? SortDirection { get; init; } = "ASC";


    }
}
