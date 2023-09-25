using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS4014
namespace Unity1Week.rhythm_battle.View.Game
{
    public sealed class EnemyLifeView : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Image _fillRed;

        public void OnChange(float life, float total)
        {
            _fill.fillAmount = life / total;
            _fillRed.DOFillAmount(life / total, 0.5f).SetDelay(0.2f);
        }
    }
}