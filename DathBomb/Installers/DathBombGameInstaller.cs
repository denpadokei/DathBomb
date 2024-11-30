using DathBomb.Models;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<DathBombController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BombEffectSpowner>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BombMeshGetter>().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<BeatmapUtil>().AsSingle().NonLazy();
        }
    }
}
