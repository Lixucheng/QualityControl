﻿using Newtonsoft.Json;
using QualityControl.Controllers;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Models.Adapters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Areas.Admin.Controllers
{
    public class CheckController : BaseController
    {
        public CheckController() { }

        public CheckController(ApplicationUserManager userManager) : base(userManager) { }

        #region Company
        public ActionResult Company()
        {
            return View(
                Db.Companies
                    .Join(Db.Users, a => a.UserId, b => b.Id, (com, acc) => new CompanyInfo
                    {
                        Company = com,
                        User = acc
                    })
                    .Where(a => a.Company.Status == EnumStatus.Unchecked || a.Company.Status == EnumStatus.FirstUncheked)
                    .OrderByDescending(a => a.Company.LastChangeTime)
                    .ToList()
            );
        }

        public string CompanyInfo(long id)
        {
            var data = new CompanyInfo();
            var company = Db.Companies.Find(id);
            if (company == null)
            {
                return null;
            }
            company.Products = null;
            data.Company = new Company();
            Util.Util.Dump(company, data.Company, isnoreId: false);
            data.User = Db.Users.Find(data.Company.UserId);
            data.User = new Models.ApplicationUser()
            {
                Id = data.User.Id,
                Email = data.User.Email,
                UserName = data.User.UserName,
            };

            return JsonConvert.SerializeObject(data);
        }

        public void CompanyAction(long id, bool isPass)
        {
            var company = Db.Companies.Find(id);
            string msg;
            if (company == null)
            {
                return;
            }
            if (isPass)
            {
                if (company.Status == EnumStatus.Unchecked)
                {
                    var model = JsonConvert.DeserializeObject<Company>(company.UpdateJson);
                    Util.Util.Dump(model, company, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "UpdateJson", "Status" });
                    company.UpdateJson = null;
                }
                company.Status = EnumStatus.Valid;
                msg = "你的企业信息修改申请已经通过审核";
            }
            else
            {
                if (company.Status == EnumStatus.Unchecked)
                {
                    company.Status = EnumStatus.Valid;
                    company.UpdateJson = null;
                }
                msg = "你的企业信息修改申请未通过审核";
            }
            Db.Messages.Add(new Message
            {
                UserId = company.UserId,
                Content = msg,
                Status = 0,
                Time = DateTime.Now
            });
            Db.Entry(company).State = EntityState.Modified;
            Db.SaveChanges();
        }
        #endregion

        #region SGS
        public ActionResult SGS()
        {
            return View(
                Db.SGSs
                    .Join(Db.Users, a => a.UserId, b => b.Id, (sgs, acc) => new SGSInfo
                    {
                        SGS = sgs,
                        User = acc
                    })
                    .Where(a => a.SGS.Status == EnumStatus.Unchecked || a.SGS.Status == EnumStatus.FirstUncheked)
                    .OrderByDescending(a => a.SGS.LastChangeTime)
                    .ToList()
            );
        }

        public string SGSInfo(long id)
        {
            var data = new CompanyInfo();
            var sgs = Db.SGSs.Find(id);
            if (sgs == null)
            {
                return null;
            }
            sgs = null;
            data.Company = new Company();
            Util.Util.Dump(sgs, data.Company, isnoreId: false);
            data.User = Db.Users.Find(data.Company.UserId);
            data.User = new Models.ApplicationUser()
            {
                Id = data.User.Id,
                Email = data.User.Email,
                UserName = data.User.UserName,
            };

            return JsonConvert.SerializeObject(data);
        }

        public void SGSAction(long id, bool isPass)
        {
            var company = Db.Companies.Find(id);
            string msg;
            if (company == null)
            {
                return;
            }
            if (isPass)
            {
                if (company.Status == EnumStatus.Unchecked)
                {
                    var model = JsonConvert.DeserializeObject<Company>(company.UpdateJson);
                    Util.Util.Dump(model, company, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "UpdateJson", "Status" });
                    company.UpdateJson = null;
                }
                company.Status = EnumStatus.Valid;
                msg = "你的企业信息修改申请已经通过审核";
            }
            else
            {
                if (company.Status == EnumStatus.Unchecked)
                {
                    company.Status = EnumStatus.Valid;
                    company.UpdateJson = null;
                }
                msg = "你的企业信息修改申请未通过审核";
            }
            Db.Messages.Add(new Message
            {
                UserId = company.UserId,
                Content = msg,
                Status = 0,
                Time = DateTime.Now
            });
            Db.Entry(company).State = EntityState.Modified;
            Db.SaveChanges();
        }
        #endregion
    }
}