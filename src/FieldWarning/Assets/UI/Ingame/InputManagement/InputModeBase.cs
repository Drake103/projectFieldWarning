using System.Linq;
using PFW.Model.Armory;
using PFW.Units.Component.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PFW.UI.Ingame.InputManagement
{
    public abstract class InputModeBase : IInputMode
    {
        protected IInputModeContext Context { get; }

        protected InputModeBase(IInputModeContext context)
        {
            Context = context;
        }

        public void Enter()
        {
            OnEnter();
        }

        protected virtual void OnEnter()
        {
        }

        public void Exit()
        {
            OnExit();
        }

        public void OnGUI()
        {
            Context.SelectionManager.OnGUI();
        }

        protected virtual void OnExit()
        {
        }

        protected abstract InputManager.MouseMode ModeEnum { get; }

        public IInputMode HandleUpdate()
        {
            Context.SelectionManager.UpdateMouseMode(ModeEnum);
            return OnUpdate();
        }

        protected IInputMode ToNormalMode()
        {
            return new NormalMode(Context);
        }

        protected IInputMode ToPurchasingMode()
        {
            return new PurchasingMode(Context);
        }


        protected IInputMode ApplyHotkeys()
        {
            if (Context.MatchSession.isChatFocused)
                return this;

            if (Commands.Unload)
            {
                Context.SelectionManager.DispatchUnloadCommand();
                return this;
            }

            if (Commands.Load)
            {
                Context.SelectionManager.DispatchLoadCommand();
                return this;
            }

            if (Commands.FirePos && !Context.SelectionManager.Empty)
            {
                return ToFirePositionMode();
            }

            if (Commands.ReverseMove && !Context.SelectionManager.Empty)
            {
                return ToReverseMoveMode();
            }

            if (Commands.FastMove && !Context.SelectionManager.Empty)
            {
                return ToFastMoveMode();
            }

            if (Commands.Split && !Context.SelectionManager.Empty)
            {
                return ToSplitMode();
            }

            return this;
        }


        protected IInputMode ShowGhostUnitsAndMaybePurchase(RaycastHit terrainHover)
        {
            // Show ghost units under mouse:
            SpawnPointBehaviour closestSpawn = GetClosestSpawn(terrainHover.point);

            Context.CurrentBuyTransaction?.PreviewPurchase(
                terrainHover.point,
                2 * terrainHover.point - closestSpawn.transform.position);

            return MaybePurchaseGhostUnits(closestSpawn);
        }

        /**
         * Purchase units if there is a buy selection.
         */
        private IInputMode MaybePurchaseGhostUnits(SpawnPointBehaviour closestSpawn)
        {
            if (Input.GetMouseButtonUp(0))
            {
                bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;

                if (!noUIcontrolsInUse)
                    return this;

                if (Context.CurrentBuyTransaction == null)
                    return this;

                closestSpawn.BuyPlatoons(Context.CurrentBuyTransaction.PreviewPlatoons);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // We turned the current ghosts into real units, so:
                    Context.CurrentBuyTransaction = Context.CurrentBuyTransaction.Clone();
                }
                else
                {
                    return ExitPurchasingMode();
                }
            }

            return this;
        }

        protected IInputMode MaybeExitPurchasingModeAndRefund()
        {
            if (Input.GetMouseButton(1))
            {
                foreach (var g in Context.CurrentBuyTransaction.PreviewPlatoons)
                {
                    g.Destroy();
                }

                int unitPrice = Context.CurrentBuyTransaction.Unit.Price;
                Context.MatchSession.LocalPlayer.Refund(unitPrice * Context.CurrentBuyTransaction.UnitCount);

                return ExitPurchasingMode();
            }

            return this;
        }


        /**
         * The ghost units are used to briefly hold the destination
         * for a move order, so they need to be moved to the cursor
         * if a move order click is issued.
         */
        protected void MoveGhostsToMouse()
        {
            if (Util.GetTerrainClickLocation(out var hit))
                Context.SelectionManager.PrepareMoveOrderPreview(hit.point);
        }

        protected void OnOrderHold()
        {
            if (Util.GetTerrainClickLocation(out var hit))
                Context.SelectionManager.RotateMoveOrderPreview(hit.point);
        }

        protected void OnOrderShortClick()
        {
            if (!Context.SelectionManager.Empty)
            {
                DisplayOrderFeedback();
            }

            Context.SelectionManager.DispatchMoveCommand(false, MoveCommandType.NORMAL);
        }

        protected void OnOrderLongClick()
        {
            Context.SelectionManager.DispatchMoveCommand(true, MoveCommandType.NORMAL);
        }

        // Show a Symbol at the position where a move order was issued:
        private void DisplayOrderFeedback()
        {
            RaycastHit hit;
            if (Util.GetTerrainClickLocation(out hit))
                GameObject.Instantiate(
                    Resources.Load(
                        "MoveMarker",
                        typeof(GameObject)),
                    hit.point + new Vector3(0, 0.01f, 0),
                    Quaternion.Euler(new Vector3(90, 0, 0))
                );
        }

        /**
         * Called when a unit card from the buy menu is pressed.
         */
        public IInputMode BuyCallback(Unit unit)
        {
            bool paid = Context.MatchSession.LocalPlayer.TryPay(unit.Price);
            if (!paid)
            {
                return this;
            }

            if (Context.CurrentBuyTransaction == null)
                Context.CurrentBuyTransaction = new BuyTransaction(unit, Context.LocalPlayerData);
            else
                Context.CurrentBuyTransaction.AddUnit();

            //buildUnit(UnitType.Tank);
            return ToPurchasingMode();
        }

        protected IInputMode ExitPurchasingMode()
        {
            Context.CurrentBuyTransaction.PreviewPlatoons.Clear();
            Context.CurrentBuyTransaction = null;

            return ToNormalMode();
        }

        private SpawnPointBehaviour GetClosestSpawn(Vector3 p)
        {
            var pointList = Context.SpawnPoints
                .Where(x => x.Team == Context.LocalPlayerData.Team)
                .ToList();

            SpawnPointBehaviour go = pointList.First();
            float minDistance = float.PositiveInfinity;

            foreach (var s in pointList)
            {
                if (Vector3.Distance(p, s.transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(p, s.transform.position);
                    go = s;
                }
            }

            return go;
        }

        protected IInputMode ToFirePositionMode()
        {
            return new FirePositionMode(Context);
        }

        protected IInputMode ToReverseMoveMode()
        {
            return new ReverseMoveMode(Context);
        }

        protected IInputMode ToFastMoveMode()
        {
            return new FastMoveMode(Context);
        }

        protected IInputMode ToSplitMode()
        {
            return new SplitMode(Context);
        }

        protected abstract IInputMode OnUpdate();
    }
}