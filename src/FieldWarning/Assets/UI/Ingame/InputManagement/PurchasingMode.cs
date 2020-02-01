namespace PFW.UI.Ingame.InputManagement
{
    public sealed class PurchasingMode : InputModeBase
    {
        public PurchasingMode(IInputModeContext context)
            : base(context)
        {
        }

        protected override InputManager.MouseMode ModeEnum => InputManager.MouseMode.PURCHASING;

        protected override IInputMode OnUpdate()
        {
            if (Util.GetTerrainClickLocation(out var hit))
                return ShowGhostUnitsAndMaybePurchase(hit);

            return MaybeExitPurchasingModeAndRefund();
        }

        protected override void OnExit()
        {
            Context.CurrentBuyTransaction?.PreviewPlatoons.Clear();
            Context.CurrentBuyTransaction = null;
        }
    }
}