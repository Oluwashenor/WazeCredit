using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Models;
using WazeCredit.Models.Service;
using WazeCredit.Models.ViewModels;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;

        public HomeController(IMarketForecaster marketForecaster)
        {
            homeVM = new HomeVM();
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
