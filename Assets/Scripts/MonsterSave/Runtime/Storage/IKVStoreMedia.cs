namespace MonsterSave.Runtime
{
    /// <summary>
    /// 分块/键值存储
    /// </summary>
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
}