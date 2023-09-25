using System;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Game;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class GameSePresenter : IInitializable, IDisposable
    {
        private readonly MusicalScoreEntity _musicalScoreEntity;
        private readonly AudioSource _audioSource;
        private readonly AudioClip[] _clips;

        private readonly CompositeDisposable _disposable = new();

        public GameSePresenter(MusicalScoreEntity musicalScoreEntity, AudioSource audioSource, AudioClip[] clips)
        {
            _musicalScoreEntity = musicalScoreEntity;
            _audioSource = audioSource;
            _clips = clips;
        }

        public void Initialize()
        {
            _musicalScoreEntity.OnSpawnSoundAsObservable()
                .Subscribe(sound =>
                {
                    _audioSource.PlayOneShot(_clips[sound.Type % 4]);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}