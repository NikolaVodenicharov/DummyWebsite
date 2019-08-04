namespace Tyres.Shared.Mappings
{
    /// <summary>
    /// We implement this interface in the destination types for our auto mapping. After that
    /// the AutoMapperProfile will map all the types that implement this interface. From where 
    /// we will get destination types and source types defined in the generic argument <typeparamref name="T"/>
    /// </summary>
    public interface IMapFrom<T>
    {
    }
}
