using Backups.Entities;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Component;

public interface IComponent
{
    IRepository Repository { get; }
    string FullName { get; }
    void Accept(IVisitor visitor);
}