﻿using System;
using System.Collections.Generic;

namespace ASPNETCOREAPI.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } 

    public string Email { get; set; } 
    public string Password { get; set; } 

    public string? Role { get; set; } 

    public string? Permission { get; set; }

    public DateTime Birthday { get; set; } 

    public virtual ICollection<Cart>? Carts { get; set; }
}
