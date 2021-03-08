using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using challenge_bitly.Models;

namespace challenge_bitly_abstraction
{
    public interface ICountryRepository
    {
        Task<string> GetGroupGuidFromUser();
        Task<List<string>> GetBitlinksFromGroup(string groupGuid);
        Task<List<Click>> GetClicksByCountries(string bitlinkId, ApiQuery query);
        Task<List<CountryResource>> GetAverageClicksByCountries(List<Click> clicks);
        Task<T> GetAsync<T>(string requestUrl);
    }
}