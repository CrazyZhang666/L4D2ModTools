namespace SharpVPK.Exceptions;

public class ArchiveParsingException : Exception
{
    public ArchiveParsingException()
    {
    }

    public ArchiveParsingException(string message)
        : base(message)
    {
    }

    public ArchiveParsingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
