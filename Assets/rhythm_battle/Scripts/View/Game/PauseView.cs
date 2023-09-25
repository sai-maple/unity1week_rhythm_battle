using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS4014
namespace Unity1Week.rhythm_battle.View.Game
{
    public sealed class PauseView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private CanvasGroup _fills;
        [SerializeField] private TextMeshProUGUI _count;
        [SerializeField] private Image _fill;

        public void Initialize(Action retry, Action resume, Action back)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            transform.localScale = new Vector3(1.2f, 1.2f, 1);
            _fill.fillAmount = 0;
            _count.text = "";
            _retryButton.onClick.AddListener(retry.Invoke);
            _resumeButton.onClick.AddListener(resume.Invoke);
            _backButton.onClick.AddListener(back.Invoke);
        }

        public async UniTask Present()
        {
            _fills.alpha = 0;
            transform.DOScale(1, 0.5f);
            await _canvasGroup.DOFade(1, 0.5f);
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        public async UniTask DismissAsync()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _fills.DOFade(1, 0.2f);
            for (var i = 3; i > 1; i--)
            {
                _fill.fillAmount = 1;
                _count.text = $"{i}";
                await _fill.DOFillAmount(0, 1);
            }

            _fill.fillAmount = 1;
            _count.text = $"1";
            _fill.DOFillAmount(0, 1);

            transform.DOScale(1.2f, 0.5f);
            await _canvasGroup.DOFade(0, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }
}