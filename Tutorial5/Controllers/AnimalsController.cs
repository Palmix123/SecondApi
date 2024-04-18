using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;
using Tutorial5.Repositories;

namespace Tutorial5.Controllers;

[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalsController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    [HttpGet]
    [Route("api/animals")]
    public IActionResult GetAnimals(string? orderBY)
    {
        var animals = _animalRepository.GetAnimals(orderBY);

        return Ok(animals);
    }

    [HttpPost]
    [Route("api/animals")]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        _animalRepository.AddAnimal(animal);
        
        return Created("", null);
    }
    
    [HttpPut]
    [Route("api/animals/{idAnimal:int}")]
    public IActionResult ChangeAnimal(int idAnimal, AddAnimal animal)
    {
        bool success = _animalRepository.ChangeAnimal(idAnimal, animal);
        
        if(success)
            return Ok("Animal has been successfully changed");
        
        return NotFound("Animal with id = " + idAnimal + " doesn't exist");
    }
    
    [HttpDelete]
    [Route("api/animals/{idAnimal:int}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        bool success = _animalRepository.DeleteAnimal(idAnimal);
        
        if(success)
            return Ok("Animal has been successfully deleted");
        
        return NotFound("Animal with id = " + idAnimal + " doesn't exist");
    }
}