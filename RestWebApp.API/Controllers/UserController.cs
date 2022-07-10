using Microsoft.AspNetCore.Mvc;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IRepositoryWrapper _repoWrapper;
    
    public UserController(IRepositoryWrapper repoWrapper, RepositoryContext context)
    {
        _repoWrapper = repoWrapper;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _repoWrapper.Users.GetAll();
        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(user == null)
        {
            return NotFound($"User with this id {user.Id} not found");
        }
        
        return Ok(user);
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        if(user == null)
        {
            return BadRequest($"User with id {user.Id} was not found");
        }
        
        _repoWrapper.Users.Create(user);
        _repoWrapper.Save();
        return Ok(user);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        if(user == null || user.Id != id)
        {
            return BadRequest("Object user is null or id is not equal to id in url");
        }
        
        var userToUpdate = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(userToUpdate == null)
        {
            return NotFound($"Car with id {id} not found");
        }
        
        userToUpdate.Name = user.Name;
        userToUpdate.Password = user.Password;
        userToUpdate.Role = user.Role;
        
        _repoWrapper.Users.Update(userToUpdate);
        _repoWrapper.Save();
        return Ok(userToUpdate);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult UserCar(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if (user == null)
        {
            return NotFound($"Car with id {id} not found");
        }
        
        _repoWrapper.Users.Delete(user);
        _repoWrapper.Save();
        return NoContent();
    }
}