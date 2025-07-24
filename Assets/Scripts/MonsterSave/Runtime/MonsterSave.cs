namespace MonsterSave.Runtime
{
    public static class MonsterSave
    {
        internal static void Initialize() => _ = MonsterSaveMgr.Instance;

        internal static void Initialize(MonsterSaveConfig config) => MonsterSaveMgr.Instance.Config = config;

        internal static void Save(string key, object value) => MonsterSaveMgr.Instance.Save(key, value);

        internal static object Load(string key) => MonsterSaveMgr.Instance.Load(key);

        internal static T Load<T>(string key) => MonsterSaveMgr.Instance.Load<T>(key);

        internal static bool Sync() => MonsterSaveMgr.Instance.Sync();

        internal static void UseConfig(MonsterSaveConfig config) => MonsterSaveMgr.Instance.Config = config;
    }
}