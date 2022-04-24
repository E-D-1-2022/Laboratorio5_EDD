using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio5_EDD.Models
{
    public class AppViewModel : Controller
    {
        // GET: AppViewModel
        public ActionResult Index()
        {
            return View();
        }

        // GET: AppViewModel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppViewModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppViewModel/Create
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

        // GET: AppViewModel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppViewModel/Edit/5
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

        // GET: AppViewModel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppViewModel/Delete/5
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
