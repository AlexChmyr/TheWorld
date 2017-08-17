using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TheWorld.Models
{
    public class WorldContextSeedData
    {
        private WorldContext _context;
        private UserManager<WorldUser> _userManager;

        public WorldContextSeedData(WorldContext context, UserManager<WorldUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            // Users
            if (await _userManager.FindByEmailAsync("admin@test.ua") == null)
            {
                var newUser = new WorldUser
                {
                    UserName = "Admin",
                    Email = "admin@test.ua",

                };

                var createResult = await _userManager.CreateAsync(newUser,"123456");
            }

            // Sample data
            if (!_context.Trips.Any())
            {
 
                var worldTrip = new Trip()
                {
                    Name = "World Trip",
                    Created = DateTime.UtcNow,
                    User = "Admin",
                    Stops = new List<Stop>()
                    {
                        new Stop() { Order = 0, Latitude =  33.748995, Longitude =  -84.387982, Name = "Atlanta, Georgia", Arrival = DateTime.Parse("Jun 3, 2014") },
                        new Stop() { Order = 1, Latitude =  51.508515, Longitude =  -0.125487, Name = "London, UK", Arrival = DateTime.Parse("Aug 1, 2014") },
                        new Stop() { Order = 2, Latitude =  46.519962, Longitude =  6.633597, Name = "Lausanne, Switzerland", Arrival = DateTime.Parse("Aug 29, 2014") },
                        new Stop() { Order = 3, Latitude =  42.697708, Longitude =  23.321868, Name = "Sofia, Bulgaria", Arrival = DateTime.Parse("Oct 16, 2014") },
                        new Stop() { Order = 4, Latitude =  -25.746111, Longitude =  28.188056, Name = "Pretoria, South Africa", Arrival = DateTime.Parse("Jan 19, 2015") },
                        new Stop() { Order = 5, Latitude =  39.904030, Longitude =  116.407526, Name = "Beijing, China", Arrival = DateTime.Parse("Apr 19, 2015") },
                        new Stop() { Order = 6, Latitude =  33.748995, Longitude =  -84.387982, Name = "Atlanta, Georgia", Arrival = DateTime.Parse("Jun 17, 2015") },
                    }
                };

                _context.Trips.Add(worldTrip);
                _context.Stops.AddRange(worldTrip.Stops);

                _context.SaveChanges();
            }
        }
    }
}
