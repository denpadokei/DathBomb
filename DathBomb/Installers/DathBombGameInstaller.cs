using DathBomb.Models;
using HMUI;
using UnityEngine;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombGameInstaller : MonoInstaller
    {
        private readonly FlyingBombNameEffect _flyingBombNameEffect;

        public DathBombGameInstaller()
        {
            this._flyingBombNameEffect = new GameObject(
                "FlyingBombNameEffect",
                //typeof(TextMeshPro),
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasRenderer),
                typeof(CurvedCanvasSettings),
                typeof(FlyingBombNameEffect)
                ).GetComponent<FlyingBombNameEffect>();
        }
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<DathBombController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BombEffectSpowner>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BombMeshGetter>().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BeatmapUtil>().AsSingle().NonLazy();
            _ = this.Container.BindMemoryPool<FlyingBombNameEffect, FlyingBombNameEffect.Pool>().WithInitialSize(10).FromNewComponentOnNewPrefab(this._flyingBombNameEffect).AsCached().NonLazy();
        }
    }
}
