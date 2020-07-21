using System;
using System.Text.RegularExpressions;

namespace EPAM_BusinessLogicLayer.BusinessModels
{
    public class FuzzySearch
    {

        private string _searchTerm;
        private string[] _searchTerms;
        private Regex _searchPattern;

        public FuzzySearch(string searchTerm)
        {
            _searchTerm = searchTerm;
            _searchTerms = searchTerm.Split(new Char[] { ' ' });
            _searchPattern = new Regex(
                "(?i)(?=.*" + String.Join(")(?=.*", _searchTerms) + ")");
        }

        public bool IsMatch(string value)
        {
            if (_searchTerm == value) return true;
            if (value.Contains(_searchTerm)) return true;
            if (_searchPattern.IsMatch(value)) return true;

            return false;
        }

    }
}