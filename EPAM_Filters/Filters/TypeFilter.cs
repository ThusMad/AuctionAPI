using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Entities.Interfaces;
using EPAM_DataAccessLayer.Enums;
using EPAM_Filters.Interfaces;

namespace EPAM_Filters.Filters
{
    public class TypeFilter<T, TEnum> : IFilter<T>
    where T : ITypeable<TEnum>
    where TEnum : struct, Enum
    {
        private readonly TEnum _filterValue;

        public TypeFilter(string filterValue)
        {
            _filterValue = (TEnum)(object)int.Parse(filterValue);
        }

        public IQueryable<T> ApplyFilter(IQueryable<T> query)
        {
            return query.Where(a => _filterValue.Equals(a.Type));
        }
    }
}
