using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class MonsterSaveMgr : Singleton<MonsterSaveMgr>
    {
        private ISerializer _serializer;
        private IStorage _storage;

        public MonsterSaveConfig Config { get; set; }

        public MonsterSaveMgr()
        {
            // load default configurations
            Config = Resources.Load<MonsterSaveConfig>("DefaultConfig");
            if (Config == null)
            {
                Config = ScriptableObject.CreateInstance<MonsterSaveConfig>();
                Config.name = "DefaultConfig";
                Debug.LogWarning(
                    $"Can't load default configurations from path <Resources/{Config.name}>,please validate the integrity of the plugin.");
            }

            _serializer = new Serializer(Config);
            _storage = new Storage(Config);
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