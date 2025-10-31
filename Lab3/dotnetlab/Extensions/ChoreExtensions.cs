using AspNetCoreGeneratedDocument;
using Lab4.Controllers;
using Lab4.Models;

namespace Lab4.Extensions;

public static class ChoreExtensions
{
    public static ChoreLaborer AddLaborer(string name, int age, int difficulty)
    {
        var laborer = new Models.ChoreLaborer { Name = name, Age = age, Difficulty = difficulty };
        return laborer;
    }
    
    public static ChoreLaborer AddRandomLaborer()
    {
        var random = new Random();
        var names = new List<string> { "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack", "Kathy", "Leo", "Mona", "Nate", "Olivia" };
        var name = names[random.Next(names.Count)];
        var age = random.Next(4, 18);
        var difficulty = random.Next(0, 10);
        if (difficulty == 10)
        {
            return null;
        }
        
        var laborer = new Models.ChoreLaborer { Name = name, Age = age, Difficulty = difficulty };
        return laborer;
    }
}


