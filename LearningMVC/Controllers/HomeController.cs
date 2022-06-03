using LearningMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

namespace LearningMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public  HomeController(IConfiguration config)
        {
            this.configuration = config;
        }
       
        private static IList<PetModel> pets = new List<PetModel>()
        {

            new PetModel(){Id = 1, Name = "Feliks", Type = "Cat", BirthYear = 2015, isAlive = true},
            new PetModel(){Id = 2, Name = "Mazekin", Type = "Dog", BirthYear = 2020, isAlive = true},
            new PetModel(){Id = 3, Name = "Aniela", Type = "Cat", BirthYear = 2010, isAlive = true},
            new PetModel(){Id = 4, Name = "Hiszpan", Type = "Horse", BirthYear = 1999, isAlive = true},
            new PetModel(){Id = 5, Name = "Alex", Type = "Dog", BirthYear = 2004, isAlive = false}
        };
        // GET: HomeController
        public ActionResult Index()
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM Pets;");
            var count = (int)com.ExecuteScalar();
            ViewData["TestData"] = count;
            connection.Close();
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Pets()
        {

            return View(pets);
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View(new PetModel());
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetModel petmodel)
        {
            petmodel.Id = pets.Count + 1;
            pets.Add(petmodel);
            return RedirectToAction(nameof(Index));


        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(pets.FirstOrDefault(x => x.Id == id));
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PetModel petmodel)
        {
            PetModel pet = pets.FirstOrDefault(x => x.Id == id);
            pet.Name = petmodel.Name;
            pet.BirthYear = petmodel.BirthYear;
            pet.isAlive = petmodel.isAlive;
           
            return RedirectToAction(nameof(Index));


        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(pets.FirstOrDefault(x => x.Id == id));
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PetModel petmodel)
        {
            pets.Remove(pets.FirstOrDefault(x => x.Id == id));
            return RedirectToAction(nameof(Index));
        }
    }
}
