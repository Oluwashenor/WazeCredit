using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Models;
using WazeCredit.Models.Service;
using WazeCredit.Models.ViewModels;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;
        private readonly StripeSettings _stripeOptions;
        private readonly SendGridSettings _sendGridOptions;
        private readonly TwilioSettings _twilioOptions;
        private readonly WazeForecastSettings _wazeForecastOptions;

        public HomeController(IMarketForecaster marketForecaster,
            IOptions<StripeSettings> stripeOptions,
            IOptions<SendGridSettings> sendGridOptions,
            IOptions<TwilioSettings> twilioOptions,
            IOptions<WazeForecastSettings> wazeForecastOptions
            )
        {
            homeVM = new HomeVM();
            _marketForecaster = marketForecaster;
            _wazeForecastOptions = wazeForecastOptions.Value;
            _stripeOptions = stripeOptions.Value;
            _sendGridOptions = sendGridOptions.Value;
            _twilioOptions = twilioOptions.Value;

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

        public IActionResult AllConfigSettings()
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe publishable Key : " + _stripeOptions.PublishableKey);
            messages.Add($"Stripe Secret Key : " + _stripeOptions.SecretKey);
            messages.Add($"Send Grid Key : " + _sendGridOptions.SendGridKey);
            messages.Add($"Twilio Phone : " + _twilioOptions.PhoneNumber);
            messages.Add($"Twilio Phone : " + _twilioOptions.AccountSid);
            messages.Add($"Send Grid Key : " + _sendGridOptions.SendGridKey);
            return View(messages);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
