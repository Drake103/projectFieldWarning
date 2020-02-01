using PFW.Model.Armory;

namespace PFW.UI.Ingame.InputManagement
{
    public interface IInputMode
    {
        void Enter();
        void Exit();
        void OnGUI();

        IInputMode HandleUpdate();

        /**
         * Called when a unit card from the buy menu is pressed.
         */
        IInputMode BuyCallback(Unit unit);
    }
}