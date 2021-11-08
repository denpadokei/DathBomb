using System;
using UnityEngine;

namespace DathBomb.Models
{
    public class DummyBombExprosionEffect : MonoBehaviour
    {
        public virtual void Awake()
        {
            this._emitParams = default;
            this._emitParams.startColor = Color.white;
            this._emitParams.applyShapeToPosition = true;
        }
        public virtual void SpawnExplosion(Vector3 pos)
        {
            this._emitParams.position = pos;
            try {
                ParticleAssetLoader.instance.Particle.Emit(this._emitParams, 250);
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }
        protected ParticleSystem.EmitParams _emitParams;
    }
}
