using Unity1Week.rhythm_battle.Presenter.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class GameSePackage : LifetimeScope
    {
        [SerializeField] private AudioClip[] _audioClips;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameSePresenter>();
            builder.RegisterComponent(GetComponent<AudioSource>());
            builder.RegisterInstance(_audioClips);
        }
    }
}