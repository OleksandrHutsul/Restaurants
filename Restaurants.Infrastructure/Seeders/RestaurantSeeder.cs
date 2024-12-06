using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles =
                [
                    new(UserRoles.User)
                    {
                        NormalizedName = UserRoles.User.ToUpper()
                    },
                    new(UserRoles.Owner)
                    {
                        NormalizedName = UserRoles.Owner.ToUpper()
                    },
                    new(UserRoles.Admin)
                    {
                        NormalizedName = UserRoles.Admin.ToUpper()
                    },
                ];

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = [
                new()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description =
                        "KFC ( short for Kentucky Fried Chicken) is an American fast food restaurant chain",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes =
                    [
                        new()
                        {
                            Name = "Nashville Hot Chicken",
                            Description = "Nashville Hot Chicken (10 pcs.)",
                            Price = 10.30M,
                        },

                        new()
                        {
                            Name = "Chicken Nuggets",
                            Description = "Chicken Nuggets (5 pcs.)",
                            Price = 5.30M,
                        },
                    ],
                    Address = new ()
                    {
                        City = "London",
                        Street = "Cork St 5",
                        PostalCode = "WC2N 5DU"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald Szewska",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's)",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new ()
                    {
                        City = "London",
                        Street = "Boots 193",
                        PostalCode = "W1F 8SR"
                    }
                }
                ];
            return restaurants;
        }
    }
}
