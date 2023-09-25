using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

#pragma warning disable 4014
namespace Unity1Week.rhythm_battle.View.Loading
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class LoadText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _loadText;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _scale;
        [SerializeField] private Vector3 _offset;
        private Sequence _loop;

        public void Initialize()
        {
            _loadText.text = "";
        }

        public async UniTask FadeInAsync(CancellationToken token = default)
        {
            _loop?.Kill();

            var animator = new DOTweenTMPAnimator(_loadText);
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);
            _loadText.text = "Now Loading...";

            var sequence = DOTween.Sequence();
            for (var i = 0; i < animator.textInfo.characterCount; i++)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;

                sequence.Join(
                    DOTween.Sequence()
                        .Append(animator.DOScaleChar(i, _scale, 0.1f).SetEase(Ease.OutSine))
                        .Join(animator.DOOffsetChar(i, animator.GetCharOffset(i) + _offset, 0.1f).SetEase(Ease.OutSine)
                            .SetLoops(2, LoopType.Yoyo))
                        .Append(animator.DOScaleChar(i, Vector3.one, 0.1f).SetEase(Ease.OutSine))
                        .SetDelay(i * 0.05f)
                );
            }

            await sequence.ToUniTask(cancellationToken: token);
            sequence.Kill();
            Loop();
        }

        private void Loop()
        {
            var animator = new DOTweenTMPAnimator(_loadText);
            _loop = DOTween.Sequence();
            _loop.SetLoops(-1);
            _loop.SetLink(gameObject);
            for (var i = 0; i < animator.textInfo.characterCount; i++)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;

                _loop.Join(
                    DOTween.Sequence()
                        .Join(animator.DOOffsetChar(i, animator.GetCharOffset(i) + new Vector3(0, 30, 0), 0.2f)
                            .SetEase(Ease.OutSine)
                            .SetLoops(2, LoopType.Yoyo))
                        .AppendInterval(1f)
                        .SetDelay(i * 0.1f)
                );
            }
        }

        public async UniTask FadeOutAsync(CancellationToken token = default)
        {
            _loop?.Kill();
            _loadText.text = "Now Loading...";
            var animator = new DOTweenTMPAnimator(_loadText);
            var sequence = DOTween.Sequence();
            for (var i = 0; i < animator.textInfo.characterCount; i++)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;

                sequence.Join(
                    DOTween.Sequence(animator.DOScaleChar(i, new Vector3(1.2f, 1.2f), 0.05f).SetEase(Ease.OutSine))
                        .Join(animator.DOOffsetChar(i, animator.GetCharOffset(i) + new Vector3(-5, 5), 0.05f)
                            .SetEase(Ease.OutSine))
                        .Append(animator.DOScaleChar(i, Vector3.zero, 0.05f).SetEase(Ease.InSine))
                        .Join(animator.DOOffsetChar(i, animator.GetCharOffset(i) + Vector3.zero, 0.05f)
                            .SetEase(Ease.InSine)).SetDelay(i * 0.05f)
                );
            }

            await sequence.ToUniTask(cancellationToken: token);
            sequence.Kill();
            _loadText.text = "";
        }


        private void Reset()
        {
            _loadText = GetComponent<TextMeshProUGUI>();
        }
    }
}