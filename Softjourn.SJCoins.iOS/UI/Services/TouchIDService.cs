using System;
using Foundation;
using LocalAuthentication;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS.UI.Services
{
    public class TouchIDService
    {
        public event EventHandler AccessGranted;
        public event EventHandler<NSError> AccessDenied;

        private readonly LAContext context = new LAContext();
        private const LAPolicy BiometricPolicy = LAPolicy.DeviceOwnerAuthenticationWithBiometrics;
        private NSError AuthError;

        public bool CanEvaluatePolicy() => context.CanEvaluatePolicy(BiometricPolicy, out AuthError);

        public void AuthenticateUser() =>
            context.EvaluatePolicy(BiometricPolicy, Const.Unlock_Access, HandleLaContextReply);

        #region Private methods

        private void HandleLaContextReply(bool success, NSError error)
        {
            if (success)
                AccessGranted?.Invoke(this, null);
            else
                AccessDenied?.Invoke(this, error);
        }

        #endregion
    }
}
