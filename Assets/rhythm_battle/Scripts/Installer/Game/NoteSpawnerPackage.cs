using Unity1Week.rhythm_battle.Presenter.Game;
using Unity1Week.rhythm_battle.Presenter.Pool;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    public sealed class NoteSpawnerPackage : LifetimeScope
    {
        [SerializeField] private Transform _lane;
        [SerializeField] private NoteAssets _noteAssets;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<NoteSpawnPresenter>();
            builder.RegisterComponent(_lane);
            builder.RegisterComponent(_noteAssets);
        }
    }
}