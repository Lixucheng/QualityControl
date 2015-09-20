using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    [Authorize]
    public class SGSController : BaseController
    {
        // GET: SGS
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            return View(sgs);
        }

        [HttpGet]
        public ActionResult Save()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            return View(sgs);
        }

        [HttpPost]
        public ActionResult Save(SGS model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.UserId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == model.UserId);
            if (sgs == null)
            {
                model.Status = EnumStatus.FirstUncheked;
                model.CreateTime = DateTime.Now;
                model.LastChangeTime = model.CreateTime;
                Db.SGSs.Add(model);
            }
            else {
                if (Util.Util.Equal(model, sgs, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "Status" }))
                {
                    return RedirectToAction("Index");
                }
                if (sgs.Status == EnumStatus.FirstUncheked)
                {
                    Util.Util.Dump(model, sgs, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "Status" });
                }
                else
                {
                    sgs.UpdateJson = JsonConvert.SerializeObject(model);
                    sgs.LastChangeTime = DateTime.Now;
                    sgs.Status = EnumStatus.Unchecked;
                }
                Db.Entry(sgs).State = EntityState.Modified;
            }
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}