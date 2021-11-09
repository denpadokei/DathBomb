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
            if (BombNoteControllerPatch.BombMesh != null) {
                Destroy(BombNoteControllerPatch.BombMesh);
                BombNoteControllerPatch.BombMesh = null;
            }
            this.Container.BindInterfacesAndSelfTo<DathBombController>().FromNewComponentOnNewGameObject(nameof(DathBombController)).AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<BombEffectSpowner>().FromNewComponentOnNewGameObject(nameof(BombEffectSpowner)).AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<BeatmapUtil>().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<CustomNoteUtil>().AsSingle().NonLazy();
        }
    }
}
