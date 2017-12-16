using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ruling
{
    public static partial class Rule
    {
        public const string GreaterThanMessage = "must be greater than {0}";
        public const string GreaterThanOrEqualToMessage = "must be greater than or equal to {0}";
        public const string LessThanMessage = "must be less than {0}";
        public const string LessThanOrEqualToMessage = "must be less than or equal to {0}";

        public static Func<TObject, (bool valid, string key, string message)> Compare<TObject, TComparable>(Expression<Func<TObject, IComparable<TComparable>>> selector, IComparable<TComparable> greaterThan = null, IComparable<TComparable> greaterThanOrEqualTo = null, IComparable<TComparable> lessThan = null, IComparable<TComparable> lessThanOrEqualTo = null, string message = null, string key = null)
        {
            if (greaterThan == null && greaterThanOrEqualTo == null &&
                lessThan == null && lessThanOrEqualTo == null)
            {
                throw new ArgumentException("Should provide a comparer");
            }

            var ruleKey = GetKey(selector, key);
            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);

                if (!checkValid(value, greaterThan, IsInvalidGreaterThan, GreaterThanMessage, out var resultGreaterThan))
                {
                    return resultGreaterThan;
                }

                if (!checkValid(value, greaterThanOrEqualTo, IsInvalidGreaterThanOrEqualTo, GreaterThanOrEqualToMessage, out var resultGreaterThanOrEqualTo))
                {
                    return resultGreaterThanOrEqualTo;
                }

                if (!checkValid(value, lessThan, IsInvalidLessThan, LessThanMessage, out var resultLessThan))
                {
                    return resultLessThan;
                }

                if (!checkValid(value, lessThanOrEqualTo, IsInvalidLessThanOrEqualTo, LessThanOrEqualToMessage, out var resultLessThanOrEqualTo))
                {
                    return resultLessThanOrEqualTo;
                }

                return (true, ruleKey, nameof(Compare));
            };

            bool checkValid(IComparable<TComparable> value, IComparable<TComparable> other, Func<IComparable<TComparable>, IComparable<TComparable>, bool> isInvalid, string invalidMessage, out (bool valid, string key, string message) result)
            {
                if (other != null && isInvalid(value, other))
                {
                    result = (false, ruleKey, GetMessage(message, string.Format(invalidMessage, other)));
                    return false;
                }

                result = (true, ruleKey, nameof(Compare));
                return true;
            }
        }

        public static Func<TObject, (bool valid, string key, string message)> Compare<TObject, TComparable>(Expression<Func<TObject, IComparable<TComparable>>> selector, Func<TObject, IComparable<TComparable>> greaterThan = null, Func<TObject, IComparable<TComparable>> greaterThanOrEqualTo = null, Func<TObject, IComparable<TComparable>> lessThan = null, Func<TObject, IComparable<TComparable>> lessThanOrEqualTo = null, string message = null, string key = null)
        {
            if (greaterThan == null && greaterThanOrEqualTo == null &&
                lessThan == null && lessThanOrEqualTo == null)
            {
                throw new ArgumentException("Should provide a comparer");
            }

            var ruleKey = GetKey(selector, key);
            return (TObject @object) =>
            {
                var value = selector.Compile().Invoke(@object);

                if (!checkValid(@object, value, greaterThan, IsInvalidGreaterThan, GreaterThanMessage, out var resultGreaterThan))
                {
                    return resultGreaterThan;
                }

                if (!checkValid(@object, value, greaterThanOrEqualTo, IsInvalidGreaterThanOrEqualTo, GreaterThanOrEqualToMessage, out var resultGreaterThanOrEqualTo))
                {
                    return resultGreaterThanOrEqualTo;
                }

                if (!checkValid(@object, value, lessThan, IsInvalidLessThan, LessThanMessage, out var resultLessThan))
                {
                    return resultLessThan;
                }

                if (!checkValid(@object, value, lessThanOrEqualTo, IsInvalidLessThanOrEqualTo, LessThanOrEqualToMessage, out var resultLessThanOrEqualTo))
                {
                    return resultLessThanOrEqualTo;
                }

                if (value == null)
                {
                    return (false, ruleKey, nameof(Compare));
                }

                return (true, ruleKey, nameof(Compare));
            };

            bool checkValid(TObject @object, IComparable<TComparable> value, Func<TObject, IComparable<TComparable>> other, Func<IComparable<TComparable>, IComparable<TComparable>, bool> isInvalid, string invalidMessage, out (bool valid, string key, string message) result)
            {
                if (other != null)
                {
                    var otherValue = other.Invoke(@object);
                    if (isInvalid(value, otherValue))
                    {
                        result = (false, ruleKey, GetMessage(message, string.Format(invalidMessage, otherValue?.ToString() ?? "null")));
                        return false;
                    }
                }

                result = (true, ruleKey, nameof(Compare));
                return true;
            }
        }

        static bool IsInvalidGreaterThan<TComparable>(IComparable<TComparable> value, IComparable<TComparable> other)
            => value == null || other == null
                ? true
                : value.CompareTo((TComparable)other) <= 0;

        static bool IsInvalidGreaterThanOrEqualTo<TComparable>(IComparable<TComparable> value, IComparable<TComparable> other)
            => value == null || other == null
                ? true
                : value.CompareTo((TComparable)other) < 0;

        static bool IsInvalidLessThan<TComparable>(IComparable<TComparable> value, IComparable<TComparable> other)
            => value == null || other == null
                ? true
                : value.CompareTo((TComparable)other) >= 0;

        static bool IsInvalidLessThanOrEqualTo<TComparable>(IComparable<TComparable> value, IComparable<TComparable> other)
            => value == null || other == null
                ? true
                : value.CompareTo((TComparable)other) > 0;
    }
}
