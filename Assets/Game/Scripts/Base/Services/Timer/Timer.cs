using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Base.Services.Timer
{
    public sealed class Timer
    {
        public event Action OnTimerEnd;
        public event Action<float> OnTimerChange;
        private readonly ICoroutineRunner _coroutineRunner;
        private float _time;
        private readonly bool _repeat;

        private Coroutine _cor;

        public float TimeLeft { get; private set; }

        public Timer(ICoroutineRunner coroutineRunner, float time, Action onTimerEnd,
            Action<float> onTimerChange = null,
            bool repeat = false)
        {
            _coroutineRunner = coroutineRunner;
            _time = time;
            OnTimerEnd = onTimerEnd;
            OnTimerChange = onTimerChange;
            _repeat = repeat;
        }

        public void StartTimer()
        {
            _cor = _coroutineRunner.StartCoroutine(Start());
        }

        public void StartTimer(float time)
        {
            _time = time;
            StartTimer();
        }

        public void StopTimer()
        {
            if ( _cor == null )
            {
                return;
            }

            _coroutineRunner.StopCoroutine(_cor);
        }

        private IEnumerator Start()
        {
            TimeLeft = _time;
        
            while ( true )
            {
                TimeLeft -= Time.deltaTime;
                yield return null;

                if ( TimeLeft > 0f )
                {
                    OnTimerChange?.Invoke(TimeLeft);
                    continue;
                }

                OnTimerEnd?.Invoke();
                if ( !_repeat )
                {
                    break;
                }

                TimeLeft = _time;
            }
        }
    }
}