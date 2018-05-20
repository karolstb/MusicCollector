using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace MusicCollector
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //tworzy kolejne singletony dla pipelinu owina
            //kontekst do danych (bazy danych)
            app.CreatePerOwinContext(Models.MyApplicationDbContext.Create); //przekazuje callback do funkcji, która zwraca context do bazy
            //manager do zarządzania userami
            app.CreatePerOwinContext<MyApplicationUserManager>(MyApplicationUserManager.Create);
            //manager do logowania/wylogowania
            app.CreatePerOwinContext<MyApplicationSignInManager>(MyApplicationSignInManager.Create);
        }
    }
}