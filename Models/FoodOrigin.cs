﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace EFCorePostgres.Models;

public partial class FoodOrigin
{
    public long FoodId { get; set; }

    public string Origin { get; set; } = null!;

    public virtual Food Food { get; set; } = null!;
}