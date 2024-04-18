using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetAnimals(string? orderBy)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        if(orderBy == null)
            command.CommandText = "SELECT * FROM Animal ORDER BY Name ASC;";
        else
        {
            string[] validColumns = { "Name", "Description", "Category", "Area" };
            if (validColumns.Contains(orderBy))
            {
                command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy} ASC;";
            }
            else
            { 
                return null;
            }
        }
        
        var reader = command.ExecuteReader();

        var animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descriptionOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
        }
        connection.Close();
        reader.Close();

        return animals;
    }

    public void AddAnimal(AddAnimal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName, @animalDescription, @animalCategory, @animalArea);";
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);
        
        command.ExecuteNonQuery();
        command.Parameters.Clear();
        
        connection.Close();
    }

    public bool ChangeAnimal(int idAnimal, AddAnimal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal WHERE IdAnimal = @idAnimal;";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);
        
        
        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            reader.Close();
            command.Parameters.Clear();
            return false;
        }
        reader.Close();
        command.Parameters.Clear();
        
        command.CommandText = "UPDATE Animal SET Name = @animalName, " +
                              "Description = @animalDescription, " +
                              "Category = @animalCategory, " +
                              "Area = @animalArea " +
                              "WHERE @idChangeAnimal = IdAnimal;";
        command.Parameters.AddWithValue("@idChangeAnimal", idAnimal);
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);
        
        command.ExecuteNonQuery();
        
        command.Parameters.Clear();
        
        connection.Close();
        
        return true;
    }

    public bool DeleteAnimal(int idAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal WHERE IdAnimal = @idAnimal;";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);
        
        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            command.Parameters.Clear();
            reader.Close();
            return false;
        }
        command.Parameters.Clear();
        reader.Close();

        command.CommandText = "DELETE Animal WHERE IdAnimal = @idAnimal;";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);
        
        command.ExecuteNonQuery();
        
        command.Parameters.Clear();
        
        connection.Close();
        
        return true;
    }
}