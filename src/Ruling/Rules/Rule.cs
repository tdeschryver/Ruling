using System;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string DefaultInvalidMessage = "is invalid";

        static string GetKey<TInput, TOutput>(Expression<Func<TInput, TOutput>> selector, string key)
            => key ?? (selector.Body as MemberExpression ?? ((UnaryExpression)selector.Body).Operand as MemberExpression).Member.Name;

        static string GetMessage(string message, string ruleMessage = null)
            => message ?? ruleMessage ?? DefaultInvalidMessage;
    }
}
