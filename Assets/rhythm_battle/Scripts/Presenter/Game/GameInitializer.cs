using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Loading;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class GameInitializer : IInitializable, IDisposable
    {
        private readonly MusicalScoreEntity _musicalScoreEntity;
        private readonly JudgeEntity _judgeEntity;
        private readonly LifeEntity _lifeEntity;
        private readonly PhaseEntity _phaseEntity;
        private readonly TextAsset _score;
        private readonly LoadingView _loadingView;

        private CancellationTokenSource _cancellation;

        public GameInitializer(MusicalScoreEntity musicalScoreEntity, JudgeEntity judgeEntity, LifeEntity lifeEntity,
            PhaseEntity phaseEntity, TextAsset score, LoadingView loadingView)
        {
            _musicalScoreEntity = musicalScoreEntity;
            _judgeEntity = judgeEntity;
            _lifeEntity = lifeEntity;
            _phaseEntity = phaseEntity;
            _score = score;
            _loadingView = loadingView;
        }

        public async void Initialize()
        {
            _cancellation = new CancellationTokenSource();
            _musicalScoreEntity.Initialize(_score);
            _judgeEntity.Initialize(_musicalScoreEntity.Score.Scores);
            _lifeEntity.Initialize(_musicalScoreEntity.Score.Scores);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await _loadingView.FadeOutAsync();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _phaseEntity.OnNext(Phase.Game);
        }

        public void Dispose()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }
    }
}