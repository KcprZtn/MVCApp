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
        
        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }
        
        private static IList<PetModel> pets = new List<PetModel>(){};
        private static LoginModel account = new LoginModel();

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel acc)
        {
            account.Login = acc.Login;
            account.Password = acc.Password;

            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand com = new SqlCommand($"SELECT Id FROM Accounts WHERE Login = '{acc.Login}' AND Password='{acc.Password}';", connection);
            var log = com.ExecuteScalar();
            
            if (log != null)
            {
                connection.Close();
                account.userId = (int)log;
                return RedirectToAction(nameof(Pets));
            }
            else
            {
                connection.Close();
                return View(nameof(Login));
            }
        }

        //POST : HomeController/LogOut
        [HttpPost]
        public ActionResult LogOut()
        {
            pets.Clear();
            account.userId = 0;
            return RedirectToAction(nameof(Index));
        }
        // GET: HomeController/Register
        public ActionResult Register()
        {
            return View(new LoginModel());
        }

        // POST: HomeController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginModel loginmodel)
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand com = new SqlCommand($"INSERT INTO Accounts(Login,Password) VALUES('{loginmodel.Login}','{loginmodel.Password}');", connection);
            com.ExecuteScalar();
           
            connection.Close();

            return RedirectToAction(nameof(Login));


        }


        // GET: HomeController/Pets
        public ActionResult Pets()
        {
            if(account.userId == 0)
            {
               return RedirectToAction(nameof(Index));
            }
            string connectionstring = configuration.GetConnectionString("DefaultConnection");        
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();

            SqlCommand com = new SqlCommand($"SELECT MAX(Id) FROM Pets WHERE userId = {account.userId};", connection);

            int max = 0;
            try
            {
                max = (int)com.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

            if (!(pets.Count > 0))
            {
                for (int i = 1; i <= max; i++)
                {
                    try
                    {
                        com.CommandText = $"SELECT Id FROM Pets WHERE Id = {i} AND userId={account.userId};";
                        var pId = (int)com.ExecuteScalar();

                        com.CommandText = $"SELECT Name FROM Pets WHERE Id = {i} AND userId={account.userId};";
                        var pName = (string)com.ExecuteScalar();

                        com.CommandText = $"SELECT Type FROM Pets WHERE Id = {i} AND userId={account.userId};";
                        var pType = (string)com.ExecuteScalar();

                        com.CommandText = $"SELECT BirthYear FROM Pets WHERE Id = {i} AND userId={account.userId};";
                        var pBirth = (int)com.ExecuteScalar();

                        com.CommandText = $"SELECT isAlive FROM Pets WHERE Id = {i} AND userId={account.userId};";
                        var pAlive = (bool)com.ExecuteScalar();

                        pets.Add(new PetModel() { Id = pId, Name = pName, Type = pType, BirthYear = pBirth, isAlive = pAlive });
                        
                    }
                    catch (System.Exception e)
                    {
                        System.Console.WriteLine(e);

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
            SqlCommand createCom = new SqlCommand($"INSERT INTO Pets(Name,Type,BirthYear,isAlive,userId) VALUES('{petmodel.Name}','{petmodel.Type}',{petmodel.BirthYear},'{petmodel.isAlive}',{account.userId});", connection);
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

                System.Console.WriteLine(e);
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
