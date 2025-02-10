namespace Flatiron.API.SharedFeatures.Services;

public interface ICacheService<T>
{
    void Set(T data);
    T? Get();
    void Remove();
}

