using Unity1Week.rhythm_battle.Presenter.Game;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    [RequireComponent(typeof(EnemyLifeView))]
    public sealed class EnemyLifePackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EnemyLifePresenter>();
            builder.RegisterComponent(GetComponent<EnemyLifeView>());
        }
    }
}