using Movimientos.Services.Interfaces;

namespace Movimientos.Platforms.Android;

public class DbPathService : IDbPathService
{
    public string GetPath(string fileName)
    {
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(folderPath, fileName);
    }
}
