using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Runtime.Helpers
{
    public sealed class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> Queue = new();
        private static int _mainThreadId;

        public static bool isOnMainThread => Environment.CurrentManagedThreadId == _mainThreadId;

        private void Awake()
        {
            _mainThreadId = Environment.CurrentManagedThreadId;
        }

        public static void Enqueue(Action action)
        {
            Queue.Enqueue(action);
        }

        private void Update()
        {
            while (Queue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
}