namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefsPersistentProperty <TPropertyType>: PersistentProperty<TPropertyType>
    {
        protected string Key; //значение по которому у нас будет сохр и забираться проперти
        protected PrefsPersistentProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key; 
        }
    }
}