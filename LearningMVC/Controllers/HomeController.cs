using LearningMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
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
        
        private static IList<PetModel> pets = new List<PetModel>(){};

        // GET: HomeController
        public ActionResult Index()
        {
           
            return View();
        }

        // GET: HomeController/Pets
        public ActionResult Pets()
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand com = new SqlCommand("SELECT MAX(Id) FROM Pets;", connection);
            var max = (int)com.ExecuteScalar();

            if (!(pets.Count > 0))
            {
                for (int i = 1; i <= max; i++)
                {
                    try
                    {
                        SqlCommand IdCom = new SqlCommand($"SELECT Id FROM Pets WHERE Id = {i}", connection);
                        SqlCommand NameCom = new SqlCommand($"SELECT Name FROM Pets WHERE Id = {i}", connection);
                        SqlCommand TypeCom = new SqlCommand($"SELECT Type FROM Pets WHERE Id = {i}", connection);
                        SqlCommand BirthCom = new SqlCommand($"SELECT BirthYear FROM Pets WHERE Id = {i}", connection);
                        SqlCommand AliveCom = new SqlCommand($"SELECT isAlive FROM Pets WHERE Id = {i}", connection);

                        var pId = (int)IdCom.ExecuteScalar();
                        var pName = (string)NameCom.ExecuteScalar();
                        var pType = (string)TypeCom.ExecuteScalar();
                        var pBirth = (int)BirthCom.ExecuteScalar();
                        var pAlive = (bool)AliveCom.ExecuteScalar();

                        pets.Add(new PetModel() { Id = pId, Name = pName, Type = pType, BirthYear = pBirth, isAlive = pAlive });
                        
                    }
                    catch (System.Exception e)
                    {
                        

                    }

                }
            }
         
            connection.Close();


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
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand createCom = new SqlCommand($"INSERT INTO Pets(Name,Type,BirthYear,isAlive) VALUES('{petmodel.Name}','{petmodel.Type}',{petmodel.BirthYear},'{petmodel.isAlive}');", connection);
            createCom.ExecuteScalar();
            pets.Add(petmodel);
            connection.Close();
           
            return RedirectToAction(nameof(Pets));


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

            try
            {
                PetModel pet = pets.FirstOrDefault(x => x.Id == id);
                pet.Name = petmodel.Name;
                pet.Type = petmodel.Type;
                pet.BirthYear = petmodel.BirthYear;
                pet.isAlive = petmodel.isAlive;


                string connectionstring = configuration.GetConnectionString("DefaultConnection");
                SqlConnection connection = new SqlConnection(connectionstring);

                connection.Open();
                SqlCommand createCom = new SqlCommand($"UPDATE Pets SET Name = '{petmodel.Name}', Type = '{petmodel.Type}', BirthYear = {petmodel.BirthYear}, isAlive = '{petmodel.isAlive}'  WHERE Id = {id};", connection);
                createCom.ExecuteScalar();
                connection.Close();
            }
            catch (System.Exception e)
            {
               
                
            }
           
            return RedirectToAction(nameof(Pets));


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
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand deleteCom = new SqlCommand($"DELETE FROM Pets WHERE Id = {id}",connection);
            deleteCom.ExecuteScalar();
            pets.Remove(pets.FirstOrDefault(x => x.Id == id));

            connection.Close();
            return RedirectToAction(nameof(Pets));
        }
    }
}
