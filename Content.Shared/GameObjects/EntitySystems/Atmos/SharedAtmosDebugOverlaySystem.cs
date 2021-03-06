﻿using System;
using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.Map;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;
using Content.Shared.Atmos;

namespace Content.Shared.GameObjects.EntitySystems.Atmos
{
    public abstract class SharedAtmosDebugOverlaySystem : EntitySystem
    {
        // Keep in mind, this system is hilariously unoptimized. The goal here is to provide accurate debug data.
        public const int LocalViewRange = 16;
        protected float AccumulatedFrameTime;

        [Serializable, NetSerializable]
        public readonly struct AtmosDebugOverlayData
        {
            public readonly float Temperature;
            public readonly float[] Moles;
            public readonly AtmosDirection PressureDirection;
            public readonly bool InExcitedGroup;

            public AtmosDebugOverlayData(float temperature, float[] moles, AtmosDirection pressureDirection, bool inExcited)
            {
                Temperature = temperature;
                Moles = moles;
                PressureDirection = pressureDirection;
                InExcitedGroup = inExcited;
            }
        }

        /// <summary>
        ///     Invalid tiles for the gas overlay.
        ///     No point re-sending every tile if only a subset might have been updated.
        /// </summary>
        [Serializable, NetSerializable]
        public sealed class AtmosDebugOverlayMessage : EntitySystemMessage
        {
            public GridId GridId { get; }

            public MapIndices BaseIdx { get; }
            // LocalViewRange*LocalViewRange
            public AtmosDebugOverlayData[] OverlayData { get; }

            public AtmosDebugOverlayMessage(GridId gridIndices, MapIndices baseIdx, AtmosDebugOverlayData[] overlayData)
            {
                GridId = gridIndices;
                BaseIdx = baseIdx;
                OverlayData = overlayData;
            }
        }
    }
}
