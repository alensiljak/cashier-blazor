﻿using System.Text.RegularExpressions;

namespace Cashier.Lib
{
    /// <summary>
    /// Creates a Regex for filtering lists
    /// </summary>
    public class ListSearch
    {
        public bool Search(string searchTerm)
        {
            var regex = getRegex(searchTerm);
            var match = regex.Match(searchTerm);
            return match.Success;
        }

        public Regex getRegex(string searchTerm)
        {
            var expression = getExpression(searchTerm);

            var options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled;
            var regex = new Regex(expression, options);
            return regex;
        }

        /// <summary>
        /// Creates a RegEx expression to search for all the terms.
        /// </summary>
        /// <param name="searchTerm">A list of terms separated by space</param>
        /// <returns>A RegEx expression</returns>
        public string getExpression(string searchTerm)
        {
            // split the search terms
            var searchTerms = searchTerm.Split(' ');
            var expression = "^";
            for (var i = 0; i < searchTerms.Length; i++)
            {
                if (searchTerms[i] == null) continue;

                expression += "(?=.*" + searchTerms[i] + ")";
            }
            expression += ".*$";
            return expression;
        }
    }
}
