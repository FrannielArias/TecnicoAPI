﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicos.Data.Context;

namespace Tecnicos.Data.DI
{
    public static class DbContextRegistar
    {
        public static IServiceCollection RegisterDbContextFactory(this IServiceCollection services)
        {
            services.AddDbContextFactory<TecnicosContext>(options => options.UseSqlServer("Name=SqlConStr"));
            return services;
        }
    }
}
