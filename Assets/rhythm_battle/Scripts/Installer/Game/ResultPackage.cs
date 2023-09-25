using Unity1Week.rhythm_battle.Presenter.Game;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    [RequireComponent(typeof(ResultView))]
    public sealed class ResultPackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ResultPresenter>();
            builder.RegisterComponent(GetComponent<ResultView>());
        }
    }
}