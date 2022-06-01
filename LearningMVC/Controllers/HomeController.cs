using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningMVC.Models;
namespace LearningMVC.Controllers
{
    public class HomeController : Controller
    {
        private static IList<PetModel> pets = new List<PetModel>()
        {
            new PetModel(){Name = "Feliks", Type = "Cat", BirthYear = 2015, Alive = true},
            new PetModel(){Name = "Mazekin", Type = "Dog", BirthYear = 2020, Alive = true},
            new PetModel(){Name = "Aniela", Type = "Cat", BirthYear = 2010, Alive = true},
            new PetModel(){Name = "Hiszpan", Type = "Horse", BirthYear = 1999, Alive = true},
            new PetModel(){Name = "Alex", Type = "Dog", BirthYear = 2004, Alive = false}
        };
        // GET: HomeController
        public ActionResult Index()
        {
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
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
