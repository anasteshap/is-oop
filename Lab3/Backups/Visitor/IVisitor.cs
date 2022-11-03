using Backups.Component;

namespace Backups.Visitor;

public interface IVisitor
{
    void CreateZipFile(FileComponent fileComponent);
    void CreateZipFile(FolderComponent folderComponent);
}