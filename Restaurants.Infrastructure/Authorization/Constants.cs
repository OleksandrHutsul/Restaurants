﻿namespace Restaurants.Infrastructure.Authorization;

public static class PolicyName
{
    public const string HasNationality = "HasNationality";
    public const string AtLeast20 = "AtLeast20";
    public const string CreatedAtleast2Restaurants = "CreatedAtleast2Restaurants";
}

public static class AppClaimsType
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}