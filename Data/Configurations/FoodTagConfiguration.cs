﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpringBootCloneApp.Data;
using SpringBootCloneApp.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace SpringBootCloneApp.Data.Configurations
{
    public partial class FoodTagConfiguration : IEntityTypeConfiguration<FoodTag>
    {
        public void Configure(EntityTypeBuilder<FoodTag> entity)
        {
            entity
                .ToTable("food_tags");

            entity.HasKey(a => new {a.FoodId, a.Tag});

            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Tag)                
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tag");

            entity.HasIndex(x => x.Tag);

            entity.HasOne(d => d.Food).WithMany(f=> f.FoodTags)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKhho6c8bc39ejtrnphfxph3ito");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<FoodTag> entity);
    }
}
