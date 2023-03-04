using api.Models;
using api.Repository;
using api.Services;
using Flurl;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace tests;

public class OpenDataTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IOptions<OpenDataSettings>> _openDataSettingsMock;
    private readonly Mock<ILogger<OpenData>> _loggerMock;
    private readonly IOptions<OpenDataSettings> _options = Options.Create(new OpenDataSettings()
    { Url = "https://opendata.rdw.nl/resource/m9d7-ebf2.json" });

    private readonly OpenData _openData;

    public OpenDataTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _openDataSettingsMock = new Mock<IOptions<OpenDataSettings>>();
        _loggerMock = new Mock<ILogger<OpenData>>();

        _openData = new OpenData(
            _carRepositoryMock.Object,
            _openDataSettingsMock.Object,
            _loggerMock.Object
        );
    }


    [Fact]
    public async Task GetVechiacleByPlate_ShouldHaveBeenCalled_AtLeastOnce()
    {
        using (var httpTest = new HttpTest())
        {
            var flurlRequestHandler = new OpenData(
                _carRepositoryMock.Object,
                _openDataSettingsMock.Object,
                _loggerMock.Object
            );
            var plate = "0002mj";
            var result = await flurlRequestHandler.GetVehiacleByPlate(plate);

            httpTest.ShouldHaveCalled(Url.Combine(_openDataSettingsMock.Object.Value.Url))
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
