# wowedit_unity
An attempt at recreating the World of Warcraft Editor used by Blizzard Entertainment to create the environments for the game.
My version's purpose is for the aiding of creating machinima / Creating custom environments using WoW's assets / exporting models etc.
Using Unity 5.6.1f1

Current Progress on importing WoW Data:
--------------------------------------------------------------------------------------
   CASC : implemented, but not enabled for terrain loading (need to build a tree structure from the list file)
   
    WDT : MPHD (ADT flags)				  - Parsed, Partially used.
		  MAIN (ADT existance)			  - Parsed, Implemented.
		  MWMO (WMO only map)			  - Not Parsed, Not implemented.
		  MODF (WMO placement info)		  - Not Parsed, Not implemented.
		  
 WDTocc : (occlusion) Not Parsed, Not implemented.
 
 WDTlgt : (lights) Not Parsed, Not implemented.
 
WDTfogs : Not Parsed, Not implemented.

    ADT : MHDR (Chunk Offsets)            - Parsed, Partially used.
          MH20 (Water Data)               - Parsed, Not implemented.
          MCNK (Terrain with Subchunks)   - Parsed, Partially used.
            MCVT (Vertex Heights)           - Parsed, Implemented.
            MCLV (Vertex Lighting)          - Parsed, Not implemented.
            MCCV (Vertex Colors)            - Parsed, Implemented.
            MCNR (Vertex Normals)           - Parsed, Implemented.
            MCSE (Sound Emitters)           - Not Parsed, Not implemented.
            MCBB (Blend Batches)            - Not Parsed, Not implemented.
            MCDD (Not sure)                 - Not Parsed, Not implemented.
          MFBO (Flight & Death plane)     - Parsed, Not implemented.
          MBMH (WMO blend header)         - Not Parsed, Not implemented.
          MBBB (WMO blend boundingbox)    - Not Parsed, Not implemented.
          MBNV (WMO blend vertices)       - Not Parsed, Not implemented.
          MBMI (WMO blend indices)        - Not Parsed, Not implemented.

 ADTtex : MAMP (Texture size)			  - Parsed, Not implemented.
		  MTEX (Texture List)			  - Parsed, Implemented.
		  MCNK (Data Chunks)			  - No header just chunks
			MCLY (Texture layers)			- Parsed, Partially used.
			MCSH (Shadow maps)				- Parsed, Not implemented.
			MCAL (Alpha maps)				- Parsed, Implemented.
			MCMT (material_id in DBC)		- Not Parsed, Not implemented.
			MTXF (MTEX flags)				- Not Parsed, Not implemented.
			MTXP (texture heights)			- Not Parsed, Not implemented.

 ADTobj : Not Parsed, Not implemented.

 ADTlod : Not Parsed, Not implemented.

	WDL : Not Parsed, Not implemented.

WMOroot : MOHD (Root Header)			  - Parsed, Partially used. 
		  MOTX (texture paths)			  - Parsed, Implemented.
		  MOMT (materials)				  - Parsed, Partially Implemented.
		  MOUV (texture animation)		  - Parsed, Not implemented.
		  MOGN (group names)			  - Parsed, Implemented.
		  MOGI (group info)				  - Parsed, Partially implemented.
		  MOSB (skybox filename)		  - Parsed, Not implemented.
		  MOPV (portal verts)			  - Parsed, Not implemented.
		  MOPT (portal info)			  - Parsed, Not implemented.
		  MOPR (portal references)		  - Parsed, Not implemented.
		  MOVV (visible block verts)	  - Parsed, Not implemented.
		  MOVB (visible block list)		  - Parsed, Not implemented.
		  MOLT (lighting)				  - Parsed, Not implemented.
		  MODS (doodad sets)			  - Parsed, Not implemented.
		  MODN (M2 filenames)			  - Parsed, Not implemented.
		  MODD (doodad instance info)	  - Parsed, Not implemented.
		  MFOG (fog info)				  - Parsed, Not implemented.
		  MCVP (convex volume planes)	  - Parsed, Not implemented.
		  GFID (fileID wmo load)		  - Parsed, Not implemented.

 WMOgrp : MOGP (group flags)			  - Parsed, Implemented.
			MOPY (material info)		  	- Parsed, Not implemented.
			MOVI (vertex indices)		  	- Parsed, Implemented.
			MOVT (vertices)				  	- Parsed, Implemented.
			MONR (normals)					- Parsed, Implemented.
			MOTV (UVs)						- Parsed, Implemented.
			MOTV2 (UVs)						- Not Parsed, Not implemented.
			MOTV3 (UVs)						- Not Parsed, Not implemented.
			MOBA (render batches)			- Parsed, Implemented.
			MOCV (vertex colors)			- Parsed, Partially used.
			MOCV2 (vertex colors)			- Not Parsed, Not implemented.
			MOLR (light references)			- Not Parsed, Not implemented.
			MODR (doodad references)		- Not Parsed, Not implemented.
			MOBN (BSP tree nodes)			- Not Parsed, Not implemented.
			MOBR (MOBN face indices)		- Not Parsed, Not implemented.
			MLIQ (liquids)					- Not Parsed, Not implemented.
			MORI (triangle strip indices)	- Not Parsed, Not implemented.
			MORB (modifies MOBA)			- Not Parsed, Not implemented.
			MOTA (map object tangents)		- Not Parsed, Not implemented.
			MOBS (map obj shadow batches)	- Not Parsed, Not implemented.
			MDAL (unknown WoD+)				- Not Parsed, Not implemented.
			MOPL (terrain cutting planes)	- Not Parsed, Not implemented.
			MOPB (map obj prepass batches)	- Not Parsed, Not implemented.
			MOLS (map obj spot lights)		- Not Parsed, Not implemented.
			MOLP (map obj point lights)		- Not Parsed, Not implemented.
			MOLM (light map list)			- Not Parsed, Not implemented.
			MOLD (lightmapTexList)			- Not Parsed, Not implemented.

	 M2 : Not Parsed, Not implemented.

      
The main source of information when i comes to extracting and parsing the World of Warcrat data comes from https://wowdev.wiki/Main_Page 
Big thanks to the guys who made and update the wiki.
