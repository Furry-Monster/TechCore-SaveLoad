namespace MonsterSave.Runtime
{
    /// <summary>
    /// 全量存储
    /// </summary>
    public interface IFullStoreMedia : IStorageMedia
    {
        bool IStorageMedia.IsFullStorage => true;

        /// <summary>
        /// 将一串文本内容写入存储介质
        /// </summary>
        void WriteAllText(string content);

        /// <summary>
        /// 从存储介质读取所有文本内容
        /// </summary>
        string ReadAllText();

        /// <summary>
        /// 将字节数组写入存储介质
        /// </summary>
        void WriteAllBytes(byte[] bytes);

        /// <summary>
        /// 从存储介质读取所有字节
        /// </summary>
        byte[] ReadAllBytes();

        /// <summary>
        /// 删除存储的数据
        /// </summary>
        void Delete();

        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        bool Exists();
    }

    // 分块/键值存储
    public interface IKVStoreMedia : IStorageMedia
    {
        bool IStorageMedia.IsFullStorage => false;

        /// <summary>
        /// 从存储截止读取对应文本
        /// </summary>
        string GetText(string key);

        /// <summary>
        /// 向存储介质写入指定文本
        /// </summary>
        void SetText(string key, string text);

        /// <summary>
        /// 从存储介质读取对应字节
        /// </summary> 
        byte[] GetBytes(string key);

        /// <summary>
        /// 向存储介质写入指定字节
        /// </summary>
        void SetBytes(string key, byte[] bytes);

        /// <summary>
        /// 删除存储的数据
        /// </summary>
        void Delete(string key);

        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        bool Exists(string key);
    }

    public interface IStorageMedia
    {
        public bool IsFullStorage { get; }
    }
}