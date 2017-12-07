using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruling
{
    public class Factory
    {
        public static Func<TObject, Result> CreateRuling<TObject>(params Func<TObject, (bool valid, string key, string message)>[] rules)
            => (TObject @object) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                return ValidateRules<TObject>(@object, false, rules);
            };

        public static Func<TObject, Result> CreateRulingFailFast<TObject>(params Func<TObject, (bool valid, string key, string message)>[] rules)
            => (TObject @object) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                return ValidateRules<TObject>(@object, true, rules);
            };

        public static Func<TObject, Result> CreateRuling<TObject>(params Func<TObject, Result>[] rulings)
            => (TObject @object) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                return ValidateRulings<TObject>(@object, false, rulings);
            };

        public static Func<TObject, Result> CreateRulingFailFast<TObject>(params Func<TObject, Result>[] rulings)
            => (TObject @object) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                return ValidateRulings<TObject>(@object, true, rulings);
            };

        static Result ValidateRules<TObject>(TObject @object, bool failFast, Func<TObject, (bool valid, string key, string message)>[] rules)
            => rules.Aggregate(new Result(), (result, rule) =>
            {
                if (!result.Valid && failFast)
                {
                    return result;
                }

                var ruleResult = rule.Invoke(@object);
                if (ruleResult.valid)
                {
                    return result;
                }

                result.AddError(ruleResult.key, ruleResult.message);
                return result;
            });

        static Result ValidateRulings<TObject>(TObject @object, bool failFast, Func<TObject, Result>[] rulings)
            => rulings.Aggregate(new Result(), (result, ruling) =>
            {
                if (!result.Valid && failFast)
                {
                    return result;
                }

                var rulingResult = ruling.Invoke(@object);
                result.Combine(rulingResult);
                return result;
            });
    }
}
