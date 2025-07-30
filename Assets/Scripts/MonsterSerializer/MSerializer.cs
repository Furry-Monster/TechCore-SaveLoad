using System;

namespace MonsterSerializer
{
    public static class MSerializer
    {
        public static byte[] Serialize<T>(T obj, SerializerSetting settings = null)
        {
            if (settings != null)
            {
                switch (settings.Format)
                {
                    case Format.JSON:
                        SerializerMgr.Instance.SetSerializer(new JSONSerializer());
                        break;
                    case Format.XML:
                        SerializerMgr.Instance.SetSerializer(new XMLSerializer());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (settings.Encryption)
                {
                    case Encryption.None:
                        SerializerMgr.Instance.SetEncryptor(null);
                        break;
                    case Encryption.AES:
                        SerializerMgr.Instance.SetEncryptor(new AESEncryptor());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (settings.TypeAdapt)
                {
                    case TypeAdapt.None:
                        SerializerMgr.Instance.SetTypeAdapter(null);
                        break;
                    case TypeAdapt.Auto:
                        SerializerMgr.Instance.SetTypeAdapter(new AutoTypeAdapter());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return SerializerMgr.Instance.Serialize(obj);
        }

        public static T Deserialize<T>(byte[] data, SerializerSetting settings = null)
        {
            if (settings != null)
            {
                switch (settings.Format)
                {
                    case Format.JSON:
                        SerializerMgr.Instance.SetSerializer(new JSONSerializer());
                        break;
                    case Format.XML:
                        SerializerMgr.Instance.SetSerializer(new XMLSerializer());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (settings.Encryption)
                {
                    case Encryption.None:
                        SerializerMgr.Instance.SetEncryptor(null);
                        break;
                    case Encryption.AES:
                        SerializerMgr.Instance.SetEncryptor(new AESEncryptor());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (settings.TypeAdapt)
                {
                    case TypeAdapt.None:
                        SerializerMgr.Instance.SetTypeAdapter(null);
                        break;
                    case TypeAdapt.Auto:
                        SerializerMgr.Instance.SetTypeAdapter(new AutoTypeAdapter());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (T)SerializerMgr.Instance.Deserialize(typeof(T), data);
        }
    }
}