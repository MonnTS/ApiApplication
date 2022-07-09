using RestWebApp.Entities.Models;
using RestWebApp.Entities;
using RestWebApp.Contracts;

namespace RestWebApp.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext context) : base(context)
    { }
}