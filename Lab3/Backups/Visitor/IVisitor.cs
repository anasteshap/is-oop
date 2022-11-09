using Backups.Component;
using Backups.ZipObjects;

namespace Backups.Visitor;

public interface IVisitor
{
    IReadOnlyCollection<IZipObject> GetZipObjects();
    void CreateZipFile(FileComponent fileComponent);
    void CreateZipFile(FolderComponent folderComponent);
}