namespace MonsterSerializer
{
    public enum Format
    {
        JSON,
        XML,
    }

    public enum Encryption
    {
        None,
        AES,
    }

    public class SerializerSetting
    {
        public Format Format = Format.JSON;
        public Encryption Encryption = Encryption.None;
    }
}