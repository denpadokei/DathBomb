﻿using DathBomb.Views;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
