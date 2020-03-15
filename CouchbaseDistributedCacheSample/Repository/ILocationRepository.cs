using CouchbaseDistributedCacheSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouchbaseDistributedCacheSample.Repository
{
    public interface ILocationRepository
    {
       Task<List<GetCitiesResponse>> GetCities();
    }
}
