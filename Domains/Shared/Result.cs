namespace Org.Domains.Shared;

public class Result
{
    private Result(bool success, List<string> errors = null)
    {
        Successed = success;
        Errors = errors;
    }

    private Result(bool success, List<ErrorCode> errors = null)
    {
        Successed = success;
        ErrorCodes = errors;
    }

    public Result()
    {
        Successed = true;
    }

    public bool Successed { get; set; }
    public List<string> Errors { get; set; }
    public List<ErrorCode> ErrorCodes { get; set; }

    public static Result Succes => new();

    public static Result Failure(List<string> errors)
    {
        return new Result(false, errors);
    }

    public static Result Failure(List<ErrorCode> errors)
    {
        return new Result(false, errors);
    }
}