using CodingExercise.Infrastructure;
using CodingExercise.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingExercise.Tests.Api
{
    public static class DataSeeder
    {
        public static List<Item> Items;

        public static void ClearDatabase(CodingExerciseContext context)
        {
            context.Items.RemoveRange(context.Items);
        }
        public static void PopulateDatabase(CodingExerciseContext context)
        {
            Items = new List<Item>
            {
                new Item { Key = "1", Value = "value1" },
                new Item { Key = "2", Value = "value2" },
                new Item { Key = "3", Value = "value3" },
                new Item { Key = "4", Value = "value4" },
                new Item { Key = "5", Value = "value5" },
            };

            context.Items.AddRange(Items);
            context.SaveChanges();
        }
    }
}
