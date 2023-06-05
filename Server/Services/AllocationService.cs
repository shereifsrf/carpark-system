using Server.Data;

namespace Server.Services;

public interface IAllocationService
{
    int GetRemainingSlots();
    bool AllocateSlot();
    bool ReleaseSlot();
}

public class AllocationService : IAllocationService
{
    // context of the database
    private readonly CpmDbContext _context;
    private int slotLimit = 100;
    private int usedSlot { get; set;}

    public AllocationService(CpmDbContext context)
    {
        _context = context;
    }

    public bool AllocateSlot()
    {
        if (usedSlot < slotLimit)
        {
            usedSlot += 1;
            return true;
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