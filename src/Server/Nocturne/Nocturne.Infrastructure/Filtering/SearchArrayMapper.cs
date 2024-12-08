using Nocturne.Core.Filtering;
using System.Reflection;

namespace Nocturne.Infrastructure.Filtering
{
    public class SearchArrayMapper<T> : ITypedSearchMapper<T>
    {
        private Dictionary<string, KeyValuePair<string, string>> _searchProperties;

        public Type Type { get; private set; }

        public SearchArrayMapper()
        {
           
        }

        //TODO: Add lost property for search
        private bool Check()
        {
            return Type.GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.GetProperty)
            .Select(p => p.Name)
            .All(n => _searchProperties.Keys.Contains(n));
        }

        public IEnumerable<ISearch> BuildSearch(string[] rawSearchCriterias)
        {
            Type = typeof(T);
            /*
             * 0 - Property
             * 1 - Comparer
             * 3 - Value
             */
            _searchProperties = rawSearchCriterias
                   .Select(s => s.Split(','))
                   .ToArray()
                   .ToDictionary(k => k[0], v => new KeyValuePair<string, string>(v[1], v[2]));
            
            List<AbstractSearch> searchCriterias = new();

            if (Check()) { throw new InvalidOperationException(); };

            foreach (var property in _searchProperties.Keys)
            {
                AbstractSearch abstractSearch;

                var propertyFilter = _searchProperties[property].Value;
                
                if (int.TryParse(_searchProperties[property].Value, out var number))
                {
                    abstractSearch = new NumericSearch()
                    {
                        Property = property,
                        SearchTerm = number,
                        TargetTypeName = Type.Name,
                        Comparator = Enum.Parse<NumericComparators>(_searchProperties[property].Key)
                    };
                }
                else if (DateTime.TryParse(_searchProperties[property].Value, out var date))
                {
                    abstractSearch = new DateSearch()
                    {
                        Property = property,
                        SearchTerm = date,
                        TargetTypeName = Type.Name,
                        Comparator = Enum.Parse<DateComparators>(_searchProperties[property].Key)
                    };
                }
                else
                {
                    abstractSearch = new TextSearch()
                    {
                        Property = property,
                        SearchTerm = (string)_searchProperties[property].Value,
                        Comparator = Enum.Parse<TextComparators>(_searchProperties[property].Key),
                        TargetTypeName = Type.Name
                    };
                }
                   
                searchCriterias.Add(abstractSearch);
            }

            return searchCriterias;
        }
    }
}
