using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using QualityControl.Controllers;
using QualityControl.Enum;

namespace QualityControl.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public ActionResult RegisterApplyList(int userType, int page = 1, int count = 20)
        {
            if (page < 1) page = 1;
            if (count < 1) count = 20;

            var data = UserManager.Users
                .Where(a => a.Type == userType && a.Status == (int) EnumUserStatus.Requiring)
                .Skip((page - 1)*count)
                .Take(count).ToList();
            return View(data);
        }

        /// <summary>
        ///     审核通过
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<string> Pass(List<string> userIds)
        {
            var result = new List<int>();
            foreach (var userId in userIds)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Status = (int) EnumUserStatus.Normal;
                    await UserManager.UpdateAsync(user);
                    result.Add(1);
                }
                else
                {
                    result.Add(0);
                }
            }
            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        ///     审核不通过
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<string> Fail(List<string> userIds)
        {
            var result = new List<int>();
            foreach (var userId in userIds)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Status = (int) EnumUserStatus.Failed;
                    await UserManager.UpdateAsync(user);
                    result.Add(1);
                }
                else
                {
                    result.Add(0);
                }
            }
            return JsonConvert.SerializeObject(result);
        }

        #region Identity

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}