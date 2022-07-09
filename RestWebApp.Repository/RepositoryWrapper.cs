using RestWebApp.Contracts;
using RestWebApp.Entities;

namespace RestWebApp.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private RepositoryContext _context;
    private ICarRepository _car;
    private IUserRepository _user;

    public ICarRepository Cars => _car ??= new CarRepository(_context);
    public IUserRepository Users => _user ??= new UserRepository(_context);

    public RepositoryWrapper(RepositoryContext context)
    {
        _context = context;
    }
    
    public void Save()
    {
        _context.SaveChanges();
    }
}