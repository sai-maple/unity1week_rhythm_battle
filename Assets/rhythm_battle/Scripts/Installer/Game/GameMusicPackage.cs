using Unity1Week.rhythm_battle.Presenter.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class GameMusicPackage : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameMusicPresenter>();
            builder.RegisterComponent(GetComponent<AudioSource>());
        }
    }
}