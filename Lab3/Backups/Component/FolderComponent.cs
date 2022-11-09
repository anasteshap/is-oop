using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FolderComponent : IComponent
{
    private readonly Func<IReadOnlyCollection<IComponent>> _componentsCreator;

    public FolderComponent(string name, Func<IReadOnlyCollection<IComponent>> componentsCreator)
    {
        _componentsCreator = componentsCreator;
        Name = name;
    }

    public IReadOnlyCollection<IComponent> Components => _componentsCreator();

    public string Name { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }

    public Stream OpenStream()
    {
        throw new NotImplementedException();
    }
}