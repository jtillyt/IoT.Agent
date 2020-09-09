D:\Development\Tools\putty\PLINK.EXE -batch -ssh pi@192.168.1.31 -pw Spr!t3 "sudo killall IoT.Agent; rm -r ~/programs/agent;mkdir ~/programs/agent"
dotnet build "Z:\Jay\IoT\IoT.Agent\IoT.Agent.csproj" -c DebugPi -f netcoreapp3.0 -r linux-arm
dotnet publish "Z:\Jay\IoT\IoT.Agent\IoT.Agent.csproj" -c DebugPi -f netcoreapp3.0 --self-contained true -r linux-arm
D:\Development\Tools\pscp.exe -r -l pi -pw Spr!t3 Z:\Jay\IoT\IoT.Agent\bin\DebugPi\netcoreapp3.0\linux-arm\publish\*.* pi@192.168.1.31:/home/pi/programs/agent
