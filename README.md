# BlockParty-SDCA
BlockParty SDCA Bot Utility.
Change deal TP according to SO count.

## Installation
.net 5.0 SDK
[dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/install/)

## Download
Binary for [x64](https://github.com/artyponp/BlockParty-SDCA/releases/download/1.0.1/publish_x64.zip) and [macOS](https://github.com/artyponp/BlockParty-SDCA/releases/download/1.0.1/publish_mac.zip)

## Usage
### Windows
open command prompt
```sh
cd <app_folder>
bptsdca "Bot name" API_KEY API_SECRET
```
### macOS
open terminal
```sh
cd <app_folder>
./bptsdca "Bot name" API_KEY API_SECRET
```

You can setup task scheduler to auto run the program periodically.

## Configuration
Default configuration based on active trader TP targets.

![default config](/docs/cfg3.png)

You can open appsettings.json and change TP as you want.

```json
{
  "TP":[
    {
      "SO":1,
      "TP":0.42
    },
    {
      "SO":2,
      "TP":0.42
    },
    {
      "SO":3,
      "TP":1.0
    },
    {
      "SO":4,
      "TP":2.0
    },
    {
      "SO":5,
      "TP":3.0
    },
    {
      "SO":6,
      "TP":4.0
    },
    {
      "SO":7,
      "TP":5.0
    },
    {
      "SO":8,
      "TP":6.0
    }
  ]
}
```

You can also change TP config using config editor.
Config editor is a cross platforms UI editor. You will need java installed on system.

Download and install [java](https://www.java.com/download/)


Lauch config editor by double-click BPT-SDCA-UI-Swing.jar

Select SO and specify TP then save.

![config editor](/docs/cfg1.png)

### Note for macOS
By default double-click to launch config editor from jar does not have permission to read/write file.

Fix this by adding
/System/Library/CoreServices/Jar Launcher.app
to
System Preferences -> Security -> Privacy -> Full Disk Access

![config editor permission](/docs/cfg2.png)

You can also launch editor from terminal with this command. (must install JDK)
```
java -jar BPT-SDCA-UI-Swing.jar
```


## Compile and Run from source code

```
dotnet run -- "Bot name" API_KEY API_SECRET
```

## Credits

[3Commas.Net](https://github.com/TheKimono/3Commas.Net)
