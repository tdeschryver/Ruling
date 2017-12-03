using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruling
{
    public class Factory
    {
        public static Func<TObject, Result> CreateRuling<TObject>(params Func<TObject, (bool valid, string key, string message)>[] rules)
            => (TObject @object) => rules.Aggregate(new Result(), (result, rule) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                var ruleResult = rule.Invoke(@object);
                if (ruleResult.valid)
                {
                    return result;
                }

                result.AddError(ruleResult.key, ruleResult.message);
                return result;
            });

        public static Func<TObject, Result> CreateRuling<TObject>(params Func<TObject, Result>[] rulings)
            => (TObject @object) => rulings.Aggregate(new Result(), (result, ruling) =>
            {
                if (@object == null)
                {
                    return new NullResult();
                }

                var rulingResult = ruling.Invoke(@object);
                result.Combine(rulingResult);
                return result;
            });
    }
}
