using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FileComponent : IComponent
{
    private readonly Func<Stream> _streamCreator;
    public FileComponent(string name, Func<Stream> streamCreator)
    {
        _streamCreator = streamCreator;
        Name = name;
    }

    public string Name { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }

    public Stream OpenStream() => _streamCreator();
}