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

    public enum TypeAdapt
    {
        None,
        Auto,
    }

    public class SerializerSetting
    {
        public TypeAdapt TypeAdapt = TypeAdapt.None;
        public Encryption Encryption = Encryption.None;
        public Format Format = Format.JSON;
    }
}