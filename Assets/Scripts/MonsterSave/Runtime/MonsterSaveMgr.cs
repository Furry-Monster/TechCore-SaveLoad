using System;
using System.Collections.Generic;
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
            set { _config = value; }
        }


        static MonsterSaveMgr()
        {
            // 加载默认配置
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
                throw new Exception("Default config not found");

            // 初始化可序列化类型
            TypeMgr.Initialize();
            // 初始化存储部分
            StorageProvider = new StorageProvider(Config);
        }

        public static void Set<T>(string key, T data) => StorageProvider.Set(key, data);
        public static T Get<T>(string key, T defaultValue = default) => StorageProvider.Get<T>(key, defaultValue);
        public static void Delete(string key) => StorageProvider.Delete(key);
        public static bool Exist(string key) => StorageProvider.Exist(key);
        
    }
}