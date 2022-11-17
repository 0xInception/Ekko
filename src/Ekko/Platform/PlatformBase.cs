using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Ekko.Platform;

public abstract class PlatformBase
{
    protected abstract string FileName { get; }
    protected abstract string AuthTokenRegex { get; }
    protected abstract string PortRegex { get; }
    protected abstract string GetCommand(int pid);

    public ClientAuthInfo? ExtractArguments(int pid)
    {
        var command = GetCommand(pid);
        var commandLine = GetCommandLine(FileName, command);
        return string.IsNullOrEmpty(commandLine) ? null : ExtractArguments(commandLine);
    }
    public ClientAuthInfo? ExtractArguments(string commandLine)
    {
        var authToken = Regex.Matches(commandLine, AuthTokenRegex);
        var port = Regex.Matches(commandLine, PortRegex);

        var authInfo = new ClientAuthInfo();
        foreach (Match match in authToken)
        {
            if (match.Groups.Count != 4)
                continue;
            
            switch (match.Groups[1].Value)
            {
                case "riotclient":
                    authInfo.RiotClientAuthToken = match.Groups[2].Value;
                    break;
                case "remoting":
                    authInfo.RemotingAuthToken = match.Groups[2].Value;
                    break;
            }
        }
        
        foreach (Match match in port)
        {
            if (match.Groups.Count != 4)
                continue;
            
            switch (match.Groups[1].Value)
            {
                case "riotclient-":
                    authInfo.RiotClientPort = Convert.ToInt32(match.Groups[2].Value);
                    break;
                case "":
                    authInfo.RemotingPort = Convert.ToInt32(match.Groups[2].Value);
                    break;
            }
        }

        return authInfo;
    }
    private string? GetCommandLine(string fileName,string command)
    {
        var output = string.Empty;
        try
        {
            var startInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = fileName,
                Arguments = command,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };

            var proc = Process.Start(startInfo);
            output = proc?.StandardOutput.ReadToEnd();
            proc?.WaitForExit(5000);

            return output;
        }
        catch (Exception)
        {
            return output;
        }
    }
}