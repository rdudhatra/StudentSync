using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class UserLogin
{
    [Key]
    public string LoginProvider { get; set; } = null!;

    public string? ProviderKey { get; set; }

    public string? ProviderDisplayName { get; set; }

    public string? UserId { get; set; }
}
