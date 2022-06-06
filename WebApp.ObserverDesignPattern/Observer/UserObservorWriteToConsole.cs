using BaseProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ObserverDesignPattern.Observer
{
    public class UserObservorWriteToConsole : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObservorWriteToConsole(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void UserCreated(AppUser appUser)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObservorWriteToConsole>>();
            logger.LogInformation($"User ID:{appUser.Id}");
        }
    }
}
