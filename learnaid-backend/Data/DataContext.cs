namespace learnaid_backend.Data
{
    using learnaid_backend.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {
        }

        public DbSet<Survey> Survey { get; set; }

        public DbSet<User> User { get; set; }

        /// <summary>
        ///     Se utiliza para la construccion de los modelos con Entity Framework.
        ///     Esta estrategia es mas conocida como fluent api, y la utilizo solamente cuando
        ///     no existen anotaciones para funciones especificas que se pueden logran con dicha 
        ///     estrategia.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>()
                .Property(b => b.Id)
                .IsRequired()
                .ValueGeneratedNever();

            modelBuilder.Entity<Survey>()
               .Property(b => b.OptionsList)
               .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)
                );
        }
    }
}
}
