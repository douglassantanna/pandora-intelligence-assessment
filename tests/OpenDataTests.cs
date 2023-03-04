using api.Models;
using api.Repository;
using api.Services;
using Flurl;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace tests;

public class OpenDataTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IOptions<OpenDataSettings>> _openDataSettingsMock;
    private readonly Mock<ILogger<OpenDataService>> _loggerMock;

    public OpenDataTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _openDataSettingsMock = new Mock<IOptions<OpenDataSettings>>();
        _loggerMock = new Mock<ILogger<OpenDataService>>();
    }
    [Fact]
    public async Task GetVehicleByPlate_ShouldReturnCars()
    {
        // Arrange
        var plate = "0002mj";
        var expectedCars = new List<Car>()
        {
            new Car("0002MJ","CHEVROLET","Personenauto","GROEN")
        };
        var openDataSettings = new OpenDataSettings() { Url = "https://opendata.rdw.nl/resource/m9d7-ebf2.json" };
        _openDataSettingsMock.Setup(x => x.Value).Returns(openDataSettings);
        var httpTest = new HttpTest();

        httpTest.RespondWithJson(expectedCars);

        var openData = new OpenDataService(_carRepositoryMock.Object, _openDataSettingsMock.Object, _loggerMock.Object);

        // Act
        var actualCars = await openData.GetVehicleByPlate(plate);

        // Assert
        Assert.NotNull(actualCars);
        Assert.Equal(expectedCars.Count, actualCars.Count());
        Assert.Equal(expectedCars.First().Kenteken, actualCars.First().Kenteken);

        httpTest.ShouldHaveCalled(Url.Combine(openDataSettings.Url, $"?kenteken={plate.ToUpper()}"))
                .WithVerb(HttpMethod.Get)
                .Times(1);
    }

    [Fact]
    public void TestCarConstructor()
    {
        // Arrange
        string expectedKenteken = "ABC123";
        string expectedMerk = "Ford";
        string expectedVoertuigsoort = "Sedan";
        string expectedEerste_kleur = "Blue";

        // Act
        var car = new Car(expectedKenteken, expectedMerk, expectedVoertuigsoort, expectedEerste_kleur);

        // Assert
        Assert.Equal(expectedKenteken, car.Kenteken);
        Assert.Equal(expectedMerk, car.Merk);
        Assert.Equal(expectedVoertuigsoort, car.Voertuigsoort);
        Assert.Equal(expectedEerste_kleur, car.Eerste_kleur);
    }

    [Fact]
    public void CreateCarWithNullValues_ShouldReturnNullException()
    {
        // Arrange
        string expectedKenteken = null;
        string expectedMerk = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Car(expectedKenteken, expectedMerk, "Sedan", "Blue"));
    }

}
