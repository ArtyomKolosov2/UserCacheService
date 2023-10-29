using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UserCacheService.Domain.Error;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Dtos;

namespace UserCacheService.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public UnhandledExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception exception)
        {
            await UnhandledExceptionToErrorResponseDto(context, exception);
        }
    }

    private static Task UnhandledExceptionToErrorResponseDto(HttpContext context, Exception exception)
    {
        context.Response.ContentType = context.Request.ContentType!;
        context.Response.StatusCode = MapExceptionTypeToHttpResponseCode(exception);
        
        var errorDto = BuildErrorResponseDto(exception);

        return WriteErrorResponseDtoToResponse(context, errorDto);
    }

    private static async Task WriteErrorResponseDtoToResponse(HttpContext httpContext, ErrorResponseDto errorResponseDto)
    {
        if (httpContext.Response.ContentType is "application/xml")
        {
            await using var stream = new MemoryStream();
            await using var writer = new XmlTextWriter(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(errorResponseDto.GetType());
            serializer.Serialize(writer, errorResponseDto);

            stream.Position = 0;
            
            using var reader = new StreamReader(stream, Encoding.UTF8);

            var result = await reader.ReadToEndAsync();
            httpContext.Response.ContentLength = result.Length;
            await httpContext.Response.WriteAsync(result);
        }
        else
            await httpContext.Response.WriteAsJsonAsync(errorResponseDto);
    }

    private static ErrorResponseDto BuildErrorResponseDto(Exception exception) =>
        new()
        {
            ErrorId = GetErrorIdFromExceptionType(exception),
            ErrorMessage = exception.Message
        };

    private static int MapExceptionTypeToHttpResponseCode(Exception exception) => (int)(exception switch
    {
        UserInfoAlreadyExistsException => HttpStatusCode.Conflict,
        UserInfoNotFoundException => HttpStatusCode.NotFound,
        ServiceBaseException => HttpStatusCode.UnprocessableEntity,
        var _ => HttpStatusCode.InternalServerError,
    });

    private static int GetErrorIdFromExceptionType(Exception exception) => exception switch
    {
        ServiceBaseException baseException => baseException.ErrorId,
        var _ => (int)ErrorCode.InternalError,
    };
}