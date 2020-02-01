using System;
using System.Collections.Generic;
using PFW.Model.Game;
using PFW.Units;
using UnityEngine;

namespace PFW.UI.Ingame.InputManagement
{
    public sealed class InputModeContext : IInputModeContext
    {
        private Texture2D _firePosReticle;
        private Texture2D _primedReticle;

        public InputModeContext()
        {
            SelectionManager = new SelectionManager();
            SelectionManager.Awake();

            _firePosReticle = (Texture2D) Resources.Load("FirePosTestTexture");
            if (_firePosReticle == null)
                throw new Exception("No fire pos reticle specified!");

            _primedReticle = (Texture2D) Resources.Load("PrimedCursor");
            if (_primedReticle == null)
                throw new Exception("No primed reticle specified!");
        }

        public MatchSession MatchSession => MatchSession.Current;

        public SelectionManager SelectionManager { get; }

        public PlayerData LocalPlayerData => MatchSession.LocalPlayer.Data;

        public IReadOnlyList<SpawnPointBehaviour> SpawnPoints => MatchSession.SpawnPoints;

        public BuyTransaction CurrentBuyTransaction { get; set; }

        public void SetCursorToFirePosReticle()
        {
            Vector2 hotspot = new Vector2(_firePosReticle.width / 2, _firePosReticle.height / 2);
            Cursor.SetCursor(_firePosReticle, hotspot, CursorMode.Auto);
        }

        public void ResetCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void SetCursorToPrimedReticle()
        {
            Cursor.SetCursor(_primedReticle, Vector2.zero, CursorMode.Auto);
        }

        public void RegisterPlatoonBirth(PlatoonBehaviour platoon)
        {
            SelectionManager.RegisterPlatoonBirth(platoon);
        }

        public void RegisterPlatoonDeath(PlatoonBehaviour platoon)
        {
            SelectionManager.RegisterPlatoonDeath(platoon);
        }
    }
}