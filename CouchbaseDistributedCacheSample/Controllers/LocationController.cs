using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CouchbaseDistributedCacheSample.Models;
using CouchbaseDistributedCacheSample.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CouchbaseDistributedCacheSample.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILocationRepository  _locationRepository;

        public LocationController(IDistributedCache distributedCache, ILocationRepository locationRepository)
        {
            _distributedCache = distributedCache;
            _locationRepository = locationRepository;
        }

       
        [Route("getcities")]
        [HttpGet]
        public async Task<List<GetCitiesResponse>> GetCities()
        {

            List<GetCitiesResponse> response = new List<GetCitiesResponse>();
            string key = "Couchbase_Cities";

            try
            {
                string result = System.Text.Encoding.UTF8.GetString(_distributedCache.Get(key));
                response = JsonConvert.DeserializeObject<List<GetCitiesResponse>>(result);
            }
            catch (Exception)
            {
                //If key not awailable in Couchbase , Get error description from DB and set.
                var item = await _locationRepository.GetCities();

                if (item != null)
                {
                    _distributedCache.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item)),
                                    new DistributedCacheEntryOptions
                                    {
                                        AbsoluteExpiration = DateTime.Now.AddYears(1)
                                    });

                }
                response = item.ToList();
            }


            return response;
        }


    }
}
