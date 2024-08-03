using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using MyNewwRedis.Models;

namespace MyNewwRedis.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.HasData(
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "as-roma-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "barcelona-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "ac-milan-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "manchester-united-kit "
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "real-madrid-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "atletico-madrid-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "fenerbahce-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "manchester-city-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "liverpool-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "chelsea-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "arsenal-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "juventus-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "tottenham-hotspur-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "porto-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "sporting-cp-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "benfica-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "inter-milan-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "dortmund-kit"
              },
             new Product
              {
                 Id = Guid.NewGuid(),
                 Name = "bayern-munchen-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "paris-saint-germain-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "ajax-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "bayer-04-leverkusen-kit"
              },
             new Product
              {
                 Id = Guid.NewGuid(),
                 Name = "psv-eindhoven-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "ssc-napoli-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "rb-leipzig-kit"
              },
             new Product
             {
                 Id = Guid.NewGuid(),
                 Name = "galatasaray-kit"
             },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "red-star-belgrade-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "marseille-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "lyon-kit"
              },
             new Product
             {
                 Id = Guid.NewGuid(),
                 Name = "sevilla-kit"
             },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "celtic-kit"
              },
              new Product
              {
                  Id = Guid.NewGuid(),
                  Name = "lazio-kit"
              }
              );
        }
    }
}
