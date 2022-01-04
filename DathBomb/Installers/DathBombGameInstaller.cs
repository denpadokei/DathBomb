using SiraUtil;
using DathBomb.HarmonyPathches;
using DathBomb.Models;
using DathBomb.Utilities;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<DathBombController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<BombEffectSpowner>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<BombMeshGetter>().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<BeatmapUtil>().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<CustomNoteUtil>().AsSingle().NonLazy();
        }
    }
}
