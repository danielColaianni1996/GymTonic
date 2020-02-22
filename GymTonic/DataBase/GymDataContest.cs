using GymTonic.DataBase.Table;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase
{
    public class GymDataContest : IdentityDbContext
    {
        public GymDataContest(DbContextOptions<GymDataContest> options) : base(options)
        {

        }
        public DbSet<Utenti> Utenti { get; set; }
        public DbSet<Esercizi> Esercizi { get; set; }
        public DbSet<SchedePersonali> SchedePersonali { get; set; }
        public DbSet<SchedeEsercizi> SchedeEsercizi { get; set; }
        public DbSet<Schede> Schede { get; set; }
        public DbSet<Abbonamenti> Abbonamenti { get; set; }
        public DbSet<TipiAbbonamenti> TipiAbbonamenti { get; set; }
        public DbSet<Peso> Peso { get; set; }
        public DbSet<OrariLavorativi> OrariLavorativi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utenti>().ToTable("Utenti");
            modelBuilder.Entity<Esercizi>().ToTable("Esercizi");
            modelBuilder.Entity<SchedePersonali>().ToTable("SchedePersonali");
            modelBuilder.Entity<SchedeEsercizi>().ToTable("SchedeEsercizi");
            modelBuilder.Entity<Schede>().ToTable("Schede");
            modelBuilder.Entity<Abbonamenti>().ToTable("Abbonamenti");
            modelBuilder.Entity<TipiAbbonamenti>().ToTable("TipiAbbonamenti");
            modelBuilder.Entity<Peso>().ToTable("Peso");
            modelBuilder.Entity<OrariLavorativi>().ToTable("OrariLavorativi");
            base.OnModelCreating(modelBuilder);
        }
    }
}
