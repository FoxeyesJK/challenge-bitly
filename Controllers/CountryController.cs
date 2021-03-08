using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge_bitly.Models;
using challenge_bitly_abstraction;

namespace challenge_bitly.Controllers
{
    [ApiController]
    [Route("countries")]
    public class CountryController : Controller
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryRepository _repository;

        public CountryController(ILogger<CountryController> logger, ICountryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries([FromQuery] ApiQuery query)
        {
            var clicks = new List<Click>();

            try
            {
                string groupGuid = await _repository.GetGroupGuidFromUser();

                if(String.IsNullOrEmpty(groupGuid))
                {
                    return NotFound();
                }

                var bitlinkIds = await _repository.GetBitlinksFromGroup(groupGuid);

                foreach(var bitlinkId in bitlinkIds)
                {
                    var clicksByCountries = await _repository.GetClicksByCountries(bitlinkId, query);
                    clicks.AddRange(clicksByCountries);
                }

                var averageClicksByCountries = await _repository.GetAverageClicksByCountries(clicks);

                var jsonResult = JsonSerializer.Serialize(averageClicksByCountries);
                
                Console.WriteLine(jsonResult);
                return Ok(jsonResult);

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
