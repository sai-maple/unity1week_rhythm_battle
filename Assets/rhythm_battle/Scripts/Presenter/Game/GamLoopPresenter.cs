using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.UseCase;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class GamLoopPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly GameLoopUseCase _useCase;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public GamLoopPresenter(PhaseEntity phaseEntity, GameLoopUseCase useCase)
        {
            _phaseEntity = phaseEntity;
            _useCase = useCase;
        }

        public void Initialize()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => _phaseEntity.Value == Phase.Game)
                .Subscribe(_ => { _useCase.TryUpdate(Time.fixedDeltaTime); }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}