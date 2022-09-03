using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Data;
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
        private readonly ICreditValidator _creditValidator;
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;
        [BindProperty]
        public CreditApplication CreditModel { get; set; }
      

        public HomeController(IMarketForecaster marketForecaster, ApplicationDbContext db,ILogger<HomeController> logger, IOptions<WazeForecastSettings> wazeForecastOptions, ICreditValidator creditValidator)
        {
            homeVM = new HomeVM();
            _wazeForecastOptions = wazeForecastOptions.Value;
            _marketForecaster = marketForecaster;
            _creditValidator = creditValidator; 
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home controllr Index Called");
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPOST([FromServices] Func<CreditApprovedEnums, ICreditApproved> _creditService)
        {
            if (ModelState.IsValid)
            {
                var (validationPassed, errorMessages) = await _creditValidator.PassAllValidations(CreditModel);
                CreditResult creditResult = new CreditResult()
                {
                    ErrorList = errorMessages,
                    CreditID = 0,
                    Success = validationPassed
                };
                if (validationPassed)
                {
                    CreditModel.CreditApproved = _creditService(CreditModel.Salary > 5000 ? CreditApprovedEnums.High : CreditApprovedEnums.Low).GetCreditApproved(CreditModel);
                    _db.CreditApplicationModel.Add(CreditModel);
                    _db.SaveChanges();
                    creditResult.CreditID = CreditModel.Id;
                    creditResult.CreditApproved = CreditModel.CreditApproved;
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
                else
                {
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
            }
            
            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResult creditResult)
        {
            return View(creditResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
