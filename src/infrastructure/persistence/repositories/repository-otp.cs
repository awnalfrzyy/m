using Microsoft.EntityFrameworkCore;

namespace diggie_server.src.infrastructure.persistence.repositories
{
    public interface IRepositoryOtp
    {
        Task<EntityOtp?> GetAsync(string email);
        Task<EntityOtp?> GetByCodeAsync(string code);
        Task CreateAsync(EntityOtp otp);
        Task UpdateAsync(EntityOtp otp);
    }

    public class RepositoryOtp : IRepositoryOtp
    {
        private readonly AppDatabaseContext _context;

        public RepositoryOtp(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<EntityOtp?> GetAsync(string email)
        {
            return await _context.Otps
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<EntityOtp?> GetActiveOtpAsync(string email, string code)
        {
            return await _context.Otps
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.Code == code &&
                    x.Status == OtpStatus.Pending &&
                    x.ExpiredAt > DateTime.UtcNow);
        }

        public async Task<EntityOtp?> GetByCodeAsync(string code)
        {
            return await _context.Otps.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task CreateAsync(EntityOtp otp)
        {
            var existingOtp = await _context.Otps.FirstOrDefaultAsync(x => x.Email == otp.Email);
            if (existingOtp != null)
            {

                existingOtp.Code = otp.Code;
                existingOtp.CreatedAt = otp.CreatedAt;
                existingOtp.ExpiredAt = otp.ExpiredAt;
                existingOtp.Status = otp.Status;
                _context.Otps.Update(existingOtp);
            }
            else
            {
                await _context.Otps.AddAsync(otp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EntityOtp otp)
        {
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}