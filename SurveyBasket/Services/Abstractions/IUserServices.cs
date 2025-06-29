﻿using SurveyBasket.Contracts.Common;
using SurveyBasket.Contracts.User;
using System.Threading.Tasks;

namespace SurveyBasket.Services.Abstractions
{
    public interface IUserServices 
    {
        Task<PageinatedList<UserResponse>> Getallasync(FilterRequest filter ,CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> getAsync(string id);

        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);

        Task<Result> TooglePublish(string id);
        Task<Result> UnLocked(string id);

    }
}
