using Entities;
using Entities.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Migrations
{
    internal static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            try
            {
                DateTime currentDate = GenericUtil.GetDateZone(DateTime.Now);

                modelBuilder.Entity<GameType>().HasData(
                    new GameType { Id = 1, Name = "Ruleta", CreatedDate = currentDate },
                    new GameType { Id = 2, Name = "Uno", CreatedDate = currentDate },
                    new GameType { Id = 3, Name = "BlackJack", CreatedDate = currentDate }
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}