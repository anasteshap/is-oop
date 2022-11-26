using Backups.Component;
using Backups.ZipObjects;

namespace Backups.Visitor;

public interface IFileComponentVisitor
{
    void Visit(FileComponent fileComponent);
    void Visit(FolderComponent folderComponent);
}