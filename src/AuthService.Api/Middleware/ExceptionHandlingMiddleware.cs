using System.Net;
using System.Text.Json;
using Amazon.CognitoIdentityProvider.Model;
using AuthService.Api.DTOs.Responses;

namespace AuthService.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error, message) = exception switch
        {
            // ── Sign Up ────────────────────────────────────────────────────────
            UsernameExistsException =>
                (HttpStatusCode.Conflict,
                "UserAlreadyExists",
                "An account with this email already exists."),

            InvalidPasswordException =>
                (HttpStatusCode.BadRequest,
                "InvalidPassword",
                "Password does not meet the requirements."),

            // ── Confirmation ───────────────────────────────────────────────────
            CodeMismatchException =>
                (HttpStatusCode.BadRequest,
                "InvalidCode",
                "The confirmation code is incorrect."),

            ExpiredCodeException =>
                (HttpStatusCode.BadRequest,
                "ExpiredCode",
                "The confirmation code has expired. Please request a new one."),

            // ── NotAuthorized — específicos antes do genérico ──────────────────
            NotAuthorizedException ex when ex.Message.Contains("Access Token") =>
                (HttpStatusCode.Unauthorized,
                "InvalidAccessToken",
                "Access token is invalid or has expired. Please sign in again."),

            NotAuthorizedException ex when ex.Message.Contains("Refresh Token") =>
                (HttpStatusCode.Unauthorized,
                "InvalidRefreshToken",
                "Refresh token is invalid or has expired. Please sign in again."),

            NotAuthorizedException =>
                (HttpStatusCode.Unauthorized,
                "NotAuthorized",
                "Incorrect email or password."),

            // ── Sign In ────────────────────────────────────────────────────────
            UserNotConfirmedException =>
                (HttpStatusCode.Forbidden,
                "UserNotConfirmed",
                "Account not confirmed. Please check your email for the confirmation code."),

            UserNotFoundException =>
                (HttpStatusCode.NotFound,
                "UserNotFound",
                "No account found with this email."),

            // ── Token ──────────────────────────────────────────────────────────
            TooManyRequestsException =>
                (HttpStatusCode.TooManyRequests,
                "TooManyRequests",
                "Too many requests. Please wait before trying again."),

            // ── Generic Cognito ────────────────────────────────────────────────
            InvalidParameterException ex =>
                (HttpStatusCode.BadRequest,
                "InvalidParameter",
                ex.Message),

            // ── Fallback ───────────────────────────────────────────────────────
            _ =>
                (HttpStatusCode.InternalServerError,
                "InternalServerError",
                "An unexpected error occurred. Please try again later.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse(error, message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}