namespace api.Models;
public class Car
{
    public Car(string kenteken,
               string merk,
               string voertuigsoort,
               string eerste_kleur)
    {
        Kenteken = kenteken ?? throw new ArgumentNullException(nameof(kenteken));
        Merk = merk ?? throw new ArgumentNullException(nameof(merk));
        Voertuigsoort = voertuigsoort;
        Eerste_kleur = eerste_kleur;
    }

    public int Id { get; private set; }
    public string Kenteken { get; private set; } = string.Empty;
    public string Merk { get; private set; } = string.Empty;
    public string Voertuigsoort { get; private set; } = string.Empty;
    public string Eerste_kleur { get; private set; } = string.Empty;
}