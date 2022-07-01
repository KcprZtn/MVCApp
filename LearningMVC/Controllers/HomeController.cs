using LearningMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using System;
using Microsoft.AspNetCore.Http;

namespace LearningMVC.Controllers
{
    public class HomeController : Controller
    {
    
       
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "eu-cdbr-west-02.cleardb.net",
            UserID = "b5fcac75dad61f",
            Password = "fee670d3",
            Database = "heroku_d8dc7e53e550dec",
        };
 

        private static IList<PetModel> pets = new List<PetModel>(){};
        //private static LoginModel account = new LoginModel();

        // GET: Index
        public ActionResult Index()
        {
            pets.Clear();
            return View();
        }

        // GET: HomeController/Login
        public ActionResult Login()
        {
            pets.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel acc)
        {

            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(acc.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            using var com = connection.CreateCommand();
            com.CommandText = $"SELECT Id FROM Accounts WHERE Login = '{acc.Login}' AND Password = '{hash}';";
            var log = com.ExecuteScalar();
            
            if (log != null)
            {
                connection.Close();
                HttpContext.Session.SetInt32("userID", (int)log);              
                ViewData["WrongAcc"] = "";
                return RedirectToAction(nameof(Pets));
            }
            else
            {
                ViewData["WrongAcc"] = "Account not found. Check if username and password are correct.";
                connection.Close();
                return View(nameof(Login));
            }
        }

        //POST : HomeController/LogOut
        [HttpPost]
        public ActionResult LogOut()
        {
            pets.Clear();
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }
        // GET: HomeController/Register
        public ActionResult Register()
        {
            pets.Clear();
            return View(new LoginModel());
        }

        // POST: HomeController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginModel loginmodel)
        {

            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            using var com = connection.CreateCommand();
            com.CommandText = $"SELECT COUNT(*) FROM Accounts WHERE Login='{loginmodel.Login}';";
            long c = (long)com.ExecuteScalar();
            if (c == 0)
            {
                ViewData["DuplicateError"] = "";
                byte[] data = System.Text.Encoding.ASCII.GetBytes(loginmodel.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                String hash = System.Text.Encoding.ASCII.GetString(data);
                com.CommandText = $"INSERT INTO Accounts(Login,Password) VALUES('{loginmodel.Login}','{hash}');";
                com.ExecuteScalar();

                connection.Close();
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewData["DuplicateError"] = "Choose different login.";
                return View(nameof(Register));
            }

        }


        // GET: HomeController/Pets
        public ActionResult Pets()
        {
            if(HttpContext.Session.GetInt32("userID") == null)
            {
               return RedirectToAction(nameof(Index));
            }

            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            using var com = connection.CreateCommand();
            com.CommandText = $"SELECT MAX(Id) FROM Pets WHERE userId = {HttpContext.Session.GetInt32("userID")};";


            if (!(pets.Count > 0))
            {
                com.CommandText = $"SELECT Id,Name,Type,BirthYear,isAlive FROM Pets WHERE userId={HttpContext.Session.GetInt32("userID")};";
                using var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var pId = reader.GetInt32("Id");
                    var pName = reader.GetString("Name");
                    var pType = reader.GetString("Type");
                    var pBirth = reader.GetInt32("BirthYear");
                    var pAlive = reader.GetBoolean("isAlive");
                    pets.Add(new PetModel() { Id = pId, Name = pName, Type = pType, BirthYear = pBirth, isAlive = pAlive });
                }
                
            }
         
            connection.Close();


            return View(pets);
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new PetModel());
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetModel petmodel)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            using var com = connection.CreateCommand();
            int alive = petmodel.isAlive ? 1 : 0;
                
            com.CommandText = $"INSERT INTO Pets(Name,Type,BirthYear,isAlive,userId) VALUES('{petmodel.Name}','{petmodel.Type}',{petmodel.BirthYear},'{alive}',{HttpContext.Session.GetInt32("userID")});";
            com.ExecuteNonQuery();
            pets.Add(petmodel);
            connection.Close();
           
            return RedirectToAction(nameof(Pets));


        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetInt32("userID")==null)
            {
                return RedirectToAction(nameof(Index));
            }
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
                int alive = petmodel.isAlive ? 1 : 0;

                using var connection = new MySqlConnection(builder.ConnectionString);

                connection.Open();
                using var com = connection.CreateCommand();
                com.CommandText = $"UPDATE Pets SET Name = '{petmodel.Name}', Type = '{petmodel.Type}', BirthYear = {petmodel.BirthYear}, isAlive = '{alive}'  WHERE Id = {id};";
                com.ExecuteScalar();
                connection.Close();
            }
            catch (System.Exception e)
            {

                System.Console.WriteLine(e);
            }
           
            return RedirectToAction(nameof(Pets));


        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(pets.FirstOrDefault(x => x.Id == id));
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PetModel petmodel)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            using var com = connection.CreateCommand();
            com.CommandText = $"DELETE FROM Pets WHERE Id = {id};";
            com.ExecuteNonQuery();
            pets.Remove(pets.FirstOrDefault(x => x.Id == id));

            connection.Close();
            return RedirectToAction(nameof(Pets));
        }
    }
}
