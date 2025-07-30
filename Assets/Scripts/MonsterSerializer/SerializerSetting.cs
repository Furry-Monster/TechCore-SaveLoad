namespace MonsterSerializer
{
    public enum Format
    {
        JSON,
        XML
    }

    public enum Encryption
    {
        None,
        AES
    }


    public class SerializerSetting
    {
        public Encryption Encryption = Encryption.None;
        public Format Format = Format.JSON;
    }
}