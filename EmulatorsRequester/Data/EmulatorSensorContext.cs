#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;
using EmulatorsRequester.Models;

namespace EmulatorsRequester.Data
{
    public class EmulatorSensorContext: DbContext
    {
        public EmulatorSensorContext(DbContextOptions<EmulatorSensorContext> options)
            : base(options)
        {

        }

        public DbSet<Sensor> Sensors { get; set; }
    }
}
