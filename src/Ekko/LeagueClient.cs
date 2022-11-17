using System.Diagnostics;

namespace Ekko;

public class LeagueClient
{
    internal LeagueClient(int pid,ClientAuthInfo info,Process process)
    {
        Pid = pid;
        ClientAuthInfo = info;
        Process = process;
    }
    public int Pid { get; set; }
   public ClientAuthInfo ClientAuthInfo { get; set; }
    public Process Process { get; set; }
}

public class ClientAuthInfo
{
    public string RiotClientAuthToken { get; set; }
    public int RiotClientPort { get; set; }
    public string RemotingAuthToken { get; set; }
    public int RemotingPort { get; set; }
}