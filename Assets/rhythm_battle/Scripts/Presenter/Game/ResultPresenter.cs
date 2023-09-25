using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Game;
using Unity1Week.rhythm_battle.View.Loading;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class ResultPresenter : IInitializable, IDisposable
    {
        private readonly PlayRecodeEntity _playRecodeEntity;
        private readonly PhaseEntity _phaseEntity;
        private readonly LifeEntity _lifeEntity;
        private readonly ResultView _resultView;
        private readonly LoadingView _loadingView;

        private readonly CompositeDisposable _disposable = new();

        public ResultPresenter(PlayRecodeEntity playRecodeEntity, PhaseEntity phaseEntity, LifeEntity lifeEntity,
            ResultView resultView, LoadingView loadingView)
        {
            _playRecodeEntity = playRecodeEntity;
            _phaseEntity = phaseEntity;
            _lifeEntity = lifeEntity;
            _resultView = resultView;
            _loadingView = loadingView;
        }

        public void Initialize()
        {
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Finish)
                .Subscribe(_ => _resultView.ResultAsync(_lifeEntity.MusicBonus, _lifeEntity.BattleBonus))
                .AddTo(_disposable);

            _resultView.OnRetryAsObservable()
                .Subscribe(_ =>
                {
                    _resultView.Disable();
                    RetryAsync().Forget();
                })
                .AddTo(_disposable);

            _resultView.OnReturnAsObservable()
                .Subscribe(_ =>
                {
                    _resultView.Disable();
                    BackAsync().Forget();
                })
                .AddTo(_disposable);
        }

        private async UniTaskVoid RetryAsync()
        {
            var grade = (_lifeEntity.MusicBonus + _lifeEntity.BattleBonus) switch
            {
                < 40f => 0,
                < 60f => 1,
                < 80f => 2,
                < 100f => 3,
                _ => 4,
            };
            _playRecodeEntity.Add(SceneEntity.SceneName, grade);
            await _loadingView.FadeInAsync();
            var current = SceneEntity.SceneName;
            await SceneManager.UnloadSceneAsync(current);
            SceneManager.LoadScene(current, LoadSceneMode.Additive);
        }

        private async UniTaskVoid BackAsync()
        {
            await _loadingView.FadeInAsync();
            await SceneManager.UnloadSceneAsync(SceneEntity.SceneName);
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}