using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Services.AuctionService.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<Auction> FilterByCategory(this IQueryable<Auction> queryable, string categoriesRaw)
        {
            var categories = categoriesRaw.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            queryable = queryable.Where(auction => auction.Categories.Count(auctionCategory => categories.Contains(auctionCategory.Category.Name)) == categories.Count);
            return queryable;
        }
        public static IQueryable<Auction> FilterByCreationTime(this IQueryable<Auction> queryable, string value)
        {
            var range = value.Split("-", StringSplitOptions.RemoveEmptyEntries);

            var from = long.Parse(range[0].GetIntPart("."));
            var to = long.Parse(range[1].GetIntPart("."));

            return queryable.Where(a => a.CreationTime > from && a.CreationTime < to);
        }

        public static IQueryable<Auction> FilterByStarted(this IQueryable<Auction> queryable)
        {
            var time =  DateTimeOffset.UtcNow.ToUnixTimeSeconds() * 1000;
            return queryable.Where(a => a.Type == AuctionType.Normal ? a.StartTime <= time && a.EndTime > time : a.StartTime <= time);
        }

        public static IQueryable<Auction> FilterByType(this IQueryable<Auction> queryable, AuctionType value)
        {
            return queryable.Where(a => a.Type == value);
        }

        public static IQueryable<Auction> FilterByUserName(this IQueryable<Auction> queryable, string user)
        {
            return queryable.Where(auction => auction.Creator.NormalizedUserName == user.ToUpperInvariant());
        }

        public static IQueryable<Auction> FilterByUserId(this IQueryable<Auction> queryable, string id)
        {
            return queryable.Where(auction => auction.Creator.Id == id);
        }

        public static IQueryable<Auction> Search(this IQueryable<Auction> queryable, string query)
        {
            //var fuzz = new FuzzySearch("query");

            return queryable.Where(auction => auction.Title.Contains(query));
        }

        public static IQueryable<Auction> ApplyFilters(this IQueryable<Auction> request, string filters)
        {
            var filtersArray = filters.Split(";", StringSplitOptions.RemoveEmptyEntries);

            var filtered = request;

            foreach (var filter in filtersArray)
            {
                if(filter != "started")
                {
                    var filterData = filter.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    var (key, value) = new KeyValuePair<string, string>(filterData[0], filterData[1]);

                    filtered = key switch
                    {
                        "type" => filtered.FilterByType((AuctionType) int.Parse(value)),
                        "created" => filtered.FilterByCreationTime(value),
                        "category" => filtered.FilterByCategory(value),
                        "user" => filtered.FilterByUserName(value),
                        "userId" => filtered.FilterByUserId(value),
                        "name" => filtered.Search(value),
                        _ => filtered
                    };
                }
                else
                {
                    filtered = filtered.FilterByStarted();
                }
            }

            return filtered;
        }

        // TODO: test

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}
