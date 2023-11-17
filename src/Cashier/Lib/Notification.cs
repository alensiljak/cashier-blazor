using MudBlazor;
using System.ComponentModel;

namespace Cashier.Lib
{
    public class NotificationService
    {
        private readonly ISnackbar _snackbar;

        public NotificationService(ISnackbar snackbar) { _snackbar = snackbar; }

        public void Error(string message)
        {
            _snackbar.Add(message, Severity.Error);
        }

        public void Info(string message)
        {
            _snackbar.Add(message, Severity.Info);
        }

        public void Show(string message)
        {
            _snackbar.Add(message);
        }

        public void Success(string message)
        {
            _snackbar.Add(message, Severity.Success);
        }

        public void Warning(string message)
        {
            _snackbar.Add(message, Severity.Warning);
        }
    }
}
