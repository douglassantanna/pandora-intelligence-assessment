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
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IOptions<OpenDataSettings>> _openDataSettingsMock;
    private readonly Mock<ILogger<OpenDataService>> _loggerMock;

    public OpenDataTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _openDataSettingsMock = new Mock<IOptions<OpenDataSettings>>();
        _loggerMock = new Mock<ILogger<OpenDataService>>();
    }
    [Fact]
    public async Task GetVehicleByPlate_ShouldReturnVehicles()
    {
        // Arrange
        var plate = "0002mj";
        var expectedVehicles = new List<Vehicle>()
        {
            new Vehicle("0002MJ","CHEVROLET","Personenauto","GROEN")
        };
        var openDataSettings = new OpenDataSettings() { Url = "https://opendata.rdw.nl/resource/m9d7-ebf2.json" };
        _openDataSettingsMock.Setup(x => x.Value).Returns(openDataSettings);
        var httpTest = new HttpTest();

        httpTest.RespondWithJson(expectedVehicles);

        var openData = new OpenDataService(_vehicleRepositoryMock.Object, _openDataSettingsMock.Object, _loggerMock.Object);

        // Act
        var actualVehicles = await openData.GetVehicleByPlate(plate);

        // Assert
        Assert.NotNull(actualVehicles);
        Assert.Equal(expectedVehicles.Count, actualVehicles.Count());
        Assert.Equal(expectedVehicles.First().Kenteken, actualVehicles.First().Kenteken);

        httpTest.ShouldHaveCalled(Url.Combine(openDataSettings.Url, $"?kenteken={plate.ToUpper()}"))
                .WithVerb(HttpMethod.Get)
                .Times(1);
    }

    [Fact]
    public void TestVehicleConstructor()
    {
        // Arrange
        string expectedKenteken = "ABC123";
        string expectedMerk = "Ford";
        string expectedVoertuigsoort = "Sedan";
        string expectedEerste_kleur = "Blue";

        // Act
        var vehicle = new Vehicle(expectedKenteken, expectedMerk, expectedVoertuigsoort, expectedEerste_kleur);

        // Assert
        Assert.Equal(expectedKenteken, vehicle.Kenteken);
        Assert.Equal(expectedMerk, vehicle.Merk);
        Assert.Equal(expectedVoertuigsoort, vehicle.Voertuigsoort);
        Assert.Equal(expectedEerste_kleur, vehicle.Eerste_kleur);
    }

    [Fact]
    public void CreateVehicleWithNullValues_ShouldReturnNullException()
    {
        // Arrange
        string expectedKenteken = null;
        string expectedMerk = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Vehicle(expectedKenteken, expectedMerk, "Sedan", "Blue"));
    }

}
