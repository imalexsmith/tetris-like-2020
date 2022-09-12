// ========================
// Revision 2020.11.10
// ========================

namespace TheGame
{
    public static class Exceptions
    {
        public const string GameDataLibraryMustBeUnique = "There cannot be more or less than single GameDataLibrary asset. Something went wrong...";
        public const string PersistentDataStorageMustBeUnique = "There cannot be more or less than single PersistentDataStorage asset. Something went wrong...";
        public const string MassSpawnerNotFound = "Mass spawner isn't on scene! Can't instantiate objects massively.";
    }
}