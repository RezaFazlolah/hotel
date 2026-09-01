using AutoMapper;
using SharedKernel.Common;

namespace Application.Common.Extensions;

public static class ResultExtensions
{
    extension<TSource, TDestination>(Result<TSource> result)
    {
        public Result<TDestination> Map(IMapper mapper)
            => result.Succeeded
                ? mapper.Map<Result<TDestination>>(result)
                : Result<TDestination>.Failure(result.Errors, result.Code, result.Message);
    }
}