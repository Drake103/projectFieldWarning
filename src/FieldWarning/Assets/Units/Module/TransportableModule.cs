﻿/**
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

using PFW.Units;

public class TransportableModule : PlatoonModule
{
    public TransportableWaypoint Waypoint
    {
        get
        {
            return base.NewWaypoint as TransportableWaypoint;
        }
    }

    public TransportableModule(PlatoonBehaviour p) : base(p)
    {
    }

    protected override Waypoint GetModuleWaypoint()
    {
        return new TransportableWaypoint(Platoon);
    }

    public void SetTransport(TransporterModule transport)
    {
        Waypoint.transporterWaypoint = transport.Waypoint;
    }
}


