using Unity1Week.rhythm_battle.Presenter.Common;
using Unity1Week.rhythm_battle.View.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Common
{
    [RequireComponent(typeof(VolumeSliderView))]
    public sealed class VolumePackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<VolumePresenter>();
            builder.RegisterComponent(GetComponent<VolumeSliderView>());
        }
    }
}