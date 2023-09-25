using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.UseCase;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class RhythmButtonPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly JudgeUseCase _judgeUseCase;
        private readonly RhythmButton _button;

        private readonly CompositeDisposable _disposable = new();

        public RhythmButtonPresenter(PhaseEntity phaseEntity, JudgeUseCase judgeUseCase, RhythmButton button)
        {
            _phaseEntity = phaseEntity;
            _judgeUseCase = judgeUseCase;
            _button = button;
        }

        public void Initialize()
        {
            _button.OnPointerDownAsObservable()
                .Where(_ => _phaseEntity.Value == Phase.Game)
                .Subscribe(_ => _judgeUseCase.Tap())
                .AddTo(_disposable);
            
            Observable.EveryUpdate()
                .Where(_ => _phaseEntity.Value == Phase.Game)
                .Where(_ => Input.anyKeyDown)
                .Where(_ => !Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => _judgeUseCase.Tap())
                .AddTo(_disposable);

            // _button.OnPointerUpAsObservable()
            //     .Subscribe(_ => _judgeUseCase.Tap())
            //     .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}