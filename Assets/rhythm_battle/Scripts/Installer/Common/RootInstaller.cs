using Unity1Week.rhythm_battle.Entity.Common;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Loading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Common
{
    public sealed class RootInstaller : LifetimeScope
    {
        [SerializeField] private LoadingView _loadingViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayRecodeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<VolumeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            var loadingView = Instantiate(_loadingViewPrefab);
            loadingView.Initialize();
            builder.RegisterInstance(loadingView);
        }
    }
}