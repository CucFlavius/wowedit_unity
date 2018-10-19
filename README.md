# wowedit_unity
An attempt at recreating the World of Warcraft Editor used by Blizzard Entertainment to create the environments for the game.
My version's purpose is for the aiding of creating machinima / Creating custom environments using WoW's assets / exporting models etc.
Using Unity 2018.2.13f1

### Current Progress on importing WoW Data:

| WDT | Description | Parsed | Implemented |
|----|----|----|----|
| MPHD | ADT flags | ✓ | % |
| MAIN | ADT existance | ✓ | ✓ |
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

| ADT | Description | Parsed | Implemented |
|----|----|----|----|
| MHDR | Chunk Offsets | ✓ | % |
| MH20 | Water Data | ✓ | X |
| MCNK | Terrain with Subchunks | ✓ | % |
| MCVT | Vertex Heights | ✓ | ✓ |
| MCLV | Vertex Lighting | ✓ | X |
| MCCV | Vertex Colors | ✓ | ✓ |
| MCNR | Vertex Normals | ✓ | ✓ |
| MCSE | Sound Emitters | X | X |
| MCBB | Blend Batches | X | X |
| MCDD | ??? | X | X |
| MFBO | Flight & Death plane | ✓ | X |
| MBMH | WMO blend header | X | X |
| MBBB | WMO blend boundingbox | X | X |
| MBNV | WMO blend vertices | X | X |
| MBMI | WMO blend indices | X | X |

| ADTtex | Description | Parsed | Implemented |
|----|----|----|----|
| MTEX | Texture File List | ✓ | ✓ |
| MCNK | Data Chunks | ✓ | % |
| MCLY | Texture Layers | ✓ | % |
| MCSH | Shadow Maps | ✓ | ✓ |
| MCAL | Alpha Maps | ✓ | ✓ |
| MCMT | Material_id in DBC | X | X |
| MTXF | MTEX flags | X | X |
| MTXP | Texture Heights | ✓ | % |
| MAMP | Texture size | ✓ | X |

| ADTobj | Description | Parsed | Implemented |
|----|----|----|----|
| MMDX | List of M2 Filenames | ✓ | X |
| MMID | List of Offsets in MMDX | ✓ | X |
| MWMO | List of WMO Filenames | ✓ | ✓ |
| MWID | List of Offsets in MWMO | ✓ | ✓ |
| MDDF | Placement information for M2s | ✓ | X |
| MODF | Placement information for WMOs | ✓ | ✓ |
| MCNK | Chunk Data | ✓ | X |
| MCRD | Per Chunk Doodad References in MDDF | ✓ | X |
| MCRW | Per Chunk WMO References in MODF | ✓ | X |

| ADTlod | Description | Parsed | Implemented |
|----|----|----|----|
|  | LoD Data | X | X |

| WDL | Description | Parsed | Implemented |
|----|----|----|----|
|  | Low-resolution Heightmap | X | X |

| WMO Root | Description | Parsed | Implemented |
|----|----|----|----|
| MOHD | Root Header | ✓ | % |
| MOTX | Texture Paths | ✓ | ✓ |
| MOMT | Materials | ✓ | % |
| MOUV | Texture UV Animation | ✓ | X |
| MOGN | Group Names | ✓ | ✓ |
| MOGI | Group Info | ✓ | % |
| MOSB | Skybox File Name | ✓ | X |
| MOPV | Portal Vertices | ✓ | X |
| MOPT | Portal Information | ✓ | X |
| MOPR | Portal References | ✓ | X |
| MOVV | Visible Block Verts | ✓ | X |
| MOVB | Visible Block List | ✓ | X |
| MOLT | Lighting | ✓ | X |
| MODS | Doodad Sets | ✓ | X |
| MODN | M2 Filenames | ✓ | X |
| MODD | Doodad Instance Info | ✓ | X |
| MFOG | Fog Info | ✓ | X |
| MCVP | Convex Volume Planes | ✓ | X |
| GFID | FileID WMO Load | ✓ | X |

| WMO Group | Description | Parsed | Implemented |
|----|----|----|----|
| MOGP | Group Flags | ✓ | ✓ |
| MOPY | Material Info | ✓ | X |
| MOVI | Vertex Indices | ✓ | ✓ |
| MOVT | Vertices | ✓ | ✓ |
| MONR | Normals | ✓ | ✓ |
| MOTV | UVs | ✓ | ✓ |
| MOTV2 | UVs | X | X |
| MOTV3 | UVs | X | X |
| MOBA | Render Batches | ✓ | ✓ |
| MOCV | Vertex Colors | ✓ | % |
| MOCV2 | Vertex Colors | X | X |
| MOLR | Light References | X | X |
| MODR | Doodad References | X | X |
| MOBN | BSP Tree Nodes | X | X |
| MOBR | MOBN Face Indices | X | X |
| MLIQ | Liquids | X | X |
| MORI | Triangle Strip Indices | X | X |
| MORB | Modifies MOBA | X | X |
| MOTA | Map Object Tangents | X | X |
| MOBS | Map Object Shadow Batches | X | X |
| MDAL | Unknown WOD+ | X | X |
| MOPL | Terrain Cutting Planes | X | X |
| MOPB | Map Object Prepass Batches | X | X |
| MOLS | Map Object Spot Lights | X | X |
| MOLP | Map OBject Point Lights | X | X |
| MOLM | Light Map List | X | X |
| MOLD | Light Map Texture List | X | X |

| M2 | Description | Parsed | Implemented |
|----|----|----|----|
|  | M2 models | ✓ | % |

| DB2 | Description | Parsed | Implemented
|----|----|----|----|
| WDC1 | WDC1 Support | ✓ | % |
| WDC2 | WDC2 Support | % | X |
| WDC3 | WDC3 Support | % | X |

The main source of information when i comes to extracting and parsing the World of Warcrat data comes from https://wowdev.wiki/Main_Page 
Big thanks to the guys who made and update the wiki.
