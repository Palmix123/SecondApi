using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Repositories;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAnimals(string? orderBy);
    void AddAnimal(AddAnimal animal);
    bool ChangeAnimal(int idAnimal, AddAnimal animal);
    bool DeleteAnimal(int idAnimal);
}