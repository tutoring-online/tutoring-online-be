namespace tutoring_online_be.Utils;

public interface IKeyVaultManager
{
    public Task<string> GetSecret(string secretName);
}