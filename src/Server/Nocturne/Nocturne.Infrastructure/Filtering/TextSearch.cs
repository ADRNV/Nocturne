using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

/* GenericSearch
 * https://github.com/danielpalme/GenericSearch
 */
namespace Nocturne.Infrastructure.Filtering
{
    public enum TextComparators
    {
        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "==")]
        Equals
    }

    public class TextSearch : AbstractSearch
    {
        public string SearchTerm { get; set; }

        public TextComparators Comparator { get; set; }

        public override Expression BuildFilterExpression(Expression property)
        {
            if (this.SearchTerm == null)
            {
                return null;
            }

            var searchExpression = Expression.Call(
                property,
                typeof(string).GetMethod(this.Comparator.ToString(), new[] { typeof(string) }),
                Expression.Constant(this.SearchTerm));

            return searchExpression;
        }
    }
}
