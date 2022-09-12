// ========================
// Revision 2019.12.18
// ========================

namespace NightFramework
{
    public interface ISaveLoadFile
    {
        string FileFullPath { get; }

        void SaveToFile(bool compressed = true);

        bool LoadFromFile();
    }
}
