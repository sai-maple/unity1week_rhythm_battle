using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Static;

namespace Unity1Week.rhythm_battle.Entity.Common
{
    public sealed class VolumeEntity : IDisposable
    {
        private readonly ReactiveProperty<float> _bgmVolume = new(StaticData.BGM);
        private readonly ReactiveProperty<float> _volume = new(StaticData.SE);

        public IObservable<float> OnBgmVolumeAsObservable()
        {
            return _bgmVolume;
        }
        
        public IObservable<float> OnSeVolumeAsObservable()
        {
            return _volume;
        }

        public void OnNextBgm(float value)
        {
            _bgmVolume.Value = value;
        }
        
        public void OnNextSe(float value)
        {
            _volume.Value = value;
        }

        public void Dispose()
        {
            _bgmVolume.Dispose();
            _volume.Dispose();
        }
    }
}