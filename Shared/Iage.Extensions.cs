namespace IAGE.Shared;

public static class IAGExtensions
{
    public static string AsString(this object obj)
    {
        return (string)obj;
    }

    public static Guid AsGuid(this object obj)
    {
        return Guid.Parse(obj.ToString());
    }

    public static bool AsBool(this object obj)
    {
        return (bool)obj;
    }

    public static int AsInt(this object obj)
    {
        return (int)obj;
    }

    public static DateTime AsDateTime(this object obj)
    {
        return (DateTime)obj;
    }
}