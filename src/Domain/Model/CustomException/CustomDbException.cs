namespace Domain.Model.CustomException;

public class CustomDbException : Exception
{
    public string ErrorMessage { get; set; }

    public string SqlState { get; set; }

    public CustomDbException(string message, string sqlState)
    {
        ErrorMessage = message;
        SqlState = sqlState;
    }

}
