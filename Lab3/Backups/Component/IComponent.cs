using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public interface IComponent
{
    string FullName { get; }
    void Accept(IVisitor visitor);
    Stream OpenStream();
}