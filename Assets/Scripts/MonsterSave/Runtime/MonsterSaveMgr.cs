using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public static class MonsterSaveMgr
    {
        private static readonly IStorage StorageSystem;

        private static MonsterSaveConfig _config;

        public static MonsterSaveConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                OnConfigUpdated?.Invoke();
            }
        }

        public static Action OnConfigUpdated { get; set; }

        static MonsterSaveMgr()
        {
            TypeRegistry.Initialize();
            StorageSystem = new StorageSystem();

            // 加载默认配置
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
                throw new Exception("Default config not found");
        }

        public static void Save(string key, object data) => StorageSystem.Save(key, data);

        public static object Load(string key) => StorageSystem.Load(key);

        public static T Load<T>(string key) => StorageSystem.Load<T>(key);

        public static bool Sync() => StorageSystem.Sync();
    }
}