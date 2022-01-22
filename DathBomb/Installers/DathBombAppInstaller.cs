using DathBomb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DathBomb.Installers
{
    public class DathBombAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<CustomNoteUtil>().AsCached().NonLazy();
        }
    }
}
