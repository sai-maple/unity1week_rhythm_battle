using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

#pragma warning disable 4014
namespace Unity1Week.rhythm_battle.View.Loading
{
    // シェイプを左に右から流すアニメーション
    public sealed class ShapeAnimation : MonoBehaviour
    {
        [SerializeField] private Transform[] _shapes;
        [SerializeField] private float _from;
        [SerializeField] private float _to;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;

        public async UniTask DoFadeAsync(float delay, CancellationToken token = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            var tasks = new List<UniTask>();

            for (var i = 0; i < _shapes.Length; i++)
            {
                var d = Mathf.Floor((i + 1) / 2f) * _delay;
                tasks.Add(_shapes[i].DOLocalMoveX(_to, _duration + d).SetDelay(d).SetEase(Ease.OutQuint)
                    .ToUniTask(cancellationToken: token));
            }

            await UniTask.WhenAll(tasks);

            foreach (var shape in _shapes)
            {
                shape.DOLocalMoveX(_from, 0);
            }
        }
    }
}