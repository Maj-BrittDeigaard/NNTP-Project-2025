using System;
using System.Net.Sockets;
using System.Text;

namespace Nntp.Core.Net;

/// <summary>
/// NNTP transport over TCP. Uses ASCII encoding and CRLF line endings.
/// </summary>
public sealed class TcpNntpTransport : INntpTransport, IDisposable
{
    private TcpClient? _tcp;
    private NetworkStream? _stream;
    private StreamReader? _reader;
    private StreamWriter? _writer;

    public async Task ConnectAsync(string host, int port)
    {
        _tcp = new TcpClient();
        await _tcp.ConnectAsync(host, port);

        _stream = _tcp.GetStream();
        _reader = new StreamReader(_stream, Encoding.ASCII, leaveOpen: true);
        _writer = new StreamWriter(_stream, Encoding.ASCII) { AutoFlush = true };

        // Read server greeting (e.g., “200 …”)
        _ = await _reader.ReadLineAsync();
    }

    public async Task<string> SendCommandAsync(string command)
    {
        if (_writer is null || _reader is null)
            throw new InvalidOperationException("Not connected.");

        // Ensure CRLF
        if (!command.EndsWith("\r\n"))
            command += "\r\n";

        await _writer.WriteAsync(command);

        // Return status line (e.g., “215 list of newsgroups follows”)
        var status = await _reader.ReadLineAsync() ?? string.Empty;
        return status;
    }

    public async Task<IReadOnlyList<string>> ReadMultilineAsync()
    {
        if (_reader is null) throw new InvalidOperationException("Not connected.");

        var lines = new List<string>(512);
        string? line;
        while ((line = await _reader.ReadLineAsync()) is not null)
        {
            // single dot terminates the block
            if (line == ".") break;   
            lines.Add(line);
        }
        return lines;
    }

    public void Dispose()
    {
        try { _writer?.Dispose(); } catch { }
        try { _reader?.Dispose(); } catch { }
        try { _stream?.Dispose(); } catch { }
        try { _tcp?.Close(); } catch { }
    }
}
