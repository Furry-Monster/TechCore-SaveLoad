namespace MonsterSerializer
{
    public interface IEncryptor
    {
        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] bytes);
    }
}