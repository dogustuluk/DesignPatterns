using BaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ObserverDesignPattern.Observer
{
    public interface IUserObserver
    {
        void UserCreated(AppUser appUser);
    }
}
