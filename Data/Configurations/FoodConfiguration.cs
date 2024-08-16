﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCorePostgres.Data;
using EFCorePostgres.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace EFCorePostgres.Data.Configurations
{
    public partial class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__food__3213E83F74626B81");

            entity.ToTable("food");

            entity.HasIndex(e => e.Name, "UK_qkhr2yo38c1g9n5ss0jl7gxk6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CookTime)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cook_time");
            entity.Property(e => e.Hidden).HasColumnName("hidden");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Food> entity);
    }
}
