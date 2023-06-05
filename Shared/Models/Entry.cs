using System;
using Shared.Enums;

namespace Shared.Models;

public class Entry
{
    public int Id { get; set; }
    public required string NumberPlate { get; set; }

    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal Amount { get; set; }
    public StatusEnum Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }
}