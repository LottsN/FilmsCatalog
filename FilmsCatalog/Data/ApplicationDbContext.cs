using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FilmsCatalog.Models;

namespace FilmsCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<FilmsGenres> FilmsGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Setting foreign keys
            builder.Entity<FilmsGenres>()
                .HasOne<Film>()
                .WithMany()
                .HasForeignKey(x => x.FilmId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FilmsGenres>()
                .HasOne<Genre>()
                .WithMany()
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            //setting primary keys
            builder.Entity<FilmsGenres>()
                .HasKey(x => new { x.FilmId, x.GenreId });

            //setting Genres data
            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 1,
                    Name = "Action"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 2,
                    Name = "Comedy"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 3,
                    Name = "Drama"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 4,
                    Name = "Fantasy"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 5,
                    Name = "Mystery"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 6,
                    Name = "Romance"
                }
            );

            builder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 7,
                    Name = "Thriller"
                }
            );

            builder.Entity<Genre>().HasData(
               new Genre
               {
                   GenreId = 8,
                   Name = "Western"
               }
           );
        }
    }
}
