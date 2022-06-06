using BaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ObserverDesignPattern.Observer
{
    public class UserObserverSubject
    {
        private readonly List<IUserObserver> _userObservers;
        public UserObserverSubject()
        {
            _userObservers = new List<IUserObserver>();
        }

        //public UserObserverSubject()
        //{
        //}

        public void RegisterObserver(IUserObserver userObserver)
        {
            _userObservers.Add(userObserver);
        }
        public void RemoverObserver(IUserObserver userObserver)
        {
            _userObservers.Remove(userObserver);
        }
        public void NotifyObserver(AppUser appUser)
        {
            _userObservers.ForEach(x => //_userObserver >> koleksiyon
            {
                x.UserCreated(appUser);
            });
        }
    }
}
