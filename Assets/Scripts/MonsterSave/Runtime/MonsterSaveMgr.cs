using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public static class MonsterSaveMgr
    {
        private static readonly IStorageProvider StorageProviderSystem;

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
            StorageProviderSystem = new StorageProvider();

            // 加载默认配置
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
                throw new Exception("Default config not found");
        }
    }
}