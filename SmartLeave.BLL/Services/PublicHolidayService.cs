using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartLeave.BLL.Interfaces;
using SmartLeave.DAL.Data;
using SmartLeave.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartLeave.BLL.Services
{
    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public PublicHolidayService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<PublicHoliday>> GetPublicHolidaysAsync(string country)
        {
            try
            {
                var cacheKey = $"Holidays_{country}_2025";
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                    return JsonSerializer.Deserialize<List<PublicHoliday>>(cached)!;

                var holidays = await _context.PublicHolidays
                    .Where(h => h.HolidayDate.Year == 2025 &&
                                h.Description!.ToLower().Contains(country.ToLower()))
                    .ToListAsync();

                var serialized = JsonSerializer.Serialize(holidays);
                await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return holidays;
            }

            catch(Exception ex)
            {
                throw new Exception("Failed to get holidays", ex);
            }
        }
    }
}
