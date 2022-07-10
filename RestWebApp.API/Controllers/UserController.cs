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
    private readonly ILogger<UserController> _logger;
    
    public UserController(IRepositoryWrapper repoWrapper, RepositoryContext context,
        ILogger<UserController> logger)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _repoWrapper.Users.GetAll();
        _logger.LogInformation("Returned all users from database.");
        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(user == null)
        {
            _logger.LogWarning("User with id: " + id + " was not found in the database.");
            return NotFound($"User with this id {user.Id} not found");
        }
        
        _logger.LogInformation("Returned user with id: " + id + " from database.");
        return Ok(user);
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        _repoWrapper.Users.Create(user);
        _repoWrapper.Save();
        _logger.LogInformation("User with id: " + user.Id + " was created in the database.");
        return Ok(user);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        if(user.Id != id)
        {
            _logger.LogError($"Wrong Id provided in the request. Expected Id: {id} and provided Id: {user.Id}");
            return BadRequest($"Id of object {typeof(User)} is not equal to id in url.");
        }
        
        var userToUpdate = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(userToUpdate == null)
        {
            _logger.LogWarning("User with id: " + id + " was not found in the database.");
            return NotFound($"User with id {id} not found");
        }
        
        userToUpdate.Name = user.Name;
        userToUpdate.Password = user.Password;
        userToUpdate.Role = user.Role;
        
        _repoWrapper.Users.Update(userToUpdate);
        _repoWrapper.Save();
        return Ok(userToUpdate);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if (user == null)
        {
            _logger.LogWarning("User with id: " + id + " was not found in the database.");
            return NotFound($"User with id {id} not found");
        }
        
        _repoWrapper.Users.Delete(user);
        _repoWrapper.Save();
        _logger.LogInformation("User with id: " + id + " was deleted from the database.");
        return NoContent();
    }
}