using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCountry.Models;
using MvcСountry.Data;
using MvcСountry.Models;
using Newtonsoft.Json;


/**
 * Контроллер для обработки запросов по загрузке с апи, выводу стран и их данных, сохранения данных и стран
 *
 * Class CountriesController
 * 
 */


namespace MvcСountry.Controllers
{

    public class CountriesController : Controller
    {
        private readonly MvcСountryContext _context;
        public CountriesController(MvcСountryContext context)
        {
            _context = context;
        }

        /**
        * Возвращает все сохраненные страны с бд.
        * @return View(await countriesFirst.ToListAsync()) 
        */
        // GET: Countries 
        public async Task<IActionResult> Index(string searchString)
        {
           
            var countriesDb = from m in _context.Country//выборка стран с бд
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                countriesDb = countriesDb.Where(s => s.Name.Contains(searchString));
            }
            return View(await countriesDb.ToListAsync());
        }




    /**
    * Возвращает страну по id
    * @param id
    * @return View(country)
    */
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


        /**Выборка стран с апи, сравнение с поисковой строкой поле Name,
         *отправка в Create view, затем сабмит пост запросом в Task<IActionResult> Create
        * @param Country.Name
        * @return View()
        */
        // GET: Countries/Create
        public  IActionResult Create(string searchString)
        {
            try
            {
                //склеивание строки запроса апи с поисковой строкой, запрос страны по названию с апи
                string url = Convert.ToString("https://restcountries.eu/rest/v2/name/" + searchString);
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
 //вызов функции launchMyForm с site.js, отправка во вью, конвертация в html, затем вывод модального окна через 2,5 секунды
                    TempData["saveCountry"] = "<script type=\"module\">setTimeout(launchMyForm, 2500);</script>";
                   
                    var countries = country.Select(
                        x => new SelectListItem()
                        {
                            Value = x.Name,
                            Text = x.Name,
                        });

                    var countryParametrs = country.ToList();
                    ViewData["MyData"] = countryParametrs;
                    ViewBag.Name = countries;
                    return View();
                }
                dataStream.Close();
                response.Close();
            }
            catch (WebException webExcp)
            { 
                WebExceptionStatus status = webExcp.Status;  
                if (status == WebExceptionStatus.ProtocolError)
                { 
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    Console.WriteLine((int)httpResponse.StatusCode + " - "
                       + httpResponse.StatusCode);
                }
            }

  //если название страны  не соответствует апи, то происходит отправка во вью, конвертация в html, потом вывод alert и редирект обратно
            TempData["msg"] = "<script>alert('К сожалению такой страны не найдено, попробуйте ввести другое название');</script>";
            return Redirect("~/Countries");
        }

        /**Принимает поток с вида, принимает потоки с апи и затем отправляет потоки асинхронно в _context.Country,
        * распределяя и сохраняя данные по таблицам country, city, regions 
        * @param City, Name, CountryId, CityForeignKey,Area, Region, capital, Region
        * @return View(country)
        */
        // POST: Countries/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City, Name, CountryId, CityForeignKey,Area, Region, capital")] Country country,  City city,[Bind("Region")] Regions regions)
        {
            string name = country.Name;//получаем название страны, которое было изначально заполнено в строке поиска
            string Baseurl = Convert.ToString("https://restcountries.eu/rest/v2/name/" + name);//склеивание строки для запроса апи по названию страны для получения остальных данных о стране
            List<Regions> regionsFirst = new List<Regions>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Convert.ToString("https://restcountries.eu/rest/v2/name/" + name));
                if (Res.IsSuccessStatusCode)
                {
                    var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                    List<Regions>  regionsSecond = JsonConvert.DeserializeObject<List<Regions>>(CountryResponse);
                     foreach (var item in regionsSecond)
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
                await _context.SaveChangesAsync();//присвоение CountryId
                country.capital += city.capital;
                country.CityForeignKey += city.CityId;
                await _context.SaveChangesAsync();//присвоение столицы стране и внешнего ключа с сити 
                country.Region += regions.Region;
                country.RegionForeignKey += regions.Id;
                await _context.SaveChangesAsync();//присвоение региона стране и внешнего ключа с регионом
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }




        /** Метод изменения страны
        * @param id
        * @return View(country)
        */
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

       /** Метод изменения страны
       * @param id, CointryId, Name, Alpha2Code, Area, Population, Region
       * @return View(country)
       */
        // POST: Countries/Edit/5
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

       /** Метод удаления страны
       * @param id
       * @return View(country)
       */
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

       /** Метод удаления страны
       * @param id
       * @return View(country)
       */
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
