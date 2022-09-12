using UnityEngine;


namespace NightFramework.UI
{
    public class MenuFinder : MonoBehaviour, IMenuMethods
    {
        // ========================================================================================
        [ReadOnly]
        public MenuBase2 Menu;
        public ComponentSearchRule FindBy;


        // ========================================================================================
        public void Switch()
        {
            if (Menu != null)
                Menu.Switch();
        }

        public void Switch(bool animate)
        {
            if (Menu != null)
                Menu.Switch(animate);
        }

        public void Close()
        {
            if (Menu != null)
                Menu.Close();
        }

        public void Close(bool animate)
        {
            if (Menu != null)
                Menu.Close(animate);
        }

        public void Open()
        {
            if (Menu != null)
                Menu.Open();
        }

        public void Open(bool animate)
        {
            if (Menu != null)
                Menu.Open(animate);
        }

        protected void OnEnable()
        {
            if (Menu == null)
            {
                var ms = Utils.FindObjectsOfTypeAtSceneAll<MenuBase2>();
                foreach (var m in ms)
                {
                    if (FindBy.Evaluate(m))
                    {
                        Menu = m;
                        return;
                    }
                }
            }
        }
    }
}