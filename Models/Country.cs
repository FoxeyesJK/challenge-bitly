using System;
using System.Collections.Generic;

namespace challenge_bitly.Models 
{
    public class User
    {
        public string default_group_guid { get; set; }
    }

    public class Bitlink
    {
        public List<Link> links { get; set; }
        public Pagination pagination { get; set; }

        public Bitlink()
        {
            links = new List<Link>();
        }

        public class Link
        {
            public string id { get; set; }
            public string link { get; set; }
            public string long_url {get; set; }
        }

        public class Pagination
        {
            public string prev { get; set; }
            public string next { get; set; }
            public int size { get; set; }
            public int page { get; set; }
            public int total { get; set; }
        }
    }

    public class Country
    {
        public int units { get; set; }
        public string unit { get; set; }
        public List<Metric> metrics { get; set;}

        public Country()
        {
            metrics = new List<Metric>();
        }

        public class Metric
        {
            public string value { get; set; }
            public int clicks { get; set; }
        }
    }

    public class Click
    {
        public string country { get; set; }
        public int clicks { get; set; }
        public string bitlinkId { get; set; }
        public int units { get; set; }
        public string unit { get; set; }
    }

    public class CountryResource
    {
        public string country { get; set; }
        public int units { get; set; }
        public string unit { get; set; }

        public int total_clicks { get; set; }
        public decimal average_clicks { get; set; }
        public List<CountryData> bitlinks { get; set; }

        public CountryResource()
        {
            bitlinks = new List<CountryData>();
        }

        public class CountryData
        {
            public string bitlinkId { get; set; }

            public int total_clicks { get; set; }
            public decimal average_clicks { get; set; }
        }
    }
    public class ApiQuery
    {
        public string unit { get; set; } = "day";
        public int units { get; set; } = -1;
    }
}