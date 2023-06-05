using Microsoft.AspNetCore.Mvc;
using Server.Services;
using CPM.Shared.DTO;
using Shared.Enums;
using Shared.Models;

namespace CPM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EntryController : ControllerBase
{

    private readonly ILogger<EntryController> _logger;
    private readonly IEntryService _entryService;
    private readonly IAllocationService _allocationService;

    public EntryController(ILogger<EntryController> logger, IEntryService entryService, IAllocationService allocationService)
    {
        _logger = logger;
        _entryService = entryService;
        _allocationService = allocationService;
    }

    [HttpPost("park")]
    public async Task<ActionResult<Entry>> Park([FromBody] EntryParkRequestDTO req)
    {
        var numberPlate = req.NumberPlate;
        if (string.IsNullOrEmpty(numberPlate))
            return BadRequest(ErrorStatusDTO.NumberPlateNotValid);

        var entry = await _entryService.GetEntryByNumberPlateAsync(numberPlate);
        // TODO: phase-2: handle logic to prompt user to pay previous amount before parking again
        if (entry != null)
            return BadRequest(ErrorStatusDTO.NumberPlateAlreadyExists);

        // check if fully parked
        var canPark = _allocationService.AllocateSlot();
        if (!canPark)
            return BadRequest(ErrorStatusDTO.FullyParked);

        var newEntry = new Entry
        {
            NumberPlate = numberPlate,
            From = DateTime.Now,
            Status = StatusEnum.Parked,
        };
        var addedEntry = await _entryService.AddEntryAsync(newEntry);
        // return EntryDTO with 201 status
        // return entryDTO
        var entryDTO = new EntryDTO
        {
            NumberPlate = addedEntry.NumberPlate,
            From = addedEntry.From,
            To = addedEntry.To,
            Status = addedEntry.Status,
            Amount = addedEntry.Amount
        };
        return CreatedAtAction(nameof(Get), new { numberPlate = addedEntry.NumberPlate }, entryDTO);
    }

    // Get method
    // Numberplate as parameter
    [HttpGet("{numberPlate}")]
    public async Task<ActionResult<Entry>> Get(string numberPlate)
    {
        var entry = await _entryService.GetEntryByNumberPlateAsync(numberPlate);
        if (entry == null)
            return NotFound(ErrorStatusDTO.NumberPlateNotParked);

        // change entry to DTO
        var entryDTO = new EntryDTO
        {
            NumberPlate = entry.NumberPlate,
            From = entry.From,
            To = entry.To,
            Status = entry.Status,
            Amount = entry.Amount
        };

        return Ok(entryDTO);
    }

    // put method to update an entry's status
    // Numberplate as parameter
    [HttpPost("complete")]
    public async Task<ActionResult<Entry>> Complete([FromBody] EntryParkRequestDTO req)
    {
        var numberPlate = req.NumberPlate;
        if (string.IsNullOrEmpty(numberPlate))
            return BadRequest(ErrorStatusDTO.NumberPlateNotValid);

        var entry = await _entryService.GetEntryByNumberPlateAsync(numberPlate);
        // TODO: handle logic to ask user From entry time
        if (entry == null)
            return BadRequest(ErrorStatusDTO.NumberPlateNotParked);

        entry.Status = StatusEnum.Completed;
        entry.Amount = _entryService.CalculateAmount(entry.From, entry.To);
        await _entryService.UpdateEntryAsync(entry);
        return Ok(entry);
    }
}
