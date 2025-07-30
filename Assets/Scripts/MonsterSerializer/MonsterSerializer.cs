namespace MonsterSerializer
{
    public static class MonsterSerializer
    {
        public static byte[] Serialize<T>(T obj, SerializerSetting settings = null)
        {
            return SerializerMgr.Instance.Serialize(obj);
        }

        public static T Deserialize<T>(byte[] data, SerializerSetting settings = null)
        {
            return (T)SerializerMgr.Instance.Deserialize(typeof(T), data);
        }
    }
}