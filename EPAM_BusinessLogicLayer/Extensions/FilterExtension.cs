using System;
using System.Collections.Generic;
using System.Linq;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;

namespace EPAM_BusinessLogicLayer.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<Auction> FilterByCategory(this IQueryable<Auction> queryable, string categoriesRaw)
        {
            var categories = categoriesRaw.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            return queryable.Where(auction => auction.Categories.Count(auctionCategory => categories.Contains(auctionCategory.Category.Name)) == categories.Count);
        }
        public static IQueryable<Auction> FilterByTime(this IQueryable<Auction> queryable, string value)
        {
            var range = value.Split("-", StringSplitOptions.RemoveEmptyEntries);

            long bottom = long.Parse(range[0]);
            long top = long.Parse(range[1]);

            return queryable.Where(a => a.CreationTime > bottom && a.CreationTime < top);
        }

        public static IQueryable<Auction> FilterByType(this IQueryable<Auction> queryable, AuctionType value)
        {
            return queryable.Where(a => a.Type == value);
        }

        public static IQueryable<Auction> ApplyFilters(this IQueryable<Auction> request, string filters)
        {
            var filtersArray = filters.Split(";", StringSplitOptions.RemoveEmptyEntries);

            foreach (var filter in filtersArray)
            {
                var filterData = filter.Split("=", StringSplitOptions.RemoveEmptyEntries);
                var (key, value) = new KeyValuePair<string, string>(filterData[0], filterData[1]);

                switch (key)
                {
                    case "type":
                        request.FilterByType((AuctionType)int.Parse(value));
                        break;
                    case "time":
                        request.FilterByTime(value);
                        break;
                    case "category":
                        request.FilterByCategory(value);
                        break;
                }
            }

            return request;
        }
    }
}
