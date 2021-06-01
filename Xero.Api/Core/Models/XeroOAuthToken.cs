using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xero.Api.Core.Models
{
    public class XeroOAuthToken : Xero.Api.Infrastructure.Interfaces.IToken
    {
        [JsonProperty("id_token")]
        [StringLength(4096)]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        [StringLength(4096)]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public DateTimeOffset Expires { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        [StringLength(4096)]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [Key]
        public string XeroUserId { get; set; }

        [NotMapped]
        public string UserId
        {
            get
            {
                return XeroUserId;
            }
            set { }
        }

        [NotMapped]
        public string OrganisationId
        {
            get
            {
                return XeroUserId;
            }
            set { }
        }

        [NotMapped]
        public string ConsumerKey { get; set; }

        [NotMapped]
        public string ConsumerSecret { get; set; }

        [NotMapped]
        public string TokenKey
        {
            get
            {
                return AccessToken;
            }
            set { }
        }

        [NotMapped]
        public string TokenSecret { get; set; }

        [NotMapped]
        public string Session { get; set; }

        [NotMapped]
        public DateTime? ExpiresAt { get; set; }

        [NotMapped]
        public DateTime? SessionExpiresAt { get; set; }

        [NotMapped]
        public bool HasExpired
        {
            get
            {
                return DateTimeOffset.Now > Expires;
            }
        }

        public bool HasSessionExpired { get; set; }
    }
}
