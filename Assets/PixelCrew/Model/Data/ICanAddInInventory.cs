namespace PixelCrew.Model.Data
{
    public interface ICanAddInInventory //интерфейс, кот позволяет иметь доступ к методу, кот добавляет что-либо в инвентарь
    {
        void AddInInventory(string id, int value);
    }
}