using CouchbaseDistributedCacheSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouchbaseDistributedCacheSample.Repository
{
    public class LocationRepository: ILocationRepository
    {
        public async Task<List<GetCitiesResponse>> GetCities()
        {
            return new List<GetCitiesResponse>() { new GetCitiesResponse { Code = "34", Name = "İstanbul" }, new GetCitiesResponse { Code = "06", Name = "Ankara" } };
        }
    }
}
