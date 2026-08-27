// using Domain.Models;
// using Microsoft.Extensions.Caching.Memory;
//
// namespace Infrastructure.Cache;
//
// // Infrastructure/Cache/Common
// public interface ICacheService<in TId, TEntity>
//     where TId : notnull
//     where TEntity : class
// {
//     string KeyBuilder(TId id);
//     
//     bool Exists(TId id);
//
//     TEntity? Get(TId id);
//
//     void Set(TId id,
//         TEntity entity,
//         TimeSpan? expiration = null);
//
//
//     void Remove(TId id);
// }
//
// // Infrastructure/Cache/Common
// public interface IMemoryCacheService<in TId, TEntity>
//     : ICacheService<TId, TEntity>
//     where TId : notnull
//     where TEntity : class
// {
//     void Set(
//         TId id,
//         TEntity entity,
//         MemoryCacheEntryOptions? options);
// }
//
// // Infrastructure/Cache/Common
// public class MemoryCacheService<TId, TEntity>(IMemoryCache memoryCache)
//     : IMemoryCacheService<TId, TEntity>
//     where TId : notnull
//     where TEntity : class
// {
//     public bool Exists(TId id)
//         => memoryCache.TryGetValue(id, out _);
//
//     public TEntity? Get(TId id)
//         => memoryCache.TryGetValue(id, out TEntity? entity)
//             ? entity
//             : null;
//
//     public void Set(
//         TId id,
//         TEntity entity,
//         TimeSpan? expiration)
//     {
//         if (expiration is null)
//             memoryCache.Set(id, entity);
//         else
//             memoryCache.Set(id, entity, expiration.Value);
//     }
//
//     public bool Remove(TId id)
//         => memoryCache.Remove(id);
//
//     public void Set(
//         TId id,
//         TEntity entity,
//         MemoryCacheEntryOptions? options)
//     {
//         options ??= new MemoryCacheEntryOptions()
//             .SetSlidingExpiration(TimeSpan.FromMinutes(10))
//             .SetAbsoluteExpiration(TimeSpan.FromHours(1));
//
//         memoryCache.Set(id, entity, options);
//     }
// }
//
// // Infrastructure/Cache/Hotel
// public interface IHotelCacheService
//     : ICacheService<Guid, Hotel>
// {
// }
//
// // Infrastructure/Cache/Hotel
// public class HotelMemoryCacheService(IMemoryCache memoryCache)
//     : MemoryCacheService<Guid, Hotel>(memoryCache)
//         , IHotelCacheService
// {
// }

