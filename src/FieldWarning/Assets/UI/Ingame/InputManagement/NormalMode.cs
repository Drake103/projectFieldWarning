namespace PFW.UI.Ingame.InputManagement
{
    public sealed class NormalMode : InputModeBase
    {
        protected override IInputMode OnUpdate()
        {
            var newMode = ApplyHotkeys();

            if (newMode != this)
            {
                return newMode;
            }

            RightClickManager.Update();

            return this;
        }

        protected override void OnEnter()
        {
            Context.ResetCursor();
        }

        public NormalMode(IInputModeContext context) : base(context)
        {
            RightClickManager = new ClickManager(
                1, MoveGhostsToMouse, OnOrderShortClick, OnOrderLongClick, OnOrderHold);
        }

        private ClickManager RightClickManager { get; }

        protected override InputManager.MouseMode ModeEnum => InputManager.MouseMode.NORMAL;
    }
}