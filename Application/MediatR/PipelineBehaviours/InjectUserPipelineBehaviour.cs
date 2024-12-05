using Application.Abstractions;
using Application.Abstractions.Repositories;
using Application.MediatR.CQRS;
using Domain.Abstractions;
using Domain.Models;
using Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.MediatR.PipelineBehaviours;

public class InjectUserPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public InjectUserPipelineBehaviour(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is UserCommand<TResponse> userCommand)
        {
            var user = await GetUserFromHttpContextAsync();
            userCommand.CurrentUser = user;
        }
        else if (request is UserQuery<TResponse> userQuery)
        {
            var user = await GetUserFromHttpContextAsync();
            userQuery.CurrentUser = user;
        }
        
        return await next();
    }
    
    private async Task<User> GetUserFromHttpContextAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context), "Http context is null, so no user could be found.");
        }
    
        var claims = context.User.Claims.ToList();
        var supabaseId = claims.Find(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
    
        if (supabaseId is null)
        {
            throw new ArgumentNullException(nameof(supabaseId),
                "No supabade Id found in the http headers.");
        }

        var user = _userRepository.GetOrCreate(new UserId(Guid.Parse(supabaseId)));
        await _unitOfWork.SaveChangesAsync();
        
        return user;
    }
}

