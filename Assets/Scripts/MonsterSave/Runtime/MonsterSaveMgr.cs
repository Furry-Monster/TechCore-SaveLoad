using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public static class MonsterSaveMgr
    {
        private static readonly IStorageProvider StorageProvider;

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
            // 初始化可序列化类型
            TypeRegistry.Initialize();
            // 初始化存储部分
            StorageProvider = new StorageProvider();
            // 加载默认配置
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
                throw new Exception("Default config not found");
        }
    }
}