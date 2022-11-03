using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FolderComponent : IComponent
{
    private readonly List<IComponent> _components = new ();
    public FolderComponent(IRepository repository, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception();
        }

        Repository = repository;
        FullName = path;

        var listObj = Repository.GetRelativePathsOfFolderSubFiles(FullName).ToList();
        foreach (var obj in listObj)
        {
            if (Repository.DirectoryExists(obj))
            {
                _components.Add(new FolderComponent(Repository, obj));
            }

            if (Repository.FileExists(obj))
            {
                _components.Add(new FileComponent(Repository, obj));
            }
        }
    }

    public IRepository Repository { get; }
    public string FullName { get; }

    public IReadOnlyCollection<IComponent> Components => _components;

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }

    /*public void AddComponent(IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);
        _components.Add(component);
    }*/
}