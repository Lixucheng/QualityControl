using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult Save()
        {
            var userId = User.Identity.GetUserId();
            var company = Db.Companies.FirstOrDefault(a => a.UserId == userId);
            return View(company);
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
                var company = Db.Companies.FirstOrDefault(a => a.UserId == model.UserId);
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
                if (Util.Util.Equal(model, company, excepts: new List<string> { "UserId" }))
                {
                    return RedirectToAction("Index");
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