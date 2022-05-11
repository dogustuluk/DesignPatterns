using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.StrategyDesignPattern.Models
{
    public class Settings
    {
        public static string claimDatabaseType = "databasetype"; //bu tip bize her yerde lazım olacağı için "static" tutuyoruz.

        public EDatabaseType databaseType;

        //kullanıcı login olduğunda direkt olarak sql Server'a kaydetmesi için yazılan kod.
        public EDatabaseType GetDefaultDatabaseType => EDatabaseType.SqlServer; //eğer lambda ile direkt olarak girersek bir property'nin sadece "get" ine karşılık gelir, set'i yoktur.

    }
}
