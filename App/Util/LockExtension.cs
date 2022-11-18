using System.Collections.Concurrent;

namespace amantiq.util
{
    public static class LockExtension
    {
        private static readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();

        public static object GetLock(this string lockKey)
        {
            return _locks.GetOrAdd(lockKey, k => new object());
        }
    }
}