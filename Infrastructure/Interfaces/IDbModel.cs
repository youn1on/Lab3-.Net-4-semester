﻿namespace Infrastructure.Interfaces;

public interface IDbModel
{
    public int Id { get; }
    public string ToCsvString();
}