using System;
using System.Collections.Generic;
using System.Linq;

namespace Ruling
{
    public static class Validator
    {
        public static Result Validate<TObject>(TObject @object,
            params Func<TObject, (bool valid, string key, string message)>[] rules)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            return new Result(rules.Select(rule => rule.Invoke(@object)));
        }

        public static Result Validate<TObject>(TObject @object,
            params Func<TObject, (bool valid, string key, string message)[]>[] rules)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            return new Result(rules.SelectMany(rule => rule.Invoke(@object)));
        }
    }
}
