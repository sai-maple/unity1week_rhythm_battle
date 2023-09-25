using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Game;
using Unity1Week.rhythm_battle.View.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class PausePresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly PauseView _pauseView;
        private readonly LoadingView _loadingView;

        private readonly CompositeDisposable _disposable = new();

        public PausePresenter(PhaseEntity phaseEntity, PauseView pauseView, LoadingView loadingView)
        {
            _phaseEntity = phaseEntity;
            _pauseView = pauseView;
            _loadingView = loadingView;
        }

        public void Initialize()
        {
            _pauseView.Initialize(
                () => RetryAsync().Forget(),
                () => ResumeAsync().Forget(),
                () => BackAsync().Forget()
            );

            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Pause)
                .Subscribe(_ => { PresentAsync().Forget(); })
                .AddTo(_disposable);
        }

        private async UniTaskVoid PresentAsync()
        {
            await _pauseView.Present();
            Observable.EveryUpdate()
                .TakeUntil(_phaseEntity.OnPhaseChangedAsObservable())
                .Where(_ => Input.anyKeyDown)
                .Subscribe(_ =>
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        ResumeAsync().Forget();
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        RetryAsync().Forget();
                    }
                    else if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        BackAsync().Forget();
                    }
                }).AddTo(_disposable);
        }

        private async UniTaskVoid RetryAsync()
        {
            await _loadingView.FadeInAsync();
            var current = SceneEntity.SceneName;
            await SceneManager.UnloadSceneAsync(current);
            SceneManager.LoadScene(current, LoadSceneMode.Additive);
        }

        private async UniTaskVoid ResumeAsync()
        {
            await _pauseView.DismissAsync();
            _phaseEntity.OnNext(Phase.Game);
        }

        private async UniTaskVoid BackAsync()
        {
            await _loadingView.FadeInAsync();
            await SceneManager.UnloadSceneAsync(SceneEntity.SceneName);
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}