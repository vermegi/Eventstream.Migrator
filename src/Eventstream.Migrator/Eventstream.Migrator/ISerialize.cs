namespace Eventstream.Migrator
{
    public interface ISerialize
    {
        TType Deserialize<TType>(object data);
    }
}