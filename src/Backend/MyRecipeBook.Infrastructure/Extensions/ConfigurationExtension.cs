﻿using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace MyRecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("Connection")!;
        }
    }
}