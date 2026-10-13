public static class StatusCodes
{
     public static ApiResponse Ok(object data) => new ApiResponse 
    { 
        StatusCode = 200, 
        Body = JsonHelper.Serialize(data) 
    };
    
    public static ApiResponse Created(object data) => new ApiResponse 
    { 
        StatusCode = 201, 
        Body = JsonHelper.Serialize(data) 
    };
    
    public static ApiResponse NoContent() => new ApiResponse { StatusCode = 204 };
    
    public static ApiResponse BadRequest(string message) => new ApiResponse 
    { 
        StatusCode = 400, 
        Body = JsonHelper.Serialize(new { error = message }) 
    };
    
    public static ApiResponse NotFound(string message = "Resource not found") => new ApiResponse 
    { 
        StatusCode = 404, 
        Body = JsonHelper.Serialize(new { error = message }) 
    };
    
    public static ApiResponse InternalServerError(string message = "Internal server error") => new ApiResponse 
    { 
        StatusCode = 500, 
        Body = JsonHelper.Serialize(new { error = message }) 
    };
}