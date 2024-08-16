﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using Microsoft.AspNetCore.Identity;
using EFCorePostgres.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCorePostgres.Models;

public partial class Client : IdentityUser<long>
{
    public Client() { }
    public Client(ClientDTO dto)
    {
        this.Id = dto.Id;
        this.Address = dto.Address;
        this.Banned = dto.Banned;
        this.Email = dto.Email;
        this.FirstName = dto.FirstName;
        this.LastName = dto.LastName;
        this.PasswordHash = dto.Password;
        this.UserName = dto.Email;
    }
    [PersonalData]
    public override long Id { get; set; }

    public string Address { get; set; } = null!;

    public bool Banned { get; set; }

    [ProtectedPersonalData]
    public override string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [EmailAddress]
    [ProtectedPersonalData]
    public override string UserName { get; set; }
    public override string PasswordHash { get; set; } = null!;

    public virtual ICollection<Authority> Authorities { get; set; } = new List<Authority>();

    public virtual ICollection<ClientFoodFav> ClientFoodFavs { get; set; } = new List<ClientFoodFav>();

    public virtual ICollection<FoodOrder> FoodOrders { get; set; } = new List<FoodOrder>();

    //public virtual RefreshToken? RefreshToken { get; set; }

    public bool Equals(Client? other)
    {

        if (other == null)
            return false;

        if (this.Id == other.Id || this.Email == other.Email)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        return $"ClientId: {Id}, Address: {Address}, Banned: {Banned}\nEmail: {Email}, Name: {LastName}, {FirstName}\nPassword: {PasswordHash}";
    }


    public Client UpdateClientFromDTO(ClientDTO dto)
    {
        this.Id = dto.Id;
        this.Address = dto.Address;
        this.Banned = dto.Banned;
        this.Email = dto.Email;
        this.FirstName = dto.FirstName;
        this.LastName = dto.LastName;
        return this;
    }

}