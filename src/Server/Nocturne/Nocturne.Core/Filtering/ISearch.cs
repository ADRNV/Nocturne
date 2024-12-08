using System.Linq.Expressions;

namespace Nocturne.Core.Filtering
{
    public interface ISearch
    {
        string Property { get; set; }

        string TargetTypeName { get; set; }

        IQueryable<T> ApplyToQuery<T>(IQueryable<T> query);

        Expression BuildFilterExpression(Expression property);
    }
}