using Grpc.Core;
using Grpc.Core.Interceptors;

namespace SkillUpHub.Profile.API.Middlewares;

public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            // Передаем запрос дальше по конвейеру
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            // Логируем или обрабатываем исключение
            throw new RpcException(new Status(StatusCode.Aborted, ex.Message));
        }
    }
}