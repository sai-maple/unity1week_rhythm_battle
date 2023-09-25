using System;
using DG.Tweening;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class GameMusicPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly TimeEntity _timeEntity;
        private readonly AudioSource _audioSource;

        public GameMusicPresenter(PhaseEntity phaseEntity, TimeEntity timeEntity, AudioSource audioSource)
        {
            _phaseEntity = phaseEntity;
            _timeEntity = timeEntity;
            _audioSource = audioSource;
        }

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _timeEntity.Initialize(_audioSource.clip.length);

            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Game)
                .Subscribe(_ => _audioSource.Play())
                .AddTo(_disposable);
            
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Pause)
                .Subscribe(_ => _audioSource.Pause())
                .AddTo(_disposable);

            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase is Phase.Finish)
                .Subscribe(_ => _audioSource.DOFade(0, 1f))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}