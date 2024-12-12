using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Examen_PROAGRO.Models;
using System.Diagnostics;
using RestSharp;
using System.Threading.Tasks;

namespace Examen_PROAGRO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly ProagrodbContext _context; // nuevo

        // Este es el único constructor que debe tener el controlador
        public HomeController(ProagrodbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MapasAddItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMarker([FromBody] Restaurante markerData)
        {
            if (markerData == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var restaurante = new Restaurante
            {
                FsqId = markerData.FsqId,
                Nombre = markerData.Nombre,
                FormattedAddress = markerData.FormattedAddress,
                FechaRegistro = markerData.FechaRegistro,
                Activo = markerData.Activo,
                Foto = markerData.Foto,
                Rating = markerData.Rating
            };

            // Agregar el nuevo restaurante a la base de datos
            await _context.Restaurantes.AddAsync(restaurante);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Información guardada con éxito" });
        }

        // Acción para guardar la información del marcador
        //[HttpPost]
        //public IActionResult SaveMarker([FromBody] Restaurante restaurante)
        //{
        //    // Aquí puedes guardar la información en una base de datos o en otro lugar
        //    // Por ejemplo, guardar el ID del marcador en la base de datos
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.FsqId}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.Nombre}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.FormattedAddress}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.FechaRegistro}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.Activo}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.Foto}");
        //    Console.WriteLine($"Guardando marcador con ID: {restaurante.Rating}");

        //    // Devolver una respuesta de éxito
        //    return Json(new { success = true, message = "Información guardada" });
        //}

        //public class Restaurante
        //{
        //    public string FsqId { get; set; }         // ID del restaurante
        //    public string Nombre { get; set; }           // Nombre del restaurante
        //    public string FormattedAddress { get; set; } // Dirección del restaurante
        //    public DateTime FechaRegistro { get; set; } // Fecha de registro (por defecto fecha actual del sistema)
        //    public bool Activo { get; set; }           // Estado del restaurante (1 = activo, 0 = inactivo)
        //    public string Foto { get; set; }           // Ruta de la foto
        //    public decimal Rating { get; set; }        // Rating del restaurante
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaces()
        {

            var options = new RestClientOptions("https://api.foursquare.com/v3/places/search");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "fsq38Qf8zd4mcVOcuegTNGhsErxUBnYRYwJcKj83M57AXgo=");
            var response = await client.GetAsync(request);
            if (response.IsSuccessful)
            {
                var rootObject = JsonConvert.DeserializeObject<RootObject>(response.Content);
                var places = rootObject.Results;
                return Json(places);
            }

            return Json(new { error = "Hubo un error al obtener los lugares." });
        }

        public class RootObject
        {
            public List<AddressResponse> Results { get; set; }
        }

        public class AddressResponse
        {
            public string FsqId { get; set; }
            public List<Category> Categories { get; set; }
            public List<string> Chains { get; set; }
            public string ClosedBucket { get; set; }
            public int Distance { get; set; }
            public Geocodes Geocodes { get; set; }
            public string Link { get; set; }
            public Location Location { get; set; }
            public string Name { get; set; }
            public RelatedPlaces RelatedPlaces { get; set; }
            public string Timezone { get; set; }
        }

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ShortName { get; set; }
            public string PluralName { get; set; }
            public Icon Icon { get; set; }
        }

        public class Icon
        {
            public string Prefix { get; set; }
            public string Suffix { get; set; }
        }

        public class Geocodes
        {
            public Coordinates Main { get; set; }
            public Coordinates Roof { get; set; }
        }

        public class Coordinates
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class Location
        {
            public string Address { get; set; }
            public string Country { get; set; }
            public string CrossStreet { get; set; }
            public string FormattedAddress { get; set; }
            public string Locality { get; set; }
            public string Postcode { get; set; }
            public string Region { get; set; }
        }

        public class RelatedPlaces
        {
            public List<Child> Children { get; set; }
        }

        public class Child
        {
            public string FsqId { get; set; }
            public List<Category> Categories { get; set; }
            public string Name { get; set; }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
