using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;
        private readonly WazeForecastSettings _wazeForecastOptions;
        [BindProperty]
        private CreditApplication CreditModel { get; set; }
      

        public HomeController(IMarketForecaster marketForecaster,IOptions<WazeForecastSettings> wazeForecastOptions)
        {
            homeVM = new HomeVM();
            _wazeForecastOptions = wazeForecastOptions.Value;
            _marketForecaster = marketForecaster;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            MarketResult currentMarket = _marketForecaster.GetMarketPrediction();
            switch (currentMarket.MarketCondition)
            {
                case MarketCondition.StableUp:
                    homeVM.MarketForecast = "Market Shows Signs to go up in a stable state";
                    break;
                case MarketCondition.StableDown:
                    homeVM.MarketForecast = "Market Shows Signs to go down in a stable state";
                    break;
                case MarketCondition.Volatile:
                    homeVM.MarketForecast = "Market Shows Signs to volatabilty";
                    break;
                default:
                    homeVM.MarketForecast = "Apply for credit card using our application";
                    break;
            }
            return View(homeVM);
        }

        public IActionResult AllConfigSettings(
            [FromServices] IOptions<StripeSettings> stripeOptions,
            [FromServices] IOptions<SendGridSettings> sendGridOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe publishable Key : " + stripeOptions.Value.PublishableKey);
            messages.Add($"Stripe Secret Key : " + stripeOptions.Value.SecretKey);
            messages.Add($"Send Grid Key : " + sendGridOptions.Value.SendGridKey);
            messages.Add($"Twilio Phone : " + twilioOptions.Value.PhoneNumber);
            messages.Add($"Twilio Account SID : " + twilioOptions.Value.AccountSid);
            messages.Add($"Send Grid Key : " + sendGridOptions.Value.SendGridKey);
            return View(messages);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication()
            {

            };
            return View(CreditModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
