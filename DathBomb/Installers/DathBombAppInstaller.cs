using DathBomb.Models;
using DathBomb.Utilities;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<CustomNoteUtil>().AsCached().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<FontAssetReader>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<ImageLoader>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            _ = this.Container.BindInterfacesAndSelfTo<ParticleAssetLoader>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}
