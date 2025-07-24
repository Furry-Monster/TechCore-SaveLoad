using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class MonsterSaveMgr : Singleton<MonsterSaveMgr>
    {
        private readonly IStorage _storageSystem;

        private MonsterSaveConfig _config;

        public MonsterSaveConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                OnConfigUpdated?.Invoke();
            }
        }

        public Action OnConfigUpdated { get; set; }

        public MonsterSaveMgr()
        {
            TypeRegistry.Initialize();
            _storageSystem = new StorageSystem();

            // 加载默认配置
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
                throw new Exception("Default config not found");
        }

        public void Save(string key, object data) => _storageSystem.Save(key, data);

        public object Load(string key) => _storageSystem.Load(key);

        public T Load<T>(string key) => _storageSystem.Load<T>(key);

        public bool Sync() => _storageSystem.Sync();
    }
}