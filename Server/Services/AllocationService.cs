using CPM.Server.Configs;
using Microsoft.Extensions.Options;
using Server.Data;

namespace Server.Services;

public interface IAllocationService
{
    int GetRemainingSlots();
    bool AllocateSlot();
    bool ReleaseSlot();
}

// should be a thread safe class
public class AllocationService : IAllocationService
{
    // context of the database
    private readonly CpmDbContext _context;
    private int slotLimit { get; set; }
    private int usedSlot { get; set; }
    static readonly object _lock = new object();

    public AllocationService(CpmDbContext context, IOptions<DefaultConfigurations> options)
    {
        _context = context;
        slotLimit = options.Value.SlotLimit;
        findUsedSlot();
    }

    public bool AllocateSlot()
    {
        // lock the object to make it thread safe
        lock (_lock)
        {
            if (usedSlot < slotLimit)
            {
                // add delay for testing
                // Thread.Sleep(1000);
                usedSlot += 1;
                return true;
            }
        }

        return false;
    }

    public int GetRemainingSlots()
    {
        return slotLimit - usedSlot;
    }

    public bool ReleaseSlot()
    {
        usedSlot -= 1;
        return true;
    }

    private void findUsedSlot()
    {
        // count the number of entries with status "Parked"
        usedSlot = _context.Entries.Count(e => e.Status == Shared.Enums.StatusEnum.Parked);

        if (usedSlot > slotLimit)
        {
            // TODO: notify the admin
            throw new Exception("The number of used slots is greater than the slot limit");
        }
    }
}