using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FolderComponent : IFolderComponent
{
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;
    public FolderComponent(IRepository repository, string path, Func<string, IReadOnlyCollection<IComponent>> componentsCreator)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception();
        }

        Repository = repository;
        FullName = path;
        _componentsCreator = componentsCreator;
    }

    public IRepository Repository { get; }
    public string FullName { get; }

    public IReadOnlyCollection<IComponent> Components => _componentsCreator(FullName);

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }

    public Stream OpenStream()
    {
        throw new NotImplementedException();
    }
}