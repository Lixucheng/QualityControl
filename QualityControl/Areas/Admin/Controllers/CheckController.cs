using Newtonsoft.Json;
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
            var data = new SGSInfo();
            var sgs = Db.SGSs.Find(id);
            if (sgs == null)
            {
                return null;
            }
            sgs.Products = null;
            data.SGS = new SGS();
            Util.Util.Dump(sgs, data.SGS, isnoreId: false);
            data.User = Db.Users.Find(data.SGS.UserId);
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
            var sgs = Db.SGSs.Find(id);
            string msg;
            if (sgs == null)
            {
                return;
            }
            if (isPass)
            {
                if (sgs.Status == EnumStatus.Unchecked)
                {
                    var model = JsonConvert.DeserializeObject<Company>(sgs.UpdateJson);
                    Util.Util.Dump(model, sgs, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "UpdateJson", "Status" });
                    sgs.UpdateJson = null;
                }
                sgs.Status = EnumStatus.Valid;
                msg = "你的机构信息修改申请已经通过审核";
            }
            else
            {
                if (sgs.Status == EnumStatus.Unchecked)
                {
                    sgs.Status = EnumStatus.Valid;
                    sgs.UpdateJson = null;
                }
                msg = "你的机构信息修改申请未通过审核";
            }
            Db.Messages.Add(new Message
            {
                UserId = sgs.UserId,
                Content = msg,
                Status = 0,
                Time = DateTime.Now
            });
            Db.Entry(sgs).State = EntityState.Modified;
            Db.SaveChanges();
        }
        #endregion

        #region CompanyProduct
        public ActionResult CompanyProduct()
        {
            return View(
                Db.Products
                    .Where(a => a.Status == EnumStatus.Unchecked || a.Status == EnumStatus.FirstUncheked)
                    .OrderByDescending(a => a.LastChangeTime)
                    .ToList()
            );
        }

        public string CompanyProductInfo(long id)
        {
            var data = new Product();
            var product = Db.Products.Find(id);
            if (product == null)
            {
                return null;
            }
            Util.Util.Dump(product, data, false);
            product.Company = null;
            data.Type = new ThirdProductType();
            Util.Util.Dump(product.Type, data.Type, false);
            data.Type.SecondType = new SecondProductType();
            Util.Util.Dump(product.Type.SecondType, data.Type.SecondType, false);
            data.Type.SecondType.FirstType = new FirstProductType();
            Util.Util.Dump(product.Type.SecondType.FirstType, data.Type.SecondType.FirstType, false);
            return JsonConvert.SerializeObject(data);
        }

        public void CompanyProductAction(long id, bool isPass)
        {
            var product = Db.Products.Find(id);
            string msg;
            if (product == null)
            {
                return;
            }
            if (isPass)
            {
                if (product.Status == EnumStatus.Unchecked)
                {
                    var model = JsonConvert.DeserializeObject<Company>(product.UpdateJson);
                    Util.Util.Dump(model, product, excepts: new List<string> { "UserId", "CreateTime", "LastChangeTime", "UpdateJson", "Status" });
                    product.UpdateJson = null;
                }
                product.Status = EnumStatus.Valid;
                msg = "你的机构信息修改申请已经通过审核";
            }
            else
            {
                if (product.Status == EnumStatus.Unchecked)
                {
                    product.Status = EnumStatus.Valid;
                    product.UpdateJson = null;
                }
                msg = "你的机构信息修改申请未通过审核";
            }
            Db.Messages.Add(new Message
            {
                UserId = product.UserId,
                Content = msg,
                Status = 0,
                Time = DateTime.Now
            });
            Db.Entry(product).State = EntityState.Modified;
            Db.SaveChanges();
        }
        #endregion
    }
}