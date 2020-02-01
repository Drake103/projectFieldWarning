using System.Collections.Generic;
using PFW.Model.Game;
using PFW.Units;

namespace PFW.UI.Ingame.InputManagement
{
    public interface IInputModeContext
    {
        MatchSession MatchSession { get; }
        PlayerData LocalPlayerData { get; }
        IReadOnlyList<SpawnPointBehaviour> SpawnPoints { get; }
        BuyTransaction CurrentBuyTransaction { get; set; }
        SelectionManager SelectionManager { get; }
        void RegisterPlatoonBirth(PlatoonBehaviour platoon);
        void RegisterPlatoonDeath(PlatoonBehaviour platoon);
        void ResetCursor();
        void SetCursorToPrimedReticle();
        void SetCursorToFirePosReticle();
    }
}