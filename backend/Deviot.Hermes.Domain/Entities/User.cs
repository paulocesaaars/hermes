using System;

namespace Deviot.Hermes.Domain.Entities
{
    public class User : Entity
    {
        public string FullName { get; protected set; }

        public string UserName { get; protected set; }

        public bool Enabled { get; protected set; }

        public bool Administrator { get; protected set; }

        public string Password { get; protected set; }

        public User()
        {

        }

        public User(Guid id, string name, string userName, string password, bool enabled = false, bool administrator = false)
        {
            Id = id;
            FullName = name;
            UserName = userName.ToLower();
            Password = password;
            Enabled = enabled;
            Administrator = administrator;
        }

        public void SetFullName(string value) => FullName = value;

        public void SetUserName(string value) => UserName = value.ToLower();

        public void SetEnabled(bool value) => Enabled = value;

        public void SetAdministrator(bool value) => Administrator = value;

        public void SetPassword(string value) => Password = value;
    }
}
