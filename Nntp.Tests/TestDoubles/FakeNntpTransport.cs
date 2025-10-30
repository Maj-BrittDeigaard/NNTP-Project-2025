using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nntp.Core.Net;


namespace Nntp.Tests.TestDoubles;

// Simple in-memory fake transport for unit tests (no real network)
public sealed class FakeNntpTransport : INntpTransport
{
    public readonly List<string> Sent = new();
    public string StatusToReturn { get; set; } = "215 list of newsgroups follows";
    public IReadOnlyList<string> MultilineToReturn { get; set; } = Array.Empty<string>();

    public Task ConnectAsync(string host, int port) => Task.CompletedTask;

    public Task<string> SendCommandAsync(string command)
    {
        Sent.Add(command);
        return Task.FromResult(StatusToReturn);
    }

    public Task<IReadOnlyList<string>> ReadMultilineAsync()
        => Task.FromResult(MultilineToReturn);
}