using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string MinLengthMessageString = "should be at least {0} character(s)";
        public const string MaxLengthMessageString = "should be at most {0} character(s)";
        public const string ExactLengthMessageString = "lengshould be {0} character(s)";
        public const string MinLengthMessageList = "should have at least {0} item(s)";
        public const string MaxLengthMessageList = "should have {0} item(s)";
        public const string ExactLengthMessageList = "should have {0} item(s)";

        public static Func<TObject, (bool valid, string key, string message)> Length<TObject>(Expression<Func<TObject, string>> selector, int? exact = null, int? min = null, int? max = null, string message = null, string key = null)
        {
            if (exact == null && min == null && max == null)
            {
                throw new ArgumentException("Should provide a length");
            }

            var ruleKey = GetKey(selector, key);
            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);

                if (exact != null && value.Length != exact)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(ExactLengthMessageString, exact)));
                }
                else if (min != null && value.Length < min)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(MinLengthMessageString, min)));
                }
                else if (value.Length > max)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(MaxLengthMessageString, max)));
                }

                return (true, ruleKey, nameof(Length));
            };
        }

        public static Func<TObject, (bool valid, string key, string message)> Length<TObject>(Expression<Func<TObject, ICollection>> selector, int? exact = null, int? min = null, int? max = null, string message = null, string key = null)
        {
            if (exact == null && min == null && max == null)
            {
                throw new ArgumentException("Should provide a length");
            }

            var ruleKey = GetKey(selector, key);
            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);

                if (exact != null && value.Count != exact)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(ExactLengthMessageList, exact)));
                }
                else if (min != null && value.Count < min)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(MinLengthMessageList, min)));
                }
                else if (value.Count > max)
                {
                    return (false, ruleKey, GetMessage(message, string.Format(MaxLengthMessageList, max)));
                }

                return (true, ruleKey, nameof(Length));
            };
        }
    }
}
