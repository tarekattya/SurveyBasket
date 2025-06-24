using SurveyBasket.Contracts;
using SurveyBasket.Contracts.Roles;

namespace SurveyBasket.Services.Abstractions
{
    public interface IRoleServices
    {
        Task<IEnumerable<RoleResponse>> GetAllAsync(bool? IncludeDisable = false, CancellationToken cancellationToken = default);
        Task<Result<RoleDeatilsResponse>> GetAsync(string id);
        Task<Result<RoleDeatilsResponse>> AddAsync(RoleRequest request);

        Task<Result> UpdateAsync(string id, RoleRequest request);

        Task<Result> TooglePublish(string id);

    }
}
