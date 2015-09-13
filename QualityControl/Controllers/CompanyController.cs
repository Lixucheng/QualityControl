using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        // GET: Company
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var company = Db.Companies.FirstOrDefault(a => a.UserId == userId);
            return View(company);
        }

        [HttpGet]
        public ActionResult Save(long id = 0, Company model = null)
        {
            return View(model == null ? Db.Companies.Find(id): model);
        }

        [HttpPost]
        public ActionResult Save(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Id == 0)
            {
                model.UserId = User.Identity.GetUserId();
                var company = Db.Companies.FirstOrDefault(a => a.UserId == User.Identity.GetUserId());
                if (company != null)
                {
                    model.Id = company.Id;
                }
                else
                {
                    Db.Companies.Add(model);
                }
            }
            if (model.Id != 0)
            {
                var company = Db.Companies.Find(model.Id);
                if (company == null)
                {
                    return View();
                }
                company.UpdateJson = JsonConvert.SerializeObject(model);
                company.Status = EnumStatus.Unchecked;
                Db.Entry(company).State = EntityState.Modified;
            }
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}