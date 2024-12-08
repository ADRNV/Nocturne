﻿using System;
using System.Linq.Expressions;

/* GenericSearch
 * https://github.com/danielpalme/GenericSearch
 */
namespace Nocturne.Infrastructure.Filtering
{
    public class EnumSearch : AbstractSearch
    {
        public EnumSearch()
        {
        }

        public EnumSearch(Type enumType)
        {
            this.EnumTypeName = enumType.AssemblyQualifiedName;
        }

        public string SearchTerm { get; set; }

        public Type EnumType
        {
            get
            {
                return Type.GetType(this.EnumTypeName);
            }
        }

        public string EnumTypeName { get; set; }

        public override System.Linq.Expressions.Expression BuildFilterExpression(Expression property)
        {
            if (this.SearchTerm == null)
            {
                return null;
            }

            var enumValue = Enum.Parse(this.EnumType, this.SearchTerm);

            Expression searchExpression = Expression.Equal(property, Expression.Constant(enumValue));

            return searchExpression;
        }
    }
}
