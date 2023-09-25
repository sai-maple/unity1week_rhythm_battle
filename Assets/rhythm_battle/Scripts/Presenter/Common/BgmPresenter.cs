using System;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Common;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Common
{
    public sealed class BgmPresenter : IInitializable, IDisposable
    {
        private readonly VolumeEntity _volumeEntity;
        private readonly AudioSource _audioSource;

        private readonly CompositeDisposable _disposable = new();

        public BgmPresenter(VolumeEntity volumeEntity, AudioSource audioSource)
        {
            _volumeEntity = volumeEntity;
            _audioSource = audioSource;
        }

        public void Initialize()
        {
            _volumeEntity.OnBgmVolumeAsObservable()
                .Subscribe(value => _audioSource.volume = value)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}