using Unity1Week.rhythm_battle.Presenter.Game;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    [RequireComponent(typeof(PauseView))]
    public sealed class PauseViewPackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PausePresenter>();
            builder.RegisterComponent(GetComponent<PauseView>());
        }
    }
}