namespace Thinktainer.Ve.HOTP.Test
{
    using System;

    using Xunit;

    public class HOTPTests
    {
        private const string UserId = "TestUser";
        private const int Counter = 1;

        [Fact]
        public void LengthOfGeneratedPasswordIs6()
        {
            var result = HOTP.Generate(UserId, Counter);
            Assert.Equal(6, result.Length);
        }

        [Fact]
        public void PasswordWithSameCounterAndSameUserIdIsSame()
        {
            var result1 = HOTP.Generate(UserId, Counter);
            var result2 = HOTP.Generate(UserId, Counter);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void PasswordWithSameCounterAndDifferentUserIdIsDifferent()
        {
            var result1 = HOTP.Generate(UserId, Counter);
            var result2 = HOTP.Generate(UserId + "diff", Counter);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void PasswordWithDifferentCounterAndSameUserIdIsDifferent()
        {
            var result1 = HOTP.Generate(UserId, Counter);
            var result2 = HOTP.Generate(UserId, Counter + 1);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void CounterShouldNotIncrementAfter29Seconds()
        {
            var now = new DateTime(2017, 9, 25, 23, 50, 0, DateTimeKind.Utc).ToUniversalTime();
            var later = now.AddMilliseconds(2999);
            var c1 = TOTP.GetCounter(() => now);
            var c2 = TOTP.GetCounter(() => later);
            Assert.Equal(c1, c2);
        }

        [Fact]
        public void CounterShouldIncrementAfter30Seconds()
        {
            var now = new DateTime(2017, 9, 25, 23, 50, 0, DateTimeKind.Utc).ToUniversalTime();
            var later = now.AddSeconds(30);
            var c1 = TOTP.GetCounter(() => now);
            var c2 = TOTP.GetCounter(() => later);
            Assert.NotEqual(c1, c2);
        }

        [Fact]
        public void InterfaceGeneratesPassword()
        {
            IPasswordGenerator sut = new PasswordGenerator();
            var result = sut.GeneratePassword(UserId);
            Assert.Equal(6, result.Length);
        }

        [Fact]
        public void InterfaceValidatesPassword()
        {
            IPasswordGenerator sut = new PasswordGenerator();
            var password = sut.GeneratePassword(UserId);
            var result = sut.IsValidPassword(UserId, password);
            Assert.True(result);
        }
    }
}
