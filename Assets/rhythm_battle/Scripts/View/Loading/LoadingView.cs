using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity1Week.rhythm_battle.Extensions;
using UnityEngine;

namespace Unity1Week.rhythm_battle.View.Loading
{
    public sealed class LoadingView : MonoBehaviour
    {
        [SerializeField] private RectTransform _background;
        [SerializeField] private ShapeAnimation[] _shapes;
        [SerializeField] private LoadText _loadText;
        [SerializeField] private Animator _accessoryAnimator;

        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");

        private Vector2 _size;
        private bool _isFading;

        public void Initialize()
        {
            _size = _background.sizeDelta;
            _background.sizeDelta = new Vector2(0, _size.y);
            _loadText.Initialize();
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask FadeInAsync(CancellationToken token = default)
        {
            await UniTask.WaitWhile(() => _isFading, cancellationToken: token);
            _isFading = true;
            var tasks = Enumerable.Select(_shapes, (t, i) => t.DoFadeAsync(i * 0.1f, token)).ToList();

            _background.pivot = new Vector2(0, 0.5f);
            _background.anchorMax = new Vector2(0, 0.5f);
            _background.anchorMin = new Vector2(0, 0.5f);
            _background.anchoredPosition = Vector2.zero;
            tasks.Add(_background.DOSizeDelta(_size, 1f).SetEase(Ease.OutQuint).SetDelay(0.2f)
                .ToUniTask(cancellationToken: token));

            tasks.Add(_loadText.FadeInAsync(token));
            tasks.Add(_accessoryAnimator.SetTriggerAsync(FadeIn, token: token));

            await UniTask.WhenAll(tasks);
            _isFading = false;
        }

        public async UniTask FadeOutAsync(CancellationToken token = default)
        {
            await UniTask.WaitWhile(() => _isFading, cancellationToken: token);
            _isFading = true;
            var tasks = Enumerable.Select(_shapes, (t, i) => t.DoFadeAsync(i * 0.1f, token)).ToList();
            _background.pivot = new Vector2(1, 0.5f);
            _background.anchorMax = new Vector2(1, 0.5f);
            _background.anchorMin = new Vector2(1, 0.5f);
            _background.anchoredPosition = Vector2.zero;

            tasks.Add(_background.DOSizeDelta(new Vector2(0, _size.y), 1f).SetEase(Ease.OutQuint).SetDelay(0.25f)
                .ToUniTask(cancellationToken: token));

            tasks.Add(_loadText.FadeOutAsync(token));
            tasks.Add(_accessoryAnimator.SetTriggerAsync(FadeOut, token: token));

            await UniTask.WhenAll(tasks);
            _isFading = false;
        }
    }
}