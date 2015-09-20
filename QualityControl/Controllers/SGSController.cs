using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class SGSController : BaseController
    {
        // GET: SGS
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
                    model.Status = EnumStatus.FirstUncheked;
                    model.CreateTime = DateTime.Now;
                    model.LastChangeTime = model.CreateTime;
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
                if (Util.Util.Equal(model, company, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime" }))
                {
                    return RedirectToAction("Index");
                }
                if (company.Status == EnumStatus.FirstUncheked)
                {
                    Util.Util.Dump(model, company, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "Status" });
                }
                else
                {
                    company.UpdateJson = JsonConvert.SerializeObject(model);
                    company.LastChangeTime = DateTime.Now;
                    company.Status = EnumStatus.Unchecked;
                }
                Db.Entry(company).State = EntityState.Modified;
            }
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}