using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Register.Handlers;

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, RequestResponse<RegisterResponse>>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterStudentCommandHandler(
        IGenericRepository<ApplicationUser> userRepo,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
    }

    public async Task<RequestResponse<RegisterResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 1. Check if Email already exists
        var existingUser = await _userRepo.Get(u => u.Email.ToLower() == normalizedEmail)
                                          .FirstOrDefaultAsync(cancellationToken);
        if (existingUser != null)
        {
            return RequestResponse<RegisterResponse>.Fail("Email already registered", statusCode: 409);
        }

        // 2. Hash password using BCrypt
        var passwordHash = _passwordHasher.Hash(request.Password);

        // 3. Generate 6-digit numeric OTP & hash it
        var plainOtp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        var hashedOtp = _passwordHasher.Hash(plainOtp);

        // 4. Create ApplicationUser with related Student and EmailVerificationOtp
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            Role = UserRole.Student,
            AccountStatus = AccountStatus.Pending,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow,
            Student = new Student
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        user.EmailVerificationOtps.Add(new EmailVerificationOtp
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Email = normalizedEmail,
            OtpHash = hashedOtp,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            CreatedAt = DateTime.UtcNow,
            IsUsed = false,
            AttemptCount = 0
        });

        await _userRepo.AddAsync(user);

        // 5. Commit to database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Send OTP via Email
        await _emailService.SendEmailAsync(
            user.Email,
            "Verify your email",
            $"<p>Welcome {user.FullName},</p><p>Your verification code is: <strong>{plainOtp}</strong>. It expires in 10 minutes.</p>"
        );

        // 7. Return Response
        var responseData = new RegisterResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Message = "Registration successful. Please verify your email with the OTP sent."
        };

        return RequestResponse<RegisterResponse>.Created(responseData, "User registered successfully");
    }
}
