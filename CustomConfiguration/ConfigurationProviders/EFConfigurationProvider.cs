﻿using CustomConfiguration.Context;
using CustomConfiguration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomConfiguration
{
    public class EFConfigurationProvider : ConfigurationProvider
    {
        public EFConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        Action<DbContextOptionsBuilder> OptionsAction { get; }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();

            OptionsAction(builder);

            using (var dbContext = new ConfigurationDbContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();

                Data = !dbContext.ConfigurationEntities.Any()
                    ? CreateAndSaveDefaultValues(dbContext)
                    : dbContext.ConfigurationEntities.ToDictionary(c => c.Key, c => c.Value);
            }
        }

        private static IDictionary<string, string> CreateAndSaveDefaultValues(ConfigurationDbContext dbContext)
        {
            var configValues =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
           

            dbContext.ConfigurationEntities.AddRange(configValues
                .Select(kvp => new ConfigurationEntity
                {
                    Key = kvp.Key,
                    Value = kvp.Value
                })
                .ToArray());

            dbContext.SaveChanges();

            return configValues;
        }
    }
}
