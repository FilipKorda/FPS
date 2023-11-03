using System;
using System.Collections.Generic;
using UnityEngine;
using FPS.ImpactSystem.Effects;

namespace FPS.ImpactSystem
{
    [CreateAssetMenu(menuName = "Impact System/Surface", fileName = "Surface")]
    public class Surface : ScriptableObject
    {
        [Serializable]
        public class SurfaceImpactTypeEffect
        {
            public ImpactType ImpactType;
            public SurfaceEffect SurfaceEffect;
        }
        public List<SurfaceImpactTypeEffect> ImpactTypeEffects = new();
    }
}