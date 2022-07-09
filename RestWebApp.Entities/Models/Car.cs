using System.ComponentModel.DataAnnotations;

namespace RestWebApp.Entities.Models;

public class Car
{
    private DateTime _dateTime;
    
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Brand {get; set; }
    
    [Required]
    public string Description {get; set; }

    [Required]
    public string Date
    {
        get => _dateTime.ToString("dd-MM-yyyy");
        set
        {
            if (value == _dateTime.ToString("dd-MM-yyyy"))
            {
                return;
            }
            
            var correctDateTime = DateTime.TryParse(value, out _dateTime);

            if (!correctDateTime)
            {
                _dateTime = DateTime.Now;
            }
        }
    }
}