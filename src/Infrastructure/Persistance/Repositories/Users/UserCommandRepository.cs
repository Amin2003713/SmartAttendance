using DNTPersianUtils.Core;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Common.General.Enums.FileType;

// Added for logging

namespace SmartAttendance.Persistence.Repositories.Users;

public class UserCommandRepository(
    WriteOnlyDbContext                      dbContext,
    ILogger<CommandRepository<User>>        logger,
    IStringLocalizer<UserCommandRepository> localizer,
    IdentityService                         service,
    IUniversityRepository                      UniversityRepository,
    UserManager<User>                       userService,
    SmartAttendanceTenantDbContext          db    ,
    IMediator                      mediator
)
    : CommandRepository<User>(dbContext, logger),
        IUserCommandRepository
{
    // Constructor with dependency injection for DbContext and Loggers

    /// <summary>
    ///     Updates the last login date of the specified user.
    /// </summary>
    /// <param name="user">The user whose last login date is to be updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            user.LastActionOnServer = DateTime.UtcNow;
            await UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating last login date for user: {UserId}", user.Id);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task<User> RegisterByOwnerAsync(RegisterByOwnerRequest request, CancellationToken cancellationToken)
    {
        if (!request.PhoneNumber.IsValidIranianMobileNumber())
            SmartAttendanceException.BadRequest();


        // Check if the user already exists
        if (await TableNoTracking
                .AnyAsync(u => u.UserName == request.PhoneNumber ||
                               u.PersonalNumber == request.PersonalNumber,
                    cancellationToken))
            SmartAttendanceException.Conflict();

        // Retrieve the University by ID
        var University = await UniversityRepository.GetByIdAsync(service.TenantInfo!.Id, cancellationToken);

        if (University == null)
            SmartAttendanceException.NotFound();

        // Get the operator (current user) information
        var operatorUser = await Entities.FindAsync(service.GetUserId());

        if (operatorUser == null)
            SmartAttendanceException.BadRequest();


        // Create a new user entity
        var newUser = new User
        {
            UserName     = request.PhoneNumber,
            FirstName    = request.FirstName,
            LastName     = request.LastName,
            Email        = $"{request.PhoneNumber}{ApplicationConstant.Const.EmailSuffix}",
            PhoneNumber  = request.PhoneNumber,
            NationalCode = request.NationalCode,
            FatherName      = request.FatherName,
            PersonalNumber = request.PersonalNumber,
            Gender          = request.Gender,
            CreatedBy = operatorUser!.Id
        };

        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var creationResult = await userService.CreateAsync(newUser, request.NationalCode);

            if (!creationResult.Succeeded)
                SmartAttendanceException.BadRequest(creationResult.Errors.Select(a => a.Description).ToString()!);


            await transaction.CommitAsync(cancellationToken);


            var UniversityUser = newUser.Adapt<UniversityUser>();
            UniversityUser.UniversityTenantInfoId = service.TenantInfo.Id;
            await UniversityRepository.CreateAsync(UniversityUser, cancellationToken);


            if (request.ProfilePicture?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(newUser.ProfilePicture))
                {
                    var path = newUser.ProfilePicture!.Replace("https://", "").Replace("http://", "");

                    var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                        cancellationToken);

                    if (!deleteResponse)
                    {
                        logger.LogError("Failed to delete old image for newUser {Id}.", newUser.Id);
                        throw SmartAttendanceException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommand
                {
                    File       = request.ProfilePicture.MediaFile,
                    ReportDate = DateTime.UtcNow,
                    RowType    = FileStorageType.ProfilePicture,
                    RowId      = (newUser.Id)
                };

                var uploadImageResponse = await mediator.Send(uploadCommand, cancellationToken);

                newUser.ProfilePicture = uploadImageResponse.Url;

                logger.LogInformation("Uploaded image for newUser{newUserId}.", newUser.Id);
            }


            // Send SMS notification to the user
            // var owner = await _userService.FindByIdAsync(University.User.Id.ToString());
            // await _smsService.SendUserPassForNewUser(new SendAddToUniversityOwnerRegisteredUserSmsDto()
            // {
            //     ReceiverName = newUser.LastName,
            //     ReceiverPhoneNumber = newUser.PhoneNumber,
            //     UniversityName = University.Name,
            //     OwnerName = owner!.LastName,
            //     UserPass = newUser.UserName
            // }, cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            SmartAttendanceException.BadRequest("مشکلی هنگام ثبت اطلاعات شما رخ داده است.");
        }

        return newUser;
    }


    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        await userService.UpdateAsync(user);

        var phoneNumber = user.PhoneNumber;

        var UniversityUser = await db.UniversityUsers.FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber, cancellationToken);

        if (UniversityUser != null)
        {
            UniversityUser.FirstName = user.FirstName;
            UniversityUser.LastName  = user.LastName;
            db.Update(UniversityUser);
        }

        var owner = await db.UniversityAdmins.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

        if (owner != null)
        {
            owner.FirstName = user.FirstName;
            owner.LastName  = user.LastName;
            db.Update(owner);
        }

        await db.SaveChangesAsync(cancellationToken);
    }


    /// <summary>
    ///     Retrieves a user by username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The plaintext password of the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching user if found; otherwise, null.</returns>
    public async Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken)
    {
        try
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            var user         = await Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);

            if (user != null)
                return user!;

            logger.LogInformation("User found with username: {Username}", username);
            throw SmartAttendanceException.NotFound(additionalData: localizer["User was not found."]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving user with username: {Username}", username);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    /// <summary>
    ///     Updates the security stamp of the specified user.
    /// </summary>
    /// <param name="user">The user whose security stamp is to be updated.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating security stamp for user: {UserId}", user.Id);

        try
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            await UpdateAsync(user, cancellationToken);
            logger.LogInformation("Successfully updated security stamp for user: {UserId}", user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating security stamp for user: {UserId}", user.Id);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    /// <summary>
    ///     Adds a new user with the specified password.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <param name="password">The plaintext password for the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to add new user with username: {Username}", user.UserName);

        try
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName, cancellationToken);

            if (exists)
            {
                logger.LogWarning("Cannot add user. Username already exists: {Username}", user.UserName);
                throw SmartAttendanceException.Conflict(additionalData: localizer["Username already exists."]);
            }

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;

            await base.AddAsync(user, cancellationToken);
            logger.LogInformation("Successfully added new user with username: {Username}", user.UserName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding new user with username: {Username}", user.UserName);
            throw SmartAttendanceException.InternalServerError();
        }
    }
}