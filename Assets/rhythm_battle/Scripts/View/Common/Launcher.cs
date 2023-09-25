using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity1Week.rhythm_battle.View.Common
{
    public sealed class Launcher : MonoBehaviour
    {
        private async void Awake()
        {
            await SceneManager.LoadSceneAsync("RootScene", LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
            await SceneManager.UnloadSceneAsync("Launcher");
        }
    }
}