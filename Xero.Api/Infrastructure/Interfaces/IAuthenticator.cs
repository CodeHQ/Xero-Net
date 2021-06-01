using System;

namespace Xero.Api.Infrastructure.Interfaces
{
    public interface IAuthenticator
    {
        IToken GetToken(IConsumer consumer, IUser user);

        string TenantId { get; set; }
        string XeroUserId { get; set; }
    }
}
