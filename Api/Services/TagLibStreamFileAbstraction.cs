namespace Api.Services;

public sealed class TagLibStreamFileAbstraction : TagLib.File.IFileAbstraction
{
    public string Name { get; }
    public Stream ReadStream { get; }
    public Stream WriteStream { get; }

    public TagLibStreamFileAbstraction(
        string name,
        Stream readStream,
        Stream writeStream)
    {
        Name = name;
        ReadStream = readStream;
        WriteStream = writeStream;
    }

    public void CloseStream(Stream stream)
    {
        // do nothing, lifecycle is managed by the caller
    }
}
