using SurveyBasket.Contracts.Roles;
using SurveyBasket.Presistence.DbContextt;

namespace SurveyBasket.Services
{
    public class RoleServices(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleServices
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? IncludeDisable = false, CancellationToken cancellationToken = default) =>

            await _roleManager.Roles
                        .Where(r => !r.IsDefault && (!r.IsDeleted || (IncludeDisable.HasValue && IncludeDisable.Value)))
                        .ProjectToType<RoleResponse>()
                        .ToListAsync(cancellationToken);

        public async Task<Result<RoleDeatilsResponse>> GetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return Result.Failure<RoleDeatilsResponse>(RoleError.RoleNotFound);

            var permission = await _roleManager.GetClaimsAsync(role);

            var response = new RoleDeatilsResponse(role.Id, role.Name!, role.IsDeleted, permission.Select(x => x.Value));

            return Result.Success(response);

        }
        public async Task<Result<RoleDeatilsResponse>> AddAsync(RoleRequest request)
        {
            var roleexists = await _roleManager.RoleExistsAsync(request.name);

            if (roleexists)
            {
                return Result.Failure<RoleDeatilsResponse>(RoleError.RoleAlreadyExists);
            }

            var role = new ApplicationRole
            {
                Name = request.name,
                ConcurrencyStamp = Guid.NewGuid().ToString(),

            };

            var allowedPermission = Permissions.GetAllPermission();

            if (request.permission.Except(allowedPermission).Any())
                return Result.Failure<RoleDeatilsResponse>(RoleError.InvalidPermission);




            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                var permission = request.permission
                     .Select(x => new IdentityRoleClaim<string>
                     {
                         ClaimType = Permissions.Type,
                         ClaimValue = x,
                         RoleId = role.Id
                     });
                await _context.RoleClaims.AddRangeAsync(permission);
                await _context.SaveChangesAsync();
                var response = new RoleDeatilsResponse(role.Id, role.Name!, role.IsDeleted, request.permission);
                return Result.Success(response);



            }

            var error = result.Errors.First();

            return Result.Failure<RoleDeatilsResponse>(new(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        public async Task<Result> UpdateAsync(string id, RoleRequest request)
        {

            var roleexists = await _context.Roles.AnyAsync(x => x.Name == request.name && x.Id != id);

            if (roleexists)
            {
                return Result.Failure<RoleDeatilsResponse>(RoleError.RoleAlreadyExists);
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return Result.Failure<RoleDeatilsResponse>(RoleError.RoleNotFound);


            role.Name = request.name;

            var allowedPermission = Permissions.GetAllPermission();

            if (request.permission.Except(allowedPermission).Any())
                return Result.Failure<RoleDeatilsResponse>(RoleError.InvalidPermission);




            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {

                var currentPermission = await _context.RoleClaims
                    .Where(x => x.RoleId == id && x.ClaimType == Permissions.Type)
                    .Select(x => x.ClaimValue)
                    .ToListAsync();


                var NewPermission = request.permission.Except(currentPermission)
                      .Select(x => new IdentityRoleClaim<string>
                      {
                          ClaimType = Permissions.Type,
                          ClaimValue = x,
                          RoleId = role.Id
                      });

                var removedpermission = currentPermission.Except(request.permission);


                await _context.RoleClaims
               .Where(x => x.RoleId == id && removedpermission.Contains(x.ClaimValue))
               .ExecuteDeleteAsync();

                await _context.RoleClaims.AddRangeAsync(NewPermission);
                await _context.SaveChangesAsync();
                return Result.Success();



            }

            var error = result.Errors.First();

            return Result.Failure<RoleDeatilsResponse>(new(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        public async Task<Result> TooglePublish(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return Result.Failure(RoleError.RoleNotFound);

            role.IsDeleted = !role.IsDeleted;

            await _roleManager.UpdateAsync(role);

            return Result.Success();





        }
    }
}
