namespace Service.Database.Interface
{
    public interface IDatabase
    {
        public Devices LoadData();
        public void SaveData(Devices devices);
    }
}