using bnbClone_API.DTOs.ProfileDTOs;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Impelementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public ProfileService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<ProfileUserDto> GetUserProfileAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            return new ProfileUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfilePictureUrl = user.ProfilePictureUrl,
                AccountStatus = user.AccountStatus,
                EmailVerified = user.EmailVerified,
                PhoneVerified = user.PhoneVerified,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                Role = user.Role
            };
        }

        public async Task<ProfileHostDto> GetHostProfileAsync(int userId)
        {
            var host = await _unitOfWork.Hosts.GetHostByUserIdAsync(userId);
            if (host == null)
                throw new ArgumentException("Host profile not found");

            return new ProfileHostDto
            {
                Id = host.Id,
                UserId = host.UserId,
                FirstName = host.User.FirstName,
                LastName = host.User.LastName,
                Email = host.User.Email,
                ProfilePictureUrl = host.User.ProfilePictureUrl,
                StartDate = host.StartDate,
                AboutMe = host.AboutMe,
                Work = host.Work,
                Rating = host.Rating,
                TotalReviews = host.TotalReviews,
                Education = host.Education,
                Languages = host.Languages,
                IsVerified = host.IsVerified,
                TotalEarnings = host.TotalEarnings,
                AvailableBalance = host.AvailableBalance,
                LivesIn = host.LivesIn,
                DreamDestination = host.DreamDestination,
                FunFact = host.FunFact,
                Pets = host.Pets,
                ObsessedWith = host.ObsessedWith,
                SpecialAbout = host.SpecialAbout
            };
        }

        public async Task<ProfileUserDto> UpdateUserProfileAsync(int userId, ProfileUpdateUserDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return await GetUserProfileAsync(userId);
        }

        public async Task<ProfileHostDto> UpdateHostProfileAsync(int userId, ProfileUpdateHostDto dto)
        {
            var host = await _unitOfWork.Hosts.GetHostByUserIdAsync(userId);
            if (host == null)
            {
                // Create host profile if doesn't exist
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    throw new ArgumentException("User not found");

                host = new Models.Host
                {
                    UserId = userId,
                    User = user,
                    StartDate = DateTime.UtcNow
                };

                await _unitOfWork.Hosts.AddAsync(host);
            }

            host.AboutMe = dto.AboutMe;
            host.Work = dto.Work;
            host.Education = dto.Education;
            host.Languages = dto.Languages;
            host.LivesIn = dto.LivesIn;
            host.DreamDestination = dto.DreamDestination;
            host.FunFact = dto.FunFact;
            host.Pets = dto.Pets;
            host.ObsessedWith = dto.ObsessedWith;
            host.SpecialAbout = dto.SpecialAbout;

            if (host.Id == 0)
            {
                await _unitOfWork.Hosts.AddAsync(host);
            }
            else
            {
                _unitOfWork.Hosts.Update(host);
            }

            await _unitOfWork.SaveAsync();

            return await GetHostProfileAsync(userId);
        }

        public async Task<string> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Invalid file type. Only JPEG and PNG are allowed.");

            // Validate file size (5MB max)
            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException("File size must be less than 5MB");

            // Generate unique filename
            var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Delete old profile picture if exists
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                var oldFileName = Path.GetFileName(user.ProfilePictureUrl);
                var oldFilePath = Path.Combine(uploadsFolder, oldFileName);
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }

            // Save new file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Update user profile picture URL
            user.ProfilePictureUrl = $"/uploads/profiles/{fileName}";
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return user.ProfilePictureUrl;
        }
    }
}
