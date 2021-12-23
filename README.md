# JoinLeaveMessages
![Discord](https://img.shields.io/discord/742861338233274418?label=Discord&logo=Discord) ![Github All Releases](https://img.shields.io/github/downloads/F-Plugins/JoinLeaveMessages/total?label=Downloads) ![GitHub release (latest by date)](https://img.shields.io/github/v/release/F-Plugins/JoinLeaveMessages?label=Version)

Broadcast users connections and disconnections to the global chat

### Download Now
RocketMod: [ClickMe](https://github.com/F-Plugins/JoinLeaveMessages/releases)

OpenMod: `openmod install Feli.JoinLeaveMessages`

## Open Mod
### Configuration
```yml
joinMessage:
  enabled: true
  showCountry: true
  customIconUrl: "" # Leave empty if you dont want a custom icon url

leaveMessage:
  enabled: false
  customIconUrl: "" # Leave empty if you dont want a custom icon url
```
### Translations
```yml
join: "<color=yellow>{User.DisplayName} has connected to the server from {Country}</color>"

leave: "<color=red>{User.DisplayName} disconnected from the server</color>"
```

## Rocket Mod
### Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <JoinMessages>true</JoinMessages>
  <ShowCountry>true</ShowCountry>
  <CustomImageUrl />
  <LeaveMessages>false</LeaveMessages>
</Configuration>
```

### Configuration with a message icon
```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <JoinMessages>true</JoinMessages>
  <ShowCountry>true</ShowCountry>
  <CustomImageUrl>https://i.imgur.com/aKaOqIh.gif</CustomImageUrl>
  <LeaveMessages>false</LeaveMessages>
</Configuration>
```


### Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="Join" Value="{0} has connected to the server from {1}" />
  <Translation Id="Leave" Value="{0} disconnected from the server" />
</Translations>
```