using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataScienceApp.Models;
using DataScienceApp.Data;
using DataScienceApp.Framework;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataScienceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataScienceAppContext _context;

        public HomeController(DataScienceAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([Bind("Symbol, Contents")] PriceEntriesUpload priceEntriesUpload)
        {
            if (ModelState.IsValid)
            {
                if (_context.Stocks.Any(x => x.Symbol == priceEntriesUpload.Symbol))
                {
                    _context.Remove(_context.Stocks.FirstOrDefault(x => x.Symbol == priceEntriesUpload.Symbol));
                    _context.PriceEntries.Clear(_context.PriceEntries.Where(x => x.Stock.Symbol == priceEntriesUpload.Symbol));
                    await _context.SaveChangesAsync();
                }

                Stock stock = new Stock() { Symbol = priceEntriesUpload.Symbol, Prices = new List<PriceEntry>() };

                using (TextReader tr = new StringReader(priceEntriesUpload.Contents))
                {
                    var csv = new CsvReader(tr, new CsvConfiguration() { HasHeaderRecord = false });
                    while (csv.Read())
                    {
                        PriceEntry priceEntry = new PriceEntry();
                        priceEntry.Date = csv.GetField<DateTime>(0);
                        priceEntry.High = csv.GetField<Decimal>(1);
                        priceEntry.Low = csv.GetField<Decimal>(2);
                        priceEntry.Open = csv.GetField<Decimal>(3);
                        priceEntry.Close = csv.GetField<Decimal>(4);
                        priceEntry.Volume = csv.GetField<int>(5);


                        stock.Prices.Add(priceEntry);
                    }

                    stock.FillCalculatedFields();

                    _context.Stocks.Add(stock);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }
            return View("Index", priceEntriesUpload);
        }

        public ActionResult DownloadCsv()
        {
            using (StringWriter textWriter = new StringWriter())
            {
                var csv = new CsvWriter(textWriter);
                csv.Configuration.RegisterClassMap<LinearModelObservationMap>();

                Stock uvxy = _context.Stocks.Include(x => x.Prices).Single(x => x.Symbol == "UVXY");
                Stock spy = _context.Stocks.Include(x => x.Prices).Single(x => x.Symbol == "SPY");
                PriceEntry explanatoryPriceEntry;

                csv.WriteHeader<LinearModelObservation>();

                foreach(PriceEntry priceEntry in uvxy.Prices.OrderBy(x => x.Date).Skip(2))
                {
                    explanatoryPriceEntry = spy.Prices.Single(x => x.Date == priceEntry.Date);

                    csv.WriteRecord(new LinearModelObservation()
                    {
                        ExplanatoryVariable = explanatoryPriceEntry.YdaPctChange,
                        OutcomeVariable = priceEntry.PctChange
                    });
                }

                return File(System.Text.UTF8Encoding.UTF8.GetBytes(textWriter.ToString()), "text/csv", "linearModel.csv");
            }
        }
    }
}
