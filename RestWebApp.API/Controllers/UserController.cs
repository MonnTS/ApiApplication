using Microsoft.AspNetCore.Mvc;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IRepositoryWrapper _repo;
    
    public UserController(IRepositoryWrapper repo, RepositoryContext context)
    {
        _repo = repo;
        context.Database.EnsureCreated();
    }
    
    
}