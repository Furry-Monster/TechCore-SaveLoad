using System;

namespace MonsterSave.Runtime
{
    public class Singleton<T> where T : class, new()
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());
        public static T Instance => _instance.Value;

        protected Singleton()
        {
            if (_instance.IsValueCreated)
            {
                throw new Exception($"{typeof(T).Name} is already created!");
            }
        }
    }
}