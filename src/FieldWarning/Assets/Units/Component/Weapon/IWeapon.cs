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

using UnityEngine;

namespace PFW.Units.Component.Weapon
{
    /// <summary>
    ///     Classes implementing this interface implement firing logic,
    ///     including audio/visual effects and calling damage callbacks.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        ///     Fire on the provided target if the weapon is not reloading etc.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displacement">Vector from the firing unit to the target unit</param>
        /// <param name="distance">Distance from the firing unit to the target unit</param>
        /// <param name="isServer">Non-server code should only affect art.</param>
        /// <returns>True if a shot was fired, false otherwise.</returns>
        bool TryShoot(
                TargetTuple target,
                Vector3 displacement,
                float distance,
                bool isServer);

        /// <summary>
        ///     Same as the update method on a MonoBehavior.
        ///     Used, for example, to update reload timers.
        /// </summary>
        void HandleUpdate();

        /// <summary>
        ///     For every target type, find the max range
        ///     that this weapon can shoot it at (0 if it can't).
        /// </summary>
        float[] CalculateMaxRanges();
    }
}
