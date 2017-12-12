using System;
using System.Linq;

namespace Ruling
{
    public class Factory
    {
        public static Func<TObject, Result> CreateRuling<TObject>(params Func<TObject, Result>[] funcs)
            => (TObject @object) =>
            {
                if (@object == null)
                {
                    throw new ArgumentNullException(nameof(@object));
                }

                return funcs.Aggregate(new Result(), (result, next) =>
                {
                    var messages = result.Messages.Concat(next.Invoke(@object).Messages);
                    return new Result(messages.SelectMany(msgs => msgs.Select(msg => (msgs.Key, msg.key, msg.message))));
                });
            };
    }
}
