using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ruling
{
    public class Result
    {
        public ReadOnlyDictionary<string, string[]> Errors
            => new ReadOnlyDictionary<string, string[]>(_errors.ToDictionary(k => k.Key, v => v.Value.ToArray()));

        public bool Valid => _errors.Count == 0;

        protected Dictionary<string, List<string>> _errors { get; }
            = new Dictionary<string, List<string>>();

        internal void AddError(string key, string message)
        {
            if (!_errors.TryGetValue(key, out List<string> values))
            {
                values = new List<string>();
                _errors.Add(key, values);
            }

            values.Add(message);
        }

        internal void AddErrors(string key, IEnumerable<string> messages)
        {
            if (!_errors.TryGetValue(key, out List<string> values))
            {
                values = new List<string>();
                _errors.Add(key, values);
            }

            values.AddRange(messages);
        }

        internal void Combine(Result result)
        {
            foreach (var error in result._errors)
            {
                if (!_errors.TryGetValue(error.Key, out List<string> values))
                {
                    values = new List<string>();
                    _errors.Add(error.Key, values);
                }

                values.AddRange(error.Value);
            }
        }
    }

    public class NullResult : Result
    {
        public NullResult() : base()
        {
            _errors.Add(string.Empty, new List<string> { "Input is null" });
        }
    }
}
