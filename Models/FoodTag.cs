﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace EFCorePostgres.Models;

public partial class FoodTag
{
    public long FoodId { get; set; }

    public string Tag { get; set; } = null!;

    public virtual Food Food { get; set; } = null!;
}