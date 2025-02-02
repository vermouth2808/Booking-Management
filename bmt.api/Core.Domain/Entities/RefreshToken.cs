using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string? Token { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public string? JwtId { get; set; }

    public int? UserId { get; set; }
}
