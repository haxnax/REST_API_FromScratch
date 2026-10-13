using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

public class UserController
{
    private static readonly List<User> _users = new List<User>
    {
        new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
        new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
    };

    private static int _nextId = 3;

    // GET /api/users
    public static async Task<ApiResponse> GetAllUsers(ApiRequest request)
    {
        var response = new ApiResponse
        {
            StatusCode = 200,
            ContentType = "application/jsoon",
            Body = JsonSerializer.Serialize(_users, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        };
        return await Task.FromResult(response);
    }

    public static async Task<ApiResponse> GetUserById(ApiRequest request)
    {
        if (!int.TryParse(request.QueryParameters["id"], out int userId))
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "Invalid User Id" }) };
        }

        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "User not found" }) };
        }

        return new ApiResponse
        {
            StatusCode = 200,
            ContentType = "application/json",
            Body = JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        };

    }


    // POST /api/users
    public static async Task<ApiResponse> CreateUser(ApiRequest request)
    {
        try
        {
            var CreateUserRequest = JsonSerializer.Deserialize<CreateUserRequest>(request.Body,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (string.IsNullOrEmpty(CreateUserRequest.Name) || string.IsNullOrEmpty(CreateUserRequest.Email))
            {
                return new ApiResponse { StatusCode = 400, Body = JsonSerializer.Serialize(new { error = "Name and Email are required" }) };
            }

            var newUser = new User
            {
                Id = _nextId++,
                Name = CreateUserRequest.Name,
                Email = CreateUserRequest.Email,
            };
            _users.Add(newUser);

            return new ApiResponse
            {
                StatusCode = 201,
                ContentType = "application/json",
                Body = JsonSerializer.Serialize(newUser, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };
        }
        catch (JsonException)
        {
            return new ApiResponse { StatusCode = 400, Body = JsonSerializer.Serialize(new { error = "Invalid JSON format" }) };
        }

    }


    // PUT /api/users/{id}
    public static async Task<ApiResponse> UpdateUser(ApiRequest request)
    {
        if (!int.TryParse(request.QueryParameters["id"], out int userId))
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "Invalid User Id" }) };
        }

        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "User not found" }) };
        }

        try
        {
            var updateUserRequest = JsonSerializer.Deserialize<UpdateUserRequest>(request.Body,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (!string.IsNullOrEmpty(updateUserRequest.Name))
            {
                user.Name = updateUserRequest.Name;
            }

            if (!string.IsNullOrEmpty(updateUserRequest.Email))
            {
                user.Email = updateUserRequest.Email;
            }



            return new ApiResponse
            {
                StatusCode = 200,
                ContentType = "application/json",
                Body = JsonSerializer.Serialize(newUser, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };
        }
        catch (JsonException)
        {
            return new ApiResponse { StatusCode = 400, Body = JsonSerializer.Serialize(new { error = "Invalid JSON format" }) };
        }
    }

    public static async Task<ApiResponse> DeleteUser(ApiRequest request)
    {
        if (!int.TryParse(request.QueryParameters["id"], out int userId))
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "Invalid User Id" }) };
        }

        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new ApiResponse { StatusCode = 404, Body = JsonSerializer.Serialize(new { error = "User not found" }) };
        }

        _users.Remove(user);
        
        return new ApiResponse { StatusCode = 204 }; // No Content



    }

















}
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

}
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UpdateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}