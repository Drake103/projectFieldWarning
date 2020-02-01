using UnityEngine;

namespace PFW.UI.Ingame.InputManagement
{
    public sealed class SplitMode : InputModeBase
    {
        public SplitMode(IInputModeContext context) : base(context)
        {
        }

        protected override IInputMode OnUpdate()
        {
            var newMode = ApplyHotkeys();

            if (newMode != this)
            {
                return newMode;
            }

            if (Input.GetMouseButtonDown(0)) {
                Context.SelectionManager.DispatchSplitCommand(Context.LocalPlayerData);
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

        protected override InputManager.MouseMode ModeEnum => InputManager.MouseMode.SPLIT;
    }
}