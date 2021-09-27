using Deviot.Common;
using Deviot.Hermes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Infra.SQLite.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class Seed
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            // User
            var users = new List<User>();
            users.Add(new User(new Guid("7011423f65144a2fb1d798dec19cf466"), "Administrador", "admin", Utils.Encript("admin"), true, true));
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
