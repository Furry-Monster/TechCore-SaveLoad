namespace MonsterSave.Runtime
{
    public interface IStorageMedia
    {
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
}