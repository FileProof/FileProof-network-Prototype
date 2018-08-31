using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CVProof.Web.Models;
using CVProof.DAL.SQL;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CVProof.Web.Controllers
{
    //public class HomeController : Controller
    //{
    //    private readonly IConfiguration configuration;

    //    public HomeController(IConfiguration config)
    //    {
    //        configuration = config;
    //        SQLData.connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
    //    }

    //    [AllowAnonymous]
    //    public IActionResult Error()
    //    {
    //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //    }

    //    private static IEnumerable<RatesViewModel> ParseResultsForm(Microsoft.AspNetCore.Http.IFormCollection form)
    //    {
    //        var ret = new List<RatesViewModel>();

    //        IEnumerable<String> RatesToSave = form["item.Id"].ToString().Split(',');

    //        foreach (string r in RatesToSave)
    //        {
    //            int id;

    //            if (int.TryParse(r, out id))
    //            {
    //                RatesViewModel rm = new RatesViewModel() { Id = id };

    //                if (!String.IsNullOrEmpty(form["rate100_" + id.ToString()]))
    //                {
    //                    decimal val;

    //                    if (Decimal.TryParse(form["rate100_" + id.ToString()], out val))
    //                    {
    //                        rm.Rate100 = val;
    //                    }
    //                }

    //                if (!String.IsNullOrEmpty(form["rate600_" + id.ToString()]))
    //                {
    //                    decimal val;

    //                    if (Decimal.TryParse(form["rate600_" + id.ToString()], out val))
    //                    {
    //                        rm.Rate600 = val;
    //                    }
    //                }

    //                if (!String.IsNullOrEmpty(form["rate1200_" + id.ToString()]))
    //                {
    //                    decimal val;

    //                    if (Decimal.TryParse(form["rate1200_" + id.ToString()], out val))
    //                    {
    //                        rm.Rate1200 = val;
    //                    }
    //                }

    //                if (!String.IsNullOrEmpty(form["rate1700_" + id.ToString()]))
    //                {
    //                    decimal val;

    //                    if (Decimal.TryParse(form["rate1700_" + id.ToString()], out val))
    //                    {
    //                        rm.Rate1700 = val;
    //                    }
    //                }

    //                if (!String.IsNullOrEmpty(form["rate5000_" + id.ToString()]))
    //                {
    //                    decimal val;

    //                    if (Decimal.TryParse(form["rate5000_" + id.ToString()], out val))
    //                    {
    //                        rm.Rate5000 = val;
    //                    }
    //                }

    //                ret.Add(rm);
    //            }
    //        }

    //        return ret;
    //    }

    //}
}
