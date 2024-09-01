﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using Microsoft.AspNetCore.Identity;
using SpringBootCloneApp.Models.Enums;
using System;
using System.Collections.Generic;

namespace SpringBootCloneApp.Models;

public partial class Authority : IdentityRole<long>
{
    public override long Id { get; set; }
    public override string Name { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public override bool Equals(object? obj)
    {
        var other = obj as Authority;
        if (other == null) return false;
        return this.Name == other.Name;
    }

    public override int GetHashCode()
    {
       return this.Id.GetHashCode();
    }
}