using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class MonsterSaveMgr : Singleton<MonsterSaveMgr>
    {
        private MonsterSaveConfig _config;

        private ISerializer _serializer;
        private IStorage _storage;

        public MonsterSaveConfig Config
        {
            get => _config;
            set => _config = value;
        }

        public MonsterSaveMgr()
        {
            // load default configurations
            _config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<MonsterSaveConfig>();
                _config.name = "DefaultConfig";
                Debug.LogWarning(
                    $"Can't load default configurations from path <Resources/{_config.name}>,please validate the integrity of the plugin.");
            }

            _serializer = new Serializer();
            _storage = new Storage();
            TypeRegistry.Initialize();
        }

        public void Save(string key, object data)
        {
            throw new NotImplementedException();
        }

        public object Load(string key)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Sync()
        {
            throw new NotImplementedException();
        }
    }
}