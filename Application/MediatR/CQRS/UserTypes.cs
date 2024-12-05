using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Models.Users;


namespace Application.MediatR.CQRS;

public abstract record UserCommand<TResponse> : ICommand<TResponse>
{
    [JsonIgnore] public User? CurrentUser { get; set; }
}

public abstract record UserQuery<TResponse> : IQuery<TResponse>
{
    [JsonIgnore] public User? CurrentUser { get; set; }
}