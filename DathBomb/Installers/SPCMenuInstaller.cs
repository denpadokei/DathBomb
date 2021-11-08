using SiraUtil;
using DathBomb.Views;
using Zenject;

namespace DathBomb.Installers
{
    public class SPCMenuInstaller : MonoInstaller
    {
        public override void InstallBindings() => this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
    }
}
