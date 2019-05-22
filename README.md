# WoWEdit Unity
An attempt at recreating the World of Warcraft Editor used by Blizzard Entertainment to create the environments for the game.
My version's purpose is for the aiding of creating machinima / Creating custom environments using WoW's assets / exporting models etc.
Using Unity 2019.2.0a9

**Currently supported** : 8.2

### Current [[Progress|Progress]] on importing WoW Data

| WDT | Description | Parsed | Implemented |
|----|----|----|----|
| MPHD | ADT flags | ✓ | % |
| MAIN | ADT existance | ✓ | % |
| MAID | Map File Data Ids | ✓ | % |
| MWMO | WMO only map | X | X |
| MODF | WMO placement info | X | X |

| WDTocc | Description | Parsed | Implemented |
|----|----|----|----|
|  | occlusion | X | X |

| WDTlgt | Description | Parsed | Implemented |
|----|----|----|----|
|  | lights | X | X |

| WDTfogs | Description | Parsed | Implemented |
|----|----|----|----|
|  | fog | X | X |




      
The main source of information when it comes to extracting and parsing the World of Warcrat data comes from https://wowdev.wiki/Main_Page 

### Big Thanks to:
* Makers of WoWDev Wiki (https://wowdev.wiki)
