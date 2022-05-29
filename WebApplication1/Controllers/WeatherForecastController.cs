using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

        private readonly IRecurringJobManager _recurringJobManager;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
                                         IBackgroundJobClient backgroundJobClient,
                                         IRecurringJobManager recurringJobManager)
        {
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;

           
        }
        [HttpPost]
        [Route("WelcomeFireForget")]
        public IActionResult WelcomeFireForget(string userName)
        {
            MyClass myClass = new MyClass();
            var jobId = _backgroundJobClient.Enqueue(() => myClass.SendWelcomeMail(userName));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }

        [HttpPost]
        [Route("WelcomeSchedule")]
        public IActionResult WelcomeSchedule(string userName)
        {
            MyClass myClass = new MyClass();
            var jobId2 = BackgroundJob.Schedule(() => myClass.SendWelcomeMail(userName), TimeSpan.FromMinutes(2));
            var jobId3 = _backgroundJobClient.Schedule(() => myClass.SendWelcomeMail(userName), TimeSpan.FromMinutes(2));


            //   _recurringJobManager.AddOrUpdate("test", () => myClass.SendWelcomeMail(userName), Cron.Minutely());
            return Ok($"Job Id {jobId3} Completed. Welcome Mail Sent!");
        }

        [HttpPost]
        [Route("WelcomeRecurring")]
        public IActionResult WelcomeRecurring(string userName)
        {
            MyClass myClass = new MyClass();
            _recurringJobManager.AddOrUpdate("test", () => myClass.SendWelcomeMail(userName), Cron.Minutely());
            return Ok($"Welcome Mail Sent!");
        }

    }
}
