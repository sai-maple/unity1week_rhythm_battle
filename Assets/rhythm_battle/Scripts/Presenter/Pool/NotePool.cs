using System;
using System.Collections.Generic;
using UniRx.Toolkit;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Presenter.Pool
{
    public sealed class NotePools : IDisposable
    {
        private readonly List<NotePool> _notePools = new();

        public NotePools(Transform content, NoteAssets noteAssets)
        {
            foreach (var note in noteAssets.NoteViews)
            {
                _notePools.Add(new NotePool(note, content));
            }
        }

        public NoteView Spawn(NoteDto dto)
        {
            var pool = _notePools[dto.Type];
            var note = pool.Rent();
            note.Spawn(dto, pool);
            return note;
        }

        public void Dispose()
        {
            foreach (var pool in _notePools)
            {
                pool?.Dispose();
            }
        }
    }

    public sealed class NotePool : ObjectPool<NoteView>
    {
        private readonly NoteView _origin;
        private readonly Transform _content;

        public NotePool(NoteView origin, Transform content)
        {
            _origin = origin;
            _content = content;
        }

        protected override NoteView CreateInstance()
        {
            return _origin.Create(_content);
        }
    }
}