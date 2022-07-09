namespace RestWebApp.Contracts;

public interface IRepositoryWrapper
{
    ICarRepository Cars { get; }
    IUserRepository Users { get; }
    void Save();
}