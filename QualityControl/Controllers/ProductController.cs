using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QualityControl.Db;
using QualityControl.Enum;
using Newtonsoft.Json;
using System.Data.Entity;

namespace QualityControl.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {


        #region 产品种类
       

        #region 第一级
        public ActionResult FirstTypeIndex()
        {
            var list = Db.FirstProductTypes.ToList();
            list.ForEach(e => {
                if (e.Description.Length > 40)
                    e.Description = e.Description.Substring(0, 40) + "...";
            });
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        public ActionResult FirstEdit(FirstProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.FirstProductTypes.Find(newone.Id);
            if (x != null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品"); }
            return Redirect("./FirstTypeIndex");
        }

        public ActionResult FirstAdd(Db.FirstProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            Db.FirstProductTypes.Add(newone);
            Db.SaveChanges();
            return Redirect("./FirstTypeIndex");
        }


        #endregion

        #region 第二级
        public ActionResult SecondTypeIndex(long id)
        {
            var list = Db.FirstProductTypes.Find(id).SecondProductTypes;
            list.ForEach(e => {
                if (e.Description.Length > 40)
                    e.Description = e.Description.Substring(0, 40) + "...";
            });
            ViewBag.list = list;
            ViewBag.count = list.Count;
            ViewBag.id = id;
            return View();
        }

        public ActionResult SecondEdit(Db.SecondProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.SecondProductTypes.Find(newone.Id);
            if (x != null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.Entry(x).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品"); }
            return Redirect("./SecondTypeIndex/"+x.FirstType.Id);
        }
                                        
        public ActionResult SecondAdd (Db.SecondProductType newone,long fid)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
          
            Db.FirstProductTypes.Find(fid).SecondProductTypes.Add(newone);            
            Db.SaveChanges();
            return Redirect("./SecondTypeIndex/"+fid);
        }
        #endregion

        #region  第三级
        // GET: Product
        /// <summary>
        /// 产品类别界面
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeIndex(long id)
        {
            ViewBag.id = id;
            var list = Db.SecondProductTypes.Find(id).Productypes;
            list.ForEach(e => {
                if (e.Description.Length > 40)
                    e.Description = e.Description.Substring(0, 40) + "...";
            });
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        public ActionResult Edit(Db.ThirdProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.ThirdProductTypes.Find(newone.Id);
            if (x != null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品"); }
            return Redirect("./TypeIndex/"+x.SecondType.Id);
        }

        public ActionResult Add(Db.ThirdProductType newone,long fid)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            Db.SecondProductTypes.Find(fid).Productypes.Add(newone);
            Db.SaveChanges();
            return Redirect("./TypeIndex/"+fid);
        }



        #endregion

        #region 种类公共部分

        /// <summary>
        /// 根据id返回整体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetTypeInfo(long id,int type)
        {

            switch (type)
            {
                case 1:
                {
                        var r = Db.FirstProductTypes.Find(id);
                        var ret = new { Title = r.Title, Id = r.Id, Description = r.Description };
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    } 
                case 2:
                {
                        var r = Db.SecondProductTypes.Find(id);
                        var ret = new { Title = r.Title, Id = r.Id, Description = r.Description };
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }
                case 3:
                {
                        var r = Db.ThirdProductTypes.Find(id);
                        var ret = new { Title = r.Title, Id = r.Id, Description = r.Description };
                        return Json(ret, JsonRequestBehavior.AllowGet);

                }
                default:
                    throw new Exception("访问错误");
                  
            }
          
        }


        public ActionResult Del(long id,int type)
        {
            switch (type)
            {
                case 1:
                {
                        var x = Db.FirstProductTypes.Find(id);
                        if (x != null)
                        {
                            Db.FirstProductTypes.Remove(x);
                            Db.SaveChanges();
                        }
                        return Redirect("./FirstTypeIndex");
                    }
                case 2:
                {
                        var x = Db.SecondProductTypes.Find(id);
                        var n = x.FirstType.Id;
                        if (x != null)
                        {
                            Db.SecondProductTypes.Remove(x);
                            Db.SaveChanges();
                        }
                        return Redirect("./SecondTypeIndex/"+n);
                    }
                case 3:
                {
                        var x = Db.ThirdProductTypes.Find(id);
                        var n = x.SecondType.Id;
                        if (x != null)
                        {
                            Db.ThirdProductTypes.Remove(x);
                            Db.SaveChanges();
                        }
                        return Redirect("./TypeIndex/"+n);
                    }
                default:
                {
                    throw new Exception("访问错误！");
                }
            }
          
        }


        public ActionResult _Options(int type,long fatherid=0)
        {

            switch (type)
            {
                case 1:
                {
                    var thislist = Db.FirstProductTypes.ToList();
                    ViewBag.thislist = thislist;
                    ViewBag.thiscount = thislist.Count;
                        break;
                }
                case 2:
                {
                    var thislist = Db.FirstProductTypes.Find(fatherid).SecondProductTypes;
                    ViewBag.thislist = thislist;
                    ViewBag.thiscount = thislist.Count;
                        break;
                    }
                case 3:
                    {
                        var thislist = Db.SecondProductTypes.Find(fatherid).Productypes;
                        ViewBag.thislist = thislist;
                        ViewBag.thiscount = thislist.Count;
                        break;
                    }
                default:
                    throw new Exception("访问种类错误！");
            }

            
            return View();
        }

        public ActionResult _ChooseType(string name="s")
        {
            ViewBag.id = name;
            return View();
        }

        public JsonResult GetType(int type, long fatherid)
        {

            switch (type)
            {
                case 2:
                    {
                        var thislist = Db.FirstProductTypes.Find(fatherid).SecondProductTypes.Select(e => new {id = e.Id, name = e.Title});
                        return Json(thislist, JsonRequestBehavior.AllowGet);
                       
                    }
                case 3:
                    {
                        var thislist = Db.SecondProductTypes.Find(fatherid).Productypes.Select(e => new { id = e.Id, name = e.Title });

                        return Json(thislist, JsonRequestBehavior.AllowGet);
                    }
                default:
                    throw new Exception("访问种类错误！");
            }

        }

        public bool CheckNewProduct(Db.ThirdProductType newone)
        {
            if (string.IsNullOrEmpty(newone.Title) || string.IsNullOrEmpty(newone.Description))
            {
                return false;
            }
            var x = Db.ThirdProductTypes.Count(e => e.Title == newone.Title && e.Id != newone.Id);
            return x <= 0;
        }
        public bool CheckNewProduct(Db.FirstProductType newone)
        {
            if (string.IsNullOrEmpty(newone.Title) || string.IsNullOrEmpty(newone.Description))
            {
                return false;
            }
            var x = Db.ThirdProductTypes.Count(e => e.Title == newone.Title && e.Id != newone.Id);
            return x <= 0;
        }
        public bool CheckNewProduct(Db.SecondProductType newone)
        {
            if (string.IsNullOrEmpty(newone.Title) || string.IsNullOrEmpty(newone.Description))
            {
                return false;
            }
            var x = Db.ThirdProductTypes.Count(e => e.Title == newone.Title && e.Id != newone.Id);
            return x <= 0;
        }

        #endregion

        #endregion


        #region  生产商产品部分

        public ActionResult CompanyProductIndex(long cid)
        {
            var c = Db.Companies.Find(cid);
            if (c == null)
            {
                throw new Exception("生产商不存在");
            }

            var list = c.Products;
            var list2=list.Select(e =>new Cp
            {
                Id=e.Id,
                CompanyName=c.Name,
                Name=e.Name,
                ProductTypeName=e.Type.Title,
                ProductionCertificateNo=e.ProductionCertificateNo,
                GetDate=e.GetDate,
                Standard=e.Standard,
                //CompanyProductStatus= e.Status               
            }).ToList();
          
            ViewBag.list = list2;
            ViewBag.count = list2.Count;
            ViewBag.cid = cid;
            return View();
        }

        public ActionResult CpDel(long id)
        {
            var x = Db.Products.Find(id);
            var cid = x.Company.Id;
            if (x != null)
            {
                Db.Products.Remove(x);
                Db.SaveChanges();
            }
            return Redirect("../CompanyProductIndex?cid="+cid);
        }

        public JsonResult GetCpInfo(long id)
        {
            var e = Db.Products.Find(id);
            var name = e.Type.Title;
            var cpx = new Cp
            {
                Id = e.Id,
                CompanyName = e.Company.Name,
                Name = e.Name,
                ProductTypeName = e.Type.Title,
                ProductionCertificateNo = e.ProductionCertificateNo,
                GetDate = e.GetDate,
                Standard = e.Standard,
                //CompanyProductStatus = e.Status
            };
            var ret = new { cp=cpx ,tname=name,status=e.Status};
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CpEdit(Db.Product newone)
        {
            if (!CheckCp(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.Products.Find(newone.Id);
            if (x != null)
            {
                x.Name = newone.Name;
                x.Price = newone.Price;
                x.Status = newone.Status;
                x.GetDate = newone.GetDate;
                x.ProductionCertificateNo = newone.ProductionCertificateNo;
                x.Standard = newone.Standard;              
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品"); }
            return Redirect("./CompanyProductIndex?cid=" + x.Company.Id);
        }

        public ActionResult CpAdd(Db.Product newone,long ProductTypeId)
        {
            if (!CheckCp(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var userid = User.Identity.GetUserId();
            var comp=Db.Companies.FirstOrDefault(e=>e.UserId==userid);
            var type = Db.ThirdProductTypes.Find(ProductTypeId);
            newone.Type = type;
            comp.Products.Add(newone);
            Db.SaveChanges();
            return Redirect("./CompanyProductIndex?cid=" + comp.Id);
        }

        public bool CheckCp(Db.Product cp)
        {         
            if (string.IsNullOrEmpty(cp.GetDate) && string.IsNullOrEmpty(cp.Name) &&
                string.IsNullOrEmpty(cp.ProductionCertificateNo) &&
                string.IsNullOrEmpty(cp.Standard))
            {
                return false;}
    
            var p = Db.Products.FirstOrDefault(e => e.Name == cp.Name&&e.Id!=cp.Id);
            if (p != null)
            {
                return false;
            }
            return true;
        }

        public class Cp
        {
            public long Id { get; set; }

            public string CompanyName { get; set; }

            public string Name { get; set; }

            public string ProductTypeName { get; set; }//所属类别

            public string ProductionCertificateNo { get; set; }//生产许可证编号

            public string GetDate { get; set; }//颁发日期

            public string Standard { get; set; }//执行标准
            public EnumProductStatus CompanyProductStatus { get; set; }

        }

        [Authorize]
        public ActionResult CompanyProductInfo(long id)
        {
            var company = MyCompany;
            var product = company.Products.FirstOrDefault(a => a.Id == id);
            if (product == null)
            {
                return RedirectToAction("CompanyProductIndex");
            }
            return View(product);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CompanyProductSave(long id = 0)
        {
            var userId = User.Identity.GetUserId();            
            var company = Db.Companies.FirstOrDefault(a => a.UserId == userId);
            if (company == null)
            {
                return Content("错误操作");
            }
            else
            {
                var product = company.Products.FirstOrDefault(a => a.Id == id);
                return View(product);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CompanyProductSave(Product model, long productTypeId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.UserId = User.Identity.GetUserId();
            var company = MyCompany;
            var product = company.Products.FirstOrDefault(a => a.Id == model.Id);
            if (product == null)
            {
                model.Type = Db.ThirdProductTypes.Find(productTypeId);
                if (model.Type == null)
                {
                    return View();
                }
                model.Status = EnumStatus.FirstUncheked;
                model.CreateTime = DateTime.Now;
                model.LastChangeTime = model.CreateTime;
                company.Products.Add(model);
            }
            else
            {
                if (Util.Util.Equal(model, product, excepts: new List<string> {  "CreateTime", "LastChangeTime", "Status"}))
                {
                    return RedirectToAction("Index");
                }
                if (product.Status == EnumStatus.FirstUncheked)
                {
                    Util.Util.Dump(model, product, excepts: new List<string> { "CreateTime", "LastChangeTime", "Status" });
                    if (model.Type.Id != productTypeId)
                    {
                        model.Type = Db.ThirdProductTypes.Find(productTypeId);
                    }
                }
                else
                {
                    model.Type = Db.ThirdProductTypes.Find(productTypeId);
                    if (model.Type == null)
                    {
                        return View();
                    }
                    model.Type.SecondType.Productypes = null;
                    model.Type.SecondType.FirstType.SecondProductTypes = null;
                    model.Type.Products = null;
                    product.LastChangeTime = DateTime.Now;
                    product.Status = EnumStatus.Unchecked;
                    product.UpdateJson = JsonConvert.SerializeObject(model);   
                }
                Db.Entry(product).State = EntityState.Modified;
            }
            Db.SaveChanges();
            return RedirectToAction("CompanyProductInfo");
        }

        private Company MyCompany
        {
            get
            {
                var userId = User.Identity.GetUserId();
                return Db.Companies.FirstOrDefault(a => a.UserId == userId);
            }
        }

        #endregion

    }
}