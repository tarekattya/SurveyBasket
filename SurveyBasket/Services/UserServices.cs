using Microsoft.VisualBasic;
using SurveyBasket.Contracts.Common;
using SurveyBasket.Contracts.User;
using SurveyBasket.Presistence.DbContextt;

namespace SurveyBasket.Services
{
    public class UserServices(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IRoleServices roleServices
        ) : IUserServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRoleServices _roleServices = roleServices;

        public async Task<PageinatedList<UserResponse>> Getallasync(FilterRequest filter, CancellationToken cancellationToken = default)
        {
          var users =  (from u in _context.Users
             join ur in _context.UserRoles
             on u.Id equals ur.UserId
             join r in _context.Roles
             on ur.RoleId equals r.Id into roles
             where !roles.Any(x => x.Name == RoleDefault.Member)
             select new
             {

                 u.Id,
                 u.FirstName,
                 u.LastName,
                 u.Email,
                 u.IsDisable,
                 roles = roles.Select(x => x.Name!).ToList()
             }
                   ).GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisable })
                    .Select(u => new UserResponse(

                       u.Key.Id,
                       u.Key.FirstName,
                       u.Key.LastName,
                       u.Key.Email,
                       u.Key.IsDisable,
                       u.SelectMany(u => u.roles)

                       )
                   );

            var response = await PageinatedList<UserResponse>.CreateAsync( users, filter.pageSize , filter.pageNumber, cancellationToken );

            return response;

        }
        public async Task<Result<UserResponse>> getAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure<UserResponse>(UserError.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var resonse = (user, roles).Adapt<UserResponse>();

            return Result.Success(resonse);
        }


        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var emailExists = await _context.Users.AnyAsync(x => x.Email == request.Email , cancellationToken);
            if (emailExists)
                return Result.Failure<UserResponse>(UserError.EmailAlreadyExists);

            var AllowedRoles = await _roleServices.GetAllAsync(cancellationToken: cancellationToken);

            if (request.Roles.Except(AllowedRoles.Select(x => x.name)).Any())
                return Result.Failure<UserResponse>(UserError.InvalidRole);

            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRolesAsync(user, request.Roles);
                var response = (user, request.Roles).Adapt<UserResponse>();

                return Result.Success(response);

            }

            var error = result.Errors.First();

            return Result.Failure<UserResponse>(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var emailExists = await _context.Users.AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken);
            if (emailExists)
                return Result.Failure(UserError.EmailAlreadyExists);

            var AllowedRoles = await _roleServices.GetAllAsync(cancellationToken: cancellationToken);

            if (request.Roles.Except(AllowedRoles.Select(x => x.name)).Any())
                return Result.Failure(UserError.InvalidRole);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure(UserError.UserNotFound);


            user = request.Adapt(user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _context.UserRoles
                    .Where(x => x.UserId == user.Id)
                    .ExecuteDeleteAsync(cancellationToken);



                await _userManager.AddToRolesAsync(user, request.Roles);
                var response = (user, request.Roles).Adapt<UserResponse>();

                return Result.Success();

            }

            var error = result.Errors.First();

            return Result.Failure<UserResponse>(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }


        public async Task<Result> TooglePublish(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure(UserError.UserNotFound);

            user.IsDisable = !user.IsDisable;

            var result = await _userManager.UpdateAsync(user);

            return Result.Success();




        }

        public async Task<Result> UnLocked(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure(UserError.UserNotFound);

            var result = await _userManager.SetLockoutEndDateAsync(user , null);

            return Result.Success();
        }

    }
}