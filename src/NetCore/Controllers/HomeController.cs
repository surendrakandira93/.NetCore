using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCore.DataTables;
using NetCore.Models;
using NetCore.Services;
using Newtonsoft.Json;

namespace NetCore.Controllers
{
    public class HomeController : Controller
    {
        private IStudentRepository _repogistory;

        public HomeController(IStudentRepository studentRepo)
        {
            _repogistory = studentRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var studenList = _repogistory.GetAll().ToList();
            // var total = _repogistory.Count();

            return View(studenList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _repogistory.Max() + 1;
                _repogistory.Add(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = _repogistory.Get(id);
            if (model == null)
            {
                RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                _repogistory.Update(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            _repogistory.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DatatableGet()
        {
            DataTablesBinder binder = new DataTablesBinder();
            DefaultDataTablesRequest requestModel = (DefaultDataTablesRequest)binder.BindModel(this.HttpContext);
            var setting = new JsonSerializerSettings();
            setting.Culture = CultureInfo.CurrentCulture;
            var result = this.GetFilterResponse(requestModel);
            return this.Json(result, setting);
        }

        private dynamic GetFilterResponse(DefaultDataTablesRequest requestModel)
        {
            var result = _repogistory.GetAll();

            return new DataTablesResponse(requestModel.Draw, result, result.Count(), result.Count());
        }
    }
}