# Features that implemented

* Run Commands
* View Logs
* Creating\Editing\Deleting the commands

![consolas main screen](screen.png)
![consolas view logs](viewlogs.png)
![consolas edit screen](edit.png)

# Install

1. Start powershell with administrator privilegies. This need for creating a folder under 'Program Files'.

```powershell
PS> git clone https://github.com/mbasiuk/consolas.git
PS> .\install.ps1 -Rebuild -CreateShortcut 
```

2. This will create 2 folder C:\Program Files\Consolar with bin files,
and C:\ProgramData\Consolar for data;

2. click Consolar shortcut on the desktop.

# Update
```powershell
PS> git pull
PS> .\install.ps1 -Rebuild 
```


# Uninstall
 ```powershell
 PS> .\unistall.ps1
 PS> ri .\consolas -recurse -force
 ```

# Install Help
```powershell
PS> git clone https://github.com/mbasiuk/consolas.git
PS> man .\install.ps1 -Full
```
