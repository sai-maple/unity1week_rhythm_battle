using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Data;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Entity.Game
{
    public sealed class MusicalScoreEntity : IDisposable
    {
        public int Bpm { get; private set; }
        public ScoreDto Score => _score;

        private readonly Subject<NoteDto> _spawnSubject = new();
        private readonly Subject<SoundDto> _soundSubject = new();

        private ScoreDto _score;

        private List<NoteDto> _spawnList;
        private int _index;
        private int _soundIndex;
        private float _beetTime;

        public IObservable<NoteDto> OnSpawnAsObservable()
        {
            return _spawnSubject.Share();
        }

        public IObservable<SoundDto> OnSpawnSoundAsObservable()
        {
            return _soundSubject.Share();
        }

        public void Initialize(TextAsset score)
        {
            _score = JsonUtility.FromJson<ScoreDto>(score.text);
            _index = 0;
            _soundIndex = 0;
            _beetTime = 60f / _score.Bpm;
            Bpm = _score.Bpm;
            // 生成順にソート(追い越しなどで生成と判定の順番が異なる場合がある)
            _spawnList = _score.Scores.OrderBy(note => note.Time - (note.Type % 4 + 1) * _beetTime).ToList();
            // オフセットの計算(3拍前)
        }

        /// <summary>
        /// 現在時刻
        /// </summary>
        /// <param name="time"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public void TrySpawn(float time)
        {
            if (_index >= _spawnList.Count) return;
            // 判定の数秒前(_offset)に生成 ノートによってオフセットが異なる
            var note = _spawnList[_index];
            var offset = (note.Type % 4 + 1) * _beetTime;
            if (time + offset < note.Time) return;
            _index++;
            _spawnSubject.OnNext(note);
        }

        public void CheckSound(float time)
        {
            if (_soundIndex >= _score.Sounds.Count) return;
            // ノートの生成タイミングに効果音を鳴らす 判定タイミングと被ったりするので譜面で設定
            var sound = _score.Sounds[_soundIndex];
            if (time < sound.Time) return;
            _soundSubject.OnNext(sound);
            _soundIndex++;
        }

        public void Dispose()
        {
            _spawnSubject?.OnCompleted();
            _spawnSubject?.Dispose();
            _soundSubject?.OnCompleted();
            _soundSubject?.Dispose();
        }
    }
}