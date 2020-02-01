using PFW.Units.Component.Movement;
using UnityEngine;

namespace PFW.UI.Ingame.InputManagement
{
    public sealed class FastMoveMode : InputModeBase
    {
        public FastMoveMode(IInputModeContext context) : base(context)
        {
        }

        protected override IInputMode OnUpdate()
        {
            var newMode = ApplyHotkeys();

            if (newMode != this)
            {
                return newMode;
            }

            if (Input.GetMouseButtonDown(0))
            {
                MoveGhostsToMouse();
                Context.SelectionManager.DispatchMoveCommand(
                    false, MoveCommandType.FAST);
            }

            if ((Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
                || Input.GetMouseButtonDown(1))
                return ToNormalMode();

            return this;
        }

        protected override void OnEnter()
        {
            Context.SetCursorToPrimedReticle();
        }

        protected override InputManager.MouseMode ModeEnum => InputManager.MouseMode.FAST_MOVE;
    }
}