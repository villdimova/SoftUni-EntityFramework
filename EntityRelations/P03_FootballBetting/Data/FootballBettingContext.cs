namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class FootballBettingContext:DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            :base(options)
        {

        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataSettings.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(k => new { k.GameId, k.PlayerId });

            modelBuilder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(b => b.Bets)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bet>()
               .HasOne(b => b.Game)
               .WithMany(b => b.Bets)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
               .HasOne(t => t.PrimaryKitColor)
               .WithMany(t => t.PrimaryKitTeams)
               .HasForeignKey(t=>t.PrimaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
               .HasOne(t => t.SecondaryKitColor)
               .WithMany(t => t.SecondaryKitTeams)
               .HasForeignKey(t=>t.SecondaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Town>()
              .HasOne(t => t.Country)
              .WithMany(c => c.Towns)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
              .HasOne(p => p.Position)
              .WithMany(p => p.Players)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
               .HasMany(t => t.Players)
               .WithOne(p => p.Team)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerStatistic>()
               .HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerStatistics)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerStatistic>()
              .HasOne(ps => ps.Game)
              .WithMany(g => g.PlayerStatistics)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
             .HasOne(g => g.HomeTeam)
             .WithMany(ht => ht.HomeGames)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
             .HasOne(g => g.AwayTeam)
             .WithMany(at => at.AwayGames)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
