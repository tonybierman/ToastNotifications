using ToastNotifications.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;

namespace ToastNotifications.Client.Shared
{
    public class ToastBase : ComponentBase, IDisposable
    {
        // Properties
        [Parameter] public string AnimationTime { get; set; }
        [Parameter] public bool IsShown { get; set; }
        [Inject] ToastService ToastService { get; set; }
        protected string Heading { get; set; }
        protected string Message { get; set; }
        protected string BackgroundCssClass { get; set; }

        // Fields
        protected string IconCssClass { get; set; }
        protected string width = "210px";
        protected string height = "90px";

        protected override Task OnInitializedAsync()
        {
            ToastService.OnShow += ShowToast;
            ToastService.OnHide += HideToast;

            return base.OnInitializedAsync();
        }

        private void ShowToast(string message, ToastLevel level)
        {
            BuildToastSettings(level, message);
            IsShown = true;
            StateHasChanged();
        }

        private void HideToast()
        {
            IsShown = false;
            StateHasChanged();
        }

        private void BuildToastSettings(ToastLevel level, string message)
        {
            switch (level)
            {
                case ToastLevel.Info:
                    BackgroundCssClass = $"bg-info";
                    IconCssClass = "info";
                    Heading = "Info";
                    break;
                case ToastLevel.Success:
                    BackgroundCssClass = $"bg-success";
                    IconCssClass = "check";
                    Heading = "Success";
                    break;
                case ToastLevel.Warning:
                    BackgroundCssClass = $"bg-warning";
                    IconCssClass = "exclamation";
                    Heading = "Warning";
                    break;
                case ToastLevel.Error:
                    BackgroundCssClass = "bg-danger";
                    IconCssClass = "times";
                    Heading = "Error";
                    break;
            }

            Message = message;
        }

        public void Dispose()
        {
            ToastService.OnShow -= ShowToast;
        }
    }
}
