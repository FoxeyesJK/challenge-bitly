using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using challenge_bitly_abstraction;
using Microsoft.Extensions.Configuration;
using challenge_bitly.Models;

namespace challenge_bitly_repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public CountryRepository(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _client = httpClientFactory.CreateClient();
            _config = config;
        }

        public async Task<string> GetGroupGuidFromUser()
        {
            string requestUrl = $"{_config["BASE_URL"]}/user";
            
            var response = await GetAsync<User>(requestUrl);

            string groupGuid = response.default_group_guid;

            return groupGuid;
        }

        public async Task<List<string>> GetBitlinksFromGroup(string groupGuid)
        {
            var bitlinkIds = new List<string>();
            string requestUrl = $"{_config["BASE_URL"]}/groups/{groupGuid}/bitlinks";

            do
            {
                var response = await GetAsync<Bitlink>(requestUrl);

                foreach(var link in response.links)
                {
                    bitlinkIds.Add(link.id);
                }

                requestUrl = response.pagination.next;

            } while(!String.IsNullOrEmpty(requestUrl));
            
            return bitlinkIds;
        }

        public async Task<List<Click>> GetClicksByCountries(string bitlinkId, ApiQuery query)
        {
            var clicks = new List<Click>();
            string requestUrl = $"{_config["BASE_URL"]}/bitlinks/{bitlinkId}/countries?units={query.units}&unit={query.unit}&size=15000";

            var response = await GetAsync<Country>(requestUrl);

            foreach(var metric in response.metrics)
            {
                clicks.Add(new Click() { 
                    country = metric.value, 
                    clicks = metric.clicks, 
                    bitlinkId = bitlinkId, 
                    units = response.units, 
                    unit = response.unit 
                });
            }
            
            return clicks;
        }

        public async Task<List<CountryResource>> GetAverageClicksByCountries(List<Click> clicks)
        {
            return await Task.FromResult(clicks
                    .AsEnumerable()
                    .GroupBy(g => g.country)
                    .Select(s => new CountryResource
                    {
                        country = s.Key,
                        units = s.FirstOrDefault().units,
                        unit = s.FirstOrDefault().unit,
                        total_clicks = s.Sum(x => x.clicks),
                        average_clicks = System.Math.Round(s.Sum(x => x.clicks) / (decimal)s.FirstOrDefault().units, 2),
                        bitlinks = s.ToList()
                                    .GroupBy(gg => gg.bitlinkId)
                                    .Select(ss => new CountryResource.CountryData
                                    {
                                        bitlinkId = ss.Key,
                                        total_clicks = ss.Sum(x => x.clicks),
                                        average_clicks = System.Math.Round(ss.Sum(x => x.clicks) / (decimal)s.FirstOrDefault().units, 2)
                                    }).ToList()
                    }).ToList());   
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);
            var result = response.Content.ReadAsStringAsync().Result;

            if(response.IsSuccessStatusCode)
            {
                var jsonResult = JsonSerializer.Deserialize<T>(result);
                return jsonResult;
            } else
            {
                throw new InvalidOperationException(result);
            }
        }
    }
}