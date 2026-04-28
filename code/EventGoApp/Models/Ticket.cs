using SQLite;

namespace EventGoApp.Models;

[Table("Tickets")]
public class Ticket
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Indexed]
    public Guid UserId { get; set; }

    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string EventImageSource { get; set; } = string.Empty;
    public string EventVenue { get; set; } = string.Empty;
    public string EventAddress { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    public int Quantity { get; set; } = 1;
    public double UnitPrice { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string BarcodeUrl => $"https://bwipjs-api.metafloor.com/?bcid=code128&text={Id.ToString().Substring(0, 8).ToUpper()}&scale=3&rotate=N&includetext";
}
