using System.Runtime.InteropServices;

namespace PHP_SRePS
{
    public class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            return InternetGetConnectedState(out int description, 0);
        }
    }
}
