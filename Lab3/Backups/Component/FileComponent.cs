using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public class FileComponent : IComponent
{
    public FileComponent(IRepository repository, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception();
        }

        Repository = repository;
        FullName = path;
    }

    public IRepository Repository { get; }
    public string FullName { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.CreateZipFile(this);
    }
}