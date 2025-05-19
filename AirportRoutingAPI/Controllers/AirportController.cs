using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportRoutingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly ILogger<AirportController> _logger;

        public AirportController(ILogger<AirportController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        [HttpGet("GetDestinations")]
        public IActionResult GetDestinations([FromQuery] string origin)
        {
            _logger.LogInformation("Received request for origin: {Origin}", origin);

            var airportRoutes = new Dictionary<string, List<string>>()
            {
                { "NYC", new List<string>{ "LON", "PAR", "LAX", "ROM" } },
                { "LAX", new List<string>{ "NYC", "SFO", "SEA", "TOK" } },
                { "DEL", new List<string>{ "DXB", "BOM", "BLR", "SIN" } },
                { "LON", new List<string>{ "NYC", "PAR", "BER", "AMS" } }
            };

            if (string.IsNullOrWhiteSpace(origin))
            {
                _logger.LogWarning("Origin airport code is missing.");
                return BadRequest("Origin airport code is required.");
            }

            if (!airportRoutes.ContainsKey(origin.ToUpper()))
            {
                _logger.LogWarning("No routes found for origin: {Origin}", origin);
                return NotFound($"No destination routes found for origin: {origin}");
            }

            var destinations = airportRoutes[origin.ToUpper()];
            _logger.LogInformation("Returning destinations for origin {Origin}: {Destinations}", origin, string.Join(",", destinations));

            return Ok(destinations);
        }
    }
}
