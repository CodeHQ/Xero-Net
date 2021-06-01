using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xero.Api.Infrastructure.Interfaces;

namespace Xero.Api.Core.Models
{
    public class XeroAuthToken : IToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string OrganisationId { get; set; }

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string TokenKey { get; set; }

        public string TokenSecret { get; set; }

        public string Session { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? SessionExpiresAt { get; set; }

        public bool HasExpired { get; set; }

        public bool HasSessionExpired { get; set; }
    }
}
