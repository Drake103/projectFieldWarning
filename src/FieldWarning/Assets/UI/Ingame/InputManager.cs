/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

using PFW.Model.Game;
using PFW.Model.Armory;
using PFW.UI.Ingame.InputManagement;
using PFW.Units;
using PFW.Units.Component.Movement;

namespace PFW.UI.Ingame
{
    /**
     * Handles almost all input during a match.
     *
     * Some input, particularly for to selecting and deselecting units,
     * is handled in SelectionManager instead.
     */
    public class InputManager : MonoBehaviour
    {
        public enum MouseMode {
            NORMAL,       //< Left click selects, right click orders normal movement or attack.
            PURCHASING,   //< Left click purchases platoon, right click cancels.
            FIRE_POS,     //< Left click orders fire position, right click cancels.
            REVERSE_MOVE, //< Left click reverse moves to cursor, right click cancels.
            FAST_MOVE,    //< Left click fast moves to cursor, right click cancels.
            SPLIT         //< Left click splits the platoon, right click cancels.
        }

        private IInputMode InputMode
        {
            get => _inputMode;
            set
            {
                var oldInputMode = _inputMode;
                if (_inputMode == value)
                {
                    return;
                }

                oldInputMode?.Exit();
                _inputMode = value;
                _inputMode.Enter();
            }
        }

        private IInputMode _inputMode;
        private InputModeContext _inputModeContext;

        private void Awake()
        {
            _inputModeContext = new InputModeContext();
            InputMode = new NormalMode(_inputModeContext);
        }

        private void Update()
        {
            InputMode = InputMode.HandleUpdate();
        }

        private void OnGUI()
        {
            InputMode.OnGUI();
        }

        public void RegisterPlatoonBirth(PlatoonBehaviour platoon)
        {
            _inputModeContext.RegisterPlatoonBirth(platoon);
        }

        public void RegisterPlatoonDeath(PlatoonBehaviour platoon)
        {
            _inputModeContext.RegisterPlatoonDeath(platoon);
        }

        public void BuyCallback(Unit unit)
        {
            InputMode = InputMode.BuyCallback(unit);
        }
    }

    public static class Commands
    {
        public static bool Unload => Input.GetKeyDown(Hotkeys.Unload);

        public static bool Load => Input.GetKeyDown(Hotkeys.Load);

        public static bool FirePos => Input.GetKeyDown(Hotkeys.FirePos);

        public static bool ReverseMove => Input.GetKeyDown(Hotkeys.ReverseMove);

        public static bool FastMove => Input.GetKeyDown(Hotkeys.FastMove);

        public static bool Split => Input.GetKeyDown(Hotkeys.Split);
    }
}