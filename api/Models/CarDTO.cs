namespace api.Models;
public class CarDTO
{
    public int Id { get; set; }
    public string Kenteken { get; set; } = string.Empty;
    public string Merk { get; set; } = string.Empty;
    public string Voertuigsoort { get; set; } = string.Empty;
    public string Eerste_kleur { get; set; } = string.Empty;
}