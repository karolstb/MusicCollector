using MusicCollector.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//to jest link do tutoriala
//https://www.tektutorialshub.com/asp-net-identity-tutorial-basics/

namespace MusicCollector
{
    //UserStore (określa przechowywanie danych)
    public class MyApplicationUserStore : UserStore<MyApplicationUser>
    {
        public MyApplicationUserStore(MyApplicationDbContext context) : base(context)
        {
        }
    }

    /// UserManager (określa metody do CRUD od danych o userze)
    public class MyApplicationUserManager : UserManager<MyApplicationUser>
    {
        public MyApplicationUserManager(IUserStore<MyApplicationUser> store) : base(store)
        {
        }

        public static MyApplicationUserManager Create(IdentityFactoryOptions<MyApplicationUserManager> options, IOwinContext context)
        {
            var store = new UserStore<MyApplicationUser>(context.Get<MyApplicationDbContext>());
            var manager = new MyApplicationUserManager(store);
            return manager;
        }
    }

    //manager do logowania się usera
    public class MyApplicationSignInManager : SignInManager<MyApplicationUser, string>
    {
        public MyApplicationSignInManager(MyApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {

        }

        //wszystkie metody Create w tych klasach to robienie singletonów
        public static MyApplicationSignInManager Create(IdentityFactoryOptions<MyApplicationSignInManager> options, IOwinContext context)
        {
            return new MyApplicationSignInManager(context.GetUserManager<MyApplicationUserManager>(), context.Authentication);
        }
    }

    //todo: skończyłem tutorial na tym: "Finally, we can then add the sign in manager in the Owin context"
    //https://www.tektutorialshub.com/asp-net-identity-tutorial-owin-setup/

    //public class IdentityConfig
    //{
    //}


    //public interface IUserStore<TUser, in TKey> : IDisposable where TUser : class, Microsoft.AspNet.Identity.IUser<TKey>
    //{
    //    Task CreateAsync(TUser user);
    //    Task DeleteAsync(TUser user);
    //    Task<TUser> FindByIdAsync(TKey userId);
    //    Task<TUser> FindByNameAsync(string userName);
    //    Task UpdateAsync(TUser user);
    //}

}