using CodingExercise.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Infrastructure
{
    public class CodingExerciseContext : DbContext
    {
        public CodingExerciseContext(DbContextOptions<CodingExerciseContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Item>(item =>
            {
                item.HasKey(i => i.Key);
            });
        }
    }
}
