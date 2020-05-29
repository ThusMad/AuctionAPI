using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using EPAM_Filters.Filters;
using EPAM_Filters.Infrastructure.Exceptions;
using EPAM_Filters.Interfaces;

namespace EPAM_Filters
{
    public static class FilterExtenstion
    {
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> queryable, string type, string value)
        {
            switch (type)
            {
                case "type":
                    return queryable.FilterBy(new TypeFilter<T, AuctionType>(value));
                case "time":
                    return queryable.FilterByTime(value);
                case "category":
                    return queryable.FilterByCategory(value);
            }

            throw new FilterNotFoundException(type, "Filter not supported");
        }

        public static IQueryable<T> FilterBy<T>(this IQueryable<T> queryable, IFilter<T> filter)
        {
            return filter.ApplyFilter(queryable);
        }
    }
}
