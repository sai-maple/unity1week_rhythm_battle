using Unity1Week.rhythm_battle.Entity.Menu;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Menu
{
    public sealed class MenuInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MenuEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}