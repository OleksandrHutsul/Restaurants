using Xunit;
using FluentAssertions;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    //[Fact()]
    public void IsInRoles_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRoles(roleName);
        //var isInRole = currentUser.IsInRoles(UserRoles.Admin);

        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRoles_WithNoMatchingRole_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRoles(UserRoles.Owner);

        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRoles_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRoles(UserRoles.Admin.ToLower());

        isInRole.Should().BeFalse();
    }
}