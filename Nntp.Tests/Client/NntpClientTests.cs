using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Nntp.Tests.TestDoubles;
using Nntp.Core.Client;

public class NntpClientTests
{
    [Fact]
    public async Task ListGroupsAsync_SendsLIST_ParsesTwoGroups()
    {
        // Arrange (fake multi-line server response)
        var fake = new FakeNntpTransport
        {
            MultilineToReturn = new[]
            {
                "comp.lang.c 12345 1 y",
                "dk.edb.programmering 9876 100 n",
                "."
            }
        };
        var client = new NntpClient(fake);

        // Act
        var groups = await client.ListGroupsAsync();

        // Assert (command + parsed objects)
        Assert.Contains("LIST", fake.Sent[0]);     // ensures LIST command sent
        Assert.Equal(2, groups.Count);             // two groups parsed
        Assert.Equal("comp.lang.c", groups[0].Name);
        Assert.Equal("dk.edb.programmering", groups[1].Name);
    }
}

