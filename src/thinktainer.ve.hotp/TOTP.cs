namespace Thinktainer.Ve.HOTP
{
    using System;

    internal static class TOTP
    {
        private static readonly DateTime StartOfTime =
            new DateTime(2016, 9, 25, 0, 0 ,0 ,0, DateTimeKind.Utc).ToUniversalTime();

        private const long Interval = 30000;

        internal static string Generate(string userId)
        {
            var counter = GetCounter();
            return HOTP.Generate(userId, counter);
        }

        internal static long GetCounter(Func<DateTime> systemTime = null)
        {
            var nowTime = systemTime ?? (() => DateTime.UtcNow);
            return (long)Math.Floor((nowTime() - StartOfTime).TotalMilliseconds) / Interval;
        }
    }
}