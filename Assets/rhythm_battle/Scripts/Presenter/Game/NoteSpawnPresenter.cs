using System;
using System.Collections.Generic;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.Presenter.Pool;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    /// <summary>
    /// ノートの生成破棄を行うPresenter
    /// </summary>
    public sealed class NoteSpawnPresenter : IInitializable, IDisposable
    {
        private readonly MusicalScoreEntity _musicalScoreEntity;

        private readonly NotePools _pools;
        private readonly CompositeDisposable _disposable = new();

        public NoteSpawnPresenter(MusicalScoreEntity musicalScoreEntity, Transform lane,
            NoteAssets noteAssets)
        {
            _musicalScoreEntity = musicalScoreEntity;
            _pools = new NotePools(lane, noteAssets);
        }

        public void Initialize()
        {
            _musicalScoreEntity.OnSpawnAsObservable()
                .Subscribe(note =>
                {
                    _pools.Spawn(note);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}