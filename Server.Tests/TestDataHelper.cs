using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;
using Shared.Enums;

namespace CPM.Server.Tests;

public class DBFixture : IDisposable
{
    public CpmDbContext Context { get; private set; }

    public DBFixture()
    {
        Context = TestDataHelper.GetInMemoryDbContextWithData();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}

public class TestDataHelper
{
    public static CpmDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CpmDbContext>()
            .UseInMemoryDatabase(databaseName: "CpmDb")
            .Options;

        var context = new CpmDbContext(options);

        return context;
    }

    public static CpmDbContext GetInMemoryDbContextWithData()
    {
        var options = new DbContextOptionsBuilder<CpmDbContext>()
            .UseInMemoryDatabase(databaseName: "CpmDb")
            .Options;

        var context = new CpmDbContext(options);

        // add some data
        context.Entries.Add(new Entry
        {
            NumberPlate = "Test 1",
            From = DateTime.Now,
            Status = StatusEnum.Parked,            
        });

        context.Entries.Add(new Entry
        {
            NumberPlate = "Test 2",
            From = DateTime.Now,
            Status = StatusEnum.Parked,  
        });

        context.Entries.Add(new Entry
        {
            NumberPlate = "Test 3",
            From = DateTime.Now,
            Status = StatusEnum.Parked,
        });

        context.SaveChanges();

        return context;
    }
}