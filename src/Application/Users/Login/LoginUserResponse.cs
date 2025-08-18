namespace Application.Users.Login;

public sealed record LoginUserResponse
{
    public string Token { get; set; }
    public UserResponse User { get; set; }
}

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
}
