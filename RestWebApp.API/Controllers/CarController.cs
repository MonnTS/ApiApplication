using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private IRepositoryWrapper _repoWrapper;

    public CarController(IRepositoryWrapper repoWrapper, RepositoryContext context)
    {
        _repoWrapper = repoWrapper;
        context.Database.EnsureCreated();
    }
    
    
}