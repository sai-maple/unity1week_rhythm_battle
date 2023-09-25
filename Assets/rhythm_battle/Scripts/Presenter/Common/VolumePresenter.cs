using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Static;
using Unity1Week.rhythm_battle.Entity.Common;
using Unity1Week.rhythm_battle.View.Common;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Common
{
    public sealed class VolumePresenter : IInitializable, IDisposable
    {
        private readonly VolumeEntity _volumeEntity;
        private readonly VolumeSliderView _volumeSliderView = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public VolumePresenter(VolumeEntity volumeEntity, VolumeSliderView volumeSliderView)
        {
            _volumeEntity = volumeEntity;
            _volumeSliderView = volumeSliderView;
        }

        public void Initialize()
        {
            _volumeSliderView.Initialize(StaticData.SE, StaticData.BGM);
            _volumeSliderView.OnBgmEditAsObservable()
                .Subscribe(SetAudioMixerBGM)
                .AddTo(_disposable);

            _volumeSliderView.OnSeEditEndAsObservable()
                .Subscribe(SetAudioMixerSe)
                .AddTo(_disposable);
            SetAudioMixerBGM(StaticData.BGM);
            SetAudioMixerSe(StaticData.SE);
        }

        private void SetAudioMixerBGM(float value)
        {

            StaticData.BGM = value;
            _volumeEntity.OnNextBgm(value);
        }

        private void SetAudioMixerSe(float value)
        {

            StaticData.SE = value;
            _volumeEntity.OnNextSe(value);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}