namespace SharedKernel.Enums;

public enum ResultCode
{
    Default,
    Ok=200,
    Created=201,
    Updated,
    Deleted,
    Patched,
    BadRequest=400,
    Unauthorized=401,
    Forbidden=403,
    NotFound=404,
}