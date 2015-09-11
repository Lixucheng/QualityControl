using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class ProductController : BaseController
    {
        #region  产品种类部分
        // GET: Product
        /// <summary>
        /// 产品类别界面
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeIndex()
        {
            var list = Db.ProductTypes.ToList();
            list.ForEach(e => {
                if(e.Description.Length>40)
                e.Description = e.Description.Substring(0, 40) + "...";
            });
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        /// <summary>
        /// 根据id返回整体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTypeInfo(long id)
        {
            var r = Db.ProductTypes.Find(id);
            var ret = new { Title = r.Title, Id = r.Id, Description = r.Description };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(Db.ProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
               throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.ProductTypes.Find(newone.Id);
            if(x!=null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品");}
            return Redirect("./TypeIndex");
        }

        public ActionResult Add(Db.ProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            Db.ProductTypes.Add(newone);
            Db.SaveChanges();
            return Redirect("./TypeIndex");
        }

        public ActionResult Del(long id)
        {
            var x = Db.ProductTypes.Find(id);
            if (x != null)
            {
                Db.ProductTypes.Remove(x);
                Db.SaveChanges();
            }
            return Redirect("../TypeIndex");
        }

        public ActionResult _Options()
        {
            var list=Db.ProductTypes.ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }


        #region productType 逻辑函数

        public bool CheckNewProduct(Db.ProductType newone)
        {
            if (string.IsNullOrEmpty(newone.Title) || string.IsNullOrEmpty(newone.Description))
            {
                return false;
            }
            var x = Db.ProductTypes.Count(e => e.Title == newone.Title&&e.Id!=newone.Id);
            return x <= 0;
        }



        #endregion
        #endregion

        #region  生产商产品部分

        public ActionResult CompanyProductIndex(long cid)
        {
            var list = Db.CompanyProducts.Where(e=>e.CompanyId==cid).ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        public ActionResult CpDel(long id)
        {
            var x = Db.CompanyProducts.Find(id);
            if (x != null)
            {
                Db.CompanyProducts.Remove(x);
                Db.SaveChanges();
            }
            return Redirect("../CompanyProductIndex");
        }

        public JsonResult GetCpInfo(long id)
        {
            var r = Db.CompanyProducts.Find(id);
            var ret = new { cp=r };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CpEdit(Db.CompanyProduct newone)
        {
            if (!CheckCp(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.CompanyProducts.Find(newone.Id);
            if (x != null)
            {
                x.Name = newone.Name;
                x.CompanyId = newone.CompanyId;
                x.CompanyProductStatus = newone.CompanyProductStatus;
                x.GetDate = newone.GetDate;
                x.ProductionCertificateNo = newone.ProductionCertificateNo;
                x.Standard = newone.Standard;
                x.ProductTypeId = newone.ProductTypeId;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品"); }
            return Redirect("./CompanyProductIndex");
        }

        public ActionResult CpAdd(Db.CompanyProduct newone)
        {
            if (!CheckCp(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            Db.CompanyProducts.Add(newone);
            Db.SaveChanges();
            return Redirect("./CompanyProductIndex");
        }

        public bool CheckCp(Db.CompanyProduct cp)
        {         
            if (string.IsNullOrEmpty(cp.GetDate) && string.IsNullOrEmpty(cp.Name) &&
                string.IsNullOrEmpty(cp.ProductionCertificateNo) &&
                string.IsNullOrEmpty(cp.Standard))
            {
                return false;}
            var x = Db.Companies.Find(cp.CompanyId);
            if (x == null)
            {
                return false;
            }

            var p = Db.CompanyProducts.FirstOrDefault(e => e.Name == cp.Name&&e.Id!=cp.Id);
            if (p != null)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}