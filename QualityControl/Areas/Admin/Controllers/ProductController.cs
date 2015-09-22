using QualityControl.Controllers;
using QualityControl.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region 类型
        /// <summary>
        /// 类型列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult TypeList(int page = 1, int count = 20, string key = null)
        {
            if (page < 1) page = 1;
            if (count < 1) count = 20;
            var data = Db.ThirdProductTypes.Where(a => a.Title.Contains(key))
                .Skip((page - 1) * count)
                .Take(count).ToList();
            return View(data);
        }

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public long AddType(ThirdProductType type)
        {
            if (string.IsNullOrEmpty(type.Title) || string.IsNullOrEmpty(type.Description))
            {
                return 0;
            }
            Db.ThirdProductTypes.Add(type);
            Db.SaveChanges();
            return type.Id;
        }

        /// <summary>
        /// 编辑类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int UpdateType(ThirdProductType type)
        {
            var oldType = Db.ThirdProductTypes.Find(type.Id);
            if (oldType == null)
            {
                return -1;
            }
            oldType.Title = type.Title;
            oldType.Description = type.Description;
            Db.Entry(oldType).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return 0;
        }


        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int RemoveType(long id)
        {
            var type = Db.ThirdProductTypes.Find(id);
            if (type == null)
            {
                return 0;
            }
            Db.ThirdProductTypes.Remove(type);
            Db.SaveChanges();
            return 1;
        }
        #endregion

        #region 产品
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult ProductList(int status, int page = 1, int count = 20, string key = null)
        {
            if (page < 1) page = 1;
            if (count < 1) count = 20;
            var data = Db.Products.Where(a => (int)a.Status == status && a.Name.Contains(key))
                .Skip((page - 1) * count)
                .Take(count).ToList();
            return View(data);
        } 



        #endregion
    }
}