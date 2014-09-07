namespace Eventstream.Migrator.Utilities
{
    public interface ISerialize
    {
        TType Deserialize<TType>(object data);
    }
}