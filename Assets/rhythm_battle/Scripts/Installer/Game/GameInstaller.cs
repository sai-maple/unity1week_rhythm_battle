using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.Presenter.Game;
using Unity1Week.rhythm_battle.UseCase;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    public sealed class GameInstaller : LifetimeScope
    {
        [SerializeField] private Material _skyBox;
        [SerializeField] private TextAsset _score;

        protected override void Configure(IContainerBuilder builder)
        {
            RenderSettings.skybox = _skyBox;
            builder.Register<MusicalScoreEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<JudgeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<PhaseEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LifeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<TimeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<GameLoopUseCase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<JudgeUseCase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterEntryPoint<GameInitializer>();
            builder.RegisterEntryPoint<GamLoopPresenter>();

            builder.RegisterInstance(_score);
        }
    }
}