using System;

namespace Xero.Api.Infrastructure.Interfaces
{
    public interface IConsumer
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
    }
    public interface IOAuth2Token
    {
        string AccessToken { get; set; }
        DateTimeOffset Expires { get; set; }
        int ExpiresIn { get; set; }
        bool HasExpired { get; }
        string IdToken { get; set; }
        string RefreshToken { get; set; }
        string Scope { get; set; }
        string TokenType { get; set; }
        string XeroUserId { get; set; }
    }
    public interface IToken
    {
        string UserId { get; set; }
        string OrganisationId { get; set; }

        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string TokenKey { get; }
        string TokenSecret { get; }
        string Session { get; }

        DateTime? ExpiresAt { get; }
        DateTime? SessionExpiresAt { get; }

        bool HasExpired { get; }
        bool HasSessionExpired { get; }
    }
}