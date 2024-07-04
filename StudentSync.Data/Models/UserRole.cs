using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class UserRole
{
    [Key]

    public string Id { get; set; } = null!;

    public string? Name { get; set; }
}
