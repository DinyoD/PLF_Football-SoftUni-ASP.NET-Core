namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public class PositionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Positions.Any())
            {
                return;
            }

            await dbContext.Positions.AddAsync(new Position { Id = 1, Name = "Goalkeeper" });
            await dbContext.Positions.AddAsync(new Position { Id = 2, Name = "Defender" });
            await dbContext.Positions.AddAsync(new Position { Id = 3, Name = "Midfielder" });
            await dbContext.Positions.AddAsync(new Position { Id = 4, Name = "Forward" });

            await dbContext.SaveChangesAsync();
        }
    }
}
