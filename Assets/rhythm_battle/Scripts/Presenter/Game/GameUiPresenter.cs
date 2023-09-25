using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class GameUiPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly CanvasGroup _canvasGroup;
        private readonly Button _pauseButton;

        private readonly CompositeDisposable _disposable = new();

        public GameUiPresenter(PhaseEntity phaseEntity, CanvasGroup canvasGroup, Button pauseButton)
        {
            _phaseEntity = phaseEntity;
            _canvasGroup = canvasGroup;
            _pauseButton = pauseButton;
        }

        public void Initialize()
        {
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Game)
                .Subscribe(_ =>
                {
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.interactable = true;
                }).AddTo(_disposable);

            _pauseButton.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Where(_ => _phaseEntity.Value == Phase.Game)
                .Subscribe(_ =>
                {
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.interactable = true;

                    _phaseEntity.OnNext(Phase.Pause);
                })
                .AddTo(_disposable);

            Observable.EveryUpdate()
                .Where(_ => _phaseEntity.Value == Phase.Game)
                .Where(_ => Input.anyKeyDown)
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.interactable = true;

                    _phaseEntity.OnNext(Phase.Pause);
                }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}