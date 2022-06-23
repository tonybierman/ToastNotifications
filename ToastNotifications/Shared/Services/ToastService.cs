using System;
using System.Timers;

namespace ToastNotifications.Shared.Services
{
    public class ToastService : IDisposable
    {
        public event Action<string, ToastLevel> OnShow;
        public event Action OnHide;
        private System.Timers.Timer Countdown;
        private double _intervalMs = 10000;
        Queue<Action> m_que = new Queue<Action>();
        private bool isShowing;

        public void ShowToast(string message, ToastLevel level)
        {
            m_que.Enqueue(() =>
            {
                isShowing = true;
                OnShow?.Invoke(message, level);
                StartCountdown();
            });

            if (!isShowing)
                m_que.Dequeue().Invoke();
        }

        private void StartCountdown()
        {
            SetCountdown();

            if (Countdown.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
            {
                Countdown.Start();
            }
        }

        private void SetCountdown()
        {
            if (Countdown == null)
            {
                Countdown = new System.Timers.Timer(_intervalMs);
                Countdown.Elapsed += HideToast;
                Countdown.AutoReset = false;
            }
        }

        private void HideToast(object source, ElapsedEventArgs args)
        {
            OnHide?.Invoke();
            isShowing = false;
            if(m_que.Count > 0)
                m_que.Dequeue().Invoke();
        }

        public void Dispose()
        {
            Countdown?.Dispose();
        }
    }
}
