using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ruling
{
    public class Result
    {
        public ILookup<bool, (string key, string message)> Messages { get; }

        public ReadOnlyDictionary<string, string[]> Errors
            => new ReadOnlyDictionary<string, string[]>(
                Messages[false]
                    .GroupBy(p => p.key)
                    .ToDictionary(k => k.Key, v => v.Select(p => p.message).ToArray())
            );

        public bool Valid => !Messages.Contains(false);

        public Result(IEnumerable<(bool valid, string key, string message)> rulesResult)
        {
            Messages = rulesResult
                .ToLookup(k => k.valid, v => (v.key, v.message));
        }

        public Result()
            : this(new(bool valid, string key, string message)[] { })
        {
        }
    }
}
