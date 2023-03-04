namespace api.DTO;
public class CarDTO
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string FirstColor { get; set; } = string.Empty;
}