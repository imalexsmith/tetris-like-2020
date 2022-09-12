// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework
{
    public static class Exceptions
    {
        public const string UnexpectedBehaviourWhileRandomSelection = "Unexpected behaviour during selection from RandomisedSet.";
        public const string ApplicationSettingsMustBeUnique = "There cannot be more or less than single ApplicationSettings asset. Something went wrong...";
        public const string AdaptiveTextHolderExpected = "This component must inherit either ITMPTextHolder or IUGUITextHolder.";
    }
}