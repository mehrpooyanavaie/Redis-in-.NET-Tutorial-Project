using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyNewwRedis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net.Http;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoLocationController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly HttpClient _httpClient;
        private const string geoKey = "locations";
        public GeoLocationController(IConnectionMultiplexer redis, IHttpClientFactory httpClientFactory)
        {
            _redis = redis;
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpPost("from-ip")]
        public async Task<ActionResult<string>> AutoAddGeoLocationFromIp(string? ipAddress = "2.147.19.6")
        {
            if (ipAddress == null)
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            GeoLocation location = await GetGeoLocationFromIpAsync(ipAddress);

            var nameWithGuid = await AddLocationToRedis(location.Longitude, location.Latitude, location.Name);
            return Ok($"Location added successfully for IP: {ipAddress}, name with Guid = {nameWithGuid}");
        }
        private async Task<GeoLocation> GetGeoLocationFromIpAsync(string ipAddress)
        {
            var response = await _httpClient.GetStringAsync($"https://freegeoip.app/json/{ipAddress}");
            var location = JsonConvert.DeserializeObject<GeoLocationResponse>(response);

            if (location != null)
            {
                return new GeoLocation
                {
                    Name = location.City,
                    Longitude = location.Longitude,
                    Latitude = location.Latitude
                };
            }

            throw new System.Exception();
        }
        private async Task<string> AddLocationToRedis(double longitude, double latitude, string name)
        {
            name += $"_{Guid.NewGuid()}";
            var db = _redis.GetDatabase();
            await db.GeoAddAsync(geoKey, new GeoEntry(longitude, latitude, name));
            return name;
        }

        [HttpPost("add-location-by-hand")]
        public async Task<ActionResult<string>> AddGeoLocationAsync([FromBody] GeoLocation location)
        {
            var db = _redis.GetDatabase();

            location.Name += $"_{Guid.NewGuid()}";
            await db.GeoAddAsync(geoKey, new GeoEntry(location.Longitude, location.Latitude, location.Name));
            return Ok(location.Name);
        }

        [HttpGet("{nameWithUnderLineGuid}")]
        public async Task<ActionResult<GeoLocation>> GetGeoLocationAsync(string nameWithUnderLineGuid)
        {
            var db = _redis.GetDatabase();
            var position = await db.GeoPositionAsync(geoKey, nameWithUnderLineGuid);
            if (position != null)
            {
                return Ok(new GeoLocation { Name = nameWithUnderLineGuid, Longitude = position.Value.Longitude, Latitude = position.Value.Latitude });
            }
            return NotFound();
        }
        [HttpGet("GetAllGeoLocationsAsync")]
        public async Task<ActionResult<GeoLocation>> GetAllSortedGeoLocationsAsync()
        {
            var db = _redis.GetDatabase();
            var members = await db.SortedSetRangeByRankAsync(geoKey);
            var locations = new List<GeoLocation>();

            foreach (var member in members)
            {
                var position = await db.GeoPositionAsync(geoKey, member);
                if (position.HasValue)
                {
                    locations.Add(new GeoLocation
                    {
                        Name = member,
                        Longitude = position.Value.Longitude,
                        Latitude = position.Value.Latitude
                    });
                }
            }
            return Ok(locations);
        }
    }
}
