using System.Linq;

namespace EPAM_Filters.Interfaces
{
    public interface IFilter<T>
    {
        IQueryable<T> ApplyFilter(IQueryable<T> query);
    }
}