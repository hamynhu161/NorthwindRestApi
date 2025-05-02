using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Firstname { get; set; } 

    public string Lastname { get; set; } 

    public string? Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; } = "abcdefgh";

    public int AccesslevelId { get; set; }
}
