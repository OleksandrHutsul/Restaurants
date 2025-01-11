using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);


    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "New Test",
            Description = "New Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Description"
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            .Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExcitingRestaurant_ShouldThrowNotFoundException()
    {
        var restaurantId = 2;
        var requst = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null); 

        Func<Task> act = async () => await _handler.Handle(requst, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact()]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        var restaurantId = 3;
        var requst = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        var existingRestaurant = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(existingRestaurant);

        _restaurantAuthorizationServiceMock.
            Setup(r => r.Authorize(existingRestaurant, Domain.Constants.ResourceOperation.Update))
            .Returns(false);

        Func<Task> act = async () => await _handler.Handle(requst, CancellationToken.None);

        await act.Should().ThrowAsync<ForbidExeption>();
    }
}