using System;
using Cysharp.Threading.Tasks;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Applications.Static;
using Unity1Week.rhythm_battle.Extensions;
using Unity1Week.rhythm_battle.Presenter.Pool;
using UnityEngine;

namespace Unity1Week.rhythm_battle.View.Game
{
    public sealed class NoteView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private NoteDto _note;
        private NotePool _pool;
        private Vector3 _position;

        public NoteView Create(Transform content)
        {
            return Instantiate(this, content, false);
        }

        public void Spawn(NoteDto note, NotePool pool)
        {
            _note = note;
            _pool = pool;
            _position.x = StaticData.SpawnPosition;
            transform.localPosition = _position;
            // 120MPB基準で1拍0.5秒のアニメーションを組んでいるので曲に合わせて速度を変える
            _animator.SetTrigger(AnimatorParameter.Move);
            _animator.speed = StaticData.SpeedLate(note.Bpm);
        }

        public async UniTaskVoid ReactionAsync(NoteDto note)
        {
            if (_note.Id != note.Id) return;
            var trigger = note.Judge switch
            {
                Judge.Perfect => AnimatorParameter.Perfect,
                Judge.Good => AnimatorParameter.Good,
                Judge.Normal => AnimatorParameter.Normal,
                Judge.Miss => AnimatorParameter.Miss,
                Judge.Non => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
            await _animator.SetTriggerAsync(trigger, token: this.GetCancellationTokenOnDestroy());
            _pool?.Return(this);
        }

        public void Pause(bool isPause)
        {
            if (_note == null) return;
            _animator.speed = isPause ? 0 : StaticData.SpeedLate(_note.Bpm);
        }

        private static class AnimatorParameter
        {
            public static readonly int Move = Animator.StringToHash("Move");
            public static readonly int Perfect = Animator.StringToHash("Perfect");
            public static readonly int Good = Animator.StringToHash("Good");
            public static readonly int Normal = Animator.StringToHash("Normal");
            public static readonly int Miss = Animator.StringToHash("Miss");
        }
    }
}