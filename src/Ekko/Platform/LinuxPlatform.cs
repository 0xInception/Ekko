namespace Ekko.Platform;

public class LinuxPlatform : PlatformBase
{
    protected override string FileName => "/bin/bash";
    protected override string AuthTokenRegex => "--(riotclient|remoting)-auth-token=(.*?)( --|\n|$|\")";
    protected override string PortRegex => "--(riotclient-|)app-port=(.*?)( --|\n|$|\")";
    protected override string GetCommand(int pid) =>$"-c \"ps -ww -fp {pid}\"";

}