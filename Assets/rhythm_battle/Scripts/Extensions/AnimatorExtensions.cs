using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Extensions
{
    public static class AnimatorExtensions
    {
        public static async UniTask SetTriggerAsync(this Animator self, int hash, int layerIndex = 0,
            CancellationToken token = default)
        {
            self.SetTrigger(hash);
            await ChangeStateAsync(self, hash, layerIndex, token);
            if (token.IsCancellationRequested) return;
            await CompleteAsync(self, hash, layerIndex, token);
        }

        private static async UniTask ChangeStateAsync(Animator self, int hash, int layerIndex = 0,
            CancellationToken token = default)
        {
            while (true)
            {
                await UniTask.Yield(token);
                if (token.IsCancellationRequested) return;
                var currentState = self.GetCurrentAnimatorStateInfo(layerIndex);
                if (currentState.shortNameHash == hash) break;
            }
        }

        private static async UniTask CompleteAsync(Animator self, int hash, int layerIndex = 0,
            CancellationToken token = default)
        {
            while (true)
            {
                await UniTask.Yield(token);
                if (token.IsCancellationRequested) return;
                var currentState = self.GetCurrentAnimatorStateInfo(layerIndex);
                if (currentState.shortNameHash != hash || currentState.loop || currentState.normalizedTime >= 1f) break;
            }
        }
    }
}