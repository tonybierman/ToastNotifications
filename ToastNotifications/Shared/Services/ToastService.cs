using System;
using System.Timers;

namespace ToastNotifications.Shared.Services
{
    public class ToastService : IDisposable
    {
        // Events
        public event Action<string, ToastLevel> OnShow;
        public event Action OnHide;

        // Fields
        private System.Timers.Timer _countdown;
        private double _intervalMs = 10000;
        Queue<Action> _que = new Queue<Action>();
        private bool _isShowing;

        public void ShowToast(string message, ToastLevel level)
        {
            _que.Enqueue(() =>
            {
                _isShowing = true;
                OnShow?.Invoke(message, level);
                StartCountdown();
            });

            if (!_isShowing)
                _que.Dequeue().Invoke();
        }

        private void StartCountdown()
        {
            SetCountdown();

            if (_countdown.Enabled)
            {
                _countdown.Stop();
                _countdown.Start();
            }
            else
            {
                _countdown.Start();
            }
        }

        private void SetCountdown()
        {
            if (_countdown == null)
            {
                _countdown = new System.Timers.Timer(_intervalMs);
                _countdown.Elapsed += HideToast;
                _countdown.AutoReset = false;
            }
        }

        private void HideToast(object source, ElapsedEventArgs args)
        {
            OnHide?.Invoke();
            _isShowing = false;
            if(_que.Count > 0)
                _que.Dequeue().Invoke();
        }

        public void Dispose()
        {
            _countdown?.Dispose();
        }
    }
}
