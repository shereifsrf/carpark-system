using Xunit;
using Server.Services;
using Moq;
using Server.Data;
using CPM.Server.Tests;
using Microsoft.Extensions.Options;
using CPM.Server.Configs;

namespace Server.Tests.Services;


public class AllocationService_Test : IClassFixture<DBFixture>
{
    DBFixture _fixture;
    private IOptions<DefaultConfigurations> _mockOptions;

    public AllocationService_Test(DBFixture fixture)
    {
        _fixture = fixture;
        _mockOptions = Options.Create(new DefaultConfigurations
        {
            SlotLimit = 5
        });
    }

    // add delay in AllocateSlot to check
    [Fact]
    public async Task ThreadSafeTest()
    {
        // Arrange
        var allocationService = new AllocationService(_fixture.Context, _mockOptions);

        var tasks = new List<Task<bool>>();
        for (int i = 0; i < 3; i++)
        {
            tasks.Add(Task.Run(() => allocationService.AllocateSlot()));
        }
        
        await Task.WhenAll(tasks);

        // one of the task should return false
        Assert.True(tasks.Any(t => t.Result == false));
    }
}