using MoviesAPI_EFC.DTOs.General;

namespace MoviesAPI_EFC.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> items,PaginationData paginationData)
        {
            return items.Skip((paginationData.PageIndex - 1) * paginationData.PageSize).Take(paginationData.PageSize);   
        }
    }
}
