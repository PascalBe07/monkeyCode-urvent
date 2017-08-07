using AutoMapper;
// ReSharper disable once CheckNamespace
public static class AutoMapperExtensions
{
    public static TDestination Map<TSource , TDestination>(this IMapper mapper, TSource source, TDestination destination)
    {
        return mapper.Map(source, destination);
    }
}

    
