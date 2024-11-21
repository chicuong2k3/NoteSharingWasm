using SharingNote.Api.Application.Services;
using System.Security.Cryptography;

namespace SharingNote.Api.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly ICacheService _cacheService;

        public OtpService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task<string> GenerateOtpAsync(string email)
        {
            const string chars = "0123456789";
            var otp = new char[6];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[1];
                for (int i = 0; i < 6; i++)
                {
                    rng.GetBytes(randomNumber);
                    otp[i] = chars[randomNumber[0] % chars.Length];
                }
            }

            var otpString = new string(otp);

            await _cacheService.SetAsync($"otp:{email}", otpString, TimeSpan.FromMinutes(5));

            return otpString;
        }

        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            var storedOtp = await _cacheService.GetAsync<string>($"otp:{email}");
            if (string.IsNullOrEmpty(storedOtp)) return false;


            return storedOtp == otp;
        }
    }
}
