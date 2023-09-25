using UnityEngine;

namespace Unity1Week.rhythm_battle.Presenter.Common
{
    public sealed class SetSkyBox : MonoBehaviour
    {
        [SerializeField] private Material _skyBox;
        
        private void Start()
        {
            RenderSettings.skybox = _skyBox;
        }
    }
}