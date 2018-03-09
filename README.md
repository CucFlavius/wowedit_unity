# wowedit_unity
An attempt at recreating the World of Warcraft Editor used by Blizzard Entertainment to create the environments for the game.
My version's purpose is for the aiding of creating machinima / Creating custom environments using WoW's assets / exporting models etc.
Using Unity 5.6.1f1

### Current Progress on importing WoW Data:

| WDT | Description | Parsed | Implemented |
|----|----|----|----|
| MPHD | ADT flags | ✓ | % |
| MAIN | ADT existance | ✓ | ✓ |
| MWMO | WMO only map | ⨉ | ⨉ |
| MODF | WMO placement info | ⨉ | ⨉ |

| WDTocc | Description | Parsed | Implemented |
|----|----|----|----|
|  | occlusion | ⨉ | ⨉ |

| WDTlgt | Description | Parsed | Implemented |
|----|----|----|----|
|  | lights | ⨉ | ⨉ |

| WDTfogs | Description | Parsed | Implemented |
|----|----|----|----|
|  | fog | ⨉ | ⨉ |

| ADT | Description | Parsed | Implemented |
|----|----|----|----|
| MHDR | Chunk Offsets | ✓ | % |
| MH20 | Water Data | ✓ | ⨉ |
| MCNK | Terrain with Subchunks | ✓ | % |
| MCVT | Vertex Heights | ✓ | ✓ |
| MCLV | Vertex Lighting | ✓ | ⨉ |
| MCCV | Vertex Colors | ✓ | ✓ |
| MCNR | Vertex Normals | ✓ | ✓ |
| MCSE | Sound Emitters | ⨉ | ⨉ |
| MCBB | Blend Batches | ⨉ | ⨉ |
| MCDD | ??? | ⨉ | ⨉ |
| MFBO | Flight & Death plane | ✓ | ⨉ |
| MBMH | WMO blend header | ⨉ | ⨉ |
| MBBB | WMO blend boundingbox | ⨉ | ⨉ |
| MBNV | WMO blend vertices | ⨉ | ⨉ |
| MBMI | WMO blend indices | ⨉ | ⨉ |

| ADTtex | Description | Parsed | Implemented |
|----|----|----|----|
| MTEX | Texture File List | ✓ | ✓ |
| MCNK | Data Chunks | ✓ | % |
| MCLY | Texture Layers | ✓ | % |
| MCSH | Shadow Maps | ✓ | ⨉ |
| MCAL | Alpha Maps | ✓ | ✓ |
| MCMT | Material_id in DBC | ⨉ | ⨉ |
| MTXF | MTEX flags | ⨉ | ⨉ |
| MTXP | Texture Heights | ✓ | % |
| MAMP | Texture size | ✓ | ⨉ |

| ADTobj | Description | Parsed | Implemented |
|----|----|----|----|
|  | WMO/M2 loading | ⨉ | ⨉ |

| ADTlod | Description | Parsed | Implemented |
|----|----|----|----|
|  | LoD Data | ⨉ | ⨉ |

| WDL | Description | Parsed | Implemented |
|----|----|----|----|
|  | Low-resolution Heightmap | ⨉ | ⨉ |

| WMO Root | Description | Parsed | Implemented |
|----|----|----|----|
| MOHD | Root Header | ✓ | % |
| MOTX | Texture Paths | ✓ | ✓ |
| MOMT | Materials | ✓ | % |
| MOUV | Texture UV Animation | ✓ | ⨉ |
| MOGN | Group Names | ✓ | ✓ |
| MOGI | Group Info | ✓ | % |
| MOSB | Skybox File Name | ✓ | ⨉ |
| MOPV | Portal Vertices | ✓ | ⨉ |
| MOPT | Portal Information | ✓ | ⨉ |
| MOPR | Portal References | ✓ | ⨉ |
| MOVV | Visible Block Verts | ✓ | ⨉ |
| MOVB | Visible Block List | ✓ | ⨉ |
| MOLT | Lighting | ✓ | ⨉ |
| MODS | Doodad Sets | ✓ | ⨉ |
| MODN | M2 Filenames | ✓ | ⨉ |
| MODD | Doodad Instance Info | ✓ | ⨉ |
| MFOG | Fog Info | ✓ | ⨉ |
| MCVP | Convex Volume Planes | ✓ | ⨉ |
| GFID | FileID WMO Load | ✓ | ⨉ |

| WMO Group | Description | Parsed | Implemented |
|----|----|----|----|
| MOGP | Group Flags | ✓ | ✓ |
| MOPY | Material Info | ✓ | ⨉ |
| MOVI | Vertex Indices | ✓ | ✓ |
| MOVT | Vertices | ✓ | ✓ |
| MONR | Normals | ✓ | ✓ |
| MOTV | UVs | ✓ | ✓ |
| MOTV2 | UVs | ⨉ | ⨉ |
| MOTV3 | UVs | ⨉ | ⨉ |
| MOBA | Render Batches | ✓ | ✓ |
| MOCV | Vertex Colors | ✓ | % |
| MOCV2 | Vertex Colors | ⨉ | ⨉ |
| MOLR | Light References | ⨉ | ⨉ |
| MODR | Doodad References | ⨉ | ⨉ |
| MOBN | BSP Tree Nodes | ⨉ | ⨉ |
| MOBR | MOBN Face Indices | ⨉ | ⨉ |
| MLIQ | Liquids | ⨉ | ⨉ |
| MORI | Triangle Strip Indices | ⨉ | ⨉ |
| MORB | Modifies MOBA | ⨉ | ⨉ |
| MOTA | Map Object Tangents | ⨉ | ⨉ |
| MOBS | Map Object Shadow Batches | ⨉ | ⨉ |
| MDAL | Unknown WOD+ | ⨉ | ⨉ |
| MOPL | Terrain Cutting Planes | ⨉ | ⨉ |
| MOPB | Map Object Prepass Batches | ⨉ | ⨉ |
| MOLS | Map Object Spot Lights | ⨉ | ⨉ |
| MOLP | Map OBject Point Lights | ⨉ | ⨉ |
| MOLM | Light Map List | ⨉ | ⨉ |
| MOLD | Light Map Texture List | ⨉ | ⨉ |

| M2 | Description | Parsed | Implemented |
|----|----|----|----|
|  | M2 models | ⨉ | ⨉ |

      
The main source of information when i comes to extracting and parsing the World of Warcrat data comes from https://wowdev.wiki/Main_Page 
Big thanks to the guys who made and update the wiki.
