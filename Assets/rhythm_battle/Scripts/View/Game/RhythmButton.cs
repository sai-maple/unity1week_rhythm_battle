using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1Week.rhythm_battle.View.Game
{
    public sealed class RhythmButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pressColor;

        private readonly Subject<Unit> _onPointerUp = new();
        private readonly Subject<Unit> _onPointerDown = new();

        public IObservable<Unit> OnPointerUpAsObservable()
        {
            return _onPointerUp.Share();
        }

        public IObservable<Unit> OnPointerDownAsObservable()
        {
            return _onPointerDown.Share();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp.OnNext(Unit.Default);
            _buttonImage.transform.localPosition = Vector3.zero;
            _buttonImage.color = _defaultColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown.OnNext(Unit.Default);
            _buttonImage.transform.localPosition = new Vector3(0, -10, 0);
            _buttonImage.color = _pressColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonImage.transform.localPosition = Vector3.zero;
            _buttonImage.color = _defaultColor;
        }

        private void OnDestroy()
        {
            _onPointerUp?.OnCompleted();
            _onPointerUp?.Dispose();
            _onPointerDown?.OnCompleted();
            _onPointerDown?.Dispose();
        }
    }
}