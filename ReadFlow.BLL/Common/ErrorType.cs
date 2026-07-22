namespace ReadFlow.BLL.Common;

public enum ErrorType
{
    NotFound,        // 404
    Validation,      // 400
    Conflict,        // 409
    Unauthorized,    // 401
    Forbidden,       // 403
    Internal,        // 500
    BadRequest       // 400
}
