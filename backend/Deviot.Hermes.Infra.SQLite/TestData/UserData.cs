using Deviot.Common;
using Deviot.Hermes.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Deviot.Hermes.Infra.SQLite.TestData
{
    public class UserData
    {
        public static IEnumerable<User> GetUsers()
        {
            var users = new List<User>(4);
            users.Add(new User(new Guid("7011423f65144a2fb1d798dec19cf466"), "Administrador", "admin", Utils.Encript("admin"), true, true));
            users.Add(new User(new Guid("e1386eb28ae443e9a13855ce66ebce7d"), "Paulo Cesar de Souza", "paulo", Utils.Encript("123456"), true, false));
            users.Add(new User(new Guid("630994d9e6c34d5cb823569560697d67"), "Bruna Stefano Marques", "bruna", Utils.Encript("123456"), true, false));
            users.Add(new User(new Guid("f22e81455a6f4961922a516c54d33dba"), "Paula Stefano Souza", "paula", Utils.Encript("123456"), true, false));

            return users;
        }
    }
}
