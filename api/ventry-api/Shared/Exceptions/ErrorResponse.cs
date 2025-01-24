namespace ventry_api.Shared.Exceptions
{
    public record ErrorResponse(
      string Type,
      int StatusCode,
      string Message
  );
}
