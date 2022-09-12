// ========================
// Revision 2020.11.18
// ========================

namespace TheGame.UI
{
    public interface ISettingsControl<T>
    {
        void SetValue(T value);
        T GetValue();
    }
}