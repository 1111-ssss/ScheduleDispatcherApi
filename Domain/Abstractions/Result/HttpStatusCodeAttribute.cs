namespace Domain.Abstractions.Result;

[AttributeUsage(AttributeTargets.Field)]
public class HttpStatusCodeAttribute : Attribute
{
    public int StatusCode { get; }

    public HttpStatusCodeAttribute(int statusCode)
    {
        StatusCode = statusCode;
    }

    public static int GetHttpStatusCode(ErrorCode errorCode)
    {
        var field = errorCode.GetType().GetField(errorCode.ToString());
        if (field == null)
            return 400;

        var attr = (HttpStatusCodeAttribute?)Attribute.GetCustomAttribute(
            field, 
            typeof(HttpStatusCodeAttribute)
        );

        return attr?.StatusCode ?? 400;
    }
}