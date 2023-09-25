using System;
using System.Collections.Generic;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Applications.Static;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Entity.Game
{
    /// <summary>
    /// ノートの判定、消滅指示を行うEntity
    /// </summary>
    public sealed class JudgeEntity : IDisposable
    {
        private List<NoteDto> _note;
        private readonly Subject<NoteDto> _spawnSubject = new();
        private readonly Subject<NoteDto> _judgeSubject = new();

        private int _index;

        public IObservable<NoteDto> OnJudgeAsObservable()
        {
            return _judgeSubject.Share();
        }

        public void Initialize(IEnumerable<NoteDto> notes)
        {
            // 判定時間順にソートされたノート
            _note = new List<NoteDto>(notes);
            _index = 0;
        }

        public bool TryTap(float time, out NoteDto note)
        {
            note = null;
            if (_index >= _note.Count) return false;
            note = _note[_index];

            // 早すぎたら無視
            if (time < note.Time - StaticData.FrameTime * 8) return false;
            _index++;

            var diff = time - note.Time;
            var abs = Mathf.Abs(diff);
            var judge = abs switch
            {
                < StaticData.FrameTime * 4 => Judge.Perfect,
                < StaticData.FrameTime * 6 => Judge.Good,
                < StaticData.FrameTime * 8 => Judge.Normal,
                _ => Judge.Miss
            };

            note.Judge = judge;

            _judgeSubject.OnNext(note);
            return true;
        }

        /// <summary>
        /// 判定されず通り過ぎたノートの処理
        /// </summary>
        /// <param name="time"></param>
        /// <param name="note"></param>
        public bool TryDespawn(float time, out NoteDto note)
        {
            note = null;
            if (_index >= _note.Count) return false;
            note = _note[_index];
            if (time < note.Time + StaticData.FrameTime * 6) return false;
            _index++;

            note.Judge = Judge.Miss;
            _judgeSubject.OnNext(note);
            return true;
        }

        public void Dispose()
        {
            _spawnSubject?.OnCompleted();
            _spawnSubject?.Dispose();
            _judgeSubject?.OnCompleted();
            _judgeSubject?.Dispose();
        }
    }
}