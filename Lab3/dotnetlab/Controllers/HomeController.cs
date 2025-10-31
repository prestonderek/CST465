using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Lab4.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Laborers()
    {
        var myWorkforce = new Models.ChoreWorkforce();
        myWorkforce.Laborers.Add(new Models.ChoreLaborer { Name="Alice", Age=3, Difficulty=5});
        myWorkforce.Laborers.Add(new Models.ChoreLaborer { Name="Bob", Age=12, Difficulty=8});
        myWorkforce.Laborers.Add(new Models.ChoreLaborer { Name = "Charlie", Age = 6, Difficulty = 3 });
        myWorkforce.Laborers.Add(new Models.ChoreLaborer { Name = "Diana", Age = 17, Difficulty = 10 });
        for (int i = 0; i < 30; i++)
        {
            var randomLaborer = Extensions.ChoreExtensions.AddRandomLaborer();
            myWorkforce.Laborers.Add(randomLaborer);
        }

        //I tried and tried to do this similar to your instructions but I couldn't get it to work
        //so I wound up doing it this way by creating another ChoreWorkforce and running the LINQ on it to sort
        var filteredWorkforce = new Models.ChoreWorkforce();

        filteredWorkforce.Laborers.AddRange(
            myWorkforce.Laborers
                .Where(l => l != null && l.Age >= 3 && l.Age <= 10 && l.Difficulty <= 7)
                .OrderBy(l => l.Name)
                .ToList()
        );

        return View(filteredWorkforce);
    }
}