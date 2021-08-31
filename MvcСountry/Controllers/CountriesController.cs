using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvcCountry.Models;
using MvcСountry.Data;
using MvcСountry.Models;
using Newtonsoft.Json;

namespace MvcСountry.Controllers
{







    public class CountriesController : Controller
    {
        private readonly MvcСountryContext _context;
       

        public CountriesController(MvcСountryContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index(string searchString)
        {
            var countries = from m in _context.Country
                            select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                countries = countries.Where(s => s.Name.Contains(searchString));

            }
           
                return View(await countries.ToListAsync());
        }





        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // GET: Countries/Create
        public  IActionResult Create()
        {
            string url = "https://restcountries.eu/rest/v2/all";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string jsonResponse = null;
            jsonResponse = reader.ReadLine();

            if (jsonResponse != null)
            {
                List<Country> country = JsonConvert.DeserializeObject<List<Country>>(jsonResponse);
                //    //items из json, p.Name из базы
                IEnumerable<SelectListItem> countries = country.Select(
                        x => new SelectListItem()
                        {
                            Value = x.Name,
                            Text = x.Name,
                        });
                        ViewBag.Name = countries;
            }
            return View();
        }


        // POST: Countries/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City, Name, CountryId, CityForeignKey,Area, Region, capital")] Country country,  City city,[Bind("Region")] Regions regions)
        {
            string name = country.Name;
            string Baseurl = Convert.ToString("https://restcountries.eu/rest/v2/name/" + name);
            List<Regions> regions1 = new List<Regions>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Convert.ToString("https://restcountries.eu/rest/v2/name/" + name));
                if (Res.IsSuccessStatusCode)
                {
                    var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                    List<Regions>  regions12 = JsonConvert.DeserializeObject<List<Regions>>(CountryResponse);
                     foreach (var item in regions12)
                    {
                        regions.Region = Convert.ToString(item.Region);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            List<City> citySecond = new List<City>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Convert.ToString("https://restcountries.eu/rest/v2/name/" + name));
                if (Res.IsSuccessStatusCode)
                {
                    var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                    List<City> citySecondTwo = JsonConvert.DeserializeObject<List<City>>(CountryResponse);
                    foreach (var item in citySecondTwo)
                    {
                        city.capital = Convert.ToString(item.capital);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            List<Country> countrySecond = new List<Country>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Convert.ToString("https://restcountries.eu/rest/v2/name/" + name));
                if (Res.IsSuccessStatusCode)
                {
                    var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                    List<Country> countrySecondTwo = JsonConvert.DeserializeObject<List<Country>>(CountryResponse);
                    foreach (var item in countrySecondTwo)
                    {
                        country.Area = Convert.ToDouble(item.Area);
                        country.Population = item.Population;
                        country.Alpha2Code = Convert.ToString(item.Alpha2Code);
                        await _context.SaveChangesAsync();

                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _context.AddRangeAsync(country, city, regions);
                await _context.SaveChangesAsync();//присваение CountryId
                country.capital += city.capital;
                country.CityForeignKey += city.CityId;
                await _context.SaveChangesAsync();
                country.Region += regions.Region;
                country.RegionForeignKey += regions.Id;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }





        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CointryId,Name,Alpha2Code,Area,Population,Region")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Country.Any(e => e.CountryId == id);
        }
    }
}
