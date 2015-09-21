﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult ChooseProduct(string key="",long ProductTypeId=0)
        {
            var x = new List<Db.Product>();
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                else
                {
                    x = t.Products;
                }
                
            }
           
            if (!string.IsNullOrEmpty(key))
            x = Db.Products.Where(e => e.Name.Contains(key)).ToList();

            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;

            
            return View();
        }

        public ActionResult Choose(long id)
        {

            return View();
        }

        
    }
}