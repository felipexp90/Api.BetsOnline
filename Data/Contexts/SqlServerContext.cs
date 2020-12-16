using Audit.Core;
using Data.Migrations;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public sealed class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
        }

        public DbSet<Bet> Bet { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GameType> GameType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
          .UseLazyLoadingProxies(true)
          .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning))
          .UseSqlServer(ConfigurationEnum.BaseConn);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region GameType
            modelBuilder.Entity<GameType>().ToTable("GameType").HasKey(x => x.Id);
            modelBuilder.Entity<GameType>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.Name).IsRequired().HasMaxLength(50);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdateDate).IsRequired(false);
            });
            #endregion

            #region Game
            modelBuilder.Entity<Game>().ToTable("Game").HasKey(x => x.Id);
            modelBuilder.Entity<Game>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.IdGameType).IsRequired();
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdateDate).IsRequired(false);
                x.Property(y => y.Enabled).IsRequired().HasDefaultValue(true);
                x.Property(y => y.WinningNumber).IsRequired(false);
                x.Property(y => y.WinningColor).HasMaxLength(10);
            });
            modelBuilder.Entity<Game>()
                    .HasOne(d => d.GameType)
                    .WithMany()
                    .HasForeignKey(d => d.IdGameType)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Bet
            modelBuilder.Entity<Bet>().ToTable("Bet").HasKey(x => x.Id);
            modelBuilder.Entity<Bet>(x =>
            {
                x.Property(y => y.Id).ValueGeneratedOnAdd();
                x.Property(y => y.IdGame).IsRequired();
                x.Property(y => y.Number).IsRequired(false);
                x.Property(y => y.Color).HasMaxLength(10);
                x.Property(y => y.MoneyBet).HasMaxLength(50);
                x.Property(y => y.CreatedDate).IsRequired();
                x.Property(y => y.UpdateDate).IsRequired(false);
                x.Property(y => y.IsWinner).IsRequired().HasDefaultValue(false);
                x.Property(y => y.EarnedMoney).HasMaxLength(50);
            });
            modelBuilder.Entity<Bet>()
                    .HasOne(d => d.Game)
                    .WithMany()
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            modelBuilder.Seed();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) => base.SaveChangesAsync(cancellationToken);
    }
}