using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FileComponent : IFileComponent
{
    private readonly Func<string, Stream> _streamCreator;
    public FileComponent(string path, Func<string, Stream> streamCreator)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception();
        }

        FullName = path;
        _streamCreator = streamCreator;
    }

    public string FullName { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }

    public Stream OpenStream() => _streamCreator(FullName);
}