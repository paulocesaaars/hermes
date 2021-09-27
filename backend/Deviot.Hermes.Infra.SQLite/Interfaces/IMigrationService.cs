namespace Deviot.Hermes.Infra.SQLite.Interfaces
{
    public interface IMigrationService
    {
        public void Execute();

        public void Deleted();

        public void Populate();
    }
}
