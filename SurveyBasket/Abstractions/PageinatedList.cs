namespace SurveyBasket.Abstractions
{
    public class PageinatedList<T>(IList<T> items , int pageNumber , int pageSize , int count )
    {
        public IList<T> items { get; private set; } = items;
        public int pageNumber { get; private set; } = pageNumber;
        public int PageCount => (int)Math.Ceiling(count / (double)pageSize);

        public bool HasPerviousPage => pageNumber > 1;
        public bool HasNextPage => pageNumber < PageCount ;


        public static async Task<PageinatedList<T>> CreateAsync(IQueryable<T> source , int pagesize , int pagenumber , CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pagenumber - 1 ) * pagesize).Take(pagesize).ToListAsync();

            return new PageinatedList<T>(items, pagenumber, pagesize, count);
        }
   }
}
