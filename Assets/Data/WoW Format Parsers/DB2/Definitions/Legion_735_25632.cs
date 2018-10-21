public static partial class DB2
{
	public static class Definitions_Legion_735_25632
	{
		public class _Achievement
		{
			public string Title;
			public string Description;
			public string Reward;
			public uint Flags;
			public ushort MapID;
			public ushort Supercedes;
			public ushort Category;
			public ushort UIOrder;
			public ushort SharesCriteria;
			public byte Faction;
			public byte Points;
			public byte MinimumCriteria;
			public int ID;
			public uint IconFileDataID;
			public uint CriteriaTree;
		}
		public class _Achievement_category
		{
			public string Field0;
			public ushort Field1;
			public byte Field2;
			public int ID;
		}
		public class _Adventurejournal
		{
			public int ID;
			public string Field01;
			public string Field02;
			public string Field03;
			public string Field04;
			public string Field05;
			public int Field06;
			public int Field07;
			public ushort Field08;
			public ushort Field09;
			public ushort Field0A;
			public ushort[] Field0B = new ushort[2];
			public ushort Field0C;
			public ushort Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
			public byte Field11;
			public byte Field12;
			public byte[] Field13 = new byte[2];
			public byte Field14;
			public int Field15;
			public byte Field16;
		}
		public class _Adventuremappoi
		{
			public int ID;
			public string Field1;
			public string Field2;
			public float[] Field3 = new float[2];
			public int Field4;
			public byte Field5;
			public ushort Field6;
			public int Field7;
			public byte Field8;
			public ushort Field9;
			public byte FieldA;
			public ushort FieldB;
			public int FieldC;
			public ushort FieldD;
		}
		public class _alliedrace
		{
			public int field0;
			public int ID;
			public byte field2;
			public ushort field3;
			public int field4;
			public int field5;
			public int field6;
			public ushort field7;
		}
		public class _Alliedrace
		{
			public int Field0;
			public int ID;
			public byte Field2;
			public ushort Field3;
			public int Field4;
		}
		public class _alliedraceracialability
		{
			public int ID;
			public string field1;
			public string field2;
			public byte field3;
			public int field4;
		}
		public class _Alliedraceracialability
		{
			public int ID;
			public string Field1;
			public string Field2;
			public int Field3;
		}
		public class _Animationdata
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _AnimKit
		{
			public int ID;
			public uint OneShotDuration;
			public ushort OneShotStopAnimKitID;
			public ushort LowDefAnimKitID;
		}
		public class _Animkitboneset
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Animkitbonesetalias
		{
			public int ID;
			public byte Field1;
			public byte Field2;
		}
		public class _Animkitconfig
		{
			public int ID;
			public int Field1;
		}
		public class _Animkitconfigboneset
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _Animkitpriority
		{
			public int ID;
			public byte Field1;
		}
		public class _Animkitreplacement
		{
			public ushort Field0;
			public ushort Field1;
			public ushort Field2;
			public int ID;
		}
		public class _Animkitsegment
		{
			public int ID;
			public int Field01;
			public int Field02;
			public int Field03;
			public float Field04;
			public int Field05;
			public ushort Field06;
			public ushort Field07;
			public ushort Field08;
			public ushort Field09;
			public ushort Field0A;
			public ushort Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
			public byte Field11;
			public int Field12;
		}
		public class _Animreplacement
		{
			public ushort Field0;
			public ushort Field1;
			public ushort Field2;
			public int ID;
		}
		public class _Animreplacementset
		{
			public int ID;
			public byte Field1;
		}
		public class _Areafarclipoverride
		{
			public int Field0;
			public float Field1;
			public float Field2;
			public int Field3;
			public int ID;
		}
		public class _AreaGroupMember
		{
			public int ID;
			public ushort AreaID;
		}
		public class _Areapoi
		{
			public int ID;
			public string Field01;
			public string Field02;
			public int Field03;
			public float[] Field04 = new float[3];
			public int Field05;
			public int Field06;
			public ushort Field07;
			public ushort Field08;
			public ushort Field09;
			public ushort Field0A;
			public byte Field0B;
			public byte Field0C;
			public ushort Field0D;
			public ushort Field0E;
			public int Field0F;
			public int Field10;
		}
		public class _Areapoistate
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
			public int Field4;
		}
		public class _AreaTable
		{
			public int ID;
			public string ZoneName;
			public string AreaName;
			public uint[] Flags = new uint[2];
			public float AmbientMultiplier;
			public ushort MapID;
			public ushort ParentAreaID;
			public short AreaBit;
			public ushort AmbienceID;
			public ushort ZoneMusic;
			public ushort IntroSound;
			public ushort[] LiquidTypeID = new ushort[4];
			public ushort UWZoneMusic;
			public ushort UWAmbience;
			public ushort PvPCombatWorldStateID;
			public byte SoundProviderPref;
			public byte SoundProviderPrefUnderwater;
			public byte ExplorationLevel;
			public byte FactionGroupMask;
			public byte MountFlags;
			public byte WildBattlePetLevelMin;
			public byte WildBattlePetLevelMax;
			public byte WindSettingsID;
			public uint UWIntroSound;
		}
		public class _AreaTrigger
		{
			UnityEngine.Vector3 Unknown;
			public float Radius;
			public float BoxLength;
			public float BoxWidth;
			public float BoxHeight;
			public float BoxYaw;
			public ushort MapID;
			public ushort PhaseID;
			public ushort PhaseGroupID;
			public ushort ShapeID;
			public ushort AreaTriggerActionSetID;
			public byte PhaseUseFlags;
			public byte ShapeType;
			public byte Flag;
			public int ID;
		}
		public class _Areatriggeractionset
		{
			public int ID;
			public ushort Field1;
		}
		public class _Areatriggerbox
		{
			public int ID;
			public float[] Field1 = new float[3];
		}
		public class _Areatriggercylinder
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
		}
		public class _Areatriggersphere
		{
			public int ID;
			public float Field1;
		}
		public class _ArmorLocation
		{
			public int ID;
			public float[] Modifier = new float[5];
		}
		public class _Artifact
		{
			public int ID;
			public string Name;
			public uint BarConnectedColor;
			public uint BarDisconnectedColor;
			public uint TitleColor;
			public ushort ClassUiTextureKitID;
			public ushort SpecID;
			public byte ArtifactCategoryID;
			public byte Flags;
			public uint UiModelSceneID;
			public uint SpellVisualKitID;
		}
		public class _ArtifactAppearance
		{
			public string Name;
			public uint SwatchColor;
			public float ModelDesaturation;
			public float ModelAlpha;
			public uint ShapeshiftDisplayID;
			public ushort ArtifactAppearanceSetID;
			public ushort Unknown;
			public byte DisplayIndex;
			public byte AppearanceModID;
			public byte Flags;
			public byte ModifiesShapeshiftFormDisplay;
			public int ID;
			public uint PlayerConditionID;
			public uint ItemAppearanceID;
			public uint AltItemAppearanceID;
		}
		public class _ArtifactAppearanceSet
		{
			public string Name;
			public string Name2;
			public ushort UiCameraID;
			public ushort AltHandUICameraID;
			public byte DisplayIndex;
			public byte AttachmentPoint;
			public byte Flags;
			public int ID;
		}
		public class _ArtifactCategory
		{
			public int ID;
			public ushort ArtifactKnowledgeCurrencyID;
			public ushort ArtifactKnowledgeMultiplierCurveID;
		}
		public class _ArtifactPower
		{
			public float[] Pos = new float[2];
			public byte ArtifactID;
			public byte Flags;
			public byte MaxRank;
			public byte ArtifactTier;
			public int ID;
			public int RelicType;
		}
		public class _ArtifactPowerLink
		{
			public int ID;
			public ushort FromArtifactPowerID;
			public ushort ToArtifactPowerID;
		}
		public class _ArtifactPowerPicker
		{
			public int ID;
			public uint PlayerConditionID;
		}
		public class _ArtifactPowerRank
		{
			public int ID;
			public uint SpellID;
			public float Value;
			public ushort Unknown;
			public byte Rank;
		}
		public class _ArtifactQuestXP
		{
			public int ID;
			public uint[] Exp = new uint[10];
		}
		public class _Artifacttier
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Artifactunlock
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public int Field3;
			public ushort Field4;
		}
		public class _AuctionHouse
		{
			public int ID;
			public string Name;
			public ushort FactionID;
			public byte DepositRate;
			public byte ConsignmentRate;
		}
		public class _BankBagSlotPrices
		{
			public int ID;
			public uint Cost;
		}
		public class _BannedAddons
		{
			public int ID;
			public string Name;
			public string Version;
			public byte Flags;
		}
		public class _BarberShopStyle
		{
			public string DisplayName;
			public string Description;
			public float CostModifier;
			public byte Type;
			public byte Race;
			public byte Sex;
			public byte Data;
			public int ID;
		}
		public class _BattlemasterList
		{
			public int ID;
			public string Name;
			public string GameType;
			public string ShortDescription;
			public string LongDescription;
			public uint IconFileDataID;
			public ushort[] MapID = new ushort[16];
			public ushort HolidayWorldState;
			public ushort PlayerConditionID;
			public byte InstanceType;
			public byte GroupsAllowed;
			public byte MaxGroupSize;
			public byte MinLevel;
			public byte MaxLevel;
			public byte RatedPlayers;
			public byte MinPlayers;
			public byte MaxPlayers;
			public byte Flags;
		}
		public class _Battlepetability
		{
			public int ID;
			public string Field1;
			public string Field2;
			public int Field3;
			public ushort Field4;
			public byte Field5;
			public byte Field6;
			public int Field7;
		}
		public class _Battlepetabilityeffect
		{
			public ushort Field0;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort[] Field4 = new ushort[6];
			public byte Field5;
			public int ID;
		}
		public class _Battlepetabilitystate
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _Battlepetabilityturn
		{
			public ushort Field0;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public int ID;
		}
		public class _BattlePetBreedQuality
		{
			public int ID;
			public float Modifier;
			public byte Quality;
		}
		public class _BattlePetBreedState
		{
			public int ID;
			public short Value;
			public byte State;
		}
		public class _Battlepetdisplayoverride
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public byte Field4;
		}
		public class _Battlepeteffectproperties
		{
			public int ID;
			public string[] Field1 = new string[6];
			public ushort Field2;
			public byte[] Field3 = new byte[6];
		}
		public class _BattlePetSpecies
		{
			public string SourceText;
			public string Description;
			public uint CreatureID;
			public uint IconFileID;
			public uint SummonSpellID;
			public ushort Flags;
			public byte PetType;
			public byte Source;
			public int ID;
			public uint CardModelSceneID;
			public uint LoadoutModelSceneID;
		}
		public class _BattlePetSpeciesState
		{
			public int ID;
			public int Value;
			public byte State;
		}
		public class _Battlepetspeciesxability
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Battlepetstate
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Battlepetvisual
		{
			public int ID;
			public string Field1;
			public int Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Beameffect
		{
			public int ID;
			public int Field1;
			public float Field2;
			public float Field3;
			public int Field4;
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public ushort Field8;
			public ushort Field9;
			public ushort FieldA;
		}
		public class _Bonewindmodifiermodel
		{
			public int ID;
			public int Field1;
			public int Field2;
		}
		public class _Bonewindmodifiers
		{
			public int ID;
			public float[] Field1 = new float[3];
			public float Field2;
		}
		public class _Bounty
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
		}
		public class _Bountyset
		{
			public int ID;
			public ushort Field1;
			public int Field2;
		}
		public class _BroadcastText
		{
			public int ID;
			public string MaleText;
			public string FemaleText;
			public ushort[] EmoteID = new ushort[3];
			public ushort[] EmoteDelay = new ushort[3];
			public ushort UnkEmoteID;
			public byte Language;
			public byte Type;
			public uint PlayerConditionID;
			public uint[] SoundID = new uint[2];
		}
		public class _Cameraeffect
		{
			public int ID;
			public byte Field1;
		}
		public class _Cameraeffectentry
		{
			public int ID;
			public float Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public float Field05;
			public float Field06;
			public float Field07;
			public float Field08;
			public ushort Field09;
			public byte Field0A;
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public byte Field0F;
		}
		public class _Cameramode
		{
			public int ID;
			public float[] Field1 = new float[3];
			public float[] Field2 = new float[3];
			public float Field3;
			public float Field4;
			public float Field5;
			public ushort Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public byte FieldB;
		}
		public class _Castableraidbuffs
		{
			public int ID;
			public ushort Field1;
		}
		public class _Celestialbody
		{
			public int Field0;
			public int Field1;
			public int[] Field2 = new int[2];
			public int Field3;
			public int Field4;
			public int[] Field5 = new int[2];
			public float[] Field6 = new float[2];
			public float[] Field7 = new float[2];
			public float Field8;
			public float[] Field9 = new float[2];
			public float FieldA;
			public float[] FieldB = new float[3];
			public float FieldC;
			public ushort FieldD;
			public int ID;
		}
		public class _Cfg_categories
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Cfg_configs
		{
			public int ID;
			public float Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Cfg_regions
		{
			public int ID;
			public string Field1;
			public int Field2;
			public float Field3;
			public ushort Field4;
			public byte Field5;
		}
		public class _Characterfaceboneset
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _CharacterFacialHairStyles
		{
			public int ID;
			public uint[] Geoset = new uint[5];
			public byte RaceID;
			public byte SexID;
			public byte VariationID;
		}
		public class _Characterloadout
		{
			public int ID;
			public ulong Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Characterloadoutitem
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _Characterserviceinfo
		{
			public int ID;
			public string Field1;
			public string Field2;
			public string Field3;
			public string Field4;
			public int Field5;
			public int Field6;
			public string Field7;
			public int Field8;
			public int Field9;
			public int FieldA;
			public int FieldB;
		}
		public class _Charbaseinfo
		{
			public int ID;
			public byte Field1;
			public byte Field2;
		}
		public class _CharBaseSection
		{
			public int ID;
			public byte Variation;
			public byte ResolutionVariation;
			public byte Resolution;
		}
		public class _Charcomponenttexturelayouts
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _Charcomponenttexturesections
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Charhairgeosets
		{
			public int ID;
			public int RootGeosetID;
			public byte Race;
			public byte Sex;
			public byte CharSection_ID;
			public byte Flag;
			public byte Expansion;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public int FieldA;
		}
		public class _CharSections
		{
			public int ID;
			public uint[] TextureFileDataID = new uint[3];
			public ushort Flags;
			public byte RaceID;
			public byte SexID;
			public byte BaseSection;
			public byte VariationIndex;
			public byte ColorIndex;
		}
		public class _Charshipment
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public ushort Field6;
			public ushort Field7;
			public byte Field8;
			public byte Field9;
		}
		public class _Charshipmentcontainer
		{
			public int ID;
			public string Field01;
			public string Field02;
			public int Field03;
			public ushort Field04;
			public ushort Field05;
			public ushort Field06;
			public ushort Field07;
			public ushort Field08;
			public ushort Field09;
			public byte Field0A;
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
		}
		public class _CharStartOutfit
		{
			public int ID;
			public int[] ItemID = new int[24];
			public uint PetDisplayID;
			public byte ClassID;
			public byte GenderID;
			public byte OutfitID;
			public byte PetFamilyID;
		}
		public class _CharTitles
		{
			public int ID;
			public string NameMale;
			public string NameFemale;
			public ushort MaskID;
			public byte Flags;
		}
		public class _ChatChannels
		{
			public int ID;
			public string Name;
			public string Shortcut;
			public uint Flags;
			public byte FactionGroup;
		}
		public class _Chatprofanity
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _ChrClasses
		{
			public string PetNameToken;
			public string Name;
			public string NameFemale;
			public string NameMale;
			public string Filename;
			public uint CreateScreenFileDataID;
			public uint SelectScreenFileDataID;
			public uint IconFileDataID;
			public uint LowResScreenFileDataID;
			public uint StartingLevel;
			public ushort Flags;
			public ushort CinematicSequenceID;
			public ushort DefaultSpec;
			public byte PowerType;
			public byte SpellClassSet;
			public byte AttackPowerPerStrength;
			public byte AttackPowerPerAgility;
			public byte RangedAttackPowerPerAgility;
			public byte Unk1;
			public int ID;
		}
		public class _ChrClassesXPowerTypes
		{
			public int ID;
			public byte PowerType;
		}
		public class _Chrclassracesex
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
			public byte Field6;
		}
		public class _Chrclasstitle
		{
			public int ID;
			public string Field1;
			public int Field2;
			public byte Field3;
		}
		public class _Chrclassuidisplay
		{
			public int ID;
			public byte Field1;
			public ushort Field2;
			public int Field3;
		}
		public class _Chrclassvillain
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Chrcustomization
		{
			public int ID;
			public string Field1;
			public int Field2;
			public byte Field3;
			public byte Field4;
			public int[] Field5 = new int[3];
		}
		public class _ChrRaces
		{
			public string ClientPrefix;
			public string ClientFileString;
			public string Name;
			public string NameFemale;
			public string LowercaseName;
			public string LowercaseNameFemale;
			public uint Flags;
			public uint MaleDisplayID;
			public uint FemaleDisplayID;
			public uint CreateScreenFileDataID;
			public uint SelectScreenFileDataID;
			public float[] MaleCustomizeOffset = new float[3];
			public float[] FemaleCustomizeOffset = new float[3];
			public uint LowResScreenFileDataID;
			public uint StartingLevel;
			public uint UIDisplayOrder;
			public ushort FactionID;
			public ushort ResSicknessSpellID;
			public ushort SplashSoundID;
			public ushort CinematicSequenceID;
			public byte BaseLanguage;
			public byte CreatureType;
			public byte TeamID;
			public byte RaceRelated;
			public byte UnalteredVisualRaceID;
			public byte CharComponentTextureLayoutID;
			public byte DefaultClassID;
			public byte NeutralRaceID;
			public byte ItemAppearanceFrameRaceID;
			public byte CharComponentTexLayoutHiResID;
			public int ID;
			public uint HighResMaleDisplayID;
			public uint HighResFemaleDisplayID;
			public uint HeritageArmorAchievementID;
			public uint MaleCorpseBonesModelFileDataID;
			public uint FemaleCorpseBonesModelFileDataID;
			public uint[] AlteredFormTransitionSpellVisualID = new uint[3];
			public uint[] AlteredFormTransitionSpellVisualKitID = new uint[3];
		}
		public class _ChrSpecialization
		{
			public string Name;
			public string Name2;
			public string Description;
			public uint[] MasterySpellID = new uint[2];
			public byte ClassID;
			public byte OrderIndex;
			public byte PetTalentType;
			public byte Role;
			public byte PrimaryStatOrder;
			public int ID;
			public uint IconFileDataID;
			public uint Flags;
			public uint AnimReplacementSetID;
		}
		public class _Chrupgradebucket
		{
			public ushort Field0;
			public int ID;
		}
		public class _Chrupgradebucketspell
		{
			public int ID;
			public int Field1;
		}
		public class _Chrupgradetier
		{
			public int Field0;
			public byte Field1;
			public byte Field2;
			public int ID;
		}
		public class _CinematicCamera
		{
			public int ID;
			public uint SoundID;
			public float[] Origin = new float[3];
			public float OriginFacing;
			public uint ModelFileDataID;
		}
		public class _CinematicSequences
		{
			public int ID;
			public uint SoundID;
			public ushort[] Camera = new ushort[8];
		}
		public class _Cloakdampening
		{
			public int ID;
			public float[] Field1 = new float[5];
			public float[] Field2 = new float[5];
			public int[] Field3 = new int[2];
			public float[] Field4 = new float[2];
			public float Field5;
			public float Field6;
			public float Field7;
		}
		public class _Combatcondition
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort[] Field4 = new ushort[2];
			public ushort[] Field5 = new ushort[2];
			public byte[] Field6 = new byte[2];
			public byte[] Field7 = new byte[2];
			public byte Field8;
			public byte[] Field9 = new byte[2];
			public byte[] FieldA = new byte[2];
			public byte FieldB;
		}
		public class _Commentatorstartlocation
		{
			public int ID;
			public float[] Field1 = new float[3];
			public ushort Field2;
		}
		public class _Commentatortrackedcooldown
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public int Field3;
		}
		public class _Componentmodelfiledata
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Componenttexturefiledata
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Configurationwarning
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _Contribution
		{
			public string Field0;
			public string Field1;
			public int ID;
			public int Field3;
			public int[] Field4 = new int[4];
			public byte Field5;
		}
		public class _ConversationLine
		{
			public int ID;
			public uint BroadcastTextID;
			public uint SpellVisualKitID;
			public uint Duration;
			public ushort NextLineID;
			public ushort Unk1;
			public byte Yell;
			public byte Unk2;
			public byte Unk3;
		}
		public class _Creature
		{
			public int ID;
			public string Field1;
			public string Field2;
			public string Field3;
			public string Field4;
			public int[] Field5 = new int[3];
			public int Field6;
			public int[] Field7 = new int[4];
			public float[] Field8 = new float[4];
			public byte Field9;
			public byte FieldA;
			public byte FieldB;
			public byte FieldC;
		}
		public class _Creaturedifficulty
		{
			public int ID;
			public int[] Field1 = new int[7];
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _CreatureDisplayInfo
		{
			public int ID;
			public float CreatureModelScale;
			public ushort ModelID;
			public ushort NPCSoundID;
			public byte SizeClass;
			public byte Flags;
			public byte Gender;
			public uint ExtendedDisplayInfoID;
			public uint PortraitTextureFileDataID;
			public byte CreatureModelAlpha;
			public ushort SoundID;
			public float PlayerModelScale;
			public uint PortraitCreatureDisplayInfoID;
			public byte BloodID;
			public ushort ParticleColorID;
			public uint CreatureGeosetData;
			public ushort ObjectEffectPackageID;
			public ushort AnimReplacementSetID;
			public byte UnarmedWeaponSubclass;
			public uint StateSpellVisualKitID;
			public float InstanceOtherPlayerPetScale;
			public uint MountSpellVisualKitID;
			public uint[] TextureVariation = new uint[3];
		}
		public class _SDReplacementModel
		{
			public int ID;
			public int SdFileDataId;
		}
		public class _Creaturedisplayinfocond
		{
			public int ID;
			public ulong Field1;
			public int[] Field2 = new int[2];
			public int[] Field3 = new int[2];
			public int[] Field4 = new int[2];
			public byte Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public ushort Field9;
			public byte FieldA;
			public byte FieldB;
			public byte FieldC;
			public ushort FieldD;
			public int[] FieldE = new int[3];
		}
		public class _Creaturedisplayinfoevt
		{
			public int ID;
			public int Field1;
			public int Field2;
			public byte Field3;
		}
		public class _CreatureDisplayInfoExtra
		{
			public int ID;
			public uint FileDataID;
			public uint HDFileDataID;
			public byte DisplayRaceID;
			public byte DisplaySexID;
			public byte DisplayClassID;
			public byte SkinID;
			public byte FaceID;
			public byte HairStyleID;
			public byte HairColorID;
			public byte FacialHairID;
			public byte[] CustomDisplayOption = new byte[3];
			public byte Flags;
		}
		public class _Creaturedisplayinfotrn
		{
			public int ID;
			public int Field1;
			public float Field2;
			public int Field3;
			public uint Field4;
			public int Field5;
		}
		public class _Creaturedispxuicamera
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _CreatureFamily
		{
			public int ID;
			public string Name;
			public float MinScale;
			public float MaxScale;
			public uint IconFileDataID;
			public ushort[] SkillLine = new ushort[2];
			public ushort PetFoodMask;
			public byte MinScaleLevel;
			public byte MaxScaleLevel;
			public byte PetTalentType;
		}
		public class _Creatureimmunities
		{
			public int ID;
			public int[] Field1 = new int[2];
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
			public int[] Field8 = new int[8];
			public int[] Field9 = new int[16];
		}
		public class _CreatureModelData
		{
			public int ID;
			public float ModelScale;
			public float FootprintTextureLength;
			public float FootprintTextureWidth;
			public float FootprintParticleScale;
			public float CollisionWidth;
			public float CollisionHeight;
			public float MountHeight;
			public float[] GeoBoxMin = new float[3];
			public float[] GeoBoxMax = new float[3];
			public float WorldEffectScale;
			public float AttachedEffectScale;
			public float MissileCollisionRadius;
			public float MissileCollisionPush;
			public float MissileCollisionRaise;
			public float OverrideLootEffectScale;
			public float OverrideNameScale;
			public float OverrideSelectionRadius;
			public float TamedPetBaseScale;
			public float HoverHeight;
			public uint Flags;
			public uint FileDataID;
			public uint SizeClass;
			public uint BloodID;
			public uint FootprintTextureID;
			public uint FoleyMaterialID;
			public uint FootstepEffectID;
			public uint DeathThudEffectID;
			public uint SoundID;
			public uint CreatureGeosetDataID;
		}
		public class _Creaturemovementinfo
		{
			public int ID;
			public float Field1;
		}
		public class _Creaturesounddata
		{
			public int ID;
			public float Field01;
			public float Field02;
			public byte Field03;
			public uint Field04;
			public int Field05;
			public uint Field06;
			public uint Field07;
			public int Field08;
			public uint Field09;
			public uint Field0A;
			public uint Field0B;
			public uint Field0C;
			public uint Field0D;
			public uint Field0E;
			public uint Field0F;
			public uint Field10;
			public uint Field11;
			public uint Field12;
			public uint Field13;
			public uint Field14;
			public uint Field15;
			public uint Field16;
			public uint Field17;
			public uint Field18;
			public uint Field19;
			public uint Field1A;
			public uint Field1B;
			public uint Field1C;
			public uint Field1D;
			public uint Field1E;
			public uint Field1F;
			public uint Field20;
			public uint Field21;
			public uint Field22;
			public uint Field23;
			public uint[] Field24 = new uint[5];
			public uint[] Field25 = new uint[4];
		}
		public class _CreatureType
		{
			public int ID;
			public string Name;
			public byte Flags;
		}
		public class _Creaturexcontribution
		{
			public int ID;
			public int Field1;
		}
		public class _Criteria
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public ushort Field5;
			public ushort Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public byte FieldB;
		}
		public class _CriteriaTree
		{
			public int ID;
			public string Description;
			public uint Amount;
			public ushort Flags;
			public byte Operator;
			public uint CriteriaID;
			public uint Parent;
			public int OrderIndex;
		}
		public class _Criteriatreexeffect
		{
			public int ID;
			public ushort Field1;
		}
		public class _Currencycategory
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _CurrencyTypes
		{
			public int ID;
			public string Name;
			public string Description;
			public uint MaxQty;
			public uint MaxEarnablePerWeek;
			public uint Flags;
			public byte CategoryID;
			public byte SpellCategory;
			public byte Quality;
			public uint InventoryIconFileDataID;
			public uint SpellWeight;
		}
		public class _Curve
		{
			public int ID;
			public byte Type;
			public byte Unused;
		}
		public class _CurvePoint
		{
			public int ID;
			public float X;
			public float Y;
			public ushort CurveID;
			public byte Index;
		}
		public class _DBCache
		{
			public int ID;
		}
		public class _Deaththudlookups
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public ushort Field3;
			public int Field4;
		}
		public class _Decalproperties
		{
			public int ID;
			public int Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public float Field05;
			public float Field06;
			public float Field07;
			public float Field08;
			public float Field09;
			public byte Field0A;
			public byte Field0B;
			public ushort Field0C;
			public ushort Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
		}
		public class _DestructibleModelData
		{
			public int ID;
			public ushort StateDamagedDisplayID;
			public ushort StateDestroyedDisplayID;
			public ushort StateRebuildingDisplayID;
			public ushort StateSmokeDisplayID;
			public ushort HealEffectSpeed;
			public byte StateDamagedImpactEffectDoodadSet;
			public byte StateDamagedAmbientDoodadSet;
			public byte StateDamagedNameSet;
			public byte StateDestroyedDestructionDoodadSet;
			public byte StateDestroyedImpactEffectDoodadSet;
			public byte StateDestroyedAmbientDoodadSet;
			public byte StateDestroyedNameSet;
			public byte StateRebuildingDestructionDoodadSet;
			public byte StateRebuildingImpactEffectDoodadSet;
			public byte StateRebuildingAmbientDoodadSet;
			public byte StateRebuildingNameSet;
			public byte StateSmokeInitDoodadSet;
			public byte StateSmokeAmbientDoodadSet;
			public byte StateSmokeNameSet;
			public byte EjectDirection;
			public byte DoNotHighlight;
			public byte HealEffect;
		}
		public class _Deviceblacklist
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _Devicedefaultsettings
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public byte Field3;
		}
		public class _Difficulty
		{
			public int ID;
			public string Name;
			public ushort GroupSizeHealthCurveID;
			public ushort GroupSizeDmgCurveID;
			public ushort GroupSizeSpellPointsCurveID;
			public byte FallbackDifficultyID;
			public byte InstanceType;
			public byte MinPlayers;
			public byte MaxPlayers;
			public byte OldEnumValue;
			public byte Flags;
			public byte ToggleDifficultyID;
			public byte ItemBonusTreeModID;
			public byte OrderIndex;
		}
		public class _Dissolveeffect
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public byte Field9;
			public byte FieldA;
			public ushort FieldB;
			public int FieldC;
			public int FieldD;
			public byte FieldE;
		}
		public class _Driverblacklist
		{
			public int ID;
			public int Field1;
			public int Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _DungeonEncounter
		{
			public string Name;
			public uint CreatureDisplayID;
			public ushort MapID;
			public byte DifficultyID;
			public byte Bit;
			public byte Flags;
			public int ID;
			public int OrderIndex;
			public uint TextureFileDataID;
		}
		public class _Dungeonmap
		{
			public float[] Field0 = new float[2];
			public float[] Field1 = new float[2];
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public int ID;
		}
		public class _Dungeonmapchunk
		{
			public int ID;
			public float Field1;
			public int Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
		}
		public class _DurabilityCosts
		{
			public int ID;
			public ushort[] WeaponSubClassCost = new ushort[21];
			public ushort[] ArmorSubClassCost = new ushort[8];
		}
		public class _DurabilityQuality
		{
			public int ID;
			public float QualityMod;
		}
		public class _Edgegloweffect
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public float Field9;
			public float FieldA;
			public byte FieldB;
			public ushort FieldC;
			public int FieldD;
		}
		public class _Emotes
		{
			public int ID;
			public long RaceMask;
			public string EmoteSlashCommand;
			public uint SpellVisualKitID;
			public uint EmoteFlags;
			public ushort AnimID;
			public byte EmoteSpecProc;
			public uint EmoteSpecProcParam;
			public uint EmoteSoundID;
			public int ClassMask;
		}
		public class _EmotesText
		{
			public int ID;
			public string Name;
			public ushort EmoteID;
		}
		public class _Emotestextdata
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _EmotesTextSound
		{
			public int ID;
			public byte RaceId;
			public byte SexId;
			public byte ClassId;
			public uint SoundId;
		}
		public class _Environmentaldamage
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _Exhaustion
		{
			public string Field0;
			public string Field1;
			public int Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public int ID;
		}
		public class _Faction
		{
			public ulong[] ReputationRaceMask = new ulong[4];
			public string Name;
			public string Description;
			public int ID;
			public int[] ReputationBase = new int[4];
			public float ParentFactionModIn;
			public float ParentFactionModOut;
			public uint[] ReputationMax = new uint[4];
			public short ReputationIndex;
			public ushort[] ReputationClassMask = new ushort[4];
			public ushort[] ReputationFlags = new ushort[4];
			public ushort ParentFactionID;
			public ushort ParagonFactionID;
			public byte ParentFactionCapIn;
			public byte ParentFactionCapOut;
			public byte Expansion;
			public byte Flags;
			public byte FriendshipRepID;
		}
		public class _Factiongroup
		{
			public string Field0;
			public string Field1;
			public int ID;
			public byte Field3;
			public int Field4;
			public int Field5;
		}
		public class _FactionTemplate
		{
			public int ID;
			public ushort Faction;
			public ushort Flags;
			public ushort[] Enemies = new ushort[4];
			public ushort[] Friends = new ushort[4];
			public byte Mask;
			public byte FriendMask;
			public byte EnemyMask;
		}
		public class _Footprinttextures
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public int Field3;
		}
		public class _Footstepterrainlookup
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public uint Field3;
			public uint Field4;
		}
		public class _Friendshiprepreaction
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
		}
		public class _Friendshipreputation
		{
			public string Field0;
			public int Field1;
			public ushort Field2;
			public int ID;
		}
		public class _Fullscreeneffect
		{
			public int ID;
			public float Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public int Field05;
			public float Field06;
			public float Field07;
			public float Field08;
			public float Field09;
			public float Field0A;
			public int Field0B;
			public float Field0C;
			public float Field0D;
			public float Field0E;
			public float Field0F;
			public float Field10;
			public int Field11;
			public float Field12;
			public float Field13;
			public int Field14;
			public int Field15;
			public float Field16;
			public float Field17;
			public byte Field18;
			public ushort Field19;
			public ushort Field1A;
			public int Field1B;
		}
		public class _Gameobjectartkit
		{
			public int ID;
			public int Field1;
			public int[] Field2 = new int[3];
		}
		public class _Gameobjectdiffanimmap
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _GameObjectDisplayInfo
		{
			public int ID;
			public uint FileDataID;
			public float[] GeoBoxMin = new float[3];
			public float[] GeoBoxMax = new float[3];
			public float OverrideLootEffectScale;
			public float OverrideNameScale;
			public ushort ObjectEffectPackageID;
		}
		public class _Gameobjectdisplayinfoxsoundkit
		{
			public int ID;
			public byte Field1;
			public uint Field2;
		}
		public class _GameObjects
		{
			public string Name;
			public float[] Position = new float[3];
			public float RotationX;
			public float RotationY;
			public float RotationZ;
			public float RotationW;
			public float Size;
			public int[] Data = new int[8];
			public ushort MapID;
			public ushort DisplayID;
			public ushort PhaseID;
			public ushort PhaseGroupID;
			public byte PhaseUseFlags;
			public byte Type;
			public int ID;
		}
		public class _Gametips
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _GarrAbility
		{
			public string Name;
			public string Description;
			public uint IconFileDataID;
			public ushort Flags;
			public ushort OtherFactionGarrAbilityID;
			public byte GarrAbilityCategoryID;
			public byte FollowerTypeID;
			public int ID;
		}
		public class _Garrabilitycategory
		{
			public int ID;
			public string Field1;
		}
		public class _Garrabilityeffect
		{
			public float Field0;
			public float Field1;
			public float Field2;
			public int Field3;
			public ushort Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public int ID;
		}
		public class _GarrBuilding
		{
			public int ID;
			public string NameAlliance;
			public string NameHorde;
			public string Description;
			public string Tooltip;
			public uint HordeGameObjectID;
			public uint AllianceGameObjectID;
			public uint IconFileDataID;
			public ushort CostCurrencyID;
			public ushort HordeTexPrefixKitID;
			public ushort AllianceTexPrefixKitID;
			public ushort AllianceActivationScenePackageID;
			public ushort HordeActivationScenePackageID;
			public ushort FollowerRequiredGarrAbilityID;
			public ushort FollowerGarrAbilityEffectID;
			public short CostMoney;
			public byte Unknown;
			public byte Type;
			public byte Level;
			public byte Flags;
			public byte MaxShipments;
			public byte GarrTypeID;
			public int BuildDuration;
			public int CostCurrencyAmount;
			public int BonusAmount;
		}
		public class _Garrbuildingdoodadset
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _GarrBuildingPlotInst
		{
			public float[] LandmarkOffset = new float[2];
			public ushort UiTextureAtlasMemberID;
			public ushort GarrSiteLevelPlotInstID;
			public byte GarrBuildingID;
			public int ID;
		}
		public class _GarrClassSpec
		{
			public string NameMale;
			public string NameFemale;
			public string NameGenderless;
			public ushort ClassAtlasID;
			public ushort GarrFollItemSetID;
			public byte Limit;
			public byte Flags;
			public int ID;
		}
		public class _Garrclassspecplayercond
		{
			public int ID;
			public string Field1;
			public int Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
			public byte Field6;
		}
		public class _Garrencounter
		{
			public string Field0;
			public int Field1;
			public float Field2;
			public float Field3;
			public int Field4;
			public int ID;
			public byte Field6;
		}
		public class _Garrencountersetxencounter
		{
			public int ID;
			public ushort Field1;
		}
		public class _Garrencounterxmechanic
		{
			public int ID;
			public byte Field1;
			public byte Field2;
		}
		public class _Garrfollitemsetmember
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public byte Field3;
		}
		public class _GarrFollower
		{
			public string HordeSourceText;
			public string AllianceSourceText;
			public string Name;
			public uint HordeCreatureID;
			public uint AllianceCreatureID;
			public uint HordePortraitIconID;
			public uint AlliancePortraitIconID;
			public uint HordeAddedBroadcastTextID;
			public uint AllianceAddedBroadcastTextID;
			public ushort HordeGarrFollItemSetID;
			public ushort AllianceGarrFollItemSetID;
			public ushort ItemLevelWeapon;
			public ushort ItemLevelArmor;
			public ushort HordeListPortraitTextureKitID;
			public ushort AllianceListPortraitTextureKitID;
			public byte FollowerTypeID;
			public byte HordeUiAnimRaceInfoID;
			public byte AllianceUiAnimRaceInfoID;
			public byte Quality;
			public byte HordeGarrClassSpecID;
			public byte AllianceGarrClassSpecID;
			public byte Level;
			public byte Unknown1;
			public byte Flags;
			public byte Unknown2;
			public byte Unknown3;
			public byte GarrTypeID;
			public byte MaxDurability;
			public byte Class;
			public byte HordeFlavorTextGarrStringID;
			public byte AllianceFlavorTextGarrStringID;
			public int ID;
		}
		public class _Garrfollowerlevelxp
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Garrfollowerquality
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Garrfollowertype
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Garrfolloweruicreature
		{
			public int ID;
			public int Field1;
			public float Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _GarrFollowerXAbility
		{
			public int ID;
			public ushort GarrAbilityID;
			public byte FactionIndex;
		}
		public class _Garrfollsupportspell
		{
			public int ID;
			public int Field1;
			public int Field2;
			public byte Field3;
		}
		public class _Garritemlevelupgradedata
		{
			public int ID;
			public byte Field1;
			public int Field2;
			public ushort Field3;
			public int Field4;
		}
		public class _Garrmechanic
		{
			public int ID;
			public float Field1;
			public byte Field2;
			public ushort Field3;
		}
		public class _Garrmechanicsetxmechanic
		{
			public byte Field0;
			public int ID;
		}
		public class _Garrmechanictype
		{
			public string Field0;
			public string Field1;
			public int Field2;
			public byte Field3;
			public int ID;
		}
		public class _Garrmission
		{
			public string Field00;
			public string Field01;
			public string Field02;
			public int Field03;
			public int Field04;
			public float[] Field05 = new float[2];
			public float[] Field06 = new float[2];
			public ushort Field07;
			public ushort Field08;
			public ushort Field09;
			public byte Field0A;
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
			public byte Field11;
			public byte Field12;
			public int ID;
			public uint Field14;
			public uint Field15;
			public uint Field16;
			public uint Field17;
			public uint Field18;
			public uint Field19;
			public uint Field1A;
			public uint Field1B;
		}
		public class _Garrmissiontexture
		{
			public int ID;
			public float[] Field1 = new float[2];
			public ushort Field2;
		}
		public class _Garrmissiontype
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Garrmissionxencounter
		{
			public byte Field0;
			public int ID;
			public ushort Field2;
			public byte Field3;
		}
		public class _Garrmissionxfollower
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _Garrmssnbonusability
		{
			public int ID;
			public float Field1;
			public int Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _GarrPlot
		{
			public int ID;
			public string Name;
			public uint AllianceConstructionGameObjectID;
			public uint HordeConstructionGameObjectID;
			public byte GarrPlotUICategoryID;
			public byte PlotType;
			public byte Flags;
			public uint MinCount;
			public uint MaxCount;
		}
		public class _GarrPlotBuilding
		{
			public int ID;
			public byte GarrPlotID;
			public byte GarrBuildingID;
		}
		public class _GarrPlotInstance
		{
			public int ID;
			public string Name;
			public byte GarrPlotID;
		}
		public class _Garrplotuicategory
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _GarrSiteLevel
		{
			public int ID;
			public float[] TownHall = new float[2];
			public ushort MapID;
			public ushort SiteID;
			public ushort MovieID;
			public ushort UpgradeResourceCost;
			public ushort UpgradeMoneyCost;
			public byte Level;
			public byte UITextureKitID;
			public byte Level2;
		}
		public class _GarrSiteLevelPlotInst
		{
			public int ID;
			public float[] Landmark = new float[2];
			public ushort GarrSiteLevelID;
			public byte GarrPlotInstanceID;
			public byte Unknown;
		}
		public class _Garrspecialization
		{
			public int ID;
			public string Field1;
			public string Field2;
			public int Field3;
			public float[] Field4 = new float[2];
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Garrstring
		{
			public int ID;
			public string Field1;
		}
		public class _Garrtalent
		{
			public string Field00;
			public string Field01;
			public int Field02;
			public int Field03;
			public byte Field04;
			public byte Field05;
			public byte Field06;
			public int ID;
			public int Field08;
			public int Field09;
			public int Field0A;
			public int Field0B;
			public int Field0C;
			public int Field0D;
			public int Field0E;
			public int Field0F;
			public int Field10;
			public int Field11;
			public int Field12;
			public int Field13;
		}
		public class _Garrtalenttree
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Garrtype
		{
			public int ID;
			public byte Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
			public int[] Field5 = new int[2];
		}
		public class _Garruianimclassinfo
		{
			public int ID;
			public float Field1;
			public byte Field2;
			public byte Field3;
			public int Field4;
			public int Field5;
			public int Field6;
		}
		public class _Garruianimraceinfo
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public float Field9;
			public float FieldA;
			public float FieldB;
			public float FieldC;
			public byte FieldD;
		}
		public class _GemProperties
		{
			public int ID;
			public uint Type;
			public ushort EnchantID;
			public ushort MinItemLevel;
		}
		public class _Globalstrings
		{
			public int ID;
			public string Field1;
			public string Field2;
			public byte Field3;
		}
		public class _GlyphBindableSpell
		{
			public int ID;
			public uint SpellID;
		}
		public class _Glyphexclusivecategory
		{
			public int ID;
			public string Field1;
		}
		public class _GlyphProperties
		{
			public int ID;
			public uint SpellID;
			public ushort SpellIconID;
			public byte Type;
			public byte GlyphExclusiveCategoryID;
		}
		public class _GlyphRequiredSpec
		{
			public int ID;
			public ushort ChrSpecializationID;
		}
		public class _Gmsurveyanswers
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _Gmsurveycurrentsurvey
		{
			public int ID;
			public byte Field1;
		}
		public class _Gmsurveyquestions
		{
			public int ID;
			public string Field1;
		}
		public class _Gmsurveysurveys
		{
			public int ID;
			public byte[] Field1 = new byte[15];
		}
		public class _Groundeffectdoodad
		{
			public int ID;
			public float Field1;
			public float Field2;
			public byte Field3;
			public int Field4;
		}
		public class _Groundeffecttexture
		{
			public int ID;
			public ushort[] Field1 = new ushort[4];
			public byte[] Field2 = new byte[4];
			public byte Field3;
			public byte Field4;
		}
		public class _Groupfinderactivity
		{
			public int ID;
			public string Field1;
			public string Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public byte FieldB;
			public byte FieldC;
			public byte FieldD;
			public byte FieldE;
		}
		public class _Groupfinderactivitygrp
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _Groupfindercategory
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _GuildColorBackground
		{
			public int ID;
			public byte Red;
			public byte Green;
			public byte Blue;
		}
		public class _GuildColorBorder
		{
			public int ID;
			public byte Red;
			public byte Green;
			public byte Blue;
		}
		public class _GuildColorEmblem
		{
			public int ID;
			public byte Red;
			public byte Green;
			public byte Blue;
		}
		public class _GuildPerkSpells
		{
			public int ID;
			public uint SpellID;
		}
		public class _Heirloom
		{
			public string SourceText;
			public uint ItemID;
			public uint[] OldItem = new uint[2];
			public uint NextDifficultyItemID;
			public uint[] UpgradeItemID = new uint[3];
			public ushort[] ItemBonusListID = new ushort[3];
			public byte Flags;
			public byte Source;
			public int ID;
		}
		public class _Helmetanimscaling
		{
			public int ID;
			public float Field1;
			public int Field2;
		}
		public class _Helmetgeosetvisdata
		{
			public int ID;
			public int[] Field1 = new int[9];
		}
		public class _Highlightcolor
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Holidaydescriptions
		{
			public int ID;
			public string Field1;
		}
		public class _Holidaynames
		{
			public int ID;
			public string Field1;
		}
		public class _Holidays
		{
			public int ID;
			public uint[] Date = new uint[16];
			public ushort[] Duration = new ushort[10];
			public ushort Region;
			public byte Looping;
			public byte[] CalendarFlags = new byte[10];
			public byte Priority;
			public byte CalendarFilterType;
			public byte Flags;
			public uint HolidayNameID;
			public uint HolidayDescriptionID;
			public int[] TextureFileDataID = new int[3];
		}
		public class _ImportPriceArmor
		{
			public int ID;
			public float ClothFactor;
			public float LeatherFactor;
			public float MailFactor;
			public float PlateFactor;
		}
		public class _ImportPriceQuality
		{
			public int ID;
			public float Factor;
		}
		public class _ImportPriceShield
		{
			public int ID;
			public float Factor;
		}
		public class _ImportPriceWeapon
		{
			public int ID;
			public float Factor;
		}
		public class _Invasionclientdata
		{
			public string Field0;
			public float[] Field1 = new float[2];
			public int ID;
			public ushort Field3;
			public int Field4;
			public int Field5;
			public ushort Field6;
			public byte Field7;
			public ushort Field8;
		}
		public class _Item
		{
			public int ID;
			public uint FileDataID;
			public byte Class;
			public byte SubClass;
			public byte SoundOverrideSubclass;
			public byte Material;
			public byte InventoryType;
			public byte Sheath;
			public byte GroupSoundsID;
		}
		public class _ItemAppearance
		{
			public int ID;
			public uint DisplayID;
			public uint IconFileDataID;
			public uint UIOrder;
			public byte ObjectComponentSlot;
		}
		public class _Itemappearancexuicamera
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _ItemArmorQuality
		{
			public int ID;
			public float[] QualityMod = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemArmorShield
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemArmorTotal
		{
			public int ID;
			public float[] Value = new float[4];
			public ushort ItemLevel;
		}
		public class _ItemBagFamily
		{
			public int ID;
			public string Name;
		}
		public class _ItemBonus
		{
			public int ID;
			public int[] Value = new int[3];
			public ushort BonusListID;
			public byte Type;
			public byte Index;
		}
		public class _ItemBonusListLevelDelta
		{
			public short Delta;
			public int ID;
		}
		public class _ItemBonusTreeNode
		{
			public int ID;
			public ushort SubTreeID;
			public ushort BonusListID;
			public ushort ItemLevelSelectorID;
			public byte BonusTreeModID;
		}
		public class _ItemChildEquipment
		{
			public int ID;
			public uint AltItemID;
			public byte AltEquipmentSlot;
		}
		public class _ItemClass
		{
			public int ID;
			public string Name;
			public float PriceMod;
			public byte OldEnumValue;
			public byte Flags;
		}
		public class _Itemcontextpickerentry
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
		}
		public class _ItemCurrencyCost
		{
			public int ID;
			public uint ItemId;
		}
		public class _ItemDamageAmmo
		{
			public int ID;
			public float[] DPS = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageOneHand
		{
			public int ID;
			public float[] DPS = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageOneHandCaster
		{
			public int ID;
			public float[] DPS = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageTwoHand
		{
			public int ID;
			public float[] DPS = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageTwoHandCaster
		{
			public int ID;
			public float[] DPS = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDisenchantLoot
		{
			public int ID;
			public ushort MinItemLevel;
			public ushort MaxItemLevel;
			public ushort RequiredDisenchantSkill;
			public byte ItemSubClass;
			public byte ItemQuality;
			public byte Expansion;
		}
		public class _Itemdisplayinfo
		{
			public int ID;
			public int Field01;
			public int Field02;
			public int ItemVisualID;
			public int Field04;
			public int Field05;
			public int Field06;
			public int Field07;
			public int Field08;
			public int Field09;
			public int Field0A;
			public int[] Modelfiledata = new int[2];
			public int[] Texturefiledata = new int[2];
			public int[] Field0D = new int[4];
			public int[] Field0E = new int[4];
			public int[] GeosetFlag = new int[2];
		}
		public class _Itemdisplayinfomaterialres
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _Itemdisplayxuicamera
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _ItemEffect
		{
			public int ID;
			public uint SpellID;
			public int Cooldown;
			public int CategoryCooldown;
			public short Charges;
			public ushort Category;
			public ushort ChrSpecializationID;
			public byte OrderIndex;
			public byte Trigger;
		}
		public class _ItemExtendedCost
		{
			public int ID;
			public uint[] RequiredItem = new uint[5];
			public uint[] RequiredCurrencyCount = new uint[5];
			public ushort[] RequiredItemCount = new ushort[5];
			public ushort RequiredPersonalArenaRating;
			public ushort[] RequiredCurrency = new ushort[5];
			public byte RequiredArenaSlot;
			public byte RequiredFactionId;
			public byte RequiredFactionStanding;
			public byte RequirementFlags;
			public byte RequiredAchievement;
		}
		public class _Itemgroupsounds
		{
			public int ID;
			public int[] Field1 = new int[4];
		}
		public class _ItemLevelSelector
		{
			public int ID;
			public ushort ItemLevel;
			public ushort ItemLevelSelectorQualitySetID;
		}
		public class _ItemLevelSelectorQuality
		{
			public int ID;
			public uint ItemBonusListID;
			public byte Quality;
		}
		public class _ItemLevelSelectorQualitySet
		{
			public int ID;
			public ushort ItemLevelMin;
			public ushort ItemLevelMax;
		}
		public class _ItemLimitCategory
		{
			public int ID;
			public string Name;
			public byte Quantity;
			public byte Flags;
		}
		public class _Itemlimitcategorycondition
		{
			public int ID;
			public byte Field1;
			public ushort Field2;
		}
		public class _ItemModifiedAppearance
		{
			public uint ItemID;
			public int ID;
			public byte AppearanceModID;
			public ushort AppearanceID;
			public byte Index;
			public byte SourceType;
		}
		public class _Itemmodifiedappearanceextra
		{
			public int ID;
			public int Field1;
			public int Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Itemnamedescription
		{
			public int ID;
			public string Field1;
			public int Field2;
		}
		public class _Itempetfood
		{
			public int ID;
			public string Field1;
		}
		public class _ItemPriceBase
		{
			public int ID;
			public float ArmorFactor;
			public float WeaponFactor;
			public ushort ItemLevel;
		}
		public class _ItemRandomProperties
		{
			public int ID;
			public string Name;
			public ushort[] Enchantment = new ushort[5];
		}
		public class _ItemRandomSuffix
		{
			public int ID;
			public string Name;
			public ushort[] Enchantment = new ushort[5];
			public ushort[] AllocationPct = new ushort[5];
		}
		public class _Itemrangeddisplayinfo
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public ushort Field3;
			public int Field4;
		}
		public class _ItemSearchName
		{
			public ulong AllowableRace;
			public string Name;
			public int ID;
			public uint[] Flags = new uint[3];
			public ushort ItemLevel;
			public byte Quality;
			public byte RequiredExpansion;
			public byte RequiredLevel;
			public ushort RequiredReputationFaction;
			public byte RequiredReputationRank;
			public int AllowableClass;
			public ushort RequiredSkill;
			public ushort RequiredSkillRank;
			public uint RequiredSpell;
		}
		public class _ItemSet
		{
			public int ID;
			public string Name;
			public uint[] ItemID = new uint[17];
			public ushort RequiredSkillRank;
			public uint RequiredSkill;
			public uint Flags;
		}
		public class _ItemSetSpell
		{
			public int ID;
			public uint SpellID;
			public ushort ChrSpecID;
			public byte Threshold;
		}
		public class _ItemSparse
		{
			public int ID;
			public long AllowableRace;
			public string Name;
			public string Name2;
			public string Name3;
			public string Name4;
			public string Description;
			public uint[] Flags = new uint[4];
			public float Unk1;
			public float Unk2;
			public uint BuyCount;
			public uint BuyPrice;
			public uint SellPrice;
			public uint RequiredSpell;
			public uint MaxCount;
			public uint Stackable;
			public int[] ItemStatAllocation = new int[10];
			public float[] ItemStatSocketCostMultiplier = new float[10];
			public float RangedModRange;
			public uint BagFamily;
			public float ArmorDamageModifier;
			public uint Duration;
			public float StatScalingFactor;
			public short AllowableClass;
			public ushort ItemLevel;
			public ushort RequiredSkill;
			public ushort RequiredSkillRank;
			public ushort RequiredReputationFaction;
			public short[] ItemStatValue = new short[10];
			public ushort ScalingStatDistribution;
			public ushort Delay;
			public ushort PageText;
			public ushort StartQuest;
			public ushort LockID;
			public ushort RandomProperty;
			public ushort RandomSuffix;
			public ushort ItemSet;
			public ushort Area;
			public ushort Map;
			public ushort TotemCategory;
			public ushort SocketBonus;
			public ushort GemProperties;
			public ushort ItemLimitCategory;
			public ushort HolidayID;
			public ushort RequiredTransmogHolidayID;
			public ushort ItemNameDescriptionID;
			public byte Quality;
			public byte InventoryType;
			public byte RequiredLevel;
			public byte RequiredHonorRank;
			public byte RequiredCityRank;
			public byte RequiredReputationRank;
			public byte ContainerSlots;
			public byte[] ItemStatType = new byte[10];
			public byte DamageType;
			public byte Bonding;
			public byte LanguageID;
			public byte PageMaterial;
			public byte Material;
			public byte Sheath;
			public byte[] SocketColor = new byte[3];
			public byte CurrencySubstitutionID;
			public byte CurrencySubstitutionCount;
			public byte ArtifactID;
			public byte RequiredExpansion;
		}
		public class _ItemSpec
		{
			public int ID;
			public ushort SpecID;
			public byte MinLevel;
			public byte MaxLevel;
			public byte ItemType;
			public byte PrimaryStat;
			public byte SecondaryStat;
		}
		public class _ItemSpecOverride
		{
			public int ID;
			public ushort SpecID;
		}
		public class _Itemsubclass
		{
			public int ID;
			public string Field1;
			public string Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
		}
		public class _Itemsubclassmask
		{
			public int ID;
			public string Field1;
			public int Field2;
			public byte Field3;
		}
		public class _ItemUpgrade
		{
			public int ID;
			public uint CurrencyCost;
			public ushort PrevItemUpgradeID;
			public ushort CurrencyID;
			public byte ItemUpgradePathID;
			public byte ItemLevelBonus;
		}
		public class _Itemvisuals
		{
			public int ID;
			public int[] Field1 = new int[5];
		}
		public class _ItemXBonusTree
		{
			public int ID;
			public ushort BonusTreeID;
		}
		public class _Journalencounter
		{
			public int ID;
			public string Field1;
			public string Field2;
			public float[] Field3 = new float[2];
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public int FieldB;
		}
		public class _Journalencountercreature
		{
			public string Field0;
			public string Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public ushort Field5;
			public byte Field6;
			public int ID;
		}
		public class _Journalencounteritem
		{
			public int Field0;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public int ID;
		}
		public class _Journalencountersection
		{
			public int ID;
			public string Field1;
			public string Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public int Field6;
			public ushort Field7;
			public ushort Field8;
			public ushort Field9;
			public ushort FieldA;
			public ushort FieldB;
			public ushort FieldC;
			public byte FieldD;
			public byte FieldE;
			public byte FieldF;
		}
		public class _Journalencounterxdifficulty
		{
			public int ID;
			public byte Field1;
		}
		public class _Journalencounterxmaploc
		{
			public int ID;
			public float[] Field1 = new float[2];
			public byte Field2;
			public ushort Field3;
			public int Field4;
			public int Field5;
		}
		public class _Journalinstance
		{
			public string Field0;
			public string Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public ushort Field6;
			public ushort Field7;
			public byte Field8;
			public byte Field9;
			public int ID;
		}
		public class _Journalitemxdifficulty
		{
			public int ID;
			public byte Field1;
		}
		public class _Journalsectionxdifficulty
		{
			public int ID;
			public byte Field1;
		}
		public class _Journaltier
		{
			public int ID;
			public string Field1;
		}
		public class _Journaltierxinstance
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _Keychain
		{
			public int ID;
			public byte[] Key = new byte[32];
		}
		public class _Keystoneaffix
		{
			public int ID;
			public string Field1;
			public string Field2;
			public int Field3;
		}
		public class _Languages
		{
			public string Field0;
			public int ID;
		}
		public class _Languagewords
		{
			public int ID;
			public string Field1;
			public byte Field2;
		}
		public class _Lfgdungeonexpansion
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
			public byte Field6;
		}
		public class _Lfgdungeongroup
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _LFGDungeons
		{
			public int ID;
			public string Name;
			public string Description;
			public uint Flags;
			public float MinItemLevel;
			public ushort MaxLevel;
			public ushort TargetLevelMax;
			public ushort MapID;
			public ushort RandomID;
			public ushort ScenarioID;
			public ushort LastBossJournalEncounterID;
			public ushort BonusReputationAmount;
			public ushort MentorItemLevel;
			public ushort PlayerConditionID;
			public byte MinLevel;
			public byte TargetLevel;
			public byte TargetLevelMin;
			public byte DifficultyID;
			public byte Type;
			public byte Faction;
			public byte Expansion;
			public byte OrderIndex;
			public byte GroupID;
			public byte CountTank;
			public byte CountHealer;
			public byte CountDamage;
			public byte MinCountTank;
			public byte MinCountHealer;
			public byte MinCountDamage;
			public byte SubType;
			public byte MentorCharLevel;
			public int TextureFileDataID;
			public int RewardIconFileDataID;
			public int ProposalTextureFileDataID;
		}
		public class _Lfgdungeonsgroupingmap
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _Lfgrolerequirement
		{
			public int ID;
			public byte Field1;
			public int Field2;
		}
		public class _Light
		{
			public int ID;
			public float[] Pos = new float[3];
			public float FalloffStart;
			public float FalloffEnd;
			public ushort MapID;
			public ushort[] LightParamsID = new ushort[8];
		}
		public class _Lightdata
		{
			public int ID;
			public int Field01;
			public int Field02;
			public int Field03;
			public int Field04;
			public int Field05;
			public int Field06;
			public int Field07;
			public int Field08;
			public int Field09;
			public int Field0A;
			public int Field0B;
			public int Field0C;
			public int Field0D;
			public int Field0E;
			public int Field0F;
			public int Field10;
			public int Field11;
			public int Field12;
			public float Field13;
			public float Field14;
			public float Field15;
			public float Field16;
			public float Field17;
			public float Field18;
			public float Field19;
			public float Field1A;
			public float Field1B;
			public int Field1C;
			public int Field1D;
			public int Field1E;
			public int Field1F;
			public int Field20;
			public int Field21;
			public ushort Field22;
		}
		public class _Lightparams
		{
			public float Field00;
			public float Field04;
			public float Field08;
			public float Field0C;
			public float Field10;
			public int[] Field14 = new int[3];
			public ushort LightSkyboxID;
			public byte Field22;
			public byte Field23;
			public byte Field24;
			public int ID;
		}
		public class _Lightskybox
		{
			public int ID;
			public string SkyboxPathName;
			public int Field2;
			public int Field3;
			public byte Field4;
		}
		public class _Liquidmaterial
		{
			public int ID;
			public byte Field1;
			public byte Field2;
		}
		public class _Liquidobject
		{
			public int ID;
			public float Field1;
			public float Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _LiquidType
		{
			public int ID;
			public string Name;
			public string[] Texture = new string[6];
			public uint SpellID;
			public float MaxDarkenDepth;
			public float FogDarkenIntensity;
			public float AmbDarkenIntensity;
			public float DirDarkenIntensity;
			public float ParticleScale;
			public uint[] Color = new uint[2];
			public float[] Float = new float[18];
			public uint[] Int = new uint[4];
			public ushort Flags;
			public ushort LightID;
			public byte Type;
			public byte ParticleMovement;
			public byte ParticleTexSlots;
			public byte MaterialID;
			public byte[] DepthTexCount = new byte[6];
			public uint SoundID;
		}
		public class _Loadingscreens
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
		}
		public class _Loadingscreentaxisplines
		{
			public int ID;
			public float[] Field1 = new float[10];
			public float[] Field2 = new float[10];
			public ushort Field3;
			public ushort Field4;
			public byte Field5;
		}
		public class _Locale
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Location
		{
			public int ID;
			public float[] Field1 = new float[3];
			public float[] Field2 = new float[3];
		}
		public class _Lock
		{
			public int ID;
			public uint[] Index = new uint[8];
			public ushort[] Skill = new ushort[8];
			public byte[] Type = new byte[8];
			public byte[] Action = new byte[8];
		}
		public class _Locktype
		{
			public string Field0;
			public string Field1;
			public string Field2;
			public string Field3;
			public int ID;
		}
		public class _Lookatcontroller
		{
			public int ID;
			public float Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public ushort Field05;
			public ushort Field06;
			public ushort Field07;
			public ushort Field08;
			public byte Field09;
			public byte Field0A;
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public ushort Field0F;
			public int Field10;
			public byte Field11;
			public int Field12;
		}
		public class _MailTemplate
		{
			public int ID;
			public string Body;
		}
		public class _Managedworldstate
		{
			public int Field0;
			public int Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public int Field6;
			public int Field7;
			public int Field8;
			public int ID;
		}
		public class _Managedworldstatebuff
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
		}
		public class _Managedworldstateinput
		{
			public int ID;
			public byte Field1;
			public int Field2;
			public int Field3;
		}
		public class _Manifestinterfaceactionicon
		{
			public int ID;
		}
		public class _Manifestinterfacedata
		{
			public int ID;
			public string Field1;
			public string Field2;
		}
		public class _Manifestinterfaceitemicon
		{
			public int ID;
		}
		public class _Manifestinterfacetocdata
		{
			public int ID;
			public string Field1;
		}
		public class _Manifestmp3
		{
			public int ID;
		}
		public class _Map
		{
			public int ID;
			public string Directory;
			public string MapName;
			public string MapDescription0;
			public string MapDescription1;
			public string ShortDescription;
			public string LongDescription;
			public uint[] Flags = new uint[2];
			public float MinimapIconScale;
			public float[] CorpsePos = new float[2];
			public ushort AreaTableID;
			public ushort LoadingScreenID;
			public ushort CorpseMapID;
			public ushort TimeOfDayOverride;
			public ushort ParentMapID;
			public ushort CosmeticParentMapID;
			public ushort WindSettingsID;
			public byte InstanceType;
			public byte unk5;
			public byte ExpansionID;
			public byte MaxPlayers;
			public byte TimeOffset;
		}
		public class _Mapcelestialbody
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _Mapchallengemode
		{
			public string Field0;
			public int ID;
			public ushort Field2;
			public ushort[] Field3 = new ushort[3];
			public byte Field4;
		}
		public class _MapDifficulty
		{
			public int ID;
			public string Message_lang;
			public byte DifficultyID;
			public byte RaidDurationType;
			public byte MaxPlayers;
			public byte LockID;
			public byte Flags;
			public byte ItemBonusTreeModID;
			public uint Context;
		}
		public class _Mapdifficultyxcondition
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
		}
		public class _Maploadingscreen
		{
			public int ID;
			public float[] Field1 = new float[2];
			public float[] Field2 = new float[2];
			public ushort Field3;
			public byte Field4;
		}
		public class _Marketingpromotionsxlocale
		{
			public int ID;
			public string Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Material
		{
			public int ID;
			public byte Field1;
			public ushort Field2;
			public int Field3;
			public int Field4;
		}
		public class _Minortalent
		{
			public int ID;
			public int Field1;
			public int Field2;
		}
		public class _Missiletargeting
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float[] Field8 = new float[2];
			public float Field9;
			public int FieldA;
			public int FieldB;
			public int[] FieldC = new int[2];
		}
		public class _Modelanimcloakdampening
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _Modelfiledata
		{
			public byte Field0;
			public int ID;
			public ushort Field2;
		}
		public class _Modelribbonquality
		{
			public int ID;
			public byte Field1;
		}
		public class _ModifierTree
		{
			public int ID;
			public uint[] Asset = new uint[2];
			public uint Parent;
			public byte Type;
			public byte Unk700;
			public byte Operator;
			public byte Amount;
		}
		public class _Mount
		{
			public string Name;
			public string Description;
			public string SourceDescription;
			public uint SpellId;
			public float CameraPivotMultiplier;
			public ushort MountTypeId;
			public ushort Flags;
			public byte Source;
			public int ID;
			public uint PlayerConditionId;
			public int UiModelSceneID;
		}
		public class _MountCapability
		{
			public uint RequiredSpell;
			public uint SpeedModSpell;
			public ushort RequiredRidingSkill;
			public ushort RequiredArea;
			public short RequiredMap;
			public byte Flags;
			public int ID;
			public uint RequiredAura;
		}
		public class _MountTypeXCapability
		{
			public int ID;
			public ushort MountTypeID;
			public ushort MountCapabilityID;
			public byte OrderIndex;
		}
		public class _MountXDisplay
		{
			public int ID;
			public uint DisplayID;
			public uint PlayerConditionID;
		}
		public class _Movie
		{
			public int ID;
			public uint AudioFileDataID;
			public uint SubtitleFileDataID;
			public byte Volume;
			public byte KeyID;
		}
		public class _Moviefiledata
		{
			public int ID;
			public ushort Field1;
		}
		public class _Movievariation
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _NameGen
		{
			public int ID;
			public string Name;
			public byte Race;
			public byte Sex;
		}
		public class _NamesProfanity
		{
			public int ID;
			public string Name;
			public byte Language;
		}
		public class _NamesReserved
		{
			public int ID;
			public string Name;
		}
		public class _NamesReservedLocale
		{
			public int ID;
			public string Name;
			public byte LocaleMask;
		}
		public class _Npcmodelitemslotdisplayinfo
		{
			public int ID;
			public int DisplayID;
			public byte Slot;
		}
		public class _Npcsounds
		{
			public int ID;
			public int[] Field1 = new int[4];
		}
		public class _Objecteffect
		{
			public int ID;
			public float[] Field1 = new float[3];
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public int Field7;
			public int Field8;
		}
		public class _Objecteffectmodifier
		{
			public int ID;
			public float[] Field1 = new float[4];
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Objecteffectpackageelem
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Outlineeffect
		{
			public int ID;
			public float Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
			public int Field6;
		}
		public class _OverrideSpellData
		{
			public int ID;
			public uint[] SpellID = new uint[10];
			public uint PlayerActionbarFileDataID;
			public byte Flags;
		}
		public class _Pagetextmaterial
		{
			public int ID;
			public string Field1;
		}
		public class _Paperdollitemframe
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public int Field3;
		}
		public class _Paragonreputation
		{
			public int ID;
			public int Field1;
			public int Field2;
			public ushort Field3;
		}
		public class _Particlecolor
		{
			public int ID;
			public int[] Field1 = new int[3];
			public int[] Field2 = new int[3];
			public int[] Field3 = new int[3];
		}
		public class _Path
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Pathnode
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Pathnodeproperty
		{
			public ushort Field0;
			public ushort Field1;
			public byte Field2;
			public int ID;
			public int Field4;
		}
		public class _Pathproperty
		{
			public int Field0;
			public ushort Field1;
			public byte Field2;
			public int ID;
		}
		public class _Phase
		{
			public int ID;
			public ushort Flags;
		}
		public class _Phaseshiftzonesounds
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public ushort FieldA;
			public ushort FieldB;
			public byte FieldC;
			public byte FieldD;
		}
		public class _PhaseXPhaseGroup
		{
			public int ID;
			public ushort PhaseID;
		}
		public class _PlayerCondition
		{
			public long RaceMask;
			public string FailureDescription;
			public int ID;
			public byte Flags;
			public ushort MinLevel;
			public ushort MaxLevel;
			public int ClassMask;
			public byte Gender;
			public byte NativeGender;
			public uint SkillLogic;
			public byte LanguageID;
			public byte MinLanguage;
			public int MaxLanguage;
			public ushort MaxFactionID;
			public byte MaxReputation;
			public uint ReputationLogic;
			public byte Unknown1;
			public byte MinPVPRank;
			public byte MaxPVPRank;
			public byte PvpMedal;
			public uint PrevQuestLogic;
			public uint CurrQuestLogic;
			public uint CurrentCompletedQuestLogic;
			public uint SpellLogic;
			public uint ItemLogic;
			public byte ItemFlags;
			public uint AuraSpellLogic;
			public ushort WorldStateExpressionID;
			public byte WeatherID;
			public byte PartyStatus;
			public byte LifetimeMaxPVPRank;
			public uint AchievementLogic;
			public uint LfgLogic;
			public uint AreaLogic;
			public uint CurrencyLogic;
			public ushort QuestKillID;
			public uint QuestKillLogic;
			public byte MinExpansionLevel;
			public byte MaxExpansionLevel;
			public byte MinExpansionTier;
			public byte MaxExpansionTier;
			public byte MinGuildLevel;
			public byte MaxGuildLevel;
			public byte PhaseUseFlags;
			public ushort PhaseID;
			public uint PhaseGroupID;
			public int MinAvgItemLevel;
			public int MaxAvgItemLevel;
			public ushort MinAvgEquippedItemLevel;
			public ushort MaxAvgEquippedItemLevel;
			public byte ChrSpecializationIndex;
			public byte ChrSpecializationRole;
			public byte PowerType;
			public byte PowerTypeComp;
			public byte PowerTypeValue;
			public uint ModifierTreeID;
			public int MainHandItemSubclassMask;
			public ushort[] SkillID = new ushort[4];
			public ushort[] MinSkill = new ushort[4];
			public ushort[] MaxSkill = new ushort[4];
			public uint[] MinFactionID = new uint[3];
			public byte[] MinReputation = new byte[3];
			public ushort[] PrevQuestID = new ushort[4];
			public ushort[] CurrQuestID = new ushort[4];
			public ushort[] CurrentCompletedQuestID = new ushort[4];
			public int[] SpellID = new int[4];
			public int[] ItemID = new int[4];
			public uint[] ItemCount = new uint[4];
			public ushort[] Explored = new ushort[2];
			public uint[] Time = new uint[2];
			public uint[] AuraSpellID = new uint[4];
			public byte[] AuraCount = new byte[4];
			public ushort[] Achievement = new ushort[4];
			public byte[] LfgStatus = new byte[4];
			public byte[] LfgCompare = new byte[4];
			public uint[] LfgValue = new uint[4];
			public ushort[] AreaID = new ushort[4];
			public uint[] CurrencyID = new uint[4];
			public uint[] CurrencyCount = new uint[4];
			public uint[] QuestKillMonster = new uint[6];
			public int[] MovementFlags = new int[2];
		}
		public class _Positioner
		{
			public int ID;
			public float Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Positionerstate
		{
			public int ID;
			public float Field1;
			public byte Field2;
			public ushort Field3;
			public byte Field4;
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public int Field8;
		}
		public class _Positionerstateentry
		{
			public int ID;
			public float Field1;
			public float Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public ushort FieldB;
		}
		public class _PowerDisplay
		{
			public int ID;
			public string GlobalStringBaseTag;
			public byte PowerType;
			public byte Red;
			public byte Green;
			public byte Blue;
		}
		public class _PowerType
		{
			public int ID;
			public string PowerTypeToken;
			public string PowerCostToken;
			public float RegenerationPeace;
			public float RegenerationCombat;
			public short MaxPower;
			public ushort RegenerationDelay;
			public ushort Flags;
			public byte PowerTypeEnum;
			public byte RegenerationMin;
			public byte RegenerationCenter;
			public byte RegenerationMax;
			public byte UIModifier;
		}
		public class _PrestigeLevelInfo
		{
			public int ID;
			public string PrestigeText;
			public uint IconID;
			public byte PrestigeLevel;
			public byte Flags;
		}
		public class _Pvpbrackettypes
		{
			public int ID;
			public byte Field1;
			public int[] Field2 = new int[4];
		}
		public class _PVPDifficulty
		{
			public int ID;
			public byte BracketID;
			public byte MinLevel;
			public byte MaxLevel;
		}
		public class _Pvpitem
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _PvpReward
		{
			public int ID;
			public uint HonorLevel;
			public uint Prestige;
			public uint RewardPackID;
		}
		public class _Pvpscalingeffect
		{
			public int ID;
			public float Field1;
			public byte Field2;
			public ushort Field3;
		}
		public class _Pvpscalingeffecttype
		{
			public int ID;
			public string Field1;
		}
		public class _Pvptalent
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public int Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public ushort Field9;
			public byte FieldA;
		}
		public class _Pvptalentunlock
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _QuestFactionReward
		{
			public int ID;
			public short[] QuestRewFactionValue = new short[10];
		}
		public class _Questfeedbackeffect
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
		}
		public class _Questinfo
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Questline
		{
			public int ID;
			public string Field1;
		}
		public class _Questlinexquest
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public byte Field3;
		}
		public class _QuestMoneyReward
		{
			public int ID;
			public uint[] Money = new uint[10];
		}
		public class _Questobjective
		{
			public int ID;
			public string Field1;
			public int Field2;
			public int Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _QuestPackageItem
		{
			public int ID;
			public uint ItemID;
			public ushort QuestPackageID;
			public byte FilterType;
			public uint ItemCount;
		}
		public class _Questpoiblob
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public int Field5;
			public ushort Field6;
			public byte Field7;
		}
		public class _Questpoipoint
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _QuestSort
		{
			public int ID;
			public string SortName;
			public byte SortOrder;
		}
		public class _QuestV2
		{
			public int ID;
			public ushort UniqueBitFlag;
		}
		public class _Questv2clitask
		{
			public ulong Field00;
			public string Field01;
			public string Field02;
			public int Field03;
			public ushort Field04;
			public ushort Field05;
			public ushort Field06;
			public ushort[] Field07 = new ushort[3];
			public ushort Field08;
			public ushort Field09;
			public byte Field0A;
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public byte Field0E;
			public byte Field0F;
			public byte Field10;
			public byte Field11;
			public byte Field12;
			public byte Field13;
			public int ID;
			public int Field15;
			public int Field16;
			public int Field17;
		}
		public class _Questxgroupactivity
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _QuestXP
		{
			public int ID;
			public ushort[] Exp = new ushort[10];
		}
		public class _RandPropPoints
		{
			public int ID;
			public uint[] EpicPropertiesPoints = new uint[5];
			public uint[] RarePropertiesPoints = new uint[5];
			public uint[] UncommonPropertiesPoints = new uint[5];
		}
		public class _Relicslottierrequirement
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Relictalent
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public int Field4;
			public byte Field5;
		}
		public class _Researchbranch
		{
			public int ID;
			public string Field1;
			public int Field2;
			public ushort Field3;
			public byte Field4;
			public int Field5;
			public int Field6;
		}
		public class _Researchfield
		{
			public string Field0;
			public byte Field1;
			public int ID;
		}
		public class _Researchproject
		{
			public string Field0;
			public string Field1;
			public int Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
			public int ID;
			public int Field7;
			public byte Field8;
		}
		public class _Researchsite
		{
			public int ID;
			public string Field1;
			public int Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _Resistances
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public ushort Field3;
		}
		public class _RewardPack
		{
			public int ID;
			public uint Money;
			public float ArtifactXPMultiplier;
			public byte ArtifactXPDifficulty;
			public byte ArtifactCategoryID;
			public uint TitleID;
			public uint Unused;
		}
		public class _Rewardpackxcurrencytype
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _RewardPackXItem
		{
			public int ID;
			public uint ItemID;
			public uint Amount;
		}
		public class _Ribbonquality
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _RulesetItemUpgrade
		{
			public int ID;
			public uint ItemID;
			public ushort ItemUpgradeID;
		}
		public class _SandboxScaling
		{
			public int ID;
			public uint MinLevel;
			public uint MaxLevel;
			public uint Flags;
		}
		public class _ScalingStatDistribution
		{
			public int ID;
			public ushort ItemLevelCurveID;
			public uint MinLevel;
			public uint MaxLevel;
		}
		public class _Scenario
		{
			public int ID;
			public string Name;
			public ushort Data;
			public byte Flags;
			public byte Type;
		}
		public class _Scenarioevententry
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _ScenarioStep
		{
			public int ID;
			public string Description;
			public string Name;
			public ushort ScenarioID;
			public ushort PreviousStepID;
			public ushort QuestRewardID;
			public byte Step;
			public byte Flags;
			public uint CriteriaTreeID;
			public uint BonusRequiredStepID;
		}
		public class _SceneScript
		{
			public int ID;
			public ushort PrevScriptId;
			public ushort NextScriptId;
		}
		public class _SceneScriptGlobalText
		{
			public int ID;
			public string Name;
			public string Script;
		}
		public class _SceneScriptPackage
		{
			public int ID;
			public string Name;
		}
		public class _Scenescriptpackagemember
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _SceneScriptText
		{
			public int ID;
			public string Name;
			public string Script;
		}
		public class _Scheduledinterval
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public int Field3;
			public int Field4;
			public byte Field5;
		}
		public class _Scheduledworldstate
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public int Field3;
			public int Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
		}
		public class _Scheduledworldstategroup
		{
			public int ID;
			public int Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public ushort Field5;
		}
		public class _Scheduledworldstatexuniqcat
		{
			public int ID;
			public ushort Field1;
		}
		public class _Screeneffect
		{
			public int ID;
			public string Field1;
			public int[] Field2 = new int[4];
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public int FieldB;
			public ushort FieldC;
		}
		public class _Screenlocation
		{
			public int ID;
			public string Field1;
		}
		public class _Seamlesssite
		{
			public int ID;
			public ushort Field1;
		}
		public class _Servermessages
		{
			public int ID;
			public string Field1;
		}
		public class _Shadowyeffect
		{
			public int ID;
			public int Field1;
			public int Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public int Field9;
			public byte FieldA;
			public byte FieldB;
			public ushort FieldC;
			public byte FieldD;
		}
		public class _SkillLine
		{
			public int ID;
			public string DisplayName;
			public string Description;
			public string AlternateVerb;
			public ushort Flags;
			public byte CategoryID;
			public byte CanLink;
			public uint IconFileDataID;
			public uint ParentSkillLineID;
		}
		public class _SkillLineAbility
		{
			public ulong RaceMask;
			public int ID;
			public uint SpellID;
			public uint SupercedesSpell;
			public ushort SkillLine;
			public ushort TrivialSkillLineRankHigh;
			public ushort TrivialSkillLineRankLow;
			public ushort UniqueBit;
			public ushort TradeSkillCategoryID;
			public byte NumSkillUps;
			public int ClassMask;
			public ushort MinSkillLineRank;
			public byte AcquireMethod;
			public byte Flags;
		}
		public class _SkillRaceClassInfo
		{
			public int ID;
			public long RaceMask;
			public ushort SkillID;
			public ushort Flags;
			public ushort SkillTierID;
			public byte Availability;
			public byte MinLevel;
			public int ClassMask;
		}
		public class _Soundambience
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public int[] Field4 = new int[2];
		}
		public class _Soundambienceflavor
		{
			public int ID;
			public int Field1;
			public uint Field2;
		}
		public class _Soundbus
		{
			public float Field0;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public int ID;
		}
		public class _Soundbusoverride
		{
			public int ID;
			public float Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public ushort Field5;
			public ushort Field6;
		}
		public class _Soundemitterpillpoints
		{
			public int ID;
			public float[] Field1 = new float[3];
			public ushort Field2;
		}
		public class _Soundemitters
		{
			public string Field0;
			public float[] Field1 = new float[3];
			public float[] Field2 = new float[3];
			public ushort Field3;
			public ushort Field4;
			public byte Field5;
			public byte Field6;
			public byte Field7;
			public int ID;
			public int Field9;
			public byte FieldA;
		}
		public class _Soundenvelope
		{
			public int ID;
			public int Field1;
			public int Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Soundfilter
		{
			public int ID;
			public string Field1;
		}
		public class _Soundfilterelem
		{
			public int ID;
			public float[] Field1 = new float[9];
			public byte Field2;
		}
		public class _SoundKit
		{
			public int ID;
			public float VolumeFloat;
			public float MinDistance;
			public float DistanceCutoff;
			public ushort Flags;
			public ushort SoundEntriesAdvancedID;
			public byte SoundType;
			public byte DialogType;
			public byte EAXDef;
			public float VolumeVariationPlus;
			public float VolumeVariationMinus;
			public float PitchVariationPlus;
			public float PitchVariationMinus;
			public float PitchAdjust;
			public ushort BusOverwriteID;
			public byte Unk700;
		}
		public class _Soundkitadvanced
		{
			public int ID;
			public float Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public float Field05;
			public int Field06;
			public int Field07;
			public float Field08;
			public byte Field09;
			public uint Field0A;
			public int Field0B;
			public int Field0C;
			public int Field0D;
			public int Field0E;
			public int Field0F;
			public int Field10;
			public int Field11;
			public int Field12;
			public int Field13;
			public int Field14;
			public float Field15;
			public float Field16;
			public float Field17;
			public float Field18;
			public float Field19;
			public float Field1A;
			public float Field1B;
			public float Field1C;
			public int Field1D;
			public int Field1E;
			public int Field1F;
			public int Field20;
			public int Field21;
			public int Field22;
			public int Field23;
			public int Field24;
			public int Field25;
			public int Field26;
			public int Field27;
		}
		public class _Soundkitchild
		{
			public int ID;
			public uint Field1;
			public int Field2;
		}
		public class _Soundkitentry
		{
			public int ID;
			public uint Field1;
			public int Field2;
			public byte Field3;
			public float Field4;
		}
		public class _Soundkitfallback
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
		}
		public class _Soundkitname
		{
			public int ID;
			public string Name;
		}
		public class _Soundoverride
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _Soundproviderpreferences
		{
			public int ID;
			public string Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public float Field05;
			public float Field06;
			public float Field07;
			public float Field08;
			public float Field09;
			public float Field0A;
			public float Field0B;
			public float Field0C;
			public float Field0D;
			public float Field0E;
			public float Field0F;
			public float Field10;
			public ushort Field11;
			public ushort Field12;
			public ushort Field13;
			public ushort Field14;
			public ushort Field15;
			public byte Field16;
			public byte Field17;
		}
		public class _Sourceinfo
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _Spammessages
		{
			public int ID;
			public string Field1;
		}
		public class _SpecializationSpells
		{
			public string Description;
			public uint SpellID;
			public uint OverridesSpellID;
			public ushort SpecID;
			public byte OrderIndex;
			public int ID;
		}
		public class _Spell
		{
			public int ID;
			public string Name;
			public string NameSubtext;
			public string Description;
			public string AuraDescription;
		}
		public class _Spellactionbarpref
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _Spellactivationoverlay
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
			public float Field4;
			public int[] Field5 = new int[4];
			public byte Field6;
			public byte Field7;
			public ushort Field8;
		}
		public class _SpellAuraOptions
		{
			public int ID;
			public uint ProcCharges;
			public uint ProcTypeMask;
			public uint ProcCategoryRecovery;
			public ushort CumulativeAura;
			public ushort SpellProcsPerMinuteID;
			public byte DifficultyID;
			public byte ProcChance;
		}
		public class _SpellAuraRestrictions
		{
			public int ID;
			public uint CasterAuraSpell;
			public uint TargetAuraSpell;
			public uint ExcludeCasterAuraSpell;
			public uint ExcludeTargetAuraSpell;
			public byte DifficultyID;
			public byte CasterAuraState;
			public byte TargetAuraState;
			public byte ExcludeCasterAuraState;
			public byte ExcludeTargetAuraState;
		}
		public class _Spellauravisibility
		{
			public byte Field0;
			public byte Field1;
			public int ID;
		}
		public class _Spellauravisxchrspec
		{
			public int ID;
			public ushort Field1;
		}
		public class _SpellCastingRequirements
		{
			public int ID;
			public uint SpellID;
			public ushort MinFactionID;
			public ushort RequiredAreasID;
			public ushort RequiresSpellFocus;
			public byte FacingCasterFlags;
			public byte MinReputation;
			public byte RequiredAuraVision;
		}
		public class _SpellCastTimes
		{
			public int ID;
			public int CastTime;
			public int MinCastTime;
			public short CastTimePerLevel;
		}
		public class _SpellCategories
		{
			public int ID;
			public ushort Category;
			public ushort StartRecoveryCategory;
			public ushort ChargeCategory;
			public byte DifficultyID;
			public byte DefenseType;
			public byte DispelType;
			public byte Mechanic;
			public byte PreventionType;
		}
		public class _SpellCategory
		{
			public int ID;
			public string Name;
			public int ChargeRecoveryTime;
			public byte Flags;
			public byte UsesPerWeek;
			public byte MaxCharges;
			public uint ChargeCategoryType;
		}
		public class _Spellchaineffects
		{
			public int ID;
			public float Field01;
			public float Field02;
			public float Field03;
			public int Field04;
			public int Field05;
			public float Field06;
			public float Field07;
			public float Field08;
			public float Field09;
			public float Field0A;
			public float Field0B;
			public float Field0C;
			public float Field0D;
			public float Field0E;
			public float Field0F;
			public float Field10;
			public float Field11;
			public float Field12;
			public float Field13;
			public float Field14;
			public float Field15;
			public float Field16;
			public float Field17;
			public float Field18;
			public float Field19;
			public float Field1A;
			public float Field1B;
			public float Field1C;
			public float Field1D;
			public float Field1E;
			public float Field1F;
			public float Field20;
			public float Field21;
			public float Field22;
			public float Field23;
			public float[] Field24 = new float[3];
			public float[] Field25 = new float[3];
			public float[] Field26 = new float[3];
			public float[] Field27 = new float[3];
			public int Field28;
			public float Field29;
			public float Field2A;
			public float Field2B;
			public float Field2C;
			public ushort Field2D;
			public ushort Field2E;
			public ushort[] Field2F = new ushort[11];
			public ushort Field30;
			public byte Field31;
			public byte Field32;
			public byte Field33;
			public byte Field34;
			public byte Field35;
			public byte Field36;
			public byte Field37;
			public byte Field38;
			public byte Field39;
			public byte Field3A;
			public int Field3B;
			public int[] Field3C = new int[3];
		}
		public class _SpellClassOptions
		{
			public int ID;
			public uint SpellID;
			public int[] SpellClassMask = new int[4];
			public byte SpellClassSet;
			public uint ModalNextSpell;
		}
		public class _SpellCooldowns
		{
			public int ID;
			public uint CategoryRecoveryTime;
			public uint RecoveryTime;
			public uint StartRecoveryTime;
			public byte DifficultyID;
		}
		public class _Spelldescriptionvariables
		{
			public int ID;
			public string Field1;
		}
		public class _Spelldispeltype
		{
			public int ID;
			public string Field1;
			public string Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _SpellDuration
		{
			public int ID;
			public int Duration;
			public int MaxDuration;
			public int DurationPerLevel;
		}
		public class _SpellEffect
		{
			public int ID;
			public uint Effect;
			public int EffectBasePoints;
			public uint EffectIndex;
			public uint EffectAura;
			public uint DifficultyID;
			public float EffectAmplitude;
			public uint EffectAuraPeriod;
			public float EffectBonusCoefficient;
			public float EffectChainAmplitude;
			public uint EffectChainTargets;
			public int EffectDieSides;
			public uint EffectItemType;
			public uint EffectMechanic;
			public float EffectPointsPerResource;
			public float EffectRealPointsPerLevel;
			public uint EffectTriggerSpell;
			public float EffectPosFacing;
			public uint EffectAttributes;
			public float BonusCoefficientFromAP;
			public float PvPMultiplier;
			public float Coefficient;
			public float Variance;
			public float ResourceCoefficient;
			public float GroupSizeCoefficient;
			public int[] EffectSpellClassMask = new int[4];
			public int EffectMiscValue;
			public int EffectMiscValueB;
			public uint EffectRadiusIndex;
			public uint EffectRadiusMaxIndex;
			public uint[] ImplicitTarget = new uint[2];
		}
		public class _Spelleffectemission
		{
			public int ID;
			public float Field1;
			public float Field2;
			public ushort Field3;
			public byte Field4;
		}
		public class _SpellEquippedItems
		{
			public int ID;
			public uint SpellID;
			public int EquippedItemInventoryTypeMask;
			public int EquippedItemSubClassMask;
			public byte EquippedItemClass;
		}
		public class _Spellflyout
		{
			public int ID;
			public ulong Field1;
			public string Field2;
			public string Field3;
			public byte Field4;
			public ushort Field5;
			public int Field6;
		}
		public class _Spellflyoutitem
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _SpellFocusObject
		{
			public int ID;
			public string Name;
		}
		public class _SpellInterrupts
		{
			public int ID;
			public byte DifficultyID;
			public ushort InterruptFlags;
			public uint[] AuraInterruptFlags = new uint[2];
			public uint[] ChannelInterruptFlags = new uint[2];
		}
		public class _SpellItemEnchantment
		{
			public int ID;
			public string Name;
			public uint[] EffectSpellID = new uint[3];
			public float[] EffectScalingPoints = new float[3];
			public uint TransmogCost;
			public uint TextureFileDataID;
			public ushort[] EffectPointsMin = new ushort[3];
			public ushort ItemVisual;
			public ushort Flags;
			public ushort RequiredSkillID;
			public ushort RequiredSkillRank;
			public ushort ItemLevel;
			public byte Charges;
			public byte[] Effect = new byte[3];
			public byte ConditionID;
			public byte MinLevel;
			public byte MaxLevel;
			public byte ScalingClass;
			public byte ScalingClassRestricted;
			public uint PlayerConditionID;
		}
		public class _SpellItemEnchantmentCondition
		{
			public int ID;
			public uint[] LTOperand = new uint[5];
			public byte[] LTOperandType = new byte[5];
			public byte[] Operator = new byte[5];
			public byte[] RTOperandType = new byte[5];
			public byte[] RTOperand = new byte[5];
			public byte[] Logic = new byte[5];
		}
		public class _Spellkeyboundoverride
		{
			public int ID;
			public string Field1;
			public int Field2;
			public byte Field3;
		}
		public class _Spelllabel
		{
			public int ID;
			public int Field1;
		}
		public class _SpellLearnSpell
		{
			public int ID;
			public uint LearnSpellID;
			public uint SpellID;
			public uint OverridesSpellID;
		}
		public class _SpellLevels
		{
			public int ID;
			public ushort BaseLevel;
			public ushort MaxLevel;
			public ushort SpellLevel;
			public byte DifficultyID;
			public byte MaxUsableLevel;
		}
		public class _Spellmechanic
		{
			public int ID;
			public string Field1;
		}
		public class _SpellMisc
		{
			public int ID;
			public ushort CastingTimeIndex;
			public ushort DurationIndex;
			public ushort RangeIndex;
			public byte SchoolMask;
			public uint IconFileDataID;
			public float Speed;
			public uint ActiveIconFileDataID;
			public float MultistrikeSpeedMod;
			public byte DifficultyID;
			public uint Attributes;
			public uint AttributesEx;
			public uint AttributesExB;
			public uint AttributesExC;
			public uint AttributesExD;
			public uint AttributesExE;
			public uint AttributesExF;
			public uint AttributesExG;
			public uint AttributesExH;
			public uint AttributesExI;
			public uint AttributesExJ;
			public uint AttributesExK;
			public uint AttributesExL;
			public uint AttributesExM;
		}
		public class _Spellmissile
		{
			public int ID;
			public int Field01;
			public float Field02;
			public float Field03;
			public float Field04;
			public float Field05;
			public float Field06;
			public float Field07;
			public int Field08;
			public float Field09;
			public int Field0A;
			public float Field0B;
			public float Field0C;
			public float Field0D;
			public float Field0E;
			public byte Field0F;
		}
		public class _Spellmissilemotion
		{
			public int ID;
			public string Field1;
			public string Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _SpellPower
		{
			public int ManaCost;
			public float ManaCostPercentage;
			public float ManaCostPercentagePerSecond;
			public uint RequiredAura;
			public float HealthCostPercentage;
			public byte PowerIndex;
			public byte PowerType;
			public int ID;
			public int ManaCostPerLevel;
			public int ManaCostPerSecond;
			public int ManaCostAdditional;
			public uint PowerDisplayID;
			public uint UnitPowerBarID;
		}
		public class _SpellPowerDifficulty
		{
			public byte DifficultyID;
			public byte PowerIndex;
			public int ID;
		}
		public class _Spellproceduraleffect
		{
			public int[] Field0 = new int[4];
			public byte Field1;
			public int ID;
		}
		public class _SpellProcsPerMinute
		{
			public int ID;
			public float BaseProcRate;
			public byte Flags;
		}
		public class _SpellProcsPerMinuteMod
		{
			public int ID;
			public float Coeff;
			public ushort Param;
			public byte Type;
		}
		public class _SpellRadius
		{
			public int ID;
			public float Radius;
			public float RadiusPerLevel;
			public float RadiusMin;
			public float RadiusMax;
		}
		public class _SpellRange
		{
			public int ID;
			public string DisplayName;
			public string DisplayNameShort;
			public float MinRangeHostile;
			public float MinRangeFriend;
			public float MaxRangeHostile;
			public float MaxRangeFriend;
			public byte Flags;
		}
		public class _SpellReagents
		{
			public int ID;
			public uint SpellID;
			public int[] Reagent = new int[8];
			public ushort[] ReagentCount = new ushort[8];
		}
		public class _Spellreagentscurrency
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _SpellScaling
		{
			public int ID;
			public uint SpellID;
			public ushort ScalesFromItemLevel;
			public int ScalingClass;
			public uint MinScalingLevel;
			public uint MaxScalingLevel;
		}
		public class _SpellShapeshift
		{
			public int ID;
			public uint SpellID;
			public uint[] ShapeshiftExclude = new uint[2];
			public uint[] ShapeshiftMask = new uint[2];
			public byte StanceBarOrder;
		}
		public class _SpellShapeshiftForm
		{
			public int ID;
			public string Name;
			public float WeaponDamageVariance;
			public uint Flags;
			public ushort CombatRoundTime;
			public ushort MountTypeID;
			public byte CreatureType;
			public byte BonusActionBar;
			public uint AttackIconFileDataID;
			public uint[] CreatureDisplayID = new uint[4];
			public uint[] PresetSpellID = new uint[8];
		}
		public class _Spellspecialuniteffect
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _SpellTargetRestrictions
		{
			public int ID;
			public float ConeAngle;
			public float Width;
			public uint Targets;
			public ushort TargetCreatureType;
			public byte DifficultyID;
			public byte MaxAffectedTargets;
			public uint MaxTargetLevel;
		}
		public class _SpellTotems
		{
			public int ID;
			public uint SpellID;
			public uint[] Totem = new uint[2];
			public ushort[] RequiredTotemCategoryID = new ushort[2];
		}
		public class _Spellvisual
		{
			public int ID;
			public float[] Field1 = new float[3];
			public float[] Field2 = new float[3];
			public int Field3;
			public ushort Field4;
			public byte Field5;
			public byte Field6;
			public int Field7;
			public int Field8;
			public int Field9;
			public int FieldA;
			public int FieldB;
			public int FieldC;
			public int FieldD;
			public int FieldE;
		}
		public class _Spellvisualanim
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Spellvisualcoloreffect
		{
			public int ID;
			public float Field1;
			public int Field2;
			public float Field3;
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public ushort Field8;
			public byte Field9;
			public byte FieldA;
			public ushort FieldB;
		}
		public class _Spellvisualeffectname
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public int Field7;
			public int Field8;
			public int Field9;
			public byte FieldA;
			public int FieldB;
			public int FieldC;
			public ushort FieldD;
		}
		public class _Spellvisualevent
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public int Field3;
			public byte Field4;
			public int Field5;
			public int Field6;
			public byte Field7;
			public int Field8;
		}
		public class _Spellvisualkit
		{
			public int ID;
			public int Field1;
			public float Field2;
			public byte Field3;
			public ushort Field4;
			public int Field5;
		}
		public class _Spellvisualkitareamodel
		{
			public int ID;
			public int Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public ushort Field5;
			public byte Field6;
		}
		public class _Spellvisualkiteffect
		{
			public int ID;
			public int Field1;
			public int Field2;
		}
		public class _Spellvisualkitmodelattach
		{
			public float[] Field00 = new float[3];
			public float[] Field01 = new float[3];
			public int ID;
			public ushort Field03;
			public byte Field04;
			public byte Field05;
			public ushort Field06;
			public float Field07;
			public float Field08;
			public float Field09;
			public float Field0A;
			public float Field0B;
			public float Field0C;
			public float Field0D;
			public float Field0E;
			public int Field0F;
			public int Field10;
			public int Field11;
			public float Field12;
			public int Field13;
			public float Field14;
		}
		public class _Spellvisualmissile
		{
			public int Field0;
			public int Field1;
			public int Field2;
			public float[] Field3 = new float[3];
			public float[] Field4 = new float[3];
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public ushort Field8;
			public ushort Field9;
			public byte FieldA;
			public byte FieldB;
			public int ID;
			public int FieldD;
			public int FieldE;
		}
		public class _Spellxdescriptionvariables
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _SpellXSpellVisual
		{
			public uint SpellVisualID;
			public int ID;
			public float Chance;
			public ushort CasterPlayerConditionID;
			public ushort CasterUnitConditionID;
			public ushort PlayerConditionID;
			public ushort UnitConditionID;
			public uint IconFileDataID;
			public uint ActiveIconFileDataID;
			public byte Flags;
			public byte DifficultyID;
			public byte Priority;
		}
		public class _Startup_strings
		{
			public int ID;
			public string Field1;
			public string Field2;
		}
		public class _Startupfiles
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
		}
		public class _Stationery
		{
			public int ID;
			public byte Field1;
			public ushort Field2;
			public int[] Field3 = new int[2];
		}
		public class _SummonProperties
		{
			public int ID;
			public uint Flags;
			public uint Category;
			public uint Faction;
			public int Type;
			public int Slot;
		}
		public class _TactKey
		{
			public int ID;
			public byte[] Key = new byte[16];
		}
		public class _Tactkeylookup
		{
			public int ID;
			public byte[] Field1 = new byte[8];
		}
		public class _Talent
		{
			public int ID;
			public string Description;
			public uint SpellID;
			public uint OverridesSpellID;
			public ushort SpecID;
			public byte TierID;
			public byte ColumnIndex;
			public byte Flags;
			public byte[] CategoryMask = new byte[2];
			public byte ClassID;
		}
		public class _TaxiNodes
		{
			public int ID;
			public string Name;
			public float[] Pos = new float[3];
			public uint[] MountCreatureID = new uint[2];
			public float[] MapOffset = new float[2];
			public float Unk730;
			public float[] FlightMapOffset = new float[2];
			public ushort MapID;
			public ushort ConditionID;
			public ushort LearnableIndex;
			public byte Flags;
			public int UiTextureKitPrefixID;
			public uint SpecialAtlasIconPlayerConditionID;
		}
		public class _TaxiPath
		{
			public ushort From;
			public ushort To;
			public int ID;
			public uint Cost;
		}
		public class _TaxiPathNode
		{
			public float[] Loc = new float[3];
			public ushort PathID;
			public ushort MapID;
			public byte NodeIndex;
			public int ID;
			public byte Flags;
			public uint Delay;
			public ushort ArrivalEventID;
			public ushort DepartureEventID;
		}
		public class _Terrainmaterial
		{
			public int ID;
			public byte Field1;
			public int Field2;
			public int Field3;
		}
		public class _Terraintype
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Terraintypesounds
		{
			public int ID;
			public string Field1;
		}
		public class _Textureblendset
		{
			public int ID;
			public int[] Field1 = new int[3];
			public float[] Field2 = new float[3];
			public float[] Field3 = new float[3];
			public float[] Field4 = new float[3];
			public float[] Field5 = new float[3];
			public float[] Field6 = new float[4];
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
		}
		public class _Texturefiledata
		{
			public int ID;
			public int Field1;
			public byte Field2;
		}
		public class _TotemCategory
		{
			public int ID;
			public string Name;
			public uint CategoryMask;
			public byte CategoryType;
		}
		public class _Toy
		{
			public string Description;
			public uint ItemID;
			public byte Flags;
			public byte CategoryFilter;
			public int ID;
		}
		public class _Tradeskillcategory
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public byte Field5;
		}
		public class _Tradeskillitem
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
		}
		public class _Transformmatrix
		{
			public int ID;
			public float[] Field1 = new float[3];
			public int Field2;
			public float Field3;
			public float Field4;
			public float Field5;
		}
		public class _TransmogHoliday
		{
			public int ID;
			public int HolidayID;
		}
		public class _TransmogSet
		{
			public string Name;
			public ushort BaseSetID;
			public ushort UIOrder;
			public byte ExpansionID;
			public int ID;
			public int Flags;
			public int QuestID;
			public int ClassMask;
			public int ItemNameDescriptionID;
			public uint TransmogSetGroupID;
		}
		public class _TransmogSetGroup
		{
			public string Label;
			public int ID;
		}
		public class _TransmogSetItem
		{
			public int ID;
			public uint TransmogSetID;
			public uint ItemModifiedAppearanceID;
			public int Flags;
		}
		public class _TransportAnimation
		{
			public int ID;
			public uint TimeIndex;
			public float[] Pos = new float[3];
			public byte SequenceID;
		}
		public class _Transportphysics
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public float Field9;
			public float FieldA;
		}
		public class _TransportRotation
		{
			public int ID;
			public uint TimeIndex;
			public float X;
			public float Y;
			public float Z;
			public float W;
		}
		public class _Trophy
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public int Field4;
		}
		public class _Uicamera
		{
			public int ID;
			public string Field1;
			public float[] Field2 = new float[3];
			public float[] Field3 = new float[3];
			public float[] Field4 = new float[3];
			public ushort Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
		}
		public class _Uicameratype
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Uicamfbacktransmogchrrace
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Uicamfbacktransmogweapon
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Uiexpansiondisplayinfo
		{
			public int ID;
			public int Field1;
			public int Field2;
			public int Field3;
		}
		public class _Uiexpansiondisplayinfoicon
		{
			public int ID;
			public string Field1;
			public int Field2;
			public int Field3;
		}
		public class _Uimappoi
		{
			public int Field0;
			public float[] Field1 = new float[3];
			public int Field2;
			public int Field3;
			public ushort Field4;
			public ushort Field5;
			public int ID;
		}
		public class _Uimodelscene
		{
			public int ID;
			public byte Field1;
			public byte Field2;
		}
		public class _Uimodelsceneactor
		{
			public string Field0;
			public float[] Field1 = new float[3];
			public float Field2;
			public int Field3;
			public int Field4;
			public float Field5;
			public byte Field6;
			public int ID;
			public byte Field8;
		}
		public class _Uimodelsceneactordisplay
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public int Field4;
			public byte Field5;
		}
		public class _Uimodelscenecamera
		{
			public string Field0;
			public float[] Field1 = new float[3];
			public int[] Field2 = new int[3];
			public float Field3;
			public float Field4;
			public int Field5;
			public float Field6;
			public int Field7;
			public int Field8;
			public float Field9;
			public float FieldA;
			public float FieldB;
			public byte FieldC;
			public byte FieldD;
			public int ID;
		}
		public class _Uitextureatlas
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Uitextureatlasmember
		{
			public string Field0;
			public int ID;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public byte Field7;
		}
		public class _Uitexturekit
		{
			public int ID;
			public string Field1;
		}
		public class _Unitblood
		{
			public int ID;
			public ushort Field1;
			public int Field2;
			public int Field3;
			public int Field4;
			public uint Field5;
			public int Field6;
		}
		public class _Unitbloodlevels
		{
			public int ID;
			public byte[] Field1 = new byte[3];
		}
		public class _Unitcondition
		{
			public int ID;
			public int[] Field1 = new int[8];
			public byte Field2;
			public byte[] Field3 = new byte[8];
			public byte[] Field4 = new byte[8];
		}
		public class _UnitPowerBar
		{
			public int ID;
			public string Name;
			public string Cost;
			public string OutOfError;
			public string ToolTip;
			public float RegenerationPeace;
			public float RegenerationCombat;
			public uint[] FileDataID = new uint[6];
			public uint[] Color = new uint[6];
			public float StartInset;
			public float EndInset;
			public ushort StartPower;
			public ushort Flags;
			public byte CenterPower;
			public byte BarType;
			public uint MinPower;
			public uint MaxPower;
		}
		public class _Vehicle
		{
			public int ID;
			public uint Flags;
			public float TurnSpeed;
			public float PitchSpeed;
			public float PitchMin;
			public float PitchMax;
			public float MouseLookOffsetPitch;
			public float CameraFadeDistScalarMin;
			public float CameraFadeDistScalarMax;
			public float CameraPitchOffset;
			public float FacingLimitRight;
			public float FacingLimitLeft;
			public float CameraYawOffset;
			public ushort[] SeatID = new ushort[8];
			public ushort VehicleUIIndicatorID;
			public ushort[] PowerDisplayID = new ushort[3];
			public byte FlagsB;
			public byte UILocomotionType;
			public int MissileTargetingID;
		}
		public class _VehicleSeat
		{
			public int ID;
			public uint[] Flags = new uint[3];
			public float[] AttachmentOffset = new float[3];
			public float EnterPreDelay;
			public float EnterSpeed;
			public float EnterGravity;
			public float EnterMinDuration;
			public float EnterMaxDuration;
			public float EnterMinArcHeight;
			public float EnterMaxArcHeight;
			public float ExitPreDelay;
			public float ExitSpeed;
			public float ExitGravity;
			public float ExitMinDuration;
			public float ExitMaxDuration;
			public float ExitMinArcHeight;
			public float ExitMaxArcHeight;
			public float PassengerYaw;
			public float PassengerPitch;
			public float PassengerRoll;
			public float VehicleEnterAnimDelay;
			public float VehicleExitAnimDelay;
			public float CameraEnteringDelay;
			public float CameraEnteringDuration;
			public float CameraExitingDelay;
			public float CameraExitingDuration;
			public float[] CameraOffset = new float[3];
			public float CameraPosChaseRate;
			public float CameraFacingChaseRate;
			public float CameraEnteringZoom;
			public float CameraSeatZoomMin;
			public float CameraSeatZoomMax;
			public uint UISkinFileDataID;
			public short EnterAnimStart;
			public short EnterAnimLoop;
			public short RideAnimStart;
			public short RideAnimLoop;
			public short RideUpperAnimStart;
			public short RideUpperAnimLoop;
			public short ExitAnimStart;
			public short ExitAnimLoop;
			public short ExitAnimEnd;
			public short VehicleEnterAnim;
			public short VehicleExitAnim;
			public short VehicleRideAnimLoop;
			public ushort EnterAnimKitID;
			public ushort RideAnimKitID;
			public ushort ExitAnimKitID;
			public ushort VehicleEnterAnimKitID;
			public ushort VehicleRideAnimKitID;
			public ushort VehicleExitAnimKitID;
			public ushort CameraModeID;
			public byte AttachmentID;
			public byte PassengerAttachmentID;
			public byte VehicleEnterAnimBone;
			public byte VehicleExitAnimBone;
			public byte VehicleRideAnimLoopBone;
			public byte VehicleAbilityDisplay;
			public uint EnterUISoundID;
			public uint ExitUISoundID;
		}
		public class _Vehicleuiindicator
		{
			public int ID;
			public int Field1;
		}
		public class _Vehicleuiindseat
		{
			public int ID;
			public float Field1;
			public float Field2;
			public byte Field3;
		}
		public class _Vignette
		{
			public int ID;
			public string Field1;
			public float Field2;
			public float Field3;
			public int Field4;
			public byte Field5;
			public ushort Field6;
			public ushort Field7;
		}
		public class _Virtualattachment
		{
			public int ID;
			public string Field1;
			public ushort Field2;
		}
		public class _Virtualattachmentcustomization
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Vocaluisounds
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public int[] Field4 = new int[2];
		}
		public class _Wbaccesscontrollist
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
		}
		public class _Wbcertwhitelist
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _Weaponimpactsounds
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public byte Field3;
			public int[] Field4 = new int[11];
			public int[] Field5 = new int[11];
			public int[] Field6 = new int[11];
			public int[] Field7 = new int[11];
		}
		public class _Weaponswingsounds2
		{
			public int ID;
			public byte Field1;
			public byte Field2;
			public int Field3;
		}
		public class _Weapontrail
		{
			public int ID;
			public int Field1;
			public float Field2;
			public int Field3;
			public int Field4;
			public int[] Field5 = new int[3];
			public float[] Field6 = new float[3];
			public float[] Field7 = new float[3];
			public float[] Field8 = new float[3];
			public float[] Field9 = new float[3];
		}
		public class _Weapontrailmodeldef
		{
			public int ID;
			public int Field1;
			public ushort Field2;
		}
		public class _Weapontrailparam
		{
			public int ID;
			public float Field1;
			public float Field2;
			public float Field3;
			public float Field4;
			public float Field5;
			public byte Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
		}
		public class _Weather
		{
			public int ID;
			public float[] Field1 = new float[2];
			public float Field2;
			public float[] Field3 = new float[3];
			public float Field4;
			public float Field5;
			public float Field6;
			public float Field7;
			public float Field8;
			public ushort Field9;
			public byte FieldA;
			public byte FieldB;
			public byte FieldC;
			public ushort FieldD;
			public int FieldE;
		}
		public class _Windsettings
		{
			public int ID;
			public float Field1;
			public float[] Field2 = new float[3];
			public float Field3;
			public float Field4;
			public float[] Field5 = new float[3];
			public float Field6;
			public float[] Field7 = new float[3];
			public int Field8;
			public int Field9;
			public byte FieldA;
		}
		public class _WMOAreaTable
		{
			public string AreaName;
			public int WMOGroupID;
			public ushort AmbienceID;
			public ushort ZoneMusic;
			public ushort IntroSound;
			public ushort AreaTableID;
			public ushort UWIntroSound;
			public ushort UWAmbience;
			public byte NameSet;
			public byte SoundProviderPref;
			public byte SoundProviderPrefUnderwater;
			public byte Flags;
			public int ID;
			public uint UWZoneMusic;
		}
		public class _Wmominimaptexture
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public byte Field3;
			public byte Field4;
		}
		public class _World_pvp_area
		{
			public int ID;
			public ushort Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public byte Field6;
			public byte Field7;
		}
		public class _Worldbosslockout
		{
			public int ID;
			public string Field1;
			public ushort Field2;
		}
		public class _Worldchunksounds
		{
			public int ID;
			public ushort Field1;
			public byte Field2;
			public byte Field3;
			public byte Field4;
			public byte Field5;
			public byte Field6;
		}
		public class _WorldEffect
		{
			public int ID;
			public uint TargetAsset;
			public ushort CombatConditionID;
			public byte TargetType;
			public byte WhenToDisplay;
			public uint QuestFeedbackEffectID;
			public uint PlayerConditionID;
		}
		public class _Worldelapsedtimer
		{
			public int ID;
			public string Field1;
			public byte Field2;
			public byte Field3;
		}
		public class _WorldMapArea
		{
			public string AreaName;
			public float LocLeft;
			public float LocRight;
			public float LocTop;
			public float LocBottom;
			public uint Flags;
			public ushort MapID;
			public ushort AreaID;
			public ushort DisplayMapID;
			public short DefaultDungeonFloor;
			public ushort ParentWorldMapID;
			public byte LevelRangeMin;
			public byte LevelRangeMax;
			public byte BountySetID;
			public byte BountyBoardLocation;
			public int ID;
			public uint PlayerConditionID;
		}
		public class _Worldmapcontinent
		{
			public int ID;
			public float[] Field1 = new float[2];
			public float Field2;
			public float[] Field3 = new float[2];
			public float[] Field4 = new float[2];
			public ushort Field5;
			public ushort Field6;
			public byte Field7;
			public byte Field8;
			public byte Field9;
			public byte FieldA;
			public byte FieldB;
		}
		public class _WorldMapOverlay
		{
			public string TextureName;
			public int ID;
			public ushort TextureWidth;
			public ushort TextureHeight;
			public uint MapAreaID;
			public uint[] AreaID = new uint[4];
			public int OffsetX;
			public int OffsetY;
			public int HitRectTop;
			public int HitRectLeft;
			public int HitRectBottom;
			public int HitRectRight;
			public uint PlayerConditionID;
			public uint Flags;
		}
		public class _WorldMapTransforms
		{
			public int ID;
			public float[] RegionMin = new float[3];
			public float[] RegionMax = new float[3];
			public float[] RegionOffset = new float[2];
			public float RegionScale;
			public ushort MapID;
			public ushort AreaID;
			public ushort NewMapID;
			public ushort NewDungeonMapID;
			public ushort NewAreaID;
			public byte Flags;
			public int Priority;
		}
		public class _WorldSafeLocs
		{
			public int ID;
			public string AreaName;
			public float[] Loc = new float[3];
			public float Facing;
			public ushort MapID;
		}
		public class _Worldstate
		{
			public int ID;
		}
		public class _Worldstateexpression
		{
			public int ID;
			public string Field1;
		}
		public class _Worldstateui
		{
			public string Field00;
			public string Field01;
			public string Field02;
			public string Field03;
			public string Field04;
			public ushort Field05;
			public ushort Field06;
			public ushort Field07;
			public ushort Field08;
			public ushort Field09;
			public ushort[] Field0A = new ushort[3];
			public byte Field0B;
			public byte Field0C;
			public byte Field0D;
			public int ID;
			public int Field0F;
			public int Field10;
		}
		public class _Worldstatezonesounds
		{
			public int ID;
			public int Field1;
			public ushort Field2;
			public ushort Field3;
			public ushort Field4;
			public ushort Field5;
			public ushort Field6;
			public ushort Field7;
			public byte Field8;
		}
		public class _Zoneintromusictable
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public byte Field3;
			public uint Field4;
		}
		public class _Zonelight
		{
			public int ID;
			public string Field1;
			public ushort Field2;
			public ushort Field3;
		}
		public class _Zonelightpoint
		{
			public int ID;
			public float[] Field1 = new float[2];
			public byte Field2;
		}
		public class _Zonemusic
		{
			public int ID;
			public string Field1;
			public int[] Field2 = new int[2];
			public int[] Field3 = new int[2];
			public int[] Field4 = new int[2];
		}
		public class _Zonestory
		{
			public int ID;
			public int Field1;
			public int Field2;
			public byte Field3;
		}
	}
}
