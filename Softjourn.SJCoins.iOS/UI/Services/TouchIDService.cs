using System;
using Foundation;
using LocalAuthentication;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public class TouchIDService
    {
        public event EventHandler AccessGranted;
        public event EventHandler<NSError> AccessDenied;

        #region Properties
        LAContext context = new LAContext();
        LAPolicy biometricPolicy = LAPolicy.DeviceOwnerAuthenticationWithBiometrics;
        NSError AuthError;
        #endregion

        #region Public methods
        public bool CanEvaluatePolicy()
        {
            return context.CanEvaluatePolicy(biometricPolicy, out AuthError);
        }

        public void AuthenticateUser()
        {
            context.EvaluatePolicy(biometricPolicy, Const.Unlock_Access, HandleLAContextReply);
        }
        #endregion

        #region Private methods
        private void HandleLAContextReply(bool success, NSError error)
        {
            if (success)
                AccessGranted?.Invoke(this, null);
            else 
                AccessDenied?.Invoke(this, error);
        }
        #endregion
    }
}
