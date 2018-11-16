using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dapper.FluentMap.Utils
{
    /// <summary>
    /// Provides helper methods for reflection operations.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns the <see cref="T:System.Reflection.MemberInfo"/> for the specified lamba expression.
        /// </summary>
        /// <param name="lambda">A lamba expression containing a MemberExpression.</param>
        /// <returns>A <see cref="MemberInfo"/> object for the member in the specified lambda expression.</returns>
        public static MemberInfo GetMemberInfo(LambdaExpression lambda)
        {
            Expression expr = lambda;
            while (true)
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;

                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;

                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expr;
                        var member = memberExpression.Member;
                        Type paramType;

                        while (memberExpression != null)
                        {
                            paramType = memberExpression.Type;

                            // Find the member on the base type of the member type
                            // E.g. EmailAddress.Value
                            var baseMember = paramType.GetMembers().FirstOrDefault(m => m.Name == member.Name);
                            if (baseMember != null)
                            {
                                // Don't use the base type if it's just the nullable type of the derived type
                                // or when the same member exists on a different type
                                // E.g. Nullable<decimal> -> decimal
                                // or:  SomeType { string Length; } -> string.Length
                                if (baseMember is PropertyInfo baseProperty && member is PropertyInfo property)
                                {
                                    if (baseProperty.DeclaringType == property.DeclaringType &&
                                        baseProperty.PropertyType != Nullable.GetUnderlyingType(property.PropertyType))
                                    {
                                        return baseMember;
                                    }
                                }
                                else
                                {
                                    return baseMember;
                                }
                            }

                            memberExpression = memberExpression.Expression as MemberExpression;
                        }

                        // Make sure we get the property from the derived type.
                        paramType = lambda.Parameters[0].Type;
                        return paramType.GetMember(member.Name)[0];

                    default:
                        return null;
                }
            }
        }
    }
}
