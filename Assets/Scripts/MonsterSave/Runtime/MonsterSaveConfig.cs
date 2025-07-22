using UnityEngine;

namespace MonsterSave.Runtime
{
    public enum Format
    {
        JSON,
        XML,
        Binary,
    }

    public enum Media
    {
        LocalFile,
        PlayerPrefs,
        MemoryOnly,
    }

    public enum Encryption
    {
        None,
        AES,
    }

    [CreateAssetMenu(fileName = "MonsterSaveConfig", menuName = "MonsterSave/MonsterSaveConfig")]
    public class MonsterSaveConfig : ScriptableObject
    {
        [Header("General")] [Tooltip("序列化格式")] public Format format = Format.JSON;
        [Tooltip("存储介质")] public Media media = Media.MemoryOnly;
        [Tooltip("加密格式")] public Encryption encryption = Encryption.None;

        [Tooltip("存储路径（只在使用LocalFile时生效）")] public string storagePath = string.Empty;
        [Tooltip("API配置路径（只在使用云存储时生效）")] public string apiKey = string.Empty;

        [Header("Advanced")] [Tooltip("定时保存")] public bool scheduledSync = false;
        [Tooltip("类型缓存，加速序列化过程")] public bool typeCache = false;
    }
}