using MusicCollector.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusicCollector.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// akcja dodania usera
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddUser()
        {
            MyApplicationUser user;
            MyApplicationUserStore Store = new MyApplicationUserStore(new MyApplicationDbContext()); //(MyApplicationUserStore)(new UserStore<MyApplicationUser>(new MyApplicationDbContext()));  //-to samo robi
            MyApplicationUserManager userManager = new MyApplicationUserManager(Store);
            user = new MyApplicationUser
            {
                UserName = "TestUser",
                Email = "TestUser@test.com",
                DataUrodzenia = DateTime.Parse("1990-05-25")
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return result.Errors.First();
            }
            return "User Added";
        }
    }
}