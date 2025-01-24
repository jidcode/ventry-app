namespace ventry_api.Shared.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message, 404, "NOT_FOUND") { }
    }

    public class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message, 400, "BAD_REQUEST") { }
    }

    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message)
            : base(message, 401, "UNAUTHORIZED") { }
    }

    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message)
            : base(message, 403, "FORBIDDEN") { }
    }
}
