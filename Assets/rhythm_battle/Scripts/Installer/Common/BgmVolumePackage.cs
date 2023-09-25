using Unity1Week.rhythm_battle.Presenter.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Common
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class BgmVolumePackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BgmPresenter>();
            builder.RegisterComponent(GetComponent<AudioSource>());
        }
    }
}