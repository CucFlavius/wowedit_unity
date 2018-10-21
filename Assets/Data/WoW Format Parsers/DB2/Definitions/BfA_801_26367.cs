public static partial class DB2
{
	public static class Definitions_BfA_801_26367
	{
		public class _Achievement
		{
			public string Description;
			public string Title;
			public string Reward;
			public int Flags;
			public ushort InstanceId;
			public ushort Supercedes;
			public ushort Category;
			public ushort UiOrder;
			public ushort SharesCriteria;
			public byte Faction;
			public byte MinimumCriteria;
			public byte Points;
			public int ID;
			public int IconFileId;
			public int CriteriaTree;
		}
		public class _Achievement_Category
		{
			public string Name;
			public ushort Parent;
			public byte UiOrder;
			public int ID;
		}
		public class _AdventureJournal
		{
			public int ID;
			public string Name;
			public string Description;
			public string ButtonText;
			public string RewardDescription;
			public string ContinueDescription;
			public int TextureFileDataId;
			public int[] BonusPlayerConditionId = new int[2];
			public int ItemId;
			public ushort LfgDungeonId;
			public ushort QuestId;
			public ushort BattleMasterListId;
			public ushort CurrencyType;
			public ushort WorldMapAreaId;
			public byte Type;
			public byte Flags;
			public byte ButtonActionType;
			public byte PriorityMin;
			public byte PriorityMax;
			public byte[] BonusValue = new byte[2];
			public byte CurrencyQuantity;
			public int PlayerConditionId;
			public int ItemQuantity;
		}
		public class _AdventureMapPOI
		{
			public int ID;
			public string Title;
			public string Description;
			public float[] WorldPosition = new float[2];
			public int RewardItemId;
			public uint MapId;
			public byte Type;
			public int PlayerConditionId;
			public int QuestId;
			public int LfgDungeonId;
			public int UiTextureAtlasMemberId;
			public int UiTextureKitId;
			public int AreaTableId;
		}
		public class _AlliedRace
		{
			public int BannerColor;
			public int ID;
			public int RaceId;
			public int CrestTextureId;
			public int ModelBackgroundTextureId;
			public int MaleCreatureDisplayId;
			public int FemaleCreatureDisplayId;
			public int UiUnlockAchievementId;
		}
		public class _AlliedRaceRacialAbility
		{
			public int ID;
			public string Name;
			public string Description;
			public byte OrderIndex;
			public int IconFileDataId;
		}
		public class _AnimationData
		{
			public int ID;
			public uint[] Flags = new uint[2];
			public ushort BehaviorId;
			public ushort Fallback;
			public byte BehaviorTier;
		}
		public class _AnimKit
		{
			public int ID;
			public int OneShotDuration;
			public ushort OneShotStopAnimKitId;
			public ushort LowDefAnimKitId;
		}
		public class _AnimKitBoneSet
		{
			public int ID;
			public string Name;
			public byte BoneDataId;
			public byte ParentAnimKitBoneSetId;
			public byte ExtraBoneCount;
			public byte AltAnimKitBoneSetId;
		}
		public class _AnimKitBoneSetAlias
		{
			public int ID;
			public byte BoneDataId;
			public byte AnimKitBoneSetId;
		}
		public class _AnimKitConfig
		{
			public int ID;
			public int ConfigFlags;
		}
		public class _AnimKitConfigBoneSet
		{
			public int ID;
			public ushort AnimKitPriorityId;
			public byte AnimKitBoneSetId;
		}
		public class _AnimKitPriority
		{
			public int ID;
			public byte Priority;
		}
		public class _AnimKitReplacement
		{
			public ushort SrcAnimKitId;
			public ushort DstAnimKitId;
			public ushort Flags;
			public int ID;
		}
		public class _AnimKitSegment
		{
			public int ID;
			public int AnimStartTime;
			public int EndConditionParam;
			public int EndConditionDelay;
			public float Speed;
			public uint OverrideConfigFlags;
			public ushort ParentAnimKitId;
			public ushort AnimId;
			public ushort AnimKitConfigId;
			public ushort SegmentFlags;
			public ushort BlendInTimeMs;
			public ushort BlendOutTimeMs;
			public byte OrderIndex;
			public byte StartCondition;
			public byte StartConditionParam;
			public byte EndCondition;
			public byte ForcedVariation;
			public byte LoopToSegmentIndex;
			public int StartConditionDelay;
		}
		public class _AnimReplacement
		{
			public ushort SrcAnimId;
			public ushort DstAnimId;
			public ushort Flags;
			public int ID;
		}
		public class _AnimReplacementSet
		{
			public int ID;
			public byte ExecOrder;
		}
		public class _AreaFarClipOverride
		{
			public int AreaId;
			public float MinFarClip;
			public float MinHorizonStart;
			public int Flags;
			public int ID;
		}
		public class _AreaGroupMember
		{
			public int ID;
			public ushort AreaId;
		}
		public class _AreaPOI
		{
			public string Name;
			public string Description;
			public int ID;
			public float[] Pos = new float[3];
			public int Flags;
			public int PoiDataType;
			public int PoiData;
			public ushort ContinentId;
			public ushort AreaId;
			public ushort WorldStateId;
			public byte Importance;
			public byte Icon;
			public int PortLocId;
			public int PlayerConditionId;
			public int UiTextureAtlasMemberId;
			public int WmoGroupId;
		}
		public class _AreaPOIState
		{
			public int ID;
			public string Description;
			public byte WorldStateValue;
			public byte IconEnumValue;
			public int UiTextureAtlasMemberId;
		}
		public class _AreaTable
		{
			public int ID;
			public string ZoneName;
			public string AreaName;
			public uint[] Flags = new uint[2];
			public float AmbientMultiplier;
			public ushort ContinentId;
			public ushort ParentAreaId;
			public ushort AreaBit;
			public ushort AmbienceId;
			public ushort UwAmbience;
			public ushort ZoneMusic;
			public ushort UwZoneMusic;
			public ushort IntroSound;
			public ushort[] LiquidTypeId = new ushort[4];
			public ushort PvpCombatWorldStateId;
			public byte SoundProviderPref;
			public byte SoundProviderPrefUnderwater;
			public byte ExplorationLevel;
			public byte FactionGroupMask;
			public byte MountFlags;
			public byte WildBattlePetLevelMin;
			public byte WildBattlePetLevelMax;
			public byte WindSettingsId;
			public int UwIntroSound;
		}
		public class _AreaTrigger
		{
			public float[] Pos = new float[3];
			public float Radius;
			public float BoxLength;
			public float BoxWidth;
			public float BoxHeight;
			public float BoxYaw;
			public ushort ContinentId;
			public ushort PhaseId;
			public ushort PhaseGroupId;
			public ushort ShapeId;
			public ushort AreaTriggerActionSetId;
			public byte PhaseUseFlags;
			public byte ShapeType;
			public byte Flags;
			public int ID;
		}
		public class _AreaTriggerActionSet
		{
			public int ID;
			public ushort Flags;
		}
		public class _AreaTriggerBox
		{
			public int ID;
			public float[] Extents = new float[3];
		}
		public class _AreaTriggerCreateProperties
		{
			public int ID;
			public ushort StartShapeId;
			public byte ShapeType;
		}
		public class _AreaTriggerCylinder
		{
			public int ID;
			public float Radius;
			public float Height;
			public float ZOffset;
		}
		public class _AreaTriggerSphere
		{
			public int ID;
			public float MaxRadius;
		}
		public class _ArmorLocation
		{
			public int ID;
			public float Clothmodifier;
			public float Leathermodifier;
			public float Chainmodifier;
			public float Platemodifier;
			public float Modifier;
		}
		public class _Artifact
		{
			public int ID;
			public string Name;
			public uint UiNameColor;
			public uint UiBarOverlayColor;
			public int UiBarBackgroundColor;
			public ushort UiTextureKitId;
			public ushort ChrSpecializationId;
			public byte Flags;
			public byte ArtifactCategoryId;
			public int UiModelSceneId;
			public int SpellVisualKitId;
		}
		public class _ArtifactAppearance
		{
			public string Name;
			public uint UiSwatchColor;
			public float UiModelSaturation;
			public float UiModelOpacity;
			public int OverrideShapeshiftDisplayId;
			public ushort ArtifactAppearanceSetId;
			public ushort UiCameraId;
			public byte DisplayIndex;
			public byte ItemAppearanceModifierId;
			public byte OverrideShapeshiftFormId;
			public byte Flags;
			public int ID;
			public int UnlockPlayerConditionId;
			public int UiItemAppearanceId;
			public int UiAltItemAppearanceId;
		}
		public class _ArtifactAppearanceSet
		{
			public string Name;
			public string Description;
			public ushort UiCameraId;
			public ushort AltHandUICameraId;
			public byte DisplayIndex;
			public byte ForgeAttachmentOverride;
			public byte Flags;
			public int ID;
		}
		public class _ArtifactCategory
		{
			public int ID;
			public ushort XpMultCurrencyId;
			public ushort XpMultCurveId;
		}
		public class _ArtifactPower
		{
			public float[] DisplayPos = new float[2];
			public byte ArtifactId;
			public byte MaxPurchasableRank;
			public byte Flags;
			public byte Tier;
			public int ID;
			public int Label;
		}
		public class _ArtifactPowerLink
		{
			public int ID;
			public ushort PowerA;
			public ushort PowerB;
		}
		public class _ArtifactPowerPicker
		{
			public int ID;
			public int PlayerConditionId;
		}
		public class _ArtifactPowerRank
		{
			public int ID;
			public int SpellId;
			public float AuraPointsOverride;
			public ushort ItemBonusListId;
			public byte RankIndex;
		}
		public class _ArtifactQuestXP
		{
			public int ID;
			public int[] Difficulty = new int[10];
		}
		public class _ArtifactTier
		{
			public int ID;
			public int ArtifactTier;
			public int MaxNumTraits;
			public int MaxArtifactKnowledge;
			public int KnowledgePlayerCondition;
			public int MinimumEmpowerKnowledge;
		}
		public class _ArtifactUnlock
		{
			public int ID;
			public ushort ItemBonusListId;
			public byte PowerRank;
			public int PowerId;
			public int PlayerConditionId;
		}
		public class _AuctionHouse
		{
			public int ID;
			public string Name;
			public ushort FactionId;
			public byte DepositRate;
			public byte ConsignmentRate;
		}
		public class _AzeriteEmpoweredItem
		{
			public int ID;
			public int ItemId;
			public int AzeriteTierUnlockSetId;
			public int AzeritePowerSetId;
		}
		public class _AzeriteItem
		{
			public int ID;
			public int ItemId;
		}
		public class _AzeriteItemMilestonePower
		{
			public int ID;
			public ushort AzeritePowerId;
			public byte RequiredLevel;
		}
		public class _AzeritePower
		{
			public int ID;
			public int SpellId;
			public int ItemBonusListId;
		}
		public class _AzeritePowerSetMember
		{
			public int ID;
			public ushort AzeritePowerId;
			public byte Class;
			public byte Tier;
			public byte OrderIndex;
		}
		public class _AzeriteTierUnlock
		{
			public int ID;
			public byte ItemCreationContext;
			public byte Tier;
			public byte AzeriteLevel;
		}
		public class _BankBagSlotPrices
		{
			public int ID;
			public int Cost;
		}
		public class _BannedAddOns
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
			public int IconFileDataId;
			public ushort[] MapId = new ushort[16];
			public ushort HolidayWorldState;
			public ushort RequiredPlayerConditionId;
			public byte InstanceType;
			public byte MinLevel;
			public byte MaxLevel;
			public byte RatedPlayers;
			public byte MinPlayers;
			public byte MaxPlayers;
			public byte GroupsAllowed;
			public byte MaxGroupSize;
			public byte Flags;
		}
		public class _BattlePetAbility
		{
			public int ID;
			public string Name;
			public string Description;
			public int IconFileDataId;
			public ushort BattlePetVisualId;
			public byte PetTypeEnum;
			public byte Flags;
			public int Cooldown;
		}
		public class _BattlePetAbilityEffect
		{
			public ushort BattlePetAbilityTurnId;
			public ushort BattlePetEffectPropertiesId;
			public ushort AuraBattlePetAbilityId;
			public ushort BattlePetVisualId;
			public ushort[] Param = new ushort[6];
			public byte OrderIndex;
			public int ID;
		}
		public class _BattlePetAbilityState
		{
			public int ID;
			public uint Value;
			public byte BattlePetStateId;
		}
		public class _BattlePetAbilityTurn
		{
			public ushort BattlePetAbilityId;
			public ushort BattlePetVisualId;
			public byte OrderIndex;
			public byte TurnTypeEnum;
			public byte EventTypeEnum;
			public int ID;
		}
		public class _BattlePetBreedQuality
		{
			public int ID;
			public float StateMultiplier;
			public byte QualityEnum;
		}
		public class _BattlePetBreedState
		{
			public int ID;
			public ushort Value;
			public byte BattlePetStateId;
		}
		public class _BattlePetDisplayOverride
		{
			public int ID;
			public int BattlePetSpeciesId;
			public int PlayerConditionId;
			public int CreatureDisplayInfoId;
			public byte PriorityCategory;
		}
		public class _BattlePetEffectProperties
		{
			public int ID;
			public string[] ParamLabel = new string[6];
			public ushort BattlePetVisualId;
			public byte[] ParamTypeEnum = new byte[6];
		}
		public class _BattlePetNPCTeamMember
		{
			public int ID;
			public string Name;
		}
		public class _BattlePetSpecies
		{
			public string Description;
			public string SourceText;
			public int CreatureId;
			public int SummonSpellId;
			public int IconFileDataId;
			public ushort Flags;
			public byte PetTypeEnum;
			public byte SourceTypeEnum;
			public int ID;
			public int CardUIModelSceneId;
			public int LoadoutUIModelSceneId;
		}
		public class _BattlePetSpeciesState
		{
			public int ID;
			public uint Value;
			public byte BattlePetStateId;
		}
		public class _BattlePetSpeciesXAbility
		{
			public int ID;
			public ushort BattlePetAbilityId;
			public byte RequiredLevel;
			public byte SlotEnum;
		}
		public class _BattlePetState
		{
			public int ID;
			public string LuaName;
			public ushort Flags;
			public ushort BattlePetVisualId;
		}
		public class _BattlePetVisual
		{
			public int ID;
			public string SceneScriptFunction;
			public int SpellVisualId;
			public ushort CastMilliSeconds;
			public ushort ImpactMilliSeconds;
			public ushort SceneScriptPackageId;
			public byte RangeTypeEnum;
			public byte Flags;
		}
		public class _BeamEffect
		{
			public int ID;
			public int BeamId;
			public float SourceMinDistance;
			public float FixedLength;
			public int Flags;
			public int SourceOffset;
			public int DestOffset;
			public int SourceAttachId;
			public int DestAttachId;
			public int SourcePositionerId;
			public int DestPositionerId;
		}
		public class _BoneWindModifierModel
		{
			public int ID;
			public int FileDataId;
			public int BoneWindModifierId;
		}
		public class _BoneWindModifiers
		{
			public int ID;
			public float[] Multiplier = new float[3];
			public float PhaseMultiplier;
		}
		public class _BonusRoll
		{
			public int ID;
			public int CurrencyTypesId;
			public int CurrencyCost;
			public int JournalEncounterId;
			public int JournalInstanceId;
		}
		public class _Bounty
		{
			public int ID;
			public int IconFileDataId;
			public ushort QuestId;
			public ushort FactionId;
			public int TurninPlayerConditionId;
		}
		public class _BountySet
		{
			public int ID;
			public ushort LockedQuestId;
			public int VisiblePlayerConditionId;
		}
		public class _BroadcastText
		{
			public string Text;
			public string Text1;
			public int ID;
			public int[] SoundEntriesId = new int[2];
			public ushort EmotesId;
			public ushort[] EmoteId = new ushort[3];
			public ushort[] EmoteDelay = new ushort[3];
			public byte LanguageId;
			public byte Flags;
			public int ConditionId;
		}
		public class _BroadcastTextVOState
		{
			public int ID;
			public byte State;
		}
		public class _CameraEffect
		{
			public int ID;
			public byte Flags;
		}
		public class _CameraEffectEntry
		{
			public int ID;
			public float Duration;
			public float Delay;
			public float Phase;
			public float Amplitude;
			public float AmplitudeB;
			public float Frequency;
			public float RadiusMin;
			public float RadiusMax;
			public ushort AmplitudeCurveId;
			public byte OrderIndex;
			public byte Flags;
			public byte EffectType;
			public byte DirectionType;
			public byte MovementType;
			public byte AttenuationType;
		}
		public class _CameraMode
		{
			public int ID;
			public float[] PositionOffset = new float[3];
			public float[] TargetOffset = new float[3];
			public float PositionSmoothing;
			public float RotationSmoothing;
			public float FieldOfView;
			public ushort Flags;
			public byte Type;
			public byte LockedPositionOffsetBase;
			public byte LockedPositionOffsetDirection;
			public byte LockedTargetOffsetBase;
			public byte LockedTargetOffsetDirection;
		}
		public class _CastableRaidBuffs
		{
			public int ID;
			public int CastingSpellId;
		}
		public class _CelestialBody
		{
			public int BaseFileDataId;
			public float BodyBaseScale;
			public float[] Position = new float[3];
			public int[] GlowMaskFileDataId = new int[2];
			public float[] GlowMaskScale = new float[2];
			public int[] GlowModifiedFileDataId = new int[2];
			public int AtmosphericMaskFileDataId;
			public int AtmosphericModifiedFileDataId;
			public float[] ScrollURate = new float[2];
			public float[] ScrollVRate = new float[2];
			public float RotateRate;
			public float AtmosphericMaskScale;
			public int LightMaskFileDataId;
			public ushort SkyArrayBand;
			public int ID;
		}
		public class _Cfg_Categories
		{
			public int ID;
			public string Name;
			public ushort LocaleMask;
			public byte CreateCharsetMask;
			public byte ExistingCharsetMask;
			public byte Flags;
		}
		public class _Cfg_Configs
		{
			public int ID;
			public float MaxDamageReductionPctPhysical;
			public ushort PlayerAttackSpeedBase;
			public byte PlayerKillingAllowed;
			public byte Roleplaying;
		}
		public class _Cfg_Regions
		{
			public int ID;
			public string Tag;
			public int Raidorigin;
			public int ChallengeOrigin;
			public ushort RegionId;
			public byte RegionGroupMask;
		}
		public class _CharacterFaceBoneSet
		{
			public int ID;
			public int ModelFileDataId;
			public int BoneSetFileDataId;
			public byte SexId;
			public byte FaceVariationIndex;
		}
		public class _CharacterFacialHairStyles
		{
			public int ID;
			public uint[] Geoset = new uint[5];
			public byte RaceId;
			public byte SexId;
			public byte VariationId;
		}
		public class _CharacterLoadout
		{
			public int ID;
			public ulong RaceMask;
			public byte ChrClassId;
			public byte Purpose;
		}
		public class _CharacterLoadoutItem
		{
			public int ID;
			public int ItemId;
			public ushort CharacterLoadoutId;
		}
		public class _CharacterServiceInfo
		{
			public int ID;
			public string FlowTitle;
			public string PopupTitle;
			public string PopupDescription;
			public int BoostType;
			public int IconFileDataId;
			public int Priority;
			public int Flags;
			public int ProfessionLevel;
			public int BoostLevel;
			public int Expansion;
			public int PopupUITextureKitId;
		}
		public class _CharBaseInfo
		{
			public int ID;
			public byte RaceId;
			public byte ClassId;
		}
		public class _CharBaseSection
		{
			public int ID;
			public byte LayoutResType;
			public byte VariationEnum;
			public byte ResolutionVariationEnum;
		}
		public class _CharComponentTextureLayouts
		{
			public int ID;
			public ushort Width;
			public ushort Height;
		}
		public class _CharComponentTextureSections
		{
			public int ID;
			public int OverlapSectionMask;
			public ushort X;
			public ushort Y;
			public ushort Width;
			public ushort Height;
			public byte CharComponentTextureLayoutId;
			public byte SectionType;
		}
		public class _CharHairGeosets
		{
			public int ID;
			public int HdCustomGeoFileDataId;
			public byte RaceId;
			public byte SexId;
			public byte VariationId;
			public byte GeosetId;
			public byte Showscalp;
			public byte VariationType;
			public byte GeosetType;
			public byte ColorIndex;
			public int CustomGeoFileDataId;
		}
		public class _CharSections
		{
			public int ID;
			public int[] MaterialResourcesId = new int[3];
			public ushort Flags;
			public byte RaceId;
			public byte SexId;
			public byte BaseSection;
			public byte VariationIndex;
			public byte ColorIndex;
		}
		public class _CharShipment
		{
			public int ID;
			public int DummyItemId;
			public int TreasureId;
			public int SpellId;
			public int OnCompleteSpellId;
			public int Duration;
			public ushort ContainerId;
			public ushort GarrFollowerId;
			public byte MaxShipments;
			public byte Flags;
		}
		public class _CharShipmentContainer
		{
			public int ID;
			public string Description;
			public string PendingText;
			public int WorkingSpellVisualId;
			public ushort UiTextureKitId;
			public ushort SmallDisplayInfoId;
			public ushort MediumDisplayInfoId;
			public ushort LargeDisplayInfoId;
			public ushort WorkingDisplayInfoId;
			public ushort CrossFactionId;
			public byte GarrTypeId;
			public byte GarrBuildingType;
			public byte BaseCapacity;
			public byte MediumThreshold;
			public byte LargeThreshold;
			public byte Faction;
			public int CompleteSpellVisualId;
		}
		public class _CharStartOutfit
		{
			public int ID;
			public int[] ItemId = new int[24];
			public int PetDisplayId;
			public byte ClassId;
			public byte SexId;
			public byte OutfitId;
			public byte PetFamilyId;
		}
		public class _CharTitles
		{
			public int ID;
			public string Name;
			public string Name1;
			public ushort MaskId;
			public byte Flags;
		}
		public class _ChatChannels
		{
			public int ID;
			public string Name;
			public string Shortcut;
			public int Flags;
			public byte FactionGroup;
		}
		public class _ChatProfanity
		{
			public int ID;
			public string Text;
			public byte Language;
		}
		public class _ChrClasses
		{
			public string Name;
			public string Filename;
			public string NameMale;
			public string NameFemale;
			public string PetNameToken;
			public int CreateScreenFileDataId;
			public int SelectScreenFileDataId;
			public int IconFileDataId;
			public int LowResScreenFileDataId;
			public int StartingLevel;
			public ushort Flags;
			public ushort CinematicSequenceId;
			public ushort DefaultSpec;
			public byte PrimaryStatPriority;
			public byte DisplayPower;
			public byte RangedAttackPowerPerAgility;
			public byte AttackPowerPerAgility;
			public byte AttackPowerPerStrength;
			public byte SpellClassSet;
			public int ID;
		}
		public class _ChrClassesXPowerTypes
		{
			public int ID;
			public byte PowerType;
		}
		public class _ChrClassRaceSex
		{
			public int ID;
			public byte ClassId;
			public byte RaceId;
			public byte Sex;
			public int Flags;
			public int SoundId;
			public int VoiceSoundFilterId;
		}
		public class _ChrClassTitle
		{
			public int ID;
			public string NameMale;
			public string NameFemale;
			public byte ChrClassId;
		}
		public class _ChrClassUIDisplay
		{
			public int ID;
			public byte ChrClassesId;
			public int AdvGuidePlayerConditionId;
			public int SplashPlayerConditionId;
		}
		public class _ChrClassVillain
		{
			public int ID;
			public string Name;
			public byte ChrClassId;
			public byte Gender;
		}
		public class _ChrCustomization
		{
			public int ID;
			public string Name;
			public int Sex;
			public int BaseSection;
			public int UiCustomizationType;
			public int Flags;
			public int[] ComponentSection = new int[3];
		}
		public class _ChrRaces
		{
			public string ClientPrefix;
			public string ClientFileString;
			public string Name;
			public string NameFemale;
			public string NameLowercase;
			public string NameFemaleLowercase;
			public int Flags;
			public int MaleDisplayId;
			public int FemaleDisplayId;
			public int CreateScreenFileDataId;
			public int SelectScreenFileDataId;
			public float[] MaleCustomizeOffset = new float[3];
			public float[] FemaleCustomizeOffset = new float[3];
			public int LowResScreenFileDataId;
			public int StartingLevel;
			public int UiDisplayOrder;
			public float MaleModelFallbackArmor2Scale;
			public float FemaleModelFallbackArmor2Scale;
			public ushort FactionId;
			public ushort CinematicSequenceId;
			public ushort ResSicknessSpellId;
			public ushort SplashSoundId;
			public byte BaseLanguage;
			public byte CreatureType;
			public byte Alliance;
			public byte RaceRelated;
			public byte UnalteredVisualRaceId;
			public byte CharComponentTextureLayoutId;
			public byte CharComponentTexLayoutHiResId;
			public byte DefaultClassId;
			public byte NeutralRaceId;
			public byte MaleModelFallbackRaceId;
			public byte MaleModelFallbackSex;
			public byte FemaleModelFallbackRaceId;
			public byte FemaleModelFallbackSex;
			public byte MaleTextureFallbackRaceId;
			public byte MaleTextureFallbackSex;
			public byte FemaleTextureFallbackRaceId;
			public byte FemaleTextureFallbackSex;
			public int ID;
			public int HighResMaleDisplayId;
			public int HighResFemaleDisplayId;
			public int HeritageArmorAchievementId;
			public int FemaleSkeletonFileDataId;
			public int MaleSkeletonFileDataId;
			public int[] AlteredFormStartVisualKitId = new int[3];
			public int[] AlteredFormFinishVisualKitId = new int[3];
		}
		public class _ChrSpecialization
		{
			public string Name;
			public string FemaleName;
			public string Description;
			public int[] MasterySpellId = new int[2];
			public byte ClassId;
			public byte OrderIndex;
			public byte PetTalentType;
			public byte Role;
			public byte PrimaryStatPriority;
			public int ID;
			public int Flags;
			public int SpellIconFileId;
			public int AnimReplacements;
		}
		public class _ChrUpgradeBucket
		{
			public ushort ChrSpecializationId;
			public int ID;
		}
		public class _ChrUpgradeBucketSpell
		{
			public int ID;
			public int SpellId;
		}
		public class _ChrUpgradeTier
		{
			public string DisplayName;
			public byte OrderIndex;
			public byte NumTalents;
			public int ID;
		}
		public class _CinematicCamera
		{
			public int ID;
			public int SoundId;
			public float[] Origin = new float[3];
			public float OriginFacing;
			public int FileDataId;
		}
		public class _CinematicSequences
		{
			public int ID;
			public int SoundId;
			public ushort[] Camera = new ushort[8];
		}
		public class _ClientSceneEffect
		{
			public int ID;
			public int SceneScriptPackageId;
		}
		public class _CloakDampening
		{
			public int ID;
			public float[] Angle = new float[5];
			public float[] Dampening = new float[5];
			public float[] TailAngle = new float[2];
			public float[] TailDampening = new float[2];
			public float TabardAngle;
			public float TabardDampening;
			public float ExpectedWeaponSize;
		}
		public class _CombatCondition
		{
			public int ID;
			public ushort WorldStateExpressionId;
			public ushort SelfConditionId;
			public ushort TargetConditionId;
			public ushort[] FriendConditionId = new ushort[2];
			public ushort[] EnemyConditionId = new ushort[2];
			public byte[] FriendConditionOp = new byte[2];
			public byte[] FriendConditionCount = new byte[2];
			public byte FriendConditionLogic;
			public byte[] EnemyConditionOp = new byte[2];
			public byte[] EnemyConditionCount = new byte[2];
			public byte EnemyConditionLogic;
		}
		public class _CommentatorStartLocation
		{
			public int ID;
			public float[] Pos = new float[3];
			public int MapId;
		}
		public class _CommentatorTrackedCooldown
		{
			public int ID;
			public byte Priority;
			public byte Flags;
			public int SpellId;
		}
		public class _ComponentModelFileData
		{
			public int ID;
			public byte GenderIndex;
			public byte ClassId;
			public byte RaceId;
			public byte PositionIndex;
		}
		public class _ComponentTextureFileData
		{
			public int ID;
			public byte GenderIndex;
			public byte ClassId;
			public byte RaceId;
		}
		public class _ConfigurationWarning
		{
			public int ID;
			public string Warning;
			public int Type;
		}
		public class _ContentTuning
		{
			public int ID;
			public int MinLevel;
			public int MaxLevel;
			public int Flags;
			public int ExpectedStatModId;
		}
		public class _Contribution
		{
			public string Description;
			public string Name;
			public int ID;
			public int ManagedWorldStateInputId;
			public int[] UiTextureAtlasMemberId = new int[4];
			public int OrderIndex;
		}
		public class _ConversationLine
		{
			public int ID;
			public int BroadcastTextId;
			public int SpellVisualKitId;
			public uint AdditionalDuration;
			public ushort NextConversationLineId;
			public ushort AnimKitId;
			public byte SpeechType;
			public byte StartAnimation;
			public byte EndAnimation;
		}
		public class _Creature
		{
			public int ID;
			public string Name;
			public string NameAlt;
			public string Title;
			public string TitleAlt;
			public int[] DisplayId = new int[4];
			public float[] DisplayProbability = new float[4];
			public int[] AlwaysItem = new int[3];
			public int MountCreatureId;
			public byte Classification;
			public byte CreatureType;
			public byte CreatureFamily;
			public byte StartAnimState;
		}
		public class _CreatureDifficulty
		{
			public int ID;
			public uint[] Flags = new uint[7];
			public ushort FactionId;
			public byte ExpansionId;
			public byte MinLevel;
			public byte MaxLevel;
			public int ContentTuningId;
		}
		public class _CreatureDisplayInfo
		{
			public int ID;
			public float CreatureModelScale;
			public ushort ModelId;
			public ushort NPCSoundId;
			public byte SizeClass;
			public byte Flags;
			public byte Gender;
			public ushort SoundId;
			public byte CreatureModelAlpha;
			public byte BloodId;
			public int ExtendedDisplayInfoId;
			public ushort ParticleColorId;
			public int PortraitCreatureDisplayInfoId;
			public int PortraitTextureFileDataId;
			public ushort ObjectEffectPackageId;
			public ushort AnimReplacementSetId;
			public int StateSpellVisualKitId;
			public float PlayerOverrideScale;
			public float PetInstanceScale;
			public byte UnarmedWeaponType;
			public int MountPoofSpellVisualKitId;
			public int DissolveEffectId;
			public int[] TextureVariationFileDataId = new int[3];
		}
		public class _CreatureDisplayInfoCond
		{
			public int ID;
			public ulong RaceMask;
			public uint[] CustomOption0Mask = new uint[2];
			public int[] CustomOption1Mask = new int[2];
			public int[] CustomOption2Mask = new int[2];
			public byte OrderIndex;
			public byte Gender;
			public int ClassMask;
			public int SkinColorMask;
			public int HairColorMask;
			public int HairStyleMask;
			public int FaceStyleMask;
			public int FacialHairStyleMask;
			public int CreatureModelDataId;
			public int[] TextureVariationFileDataId = new int[3];
		}
		public class _CreatureDisplayInfoEvt
		{
			public int ID;
			public int Fourcc;
			public int SpellVisualKitId;
			public byte Flags;
		}
		public class _CreatureDisplayInfoExtra
		{
			public int ID;
			public int BakeMaterialResourcesId;
			public int HDBakeMaterialResourcesId;
			public byte DisplayRaceId;
			public byte DisplaySexId;
			public byte DisplayClassId;
			public byte SkinId;
			public byte FaceId;
			public byte HairStyleId;
			public byte HairColorId;
			public byte FacialHairId;
			public byte[] CustomDisplayOption = new byte[3];
			public byte Flags;
		}
		public class _CreatureDisplayInfoGeosetData
		{
			public int ID;
			public byte GeosetIndex;
			public byte GeosetValue;
		}
		public class _CreatureDisplayInfoTrn
		{
			public int ID;
			public int DstCreatureDisplayInfoId;
			public float MaxTime;
			public int DissolveEffectId;
			public int StartVisualKitId;
			public int FinishVisualKitId;
		}
		public class _CreatureDispXUiCamera
		{
			public int ID;
			public int CreatureDisplayInfoId;
			public ushort UiCameraId;
		}
		public class _CreatureFamily
		{
			public int ID;
			public string Name;
			public float MinScale;
			public float MaxScale;
			public int IconFileId;
			public ushort[] SkillLine = new ushort[2];
			public ushort PetFoodMask;
			public byte MinScaleLevel;
			public byte MaxScaleLevel;
			public byte PetTalentType;
		}
		public class _CreatureImmunities
		{
			public int ID;
			public int[] Mechanic = new int[2];
			public byte School;
			public byte MechanicsAllowed;
			public byte EffectsAllowed;
			public byte StatesAllowed;
			public byte Flags;
			public int DispelType;
			public int[] Effect = new int[9];
			public int[] State = new int[16];
		}
		public class _CreatureModelData
		{
			public int ID;
			public float FootprintTextureLength;
			public float FootprintTextureWidth;
			public float FootprintParticleScale;
			public float CollisionWidth;
			public float CollisionHeight;
			public float WorldEffectScale;
			public float HoverHeight;
			public float AttachedEffectScale;
			public float ModelScale;
			public float MissileCollisionRadius;
			public float MissileCollisionPush;
			public float MissileCollisionRaise;
			public float MountHeight;
			public float OverrideLootEffectScale;
			public float OverrideNameScale;
			public float OverrideSelectionRadius;
			public float TamedPetBaseScale;
			public float[] GeoBox = new float[6];
			public int Flags;
			public int FileDataId;
			public int BloodId;
			public int FootprintTextureId;
			public int FoleyMaterialId;
			public int FootstepCameraEffectId;
			public int DeathThudCameraEffectId;
			public int SoundId;
			public int SizeClass;
			public int CreatureGeosetDataId;
		}
		public class _CreatureMovementInfo
		{
			public int ID;
			public float SmoothFacingChaseRate;
		}
		public class _CreatureSoundData
		{
			public int ID;
			public float FidgetDelaySecondsMin;
			public float FidgetDelaySecondsMax;
			public byte CreatureImpactType;
			public int SoundExertionId;
			public int SoundExertionCriticalId;
			public int SoundInjuryId;
			public int SoundInjuryCriticalId;
			public int SoundInjuryCrushingBlowId;
			public int SoundDeathId;
			public int SoundStunId;
			public int SoundStandId;
			public int SoundFootstepId;
			public int SoundAggroId;
			public int SoundWingFlapId;
			public int SoundWingGlideId;
			public int SoundAlertId;
			public int SoundJumpStartId;
			public int SoundJumpEndId;
			public int SoundPetAttackId;
			public int SoundPetOrderId;
			public int SoundPetDismissId;
			public int LoopSoundId;
			public int BirthSoundId;
			public int SpellCastDirectedSoundId;
			public int SubmergeSoundId;
			public int SubmergedSoundId;
			public int WindupSoundId;
			public int WindupCriticalSoundId;
			public int ChargeSoundId;
			public int ChargeCriticalSoundId;
			public int BattleShoutSoundId;
			public int BattleShoutCriticalSoundId;
			public int TauntSoundId;
			public int CreatureSoundDataIDPet;
			public int NPCSoundId;
			public int[] SoundFidget = new int[5];
			public int[] CustomAttack = new int[4];
		}
		public class _CreatureType
		{
			public int ID;
			public string Name;
			public byte Flags;
		}
		public class _CreatureXContribution
		{
			public int ID;
			public int ContributionId;
		}
		public class _CreatureXDisplayInfo
		{
			public int ID;
			public int CreatureDisplayInfoId;
			public float Probability;
			public float Scale;
			public byte OrderIndex;
		}
		public class _Criteria
		{
			public int ID;
			public int Asset;
			public int ModifierTreeId;
			public int StartAsset;
			public uint FailAsset;
			public ushort StartTimer;
			public ushort EligibilityWorldStateId;
			public byte Type;
			public byte StartEvent;
			public byte FailEvent;
			public byte Flags;
			public byte EligibilityWorldStateValue;
		}
		public class _CriteriaTree
		{
			public int ID;
			public string Description;
			public int Amount;
			public ushort Flags;
			public byte Operator;
			public int Parent;
			public int CriteriaId;
			public int OrderIndex;
		}
		public class _CriteriaTreeXEffect
		{
			public int ID;
			public ushort WorldEffectId;
		}
		public class _CurrencyCategory
		{
			public int ID;
			public string Name;
			public byte Flags;
			public byte ExpansionId;
		}
		public class _CurrencyContainer
		{
			public int ID;
			public string ContainerName;
			public string ContainerDescription;
			public int MinAmount;
			public int MaxAmount;
			public int ContainerIconId;
			public int ContainerQuality;
		}
		public class _CurrencyTypes
		{
			public int ID;
			public string Name;
			public string Description;
			public int MaxQty;
			public int MaxEarnablePerWeek;
			public int Flags;
			public byte CategoryId;
			public byte SpellCategory;
			public byte Quality;
			public int InventoryIconFileId;
			public int SpellWeight;
		}
		public class _Curve
		{
			public int ID;
			public byte Type;
			public byte Flags;
		}
		public class _CurvePoint
		{
			public int ID;
			public float[] Pos = new float[2];
			public ushort CurveId;
			public byte OrderIndex;
		}
		public class _DeathThudLookups
		{
			public int ID;
			public byte SizeClass;
			public byte TerrainTypeSoundId;
			public int SoundEntryId;
			public int SoundEntryIDWater;
		}
		public class _DecalProperties
		{
			public int ID;
			public int FileDataId;
			public float ModX;
			public float InnerRadius;
			public float OuterRadius;
			public float Rim;
			public float Gain;
			public float Scale;
			public float FadeIn;
			public float FadeOut;
			public byte Priority;
			public byte BlendMode;
			public int TopTextureBlendSetId;
			public int BotTextureBlendSetId;
			public int Flags;
			public int GameFlags;
			public int CasterDecalPropertiesId;
		}
		public class _DeclinedWord
		{
			public string Word;
			public int ID;
		}
		public class _DeclinedWordCases
		{
			public int ID;
			public string DeclinedWord;
			public byte CaseIndex;
		}
		public class _DestructibleModelData
		{
			public int ID;
			public ushort State1Wmo;
			public ushort State2Wmo;
			public ushort State3Wmo;
			public ushort State0Wmo;
			public ushort HealEffectSpeed;
			public byte State0ImpactEffectDoodadSet;
			public byte State0AmbientDoodadSet;
			public byte State1DestructionDoodadSet;
			public byte State1ImpactEffectDoodadSet;
			public byte State1AmbientDoodadSet;
			public byte State2DestructionDoodadSet;
			public byte State2ImpactEffectDoodadSet;
			public byte State2AmbientDoodadSet;
			public byte State3InitDoodadSet;
			public byte State3AmbientDoodadSet;
			public byte EjectDirection;
			public byte DoNotHighlight;
			public byte HealEffect;
			public byte State0NameSet;
			public byte State1NameSet;
			public byte State2NameSet;
			public byte State3NameSet;
		}
		public class _DeviceBlacklist
		{
			public int ID;
			public ushort VendorId;
			public ushort DeviceId;
		}
		public class _DeviceDefaultSettings
		{
			public int ID;
			public ushort VendorId;
			public ushort DeviceId;
			public byte DefaultSetting;
		}
		public class _Difficulty
		{
			public int ID;
			public string Name;
			public ushort GroupSizeHealthCurveId;
			public ushort GroupSizeDmgCurveId;
			public ushort GroupSizeSpellPointsCurveId;
			public byte InstanceType;
			public byte OrderIndex;
			public byte OldEnumValue;
			public byte FallbackDifficultyId;
			public byte MinPlayers;
			public byte MaxPlayers;
			public byte Flags;
			public byte ItemContext;
			public byte ToggleDifficultyId;
		}
		public class _DissolveEffect
		{
			public int ID;
			public float Ramp;
			public float StartValue;
			public float EndValue;
			public float FadeInTime;
			public float FadeOutTime;
			public float Duration;
			public float Scale;
			public float FresnelIntensity;
			public byte AttachId;
			public byte ProjectionType;
			public int TextureBlendSetId;
			public int Flags;
			public int CurveId;
			public int Priority;
		}
		public class _DriverBlacklist
		{
			public int ID;
			public int DriverVersionHi;
			public int DriverVersionLow;
			public ushort VendorId;
			public byte DeviceId;
			public byte OsVersion;
			public byte OsBits;
			public byte Flags;
		}
		public class _DungeonEncounter
		{
			public string Name;
			public int CreatureDisplayId;
			public ushort MapId;
			public byte DifficultyId;
			public byte Bit;
			public byte Flags;
			public int ID;
			public int OrderIndex;
			public int SpellIconFileId;
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
			public float Data;
		}
		public class _EdgeGlowEffect
		{
			public int ID;
			public float Duration;
			public float FadeIn;
			public float FadeOut;
			public float FresnelCoefficient;
			public float GlowRed;
			public float GlowGreen;
			public float GlowBlue;
			public float GlowAlpha;
			public float GlowMultiplier;
			public float InitialDelay;
			public byte Flags;
			public int CurveId;
			public int Priority;
		}
		public class _Emotes
		{
			public int ID;
			public ulong RaceMask;
			public string EmoteSlashCommand;
			public int EmoteFlags;
			public int SpellVisualKitId;
			public ushort AnimId;
			public byte EmoteSpecProc;
			public int EmoteSpecProcParam;
			public int EventSoundId;
			public int ClassMask;
		}
		public class _EmotesText
		{
			public int ID;
			public string Name;
			public ushort EmoteId;
		}
		public class _EmotesTextData
		{
			public int ID;
			public string Text;
			public byte RelationshipFlags;
		}
		public class _EmotesTextSound
		{
			public int ID;
			public byte RaceId;
			public byte ClassId;
			public byte SexId;
			public int SoundId;
		}
		public class _EnvironmentalDamage
		{
			public int ID;
			public ushort VisualKitId;
			public byte EnumId;
		}
		public class _Exhaustion
		{
			public string Name;
			public string CombatLogText;
			public uint Xp;
			public float Factor;
			public float OutdoorHours;
			public float InnHours;
			public float Threshold;
			public int ID;
		}
		public class _ExpectedStat
		{
			public int ID;
			public uint ExpansionId;
			public float CreatureHealth;
			public float PlayerHealth;
			public float CreatureAutoAttackDps;
			public float CreatureArmor;
			public float PlayerMana;
			public float PlayerPrimaryStat;
			public float PlayerSecondaryStat;
			public float ArmorConstant;
			public float CreatureSpellDamage;
		}
		public class _ExpectedStatMod
		{
			public int ID;
			public float CreatureHealthMod;
			public float PlayerHealthMod;
			public float CreatureAutoAttackDPSMod;
			public float CreatureArmorMod;
			public float PlayerManaMod;
			public float PlayerPrimaryStatMod;
			public float PlayerSecondaryStatMod;
			public float ArmorConstantMod;
			public float CreatureSpellDamageMod;
		}
		public class _Faction
		{
			public ulong[] ReputationRaceMask = new ulong[4];
			public string Name;
			public string Description;
			public int ID;
			public int[] ReputationBase = new int[4];
			public int[] ReputationMax = new int[4];
			public float[] ParentFactionMod = new float[2];
			public ushort ReputationIndex;
			public ushort[] ReputationClassMask = new ushort[4];
			public ushort[] ReputationFlags = new ushort[4];
			public ushort ParentFactionId;
			public ushort ParagonFactionId;
			public byte[] ParentFactionCap = new byte[2];
			public byte Expansion;
			public byte FriendshipRepId;
			public byte Flags;
		}
		public class _FactionGroup
		{
			public string InternalName;
			public string Name;
			public int ID;
			public byte MaskId;
			public int HonorCurrencyTextureFileId;
			public int ConquestCurrencyTextureFileId;
		}
		public class _FactionTemplate
		{
			public int ID;
			public ushort Faction;
			public ushort Flags;
			public ushort[] Enemies = new ushort[4];
			public ushort[] Friend = new ushort[4];
			public byte FactionGroup;
			public byte FriendGroup;
			public byte EnemyGroup;
		}
		public class _FootprintTextures
		{
			public int ID;
			public int FileDataId;
			public int TextureBlendsetId;
			public int Flags;
		}
		public class _FootstepTerrainLookup
		{
			public int ID;
			public ushort CreatureFootstepId;
			public byte TerrainSoundId;
			public int SoundId;
			public int SoundIDSplash;
		}
		public class _FriendshipRepReaction
		{
			public int ID;
			public string Reaction;
			public ushort ReactionThreshold;
			public byte FriendshipRepId;
		}
		public class _FriendshipReputation
		{
			public string Description;
			public int TextureFileId;
			public ushort FactionId;
			public int ID;
		}
		public class _FullScreenEffect
		{
			public int ID;
			public float Saturation;
			public float GammaRed;
			public float GammaGreen;
			public float GammaBlue;
			public float MaskOffsetY;
			public float MaskSizeMultiplier;
			public float MaskPower;
			public float ColorMultiplyRed;
			public float ColorMultiplyGreen;
			public float ColorMultiplyBlue;
			public float ColorMultiplyOffsetY;
			public float ColorMultiplyMultiplier;
			public float ColorMultiplyPower;
			public float ColorAdditionRed;
			public float ColorAdditionGreen;
			public float ColorAdditionBlue;
			public float ColorAdditionOffsetY;
			public float ColorAdditionMultiplier;
			public float ColorAdditionPower;
			public float BlurIntensity;
			public float BlurOffsetY;
			public float BlurMultiplier;
			public float BlurPower;
			public int Flags;
			public int EffectFadeInMs;
			public int EffectFadeOutMs;
			public int TextureBlendSetId;
		}
		public class _GameObjectArtKit
		{
			public int ID;
			public int AttachModelFileId;
			public int[] TextureVariationFileId = new int[3];
		}
		public class _GameObjectDiffAnimMap
		{
			public int ID;
			public ushort AttachmentDisplayId;
			public byte DifficultyId;
			public byte Animation;
		}
		public class _GameObjectDisplayInfo
		{
			public int ID;
			public int FileDataId;
			public float OverrideLootEffectScale;
			public float OverrideNameScale;
			public float[] GeoBox = new float[6];
			public ushort ObjectEffectPackageId;
		}
		public class _GameObjectDisplayInfoXSoundKit
		{
			public int ID;
			public byte EventIndex;
			public int SoundKitId;
		}
		public class _GameObjects
		{
			public string Name;
			public float[] Pos = new float[3];
			public float[] Rot = new float[4];
			public float Scale;
			public uint[] PropValue = new uint[8];
			public ushort OwnerId;
			public ushort DisplayId;
			public ushort PhaseId;
			public ushort PhaseGroupId;
			public byte TypeId;
			public byte PhaseUseFlags;
			public int ID;
		}
		public class _GameTips
		{
			public int ID;
			public string Text;
			public ushort MinLevel;
			public ushort MaxLevel;
			public byte SortIndex;
		}
		public class _GarrAbility
		{
			public string Name;
			public string Description;
			public int IconFileDataId;
			public ushort FactionChangeGarrAbilityId;
			public ushort Flags;
			public byte GarrAbilityCategoryId;
			public byte GarrFollowerTypeId;
			public int ID;
		}
		public class _GarrAbilityCategory
		{
			public int ID;
			public string Name;
		}
		public class _GarrAbilityEffect
		{
			public float CombatWeightBase;
			public float CombatWeightMax;
			public float ActionValueFlat;
			public int ActionRecordId;
			public ushort GarrAbilityId;
			public byte AbilityAction;
			public byte AbilityTargetType;
			public byte GarrMechanicTypeId;
			public byte ActionRace;
			public byte ActionHours;
			public byte Flags;
			public int ID;
		}
		public class _GarrBuilding
		{
			public int ID;
			public string HordeName;
			public string AllianceName;
			public string Description;
			public string Tooltip;
			public int HordeGameObjectId;
			public int AllianceGameObjectId;
			public int IconFileDataId;
			public ushort CurrencyTypeId;
			public ushort HordeUiTextureKitId;
			public ushort AllianceUiTextureKitId;
			public ushort AllianceSceneScriptPackageId;
			public ushort HordeSceneScriptPackageId;
			public ushort GarrAbilityId;
			public ushort BonusGarrAbilityId;
			public ushort GoldCost;
			public byte GarrTypeId;
			public byte BuildingType;
			public byte GarrSiteId;
			public byte UpgradeLevel;
			public byte ShipmentCapacity;
			public byte Flags;
			public int BuildSeconds;
			public int CurrencyQty;
			public int MaxAssignments;
		}
		public class _GarrBuildingDoodadSet
		{
			public int ID;
			public byte GarrBuildingId;
			public byte Type;
			public byte HordeDoodadSet;
			public byte AllianceDoodadSet;
			public byte SpecializationId;
		}
		public class _GarrBuildingPlotInst
		{
			public float[] MapOffset = new float[2];
			public ushort GarrSiteLevelPlotInstId;
			public ushort UiTextureAtlasMemberId;
			public byte GarrBuildingId;
			public int ID;
		}
		public class _GarrClassSpec
		{
			public string ClassSpec;
			public string ClassSpecMale;
			public string ClassSpecFemale;
			public ushort UiTextureAtlasMemberId;
			public ushort GarrFollItemSetId;
			public byte FollowerClassLimit;
			public byte Flags;
			public int ID;
		}
		public class _GarrClassSpecPlayerCond
		{
			public int ID;
			public string Name;
			public int IconFileDataId;
			public byte OrderIndex;
			public int GarrClassSpecId;
			public int PlayerConditionId;
			public int FlavorGarrStringId;
		}
		public class _GarrEncounter
		{
			public string Name;
			public int CreatureId;
			public int PortraitFileDataId;
			public float UiAnimScale;
			public float UiAnimHeight;
			public int ID;
			public int UiTextureKitId;
		}
		public class _GarrEncounterSetXEncounter
		{
			public int ID;
			public int GarrEncounterId;
		}
		public class _GarrEncounterXMechanic
		{
			public int ID;
			public byte GarrMechanicId;
			public byte GarrMechanicSetId;
		}
		public class _GarrFollItemSetMember
		{
			public int ID;
			public int ItemId;
			public ushort MinItemLevel;
			public byte ItemSlot;
		}
		public class _GarrFollower
		{
			public string HordeSourceText;
			public string AllianceSourceText;
			public string TitleName;
			public int HordeCreatureId;
			public int AllianceCreatureId;
			public int HordeIconFileDataId;
			public int AllianceIconFileDataId;
			public int HordeSlottingBroadcastTextId;
			public int AllySlottingBroadcastTextId;
			public ushort ItemLevelWeapon;
			public ushort ItemLevelArmor;
			public ushort HordeGarrFollItemSetId;
			public ushort AllianceGarrFollItemSetId;
			public ushort HordeUITextureKitId;
			public ushort AllianceUITextureKitId;
			public byte GarrTypeId;
			public byte GarrFollowerTypeId;
			public byte HordeGarrFollRaceId;
			public byte AllianceGarrFollRaceId;
			public byte HordeGarrClassSpecId;
			public byte AllianceGarrClassSpecId;
			public byte Quality;
			public byte FollowerLevel;
			public byte HordeSourceTypeEnum;
			public byte AllianceSourceTypeEnum;
			public byte Vitality;
			public byte HordeFlavorGarrStringId;
			public byte AllianceFlavorGarrStringId;
			public byte ChrClassId;
			public byte Flags;
			public byte Gender;
			public int ID;
		}
		public class _GarrFollowerLevelXP
		{
			public int ID;
			public ushort XpToNextLevel;
			public ushort ShipmentXP;
			public byte GarrFollowerTypeId;
			public byte FollowerLevel;
		}
		public class _GarrFollowerQuality
		{
			public int ID;
			public int XpToNextQuality;
			public ushort ShipmentXP;
			public byte Quality;
			public byte GarrFollowerTypeId;
			public byte AbilityCount;
			public byte TraitCount;
			public int ClassSpecId;
		}
		public class _GarrFollowerSetXFollower
		{
			public int GarrFollowerId;
		}
		public class _GarrFollowerType
		{
			public int ID;
			public ushort MaxItemLevel;
			public byte GarrTypeId;
			public byte MaxFollowers;
			public byte MaxFollowerBuildingType;
			public byte LevelRangeBias;
			public byte ItemLevelRangeBias;
			public byte Flags;
		}
		public class _GarrFollowerUICreature
		{
			public int ID;
			public int CreatureId;
			public float Scale;
			public byte OrderIndex;
			public byte FactionIndex;
			public byte Flags;
		}
		public class _GarrFollowerXAbility
		{
			public int ID;
			public ushort GarrAbilityId;
			public byte FactionIndex;
		}
		public class _GarrFollSupportSpell
		{
			public int ID;
			public int HordeSpellId;
			public int AllianceSpellId;
			public byte OrderIndex;
		}
		public class _GarrItemLevelUpgradeData
		{
			public int ID;
			public int Operation;
			public int MinItemLevel;
			public int MaxItemLevel;
			public int FollowerTypeId;
		}
		public class _GarrMechanic
		{
			public int ID;
			public float Factor;
			public byte GarrMechanicTypeId;
			public int GarrAbilityId;
		}
		public class _GarrMechanicSetXMechanic
		{
			public byte GarrMechanicId;
			public int ID;
		}
		public class _GarrMechanicType
		{
			public string Name;
			public string Description;
			public int IconFileDataId;
			public byte Category;
			public int ID;
		}
		public class _GarrMission
		{
			public string Name;
			public string Location;
			public string Description;
			public int MissionDuration;
			public int OfferDuration;
			public float[] MapPos = new float[2];
			public float[] WorldPos = new float[2];
			public ushort MissionCostCurrencyTypesId;
			public ushort UiTextureKitId;
			public ushort TargetItemLevel;
			public byte GarrTypeId;
			public byte GarrMissionTypeId;
			public byte GarrFollowerTypeId;
			public byte MaxFollowers;
			public byte OfferedGarrMissionTextureId;
			public byte EnvGarrMechanicTypeId;
			public byte TargetLevel;
			public byte BaseCompletionChance;
			public byte FollowerDeathChance;
			public int ID;
			public int MissionCost;
			public int EnvGarrMechanicId;
			public int PlayerConditionId;
			public int TravelDuration;
			public int BaseFollowerXP;
			public int OvermaxRewardPackId;
			public int AreaId;
			public int Flags;
		}
		public class _GarrMissionTexture
		{
			public int ID;
			public float[] Pos = new float[2];
			public ushort UiTextureKitId;
		}
		public class _GarrMissionType
		{
			public int ID;
			public string Name;
			public ushort UiTextureAtlasMemberId;
			public ushort UiTextureKitId;
		}
		public class _GarrMissionXEncounter
		{
			public byte OrderIndex;
			public int ID;
			public int GarrEncounterId;
			public int GarrEncounterSetId;
		}
		public class _GarrMissionXFollower
		{
			public int ID;
			public int GarrFollowerId;
			public int GarrFollowerSetId;
		}
		public class _GarrMssnBonusAbility
		{
			public int ID;
			public float Radius;
			public int DurationSecs;
			public ushort GarrAbilityId;
			public byte GarrFollowerTypeId;
			public byte GarrMissionTextureId;
		}
		public class _GarrPlot
		{
			public int ID;
			public string Name;
			public int HordeConstructObjId;
			public int AllianceConstructObjId;
			public byte PlotType;
			public byte Flags;
			public byte UiCategoryId;
			public int[] UpgradeRequirement = new int[2];
		}
		public class _GarrPlotBuilding
		{
			public int ID;
			public byte GarrPlotId;
			public byte GarrBuildingId;
		}
		public class _GarrPlotInstance
		{
			public int ID;
			public string Name;
			public byte GarrPlotId;
		}
		public class _GarrPlotUICategory
		{
			public int ID;
			public string CategoryName;
			public byte PlotType;
		}
		public class _GarrSiteLevel
		{
			public int ID;
			public float[] TownHallUiPos = new float[2];
			public ushort MapId;
			public ushort UpgradeMovieId;
			public ushort UiTextureKitId;
			public ushort UpgradeCost;
			public ushort UpgradeGoldCost;
			public byte GarrSiteId;
			public byte GarrLevel;
			public byte MaxBuildingLevel;
		}
		public class _GarrSiteLevelPlotInst
		{
			public int ID;
			public float[] UiMarkerPos = new float[2];
			public ushort GarrSiteLevelId;
			public byte GarrPlotInstanceId;
			public byte UiMarkerSize;
		}
		public class _GarrSpecialization
		{
			public int ID;
			public string Name;
			public string Tooltip;
			public float[] Param = new float[2];
			public int IconFileDataId;
			public byte BuildingType;
			public byte SpecType;
			public byte RequiredUpgradeLevel;
		}
		public class _GarrString
		{
			public int ID;
			public string Text;
		}
		public class _GarrTalent
		{
			public string Name;
			public string Description;
			public int IconFileDataId;
			public int ResearchDurationSecs;
			public byte Tier;
			public byte UiOrder;
			public byte Flags;
			public int ID;
			public int GarrTalentTreeId;
			public int PlayerConditionId;
			public int GarrAbilityId;
			public int PerkSpellId;
			public int PerkPlayerConditionId;
			public int ResearchGoldCost;
			public int ResearchCost;
			public int ResearchCostCurrencyTypesId;
			public int RespecDurationSecs;
			public int RespecGoldCost;
			public int RespecCost;
			public int RespecCostCurrencyTypesId;
		}
		public class _GarrTalentTree
		{
			public int ID;
			public ushort UiTextureKitId;
			public byte MaxTiers;
			public byte UiOrder;
			public int GarrTypeId;
			public int ClassId;
		}
		public class _GarrType
		{
			public int ID;
			public int PrimaryCurrencyTypeId;
			public int SecondaryCurrencyTypeId;
			public int ExpansionId;
			public int Flags;
			public int[] MapIDs = new int[2];
		}
		public class _GarrUiAnimClassInfo
		{
			public int ID;
			public float ImpactDelaySecs;
			public byte GarrClassSpecId;
			public byte MovementType;
			public int CastKit;
			public int ImpactKit;
			public int TargetImpactKit;
		}
		public class _GarrUiAnimRaceInfo
		{
			public int ID;
			public float MaleScale;
			public float MaleHeight;
			public float FemaleScale;
			public float FemaleHeight;
			public float MaleSingleModelScale;
			public float MaleSingleModelHeight;
			public float FemaleSingleModelScale;
			public float FemaleSingleModelHeight;
			public float MaleFollowerPageScale;
			public float MaleFollowerPageHeight;
			public float FemaleFollowerPageScale;
			public float FemaleFollowerPageHeight;
			public byte GarrFollRaceId;
		}
		public class _GemProperties
		{
			public int ID;
			public int Type;
			public ushort EnchantId;
			public ushort MinItemLevel;
		}
		public class _GlobalStrings
		{
			public int ID;
			public string BaseTag;
			public string TagText;
			public byte Flags;
		}
		public class _GlyphBindableSpell
		{
			public int ID;
			public int SpellId;
		}
		public class _GlyphExclusiveCategory
		{
			public int ID;
			public string Name;
		}
		public class _GlyphProperties
		{
			public int ID;
			public int SpellId;
			public ushort SpellIconId;
			public byte GlyphType;
			public byte GlyphExclusiveCategoryId;
		}
		public class _GlyphRequiredSpec
		{
			public int ID;
			public ushort ChrSpecializationId;
		}
		public class _GMSurveyAnswers
		{
			public int ID;
			public string Answer;
			public byte SortIndex;
		}
		public class _GMSurveyCurrentSurvey
		{
			public int ID;
			public byte GmsurveyId;
		}
		public class _GMSurveyQuestions
		{
			public int ID;
			public string Question;
		}
		public class _GMSurveySurveys
		{
			public int ID;
			public byte[] Q = new byte[15];
		}
		public class _GroundEffectDoodad
		{
			public int ID;
			public float Animscale;
			public float PushScale;
			public byte Flags;
			public int ModelFileId;
		}
		public class _GroundEffectTexture
		{
			public int ID;
			public ushort[] DoodadId = new ushort[4];
			public byte[] DoodadWeight = new byte[4];
			public byte Sound;
			public int Density;
		}
		public class _GroupFinderActivity
		{
			public int ID;
			public string FullName;
			public string ShortName;
			public ushort MinGearLevelSuggestion;
			public ushort MapId;
			public ushort AreaId;
			public byte GroupFinderCategoryId;
			public byte OrderIndex;
			public byte GroupFinderActivityGrpId;
			public byte MinLevel;
			public byte MaxLevelSuggestion;
			public byte Flags;
			public byte DifficultyId;
			public byte MaxPlayers;
			public byte DisplayType;
		}
		public class _GroupFinderActivityGrp
		{
			public int ID;
			public string Name;
			public byte OrderIndex;
		}
		public class _GroupFinderCategory
		{
			public int ID;
			public string Name;
			public byte OrderIndex;
			public byte Flags;
		}
		public class _GuildColorBackground
		{
			public int ID;
			public byte Red;
			public byte Blue;
			public byte Green;
		}
		public class _GuildColorBorder
		{
			public int ID;
			public byte Red;
			public byte Blue;
			public byte Green;
		}
		public class _GuildColorEmblem
		{
			public int ID;
			public byte Red;
			public byte Blue;
			public byte Green;
		}
		public class _GuildPerkSpells
		{
			public int ID;
			public int SpellId;
		}
		public class _Heirloom
		{
			public string SourceText;
			public int ItemId;
			public int LegacyUpgradedItemId;
			public int StaticUpgradedItemId;
			public int[] UpgradeItemId = new int[3];
			public int LegacyItemId;
			public ushort[] UpgradeItemBonusListId = new ushort[3];
			public byte SourceTypeEnum;
			public byte Flags;
			public int ID;
		}
		public class _HelmetAnimScaling
		{
			public int ID;
			public float Amount;
			public int RaceId;
		}
		public class _HelmetGeosetVisData
		{
			public int ID;
			public uint[] HideGeoset = new uint[9];
		}
		public class _HighlightColor
		{
			public int ID;
			public uint StartColor;
			public uint MidColor;
			public int EndColor;
			public byte Type;
			public byte Flags;
		}
		public class _HolidayDescriptions
		{
			public int ID;
			public string Description;
		}
		public class _HolidayNames
		{
			public int ID;
			public string Name;
		}
		public class _Holidays
		{
			public int ID;
			public int[] Date = new int[16];
			public ushort[] Duration = new ushort[10];
			public ushort Region;
			public byte Looping;
			public byte[] CalendarFlags = new byte[10];
			public byte Priority;
			public byte CalendarFilterType;
			public byte Flags;
			public int HolidayNameId;
			public int HolidayDescriptionId;
			public int[] TextureFileDataId = new int[3];
		}
		public class _Hotfixes
		{
			public int ID;
			public string Tablename;
			public int ObjectId;
			public int Flags;
		}
		public class _ImportPriceArmor
		{
			public int ID;
			public float ClothModifier;
			public float LeatherModifier;
			public float ChainModifier;
			public float PlateModifier;
		}
		public class _ImportPriceQuality
		{
			public int ID;
			public float Data;
		}
		public class _ImportPriceShield
		{
			public int ID;
			public float Data;
		}
		public class _ImportPriceWeapon
		{
			public int ID;
			public float Data;
		}
		public class _InvasionClientData
		{
			public string Name;
			public float[] IconLocation = new float[2];
			public int ID;
			public int WorldStateId;
			public int UiTextureAtlasMemberId;
			public int ScenarioId;
			public int WorldQuestId;
			public int WorldStateValue;
			public int InvasionEnabledWorldStateId;
		}
		public class _Item
		{
			public int ID;
			public int IconFileDataId;
			public byte ClassId;
			public byte SubclassId;
			public byte Material;
			public byte InventoryType;
			public byte SheatheType;
			public byte SoundOverrideSubclassId;
			public byte ItemGroupSoundsId;
		}
		public class _ItemAppearance
		{
			public int ID;
			public int ItemDisplayInfoId;
			public int DefaultIconFileDataId;
			public int UiOrder;
			public byte DisplayType;
		}
		public class _ItemAppearanceXUiCamera
		{
			public int ID;
			public ushort ItemAppearanceId;
			public ushort UiCameraId;
		}
		public class _ItemArmorQuality
		{
			public int ID;
			public float[] Qualitymod = new float[7];
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
			public float Cloth;
			public float Leather;
			public float Mail;
			public float Plate;
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
			public uint[] Value = new uint[3];
			public ushort ParentItemBonusListId;
			public byte Type;
			public byte OrderIndex;
		}
		public class _ItemBonusListLevelDelta
		{
			public ushort ItemLevelDelta;
			public int ID;
		}
		public class _ItemBonusTreeNode
		{
			public int ID;
			public ushort ChildItemBonusTreeId;
			public ushort ChildItemBonusListId;
			public ushort ChildItemLevelSelectorId;
			public byte ItemContext;
		}
		public class _ItemChildEquipment
		{
			public int ID;
			public int ChildItemId;
			public byte ChildItemEquipSlot;
		}
		public class _ItemClass
		{
			public int ID;
			public string ClassName;
			public float PriceModifier;
			public byte ClassId;
			public byte Flags;
		}
		public class _ItemContextPickerEntry
		{
			public int ID;
			public byte ItemCreationContext;
			public byte OrderIndex;
			public int PVal;
			public int Flags;
			public int PlayerConditionId;
		}
		public class _ItemCurrencyCost
		{
			public int ID;
			public int ItemId;
		}
		public class _ItemDamageAmmo
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageOneHand
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageOneHandCaster
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageTwoHand
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDamageTwoHandCaster
		{
			public int ID;
			public float[] Quality = new float[7];
			public ushort ItemLevel;
		}
		public class _ItemDisenchantLoot
		{
			public int ID;
			public ushort MinLevel;
			public ushort MaxLevel;
			public ushort SkillRequired;
			public byte Subclass;
			public byte Quality;
			public byte ExpansionId;
		}
		public class _ItemDisplayInfo
		{
			public int ID;
			public int ModelType1;
			public int ItemVisual;
			public int ParticleColorId;
			public int ItemRangedDisplayInfoId;
			public int OverrideSwooshSoundKitId;
			public int SheatheTransformMatrixId;
			public int StateSpellVisualKitId;
			public int SheathedSpellVisualKitId;
			public int UnsheathedSpellVisualKitId;
			public int Flags;
			public int[] ModelResourcesId = new int[2];
			public int[] ModelMaterialResourcesId = new int[2];
			public int[] GeosetGroup = new int[4];
			public int[] AttachmentGeosetGroup = new int[4];
			public int[] HelmetGeosetVis = new int[2];
		}
		public class _ItemDisplayInfoMaterialRes
		{
			public int ID;
			public int MaterialResourcesId;
			public byte ComponentSection;
		}
		public class _ItemDisplayXUiCamera
		{
			public int ID;
			public int ItemDisplayInfoId;
			public ushort UiCameraId;
		}
		public class _ItemEffect
		{
			public int ID;
			public uint CoolDownMSec;
			public uint CategoryCoolDownMSec;
			public int SpellId;
			public ushort Charges;
			public ushort SpellCategoryId;
			public ushort ChrSpecializationId;
			public byte LegacySlotIndex;
			public byte TriggerType;
		}
		public class _ItemExtendedCost
		{
			public int ID;
			public int[] ItemId = new int[5];
			public int[] CurrencyCount = new int[5];
			public ushort RequiredArenaRating;
			public ushort[] ItemCount = new ushort[5];
			public ushort[] CurrencyId = new ushort[5];
			public byte ArenaBracket;
			public byte Flags;
			public byte MinFactionId;
			public byte MinReputation;
			public byte RequiredAchievement;
		}
		public class _ItemGroupSounds
		{
			public int ID;
			public int[] Sound = new int[4];
		}
		public class _ItemLevelSelector
		{
			public int ID;
			public ushort MinItemLevel;
			public ushort ItemLevelSelectorQualitySetId;
		}
		public class _ItemLevelSelectorQuality
		{
			public int ID;
			public int QualityItemBonusListId;
			public byte Quality;
		}
		public class _ItemLevelSelectorQualitySet
		{
			public int ID;
			public ushort IlvlRare;
			public ushort IlvlEpic;
		}
		public class _ItemLimitCategory
		{
			public int ID;
			public string Name;
			public byte Quantity;
			public byte Flags;
		}
		public class _ItemLimitCategoryCondition
		{
			public int ID;
			public byte AddQuantity;
			public int PlayerConditionId;
		}
		public class _ItemModifiedAppearance
		{
			public int ItemId;
			public int ID;
			public byte ItemAppearanceModifierId;
			public ushort ItemAppearanceId;
			public byte OrderIndex;
			public byte TransmogSourceTypeEnum;
		}
		public class _ItemModifiedAppearanceExtra
		{
			public int ID;
			public int IconFileDataId;
			public int UnequippedIconFileDataId;
			public byte SheatheType;
			public byte DisplayWeaponSubclassId;
			public byte DisplayInventoryType;
		}
		public class _ItemNameDescription
		{
			public int ID;
			public string Description;
			public uint Color;
		}
		public class _ItemPetFood
		{
			public int ID;
			public string Name;
		}
		public class _ItemPriceBase
		{
			public int ID;
			public float Armor;
			public float Weapon;
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
		public class _ItemRangedDisplayInfo
		{
			public int ID;
			public int CastSpellVisualId;
			public int AutoAttackSpellVisualId;
			public int QuiverFileDataId;
			public int MissileSpellVisualEffectNameId;
		}
		public class _ItemSearchName
		{
			public ulong AllowableRace;
			public string Display;
			public int ID;
			public uint[] Flags = new uint[3];
			public ushort ItemLevel;
			public byte OverallQualityId;
			public byte ExpansionId;
			public byte RequiredLevel;
			public ushort MinFactionId;
			public byte MinReputation;
			public int AllowableClass;
			public ushort RequiredSkill;
			public ushort RequiredSkillRank;
			public int RequiredAbility;
		}
		public class _ItemSet
		{
			public int ID;
			public string Name;
			public int[] ItemId = new int[17];
			public ushort RequiredSkillRank;
			public int SetFlags;
			public int RequiredSkill;
		}
		public class _ItemSetSpell
		{
			public int ID;
			public int SpellId;
			public ushort ChrSpecId;
			public byte Threshold;
		}
		public class _ItemSparse
		{
			public int ID;
			public ulong AllowableRace;
			public string Description;
			public string Display3;
			public string Display2;
			public string Display1;
			public string Display;
			public float DmgVariance;
			public int DurationInInventory;
			public float QualityModifier;
			public int BagFamily;
			public float ItemRange;
			public float[] StatPercentageOfSocket = new float[10];
			public int[] StatPercentEditor = new int[10];
			public int Stackable;
			public int MaxCount;
			public int RequiredAbility;
			public int SellPrice;
			public int BuyPrice;
			public int VendorStackCount;
			public float PriceVariance;
			public float PriceRandomValue;
			public uint[] Flags = new uint[4];
			public ushort ItemNameDescriptionId;
			public ushort RequiredTransmogHoliday;
			public ushort RequiredHoliday;
			public ushort LimitCategory;
			public ushort GemProperties;
			public ushort SocketMatchEnchantmentId;
			public ushort TotemCategoryId;
			public ushort InstanceBound;
			public ushort ZoneBound;
			public ushort ItemSet;
			public ushort ItemRandomSuffixGroupId;
			public ushort RandomSelect;
			public ushort LockId;
			public ushort StartQuestId;
			public ushort PageId;
			public ushort ItemDelay;
			public ushort ScalingStatDistributionId;
			public ushort MinFactionId;
			public ushort RequiredSkillRank;
			public ushort RequiredSkill;
			public ushort ItemLevel;
			public ushort AllowableClass;
			public byte ExpansionId;
			public byte ArtifactId;
			public byte SpellWeight;
			public byte SpellWeightCategory;
			public byte[] SocketType = new byte[3];
			public byte SheatheType;
			public byte Material;
			public byte PageMaterialId;
			public byte LanguageId;
			public byte Bonding;
			public byte DamageDamageType;
			public byte[] StatModifierBonusStat = new byte[10];
			public byte ContainerSlots;
			public byte MinReputation;
			public byte RequiredPVPMedal;
			public byte RequiredPVPRank;
			public byte RequiredLevel;
			public byte InventoryType;
			public byte OverallQualityId;
		}
		public class _ItemSpec
		{
			public int ID;
			public ushort SpecializationId;
			public byte MinLevel;
			public byte MaxLevel;
			public byte ItemType;
			public byte PrimaryStat;
			public byte SecondaryStat;
		}
		public class _ItemSpecOverride
		{
			public int ID;
			public ushort SpecId;
		}
		public class _ItemSubClass
		{
			public int ID;
			public string DisplayName;
			public string VerboseName;
			public ushort Flags;
			public byte ClassId;
			public byte SubClassId;
			public byte AuctionHouseSortOrder;
			public byte PrerequisiteProficiency;
			public byte DisplayFlags;
			public byte WeaponSwingSize;
			public byte PostrequisiteProficiency;
		}
		public class _ItemSubClassMask
		{
			public int ID;
			public string Name;
			public int Mask;
			public byte ClassId;
		}
		public class _ItemUpgrade
		{
			public int ID;
			public int CurrencyAmount;
			public ushort PrerequisiteId;
			public ushort CurrencyType;
			public byte ItemUpgradePathId;
			public byte ItemLevelIncrement;
		}
		public class _ItemVisuals
		{
			public int ID;
			public int[] ModelFileId = new int[5];
		}
		public class _ItemXBonusTree
		{
			public int ID;
			public ushort ItemBonusTreeId;
		}
		public class _JournalEncounter
		{
			public int ID;
			public string Name;
			public string Description;
			public float[] Map = new float[2];
			public ushort JournalInstanceId;
			public ushort FirstSectionId;
			public ushort UiMapId;
			public byte Flags;
			public byte DifficultyMask;
			public int OrderIndex;
			public int MapDisplayConditionId;
		}
		public class _JournalEncounterCreature
		{
			public string Name;
			public string Description;
			public int CreatureDisplayInfoId;
			public int FileDataId;
			public ushort JournalEncounterId;
			public byte OrderIndex;
			public int ID;
			public int UiModelSceneId;
		}
		public class _JournalEncounterItem
		{
			public int ItemId;
			public ushort JournalEncounterId;
			public byte FactionMask;
			public byte Flags;
			public byte DifficultyMask;
			public int ID;
		}
		public class _JournalEncounterSection
		{
			public int ID;
			public string Title;
			public string BodyText;
			public int IconCreatureDisplayInfoId;
			public int SpellId;
			public int IconFileDataId;
			public ushort JournalEncounterId;
			public ushort ParentSectionId;
			public ushort FirstChildSectionId;
			public ushort NextSiblingSectionId;
			public ushort Flags;
			public ushort IconFlags;
			public byte OrderIndex;
			public byte Type;
			public byte DifficultyMask;
			public int UiModelSceneId;
		}
		public class _JournalEncounterXDifficulty
		{
			public int ID;
			public byte DifficultyId;
		}
		public class _JournalEncounterXMapLoc
		{
			public int ID;
			public float[] Map = new float[2];
			public byte Flags;
			public int JournalEncounterId;
			public int MapDisplayConditionId;
		}
		public class _JournalInstance
		{
			public string Name;
			public string Description;
			public int BackgroundFileDataId;
			public int ButtonFileDataId;
			public int ButtonSmallFileDataId;
			public int LoreFileDataId;
			public ushort MapId;
			public ushort AreaId;
			public byte OrderIndex;
			public byte Flags;
			public int ID;
		}
		public class _JournalItemXDifficulty
		{
			public int ID;
			public byte DifficultyId;
		}
		public class _JournalSectionXDifficulty
		{
			public int ID;
			public byte DifficultyId;
		}
		public class _JournalTier
		{
			public int ID;
			public string Name;
		}
		public class _JournalTierXInstance
		{
			public int ID;
			public ushort JournalTierId;
			public ushort JournalInstanceId;
		}
		public class _KeyChain
		{
			public int ID;
			public byte[] Key = new byte[32];
		}
		public class _KeystoneAffix
		{
			public int ID;
			public string Name;
			public string Description;
			public int Filedataid;
		}
		public class _Languages
		{
			public string Name;
			public int ID;
		}
		public class _LanguageWords
		{
			public int ID;
			public string Word;
			public byte LanguageId;
		}
		public class _LfgDungeonExpansion
		{
			public int ID;
			public ushort RandomId;
			public byte ExpansionLevel;
			public byte HardLevelMin;
			public byte HardLevelMax;
			public int TargetLevelMin;
			public int TargetLevelMax;
		}
		public class _LfgDungeonGroup
		{
			public int ID;
			public string Name;
			public ushort OrderIndex;
			public byte Typeid;
			public byte ParentGroupId;
		}
		public class _LfgDungeons
		{
			public int ID;
			public string Name;
			public string Description;
			public uint Flags;
			public float MinGear;
			public int RequiredPlayerConditionId;
			public ushort MaxLevel;
			public ushort MapId;
			public ushort TargetLevelMax;
			public ushort RandomId;
			public ushort ScenarioId;
			public ushort FinalEncounterId;
			public ushort BonusReputationAmount;
			public ushort MentorItemLevel;
			public byte MinLevel;
			public byte TypeId;
			public byte Subtype;
			public byte Faction;
			public byte ExpansionLevel;
			public byte DifficultyId;
			public byte GroupId;
			public byte OrderIndex;
			public byte TargetLevel;
			public byte TargetLevelMin;
			public byte CountTank;
			public byte CountHealer;
			public byte CountDamage;
			public byte MinCountTank;
			public byte MinCountHealer;
			public byte MinCountDamage;
			public byte MentorCharLevel;
			public int IconTextureFileId;
			public int RewardsBgTextureFileId;
			public int PopupBgTextureFileId;
		}
		public class _LfgDungeonsGroupingMap
		{
			public int ID;
			public ushort RandomLfgDungeonsId;
			public byte GroupId;
		}
		public class _LfgRoleRequirement
		{
			public int ID;
			public byte RoleType;
			public int PlayerConditionId;
		}
		public class _Light
		{
			public int ID;
			public float[] GameCoords = new float[3];
			public float GameFalloffStart;
			public float GameFalloffEnd;
			public ushort ContinentId;
			public ushort[] LightParamsId = new ushort[8];
		}
		public class _LightData
		{
			public int ID;
			public int DirectColor;
			public int AmbientColor;
			public int SkyTopColor;
			public int SkyMiddleColor;
			public int SkyBand1Color;
			public uint SkyBand2Color;
			public int SkySmogColor;
			public int SkyFogColor;
			public int SunColor;
			public int CloudSunColor;
			public int CloudEmissiveColor;
			public int CloudLayer1AmbientColor;
			public int CloudLayer2AmbientColor;
			public int OceanCloseColor;
			public int OceanFarColor;
			public int RiverCloseColor;
			public int RiverFarColor;
			public uint ShadowOpacity;
			public float FogEnd;
			public float FogScaler;
			public float FogDensity;
			public float FogHeight;
			public float FogHeightScaler;
			public float FogHeightDensity;
			public float SunFogAngle;
			public float CloudDensity;
			public int ColorGradingFileDataId;
			public int HorizonAmbientColor;
			public int GroundAmbientColor;
			public int EndFogColor;
			public float EndFogColorDistance;
			public int SunFogColor;
			public int FogHeightColor;
			public ushort LightParamId;
			public ushort Time;
		}
		public class _LightParams
		{
			public float Glow;
			public float WaterShallowAlpha;
			public float WaterDeepAlpha;
			public float OceanShallowAlpha;
			public float OceanDeepAlpha;
			public float[] OverrideCelestialSphere = new float[3];
			public ushort LightSkyboxId;
			public byte HighlightSky;
			public byte CloudTypeId;
			public byte Flags;
			public int ID;
		}
		public class _LightSkybox
		{
			public int ID;
			public string Name;
			public int SkyboxFileDataId;
			public int CelestialSkyboxFileDataId;
			public byte Flags;
		}
		public class _LiquidMaterial
		{
			public int ID;
			public byte Flags;
			public byte LVF;
		}
		public class _LiquidObject
		{
			public int ID;
			public float FlowDirection;
			public float FlowSpeed;
			public ushort LiquidTypeId;
			public byte Fishable;
			public byte Reflection;
		}
		public class _LiquidType
		{
			public int ID;
			public string Name;
			public string[] Texture = new string[6];
			public int SpellId;
			public float MaxDarkenDepth;
			public float FogDarkenIntensity;
			public float AmbDarkenIntensity;
			public float DirDarkenIntensity;
			public float ParticleScale;
			public int MinimapStaticCol;
			public int[] Color = new int[2];
			public float[] Float = new float[18];
			public int[] Int = new int[4];
			public float[] Coefficient = new float[4];
			public ushort Flags;
			public ushort LightId;
			public byte SoundBank;
			public byte ParticleMovement;
			public byte ParticleTexSlots;
			public byte MaterialId;
			public byte[] FrameCountTexture = new byte[6];
			public int SoundId;
		}
		public class _LoadingScreens
		{
			public int ID;
			public int NarrowScreenFileDataId;
			public int WideScreenFileDataId;
			public int WideScreen169FileDataId;
		}
		public class _LoadingScreenTaxiSplines
		{
			public int ID;
			public float[] LocX = new float[10];
			public float[] LocY = new float[10];
			public ushort PathId;
			public ushort LoadingScreenId;
			public byte LegIndex;
		}
		public class _Locale
		{
			public int ID;
			public int FontFileDataId;
			public byte WowLocale;
			public byte ClientDisplayExpansion;
			public byte Secondary;
		}
		public class _Location
		{
			public int ID;
			public float[] Pos = new float[3];
			public float[] Rot = new float[3];
		}
		public class _Lock
		{
			public int ID;
			public int[] Index = new int[8];
			public ushort[] Skill = new ushort[8];
			public byte[] Type = new byte[8];
			public byte[] Action = new byte[8];
		}
		public class _LockType
		{
			public string Name;
			public string ResourceName;
			public string Verb;
			public string CursorName;
			public int ID;
		}
		public class _LookAtController
		{
			public int ID;
			public float ReactionEnableDistance;
			public float ReactionGiveupDistance;
			public float TorsoSpeedFactor;
			public float HeadSpeedFactor;
			public ushort ReactionEnableFOVDeg;
			public ushort ReactionGiveupTimeMS;
			public ushort ReactionIgnoreTimeMinMS;
			public ushort ReactionIgnoreTimeMaxMS;
			public byte MaxTorsoYaw;
			public byte MaxTorsoYawWhileMoving;
			public byte MaxHeadYaw;
			public byte MaxHeadPitch;
			public byte Flags;
			public int ReactionWarmUpTimeMSMin;
			public int ReactionWarmUpTimeMSMax;
			public int ReactionGiveupFOVDeg;
			public int MaxTorsoPitchUp;
			public int MaxTorsoPitchDown;
		}
		public class _MailTemplate
		{
			public int ID;
			public string Body;
		}
		public class _ManagedWorldState
		{
			public int CurrentStageWorldStateId;
			public int ProgressWorldStateId;
			public int UpTimeSecs;
			public int DownTimeSecs;
			public int OccurrencesWorldStateId;
			public int AccumulationStateTargetValue;
			public int DepletionStateTargetValue;
			public int AccumulationAmountPerMinute;
			public int DepletionAmountPerMinute;
			public int ID;
		}
		public class _ManagedWorldStateBuff
		{
			public int ID;
			public int BuffSpellId;
			public int OccurrenceValue;
			public int PlayerConditionId;
		}
		public class _ManagedWorldStateInput
		{
			public int ID;
			public int ManagedWorldStateId;
			public int QuestId;
			public int ValidInputConditionId;
		}
		public class _ManifestInterfaceActionIcon
		{
			public int ID;
		}
		public class _ManifestInterfaceData
		{
			public int ID;
			public string FilePath;
			public string FileName;
		}
		public class _ManifestInterfaceItemIcon
		{
			public int ID;
		}
		public class _ManifestInterfaceTOCData
		{
			public int ID;
			public string FilePath;
		}
		public class _ManifestMP3
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
			public string PvpShortDescription;
			public string PvpLongDescription;
			public uint[] Flags = new uint[2];
			public float MinimapIconScale;
			public float[] Corpse = new float[2];
			public int ZmpFileDataId;
			public ushort AreaTableId;
			public ushort LoadingScreenId;
			public ushort TimeOfDayOverride;
			public ushort ParentMapId;
			public ushort CosmeticParentMapId;
			public ushort CorpseMapId;
			public ushort WindSettingsId;
			public byte MapType;
			public byte InstanceType;
			public byte ExpansionId;
			public byte TimeOffset;
			public byte MaxPlayers;
		}
		public class _MapCelestialBody
		{
			public int ID;
			public ushort CelestialBodyId;
			public int PlayerConditionId;
		}
		public class _MapChallengeMode
		{
			public string Name;
			public int ID;
			public ushort MapId;
			public ushort[] CriteriaCount = new ushort[3];
			public byte Flags;
		}
		public class _MapDifficulty
		{
			public int ID;
			public string Message;
			public byte DifficultyId;
			public byte LockId;
			public byte ResetInterval;
			public byte MaxPlayers;
			public byte ItemContext;
			public byte Flags;
			public int ItemContextPickerId;
		}
		public class _MapDifficultyXCondition
		{
			public int ID;
			public string FailureDescription;
			public int PlayerConditionId;
			public int OrderIndex;
		}
		public class _MapLoadingScreen
		{
			public int ID;
			public float[] Min = new float[2];
			public float[] Max = new float[2];
			public int LoadingScreenId;
			public int OrderIndex;
		}
		public class _MarketingPromotionsXLocale
		{
			public int ID;
			public string AcceptURL;
			public int AdTexture;
			public int LogoTexture;
			public int AcceptButtonTexture;
			public int DeclineButtonTexture;
			public byte PromotionId;
			public byte LocaleId;
		}
		public class _Material
		{
			public int ID;
			public byte Flags;
			public int FoleySoundId;
			public int SheatheSoundId;
			public int UnsheatheSoundId;
		}
		public class _MinorTalent
		{
			public int ID;
			public int SpellId;
			public int OrderIndex;
		}
		public class _MissileTargeting
		{
			public int ID;
			public float TurnLingering;
			public float PitchLingering;
			public float MouseLingering;
			public float EndOpacity;
			public float ArcSpeed;
			public float ArcRepeat;
			public float ArcWidth;
			public float[] ImpactRadius = new float[2];
			public float ImpactTexRadius;
			public int ArcTextureFileId;
			public int ImpactTextureFileId;
			public int[] ImpactModelFileId = new int[2];
		}
		public class _ModelAnimCloakDampening
		{
			public int ID;
			public int AnimationDataId;
			public int CloakDampeningId;
		}
		public class _ModelFileData
		{
			public byte Flags;
			public byte LodCount;
			public int ID;
			public int ModelResourcesId;
		}
		public class _ModelRibbonQuality
		{
			public int ID;
			public byte RibbonQualityId;
		}
		public class _ModifierTree
		{
			public int ID;
			public int Parent;
			public uint Asset;
			public int SecondaryAsset;
			public byte Operator;
			public byte Amount;
			public byte Type;
			public byte TertiaryAsset;
		}
		public class _Mount
		{
			public string Name;
			public string SourceText;
			public string Description;
			public int SourceSpellId;
			public float MountFlyRideHeight;
			public ushort MountTypeId;
			public ushort Flags;
			public byte SourceTypeEnum;
			public int ID;
			public int PlayerConditionId;
			public int UiModelSceneId;
		}
		public class _MountCapability
		{
			public int ReqSpellKnownId;
			public int ModSpellAuraId;
			public ushort ReqRidingSkill;
			public ushort ReqAreaId;
			public ushort ReqMapId;
			public byte Flags;
			public int ID;
			public int ReqSpellAuraId;
		}
		public class _MountTypeXCapability
		{
			public int ID;
			public ushort MountTypeId;
			public ushort MountCapabilityId;
			public byte OrderIndex;
		}
		public class _MountXDisplay
		{
			public int ID;
			public int CreatureDisplayInfoId;
			public int PlayerConditionId;
		}
		public class _Movie
		{
			public int ID;
			public int AudioFileDataId;
			public int SubtitleFileDataId;
			public byte Volume;
			public byte KeyId;
		}
		public class _MovieFileData
		{
			public int ID;
			public ushort Resolution;
		}
		public class _MovieVariation
		{
			public int ID;
			public int FileDataId;
			public int OverlayFileDataId;
		}
		public class _MultiStateProperties
		{
			public int ID;
			public float[] Offset = new float[3];
			public byte StateIndex;
			public int GameObjectId;
			public int GameEventId;
			public float Facing;
			public int TransitionInId;
			public int TransitionOutId;
			public int CollisionHull;
			public int Flags;
		}
		public class _MultiTransitionProperties
		{
			public int ID;
			public int TransitionType;
			public int DurationMS;
		}
		public class _NameGen
		{
			public int ID;
			public string Name;
			public byte RaceId;
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
		public class _NpcModelItemSlotDisplayInfo
		{
			public int ID;
			public int ItemDisplayInfoId;
			public byte ItemSlot;
		}
		public class _NPCSounds
		{
			public int ID;
			public int[] SoundId = new int[4];
		}
		public class _NumTalentsAtLevel
		{
			public int ID;
			public int NumTalents;
			public int NumTalentsDeathKnight;
			public int NumTalentsDemonHunter;
		}
		public class _ObjectEffect
		{
			public int ID;
			public float[] Offset = new float[3];
			public ushort ObjectEffectGroupId;
			public byte TriggerType;
			public byte EventType;
			public byte EffectRecType;
			public byte Attachment;
			public int EffectRecId;
			public int ObjectEffectModifierId;
		}
		public class _ObjectEffectModifier
		{
			public int ID;
			public float[] Param = new float[4];
			public byte InputType;
			public byte MapType;
			public byte OutputType;
		}
		public class _ObjectEffectPackageElem
		{
			public int ID;
			public ushort ObjectEffectPackageId;
			public ushort ObjectEffectGroupId;
			public ushort StateType;
		}
		public class _OutlineEffect
		{
			public int ID;
			public int PassiveHighlightColorId;
			public int HighlightColorId;
			public int Priority;
			public int Flags;
			public float Range;
			public int[] UnitConditionId = new int[2];
		}
		public class _OverrideSpellData
		{
			public int ID;
			public int[] Spells = new int[10];
			public uint PlayerActionBarFileDataId;
			public byte Flags;
		}
		public class _PageTextMaterial
		{
			public int ID;
			public string Name;
		}
		public class _PaperDollItemFrame
		{
			public int ID;
			public string ItemButtonName;
			public byte SlotNumber;
			public int SlotIconFileId;
		}
		public class _ParagonReputation
		{
			public int ID;
			public int LevelThreshold;
			public int QuestId;
			public int FactionId;
		}
		public class _ParticleColor
		{
			public int ID;
			public uint[] Start = new uint[3];
			public uint[] Mid = new uint[3];
			public int[] End = new int[3];
		}
		public class _Path
		{
			public int ID;
			public byte Type;
			public byte SplineType;
			public byte Red;
			public byte Green;
			public byte Blue;
			public byte Alpha;
			public byte Flags;
		}
		public class _PathNode
		{
			public int ID;
			public int LocationId;
			public ushort PathId;
			public ushort Sequence;
		}
		public class _PathNodeProperty
		{
			public ushort PathId;
			public ushort Sequence;
			public byte PropertyIndex;
			public int ID;
			public int Value;
		}
		public class _PathProperty
		{
			public int Value;
			public ushort PathId;
			public byte PropertyIndex;
			public int ID;
		}
		public class _Phase
		{
			public int ID;
			public ushort Flags;
		}
		public class _PhaseShiftZoneSounds
		{
			public int ID;
			public ushort AreaId;
			public ushort PhaseId;
			public ushort PhaseGroupId;
			public ushort SoundAmbienceId;
			public ushort UwSoundAmbienceId;
			public byte WmoAreaId;
			public byte PhaseUseFlags;
			public byte SoundProviderPreferencesId;
			public byte UwSoundProviderPreferencesId;
			public int ZoneIntroMusicId;
			public int ZoneMusicId;
			public int UwZoneIntroMusicId;
			public int UwZoneMusicId;
		}
		public class _PhaseXPhaseGroup
		{
			public int ID;
			public ushort PhaseId;
		}
		public class _PlayerCondition
		{
			public ulong RaceMask;
			public string FailureDescription;
			public int ID;
			public byte Flags;
			public ushort MinLevel;
			public ushort MaxLevel;
			public int ClassMask;
			public int SkillLogic;
			public byte LanguageId;
			public byte MinLanguage;
			public int MaxLanguage;
			public ushort MaxFactionId;
			public byte MaxReputation;
			public int ReputationLogic;
			public byte CurrentPvpFaction;
			public byte PvpMedal;
			public int PrevQuestLogic;
			public int CurrQuestLogic;
			public int CurrentCompletedQuestLogic;
			public int SpellLogic;
			public int ItemLogic;
			public byte ItemFlags;
			public int AuraSpellLogic;
			public ushort WorldStateExpressionId;
			public byte WeatherId;
			public byte PartyStatus;
			public byte LifetimeMaxPVPRank;
			public int AchievementLogic;
			public byte Gender;
			public byte NativeGender;
			public int AreaLogic;
			public int LfgLogic;
			public int CurrencyLogic;
			public ushort QuestKillId;
			public int QuestKillLogic;
			public byte MinExpansionLevel;
			public byte MaxExpansionLevel;
			public int MinAvgItemLevel;
			public int MaxAvgItemLevel;
			public ushort MinAvgEquippedItemLevel;
			public ushort MaxAvgEquippedItemLevel;
			public byte PhaseUseFlags;
			public ushort PhaseId;
			public int PhaseGroupId;
			public byte ChrSpecializationIndex;
			public byte ChrSpecializationRole;
			public int ModifierTreeId;
			public byte PowerType;
			public byte PowerTypeComp;
			public byte PowerTypeValue;
			public int WeaponSubclassMask;
			public byte MaxGuildLevel;
			public byte MinGuildLevel;
			public byte MaxExpansionTier;
			public byte MinExpansionTier;
			public byte MinPVPRank;
			public byte MaxPVPRank;
			public ushort[] SkillId = new ushort[4];
			public ushort[] MinSkill = new ushort[4];
			public ushort[] MaxSkill = new ushort[4];
			public int[] MinFactionId = new int[3];
			public byte[] MinReputation = new byte[3];
			public ushort[] PrevQuestId = new ushort[4];
			public ushort[] CurrQuestId = new ushort[4];
			public ushort[] CurrentCompletedQuestId = new ushort[4];
			public int[] SpellId = new int[4];
			public int[] ItemId = new int[4];
			public int[] ItemCount = new int[4];
			public ushort[] Explored = new ushort[2];
			public int[] Time = new int[2];
			public int[] AuraSpellId = new int[4];
			public byte[] AuraStacks = new byte[4];
			public ushort[] Achievement = new ushort[4];
			public ushort[] AreaId = new ushort[4];
			public byte[] LfgStatus = new byte[4];
			public byte[] LfgCompare = new byte[4];
			public int[] LfgValue = new int[4];
			public int[] CurrencyId = new int[4];
			public int[] CurrencyCount = new int[4];
			public int[] QuestKillMonster = new int[6];
			public int[] MovementFlags = new int[2];
		}
		public class _Positioner
		{
			public int ID;
			public float StartLife;
			public ushort FirstStateId;
			public byte Flags;
			public byte StartLifePercent;
		}
		public class _PositionerState
		{
			public int ID;
			public float EndLife;
			public byte EndLifePercent;
			public int NextStateId;
			public int TransformMatrixId;
			public int PosEntryId;
			public int RotEntryId;
			public int ScaleEntryId;
			public int Flags;
		}
		public class _PositionerStateEntry
		{
			public int ID;
			public float ParamA;
			public float ParamB;
			public ushort SrcValType;
			public ushort SrcVal;
			public ushort DstValType;
			public ushort DstVal;
			public byte EntryType;
			public byte Style;
			public byte SrcType;
			public byte DstType;
			public int CurveId;
		}
		public class _PowerDisplay
		{
			public int ID;
			public string GlobalStringBaseTag;
			public byte ActualType;
			public byte Red;
			public byte Green;
			public byte Blue;
		}
		public class _PowerType
		{
			public int ID;
			public string NameGlobalStringTag;
			public string CostGlobalStringTag;
			public float RegenPeace;
			public float RegenCombat;
			public ushort MaxBasePower;
			public ushort RegenInterruptTimeMS;
			public ushort Flags;
			public byte PowerTypeEnum;
			public byte MinPower;
			public byte CenterPower;
			public byte DefaultPower;
			public byte DisplayModifier;
		}
		public class _PrestigeLevelInfo
		{
			public int ID;
			public string Name;
			public int BadgeTextureFileDataId;
			public byte PrestigeLevel;
			public byte Flags;
		}
		public class _PvpBracketTypes
		{
			public int ID;
			public byte BracketId;
			public int[] WeeklyQuestId = new int[4];
		}
		public class _PvpDifficulty
		{
			public int ID;
			public byte RangeIndex;
			public byte MinLevel;
			public byte MaxLevel;
		}
		public class _PvpItem
		{
			public int ID;
			public int ItemId;
			public byte ItemLevelDelta;
		}
		public class _PvpReward
		{
			public int ID;
			public int HonorLevel;
			public int PrestigeLevel;
			public int RewardPackId;
		}
		public class _PvpScalingEffect
		{
			public int ID;
			public float Value;
			public int SpecializationId;
			public int PvpScalingEffectTypeId;
		}
		public class _PvpScalingEffectType
		{
			public int ID;
			public string Name;
		}
		public class _PvpTalent
		{
			public string Description;
			public int ID;
			public int SpecId;
			public int SpellId;
			public int OverridesSpellId;
			public int Flags;
			public int ActionBarSpellId;
			public int PvpTalentCategoryId;
			public int LevelRequired;
		}
		public class _PvpTalentCategory
		{
			public int ID;
			public byte TalentSlotMask;
		}
		public class _PvpTalentSlotUnlock
		{
			public int ID;
			public byte Slot;
			public int LevelRequired;
			public int DeathKnightLevelRequired;
			public int DemonHunterLevelRequired;
		}
		public class _QuestFactionReward
		{
			public int ID;
			public ushort[] Difficulty = new ushort[10];
		}
		public class _QuestFeedbackEffect
		{
			public int ID;
			public int FileDataId;
			public ushort MinimapAtlasMemberId;
			public byte AttachPoint;
			public byte PassiveHighlightColorType;
			public byte Priority;
			public byte Flags;
		}
		public class _QuestInfo
		{
			public int ID;
			public string InfoName;
			public ushort Profession;
			public byte Type;
			public byte Modifiers;
		}
		public class _QuestLine
		{
			public int ID;
			public string Name;
		}
		public class _QuestLineXQuest
		{
			public int ID;
			public int QuestLineId;
			public int QuestId;
			public int OrderIndex;
		}
		public class _QuestMoneyReward
		{
			public int ID;
			public int[] Difficulty = new int[10];
		}
		public class _QuestObjective
		{
			public int ID;
			public string Description;
			public int Amount;
			public int ObjectId;
			public byte Type;
			public byte OrderIndex;
			public byte Flags;
			public byte StorageIndex;
		}
		public class _QuestPackageItem
		{
			public int ID;
			public int ItemId;
			public ushort PackageId;
			public byte DisplayType;
			public int ItemQuantity;
		}
		public class _QuestPOIBlob
		{
			public int ID;
			public ushort MapId;
			public byte NumPoints;
			public int UiMapId;
			public int QuestId;
			public int ObjectiveIndex;
			public int PlayerConditionId;
		}
		public class _QuestPOIPoint
		{
			public int ID;
			public ushort X;
			public ushort Y;
		}
		public class _QuestSort
		{
			public int ID;
			public string SortName;
			public byte UiOrderIndex;
		}
		public class _QuestV2
		{
			public int ID;
			public ushort UniqueBitFlag;
		}
		public class _QuestV2CliTask
		{
			public ulong FiltRaces;
			public string QuestTitle;
			public string BulletText;
			public int ConditionId;
			public int FiltCompletedQuestLogic;
			public int StartItem;
			public ushort UniqueBitFlag;
			public ushort FiltClasses;
			public ushort[] FiltCompletedQuest = new ushort[3];
			public ushort FiltMinSkillId;
			public ushort WorldStateExpressionId;
			public byte FiltActiveQuest;
			public byte FiltMaxFactionId;
			public byte FiltMaxFactionValue;
			public byte FiltMaxLevel;
			public byte FiltMinFactionId;
			public byte FiltMinFactionValue;
			public byte FiltMinLevel;
			public byte FiltMinSkillValue;
			public byte FiltNonActiveQuest;
			public int ID;
			public int BreadCrumbId;
			public int QuestInfoId;
			public int ContentTuningId;
		}
		public class _QuestXGroupActivity
		{
			public int ID;
			public int QuestId;
			public int GroupFinderActivityId;
		}
		public class _QuestXP
		{
			public int ID;
			public ushort[] Difficulty = new ushort[10];
		}
		public class _RandPropPoints
		{
			public int ID;
			public int[] Epic = new int[5];
			public int[] Superior = new int[5];
			public int[] Good = new int[5];
		}
		public class _RelicSlotTierRequirement
		{
			public int ID;
			public int PlayerConditionId;
			public byte RelicIndex;
			public byte RelicTier;
		}
		public class _RelicTalent
		{
			public int ID;
			public ushort ArtifactPowerId;
			public byte ArtifactPowerLabel;
			public int Type;
			public int PVal;
			public int Flags;
		}
		public class _ResearchBranch
		{
			public int ID;
			public string Name;
			public int ItemId;
			public ushort CurrencyId;
			public byte ResearchFieldId;
			public int TextureFileId;
			public int BigTextureFileId;
		}
		public class _ResearchField
		{
			public string Name;
			public byte Slot;
			public int ID;
		}
		public class _ResearchProject
		{
			public string Name;
			public string Description;
			public int SpellId;
			public ushort ResearchBranchId;
			public byte Rarity;
			public byte NumSockets;
			public int ID;
			public int TextureFileId;
			public int RequiredWeight;
		}
		public class _ResearchSite
		{
			public int ID;
			public string Name;
			public int QuestPoiBlobId;
			public ushort MapId;
			public int AreaPOIIconEnum;
		}
		public class _Resistances
		{
			public int ID;
			public string Name;
			public byte Flags;
			public int FizzleSoundId;
		}
		public class _RewardPack
		{
			public int ID;
			public int Money;
			public float ArtifactXPMultiplier;
			public byte ArtifactXPDifficulty;
			public byte ArtifactXPCategoryId;
			public int CharTitleId;
			public int TreasurePickerId;
		}
		public class _RewardPackXCurrencyType
		{
			public int ID;
			public int CurrencyTypeId;
			public int Quantity;
		}
		public class _RewardPackXItem
		{
			public int ID;
			public int ItemId;
			public int ItemQuantity;
		}
		public class _RibbonQuality
		{
			public int ID;
			public float MaxSampleTimeDelta;
			public float AngleThreshold;
			public float MinDistancePerSlice;
			public byte NumStrips;
			public int Flags;
		}
		public class _RulesetItemUpgrade
		{
			public int ID;
			public int ItemId;
			public ushort ItemUpgradeId;
		}
		public class _ScalingStatDistribution
		{
			public int ID;
			public ushort PlayerLevelToItemLevelCurveId;
			public int MinLevel;
			public int MaxLevel;
		}
		public class _Scenario
		{
			public int ID;
			public string Name;
			public ushort AreaTableId;
			public byte Type;
			public byte Flags;
		}
		public class _ScenarioEventEntry
		{
			public int ID;
			public int TriggerAsset;
			public byte TriggerType;
		}
		public class _ScenarioStep
		{
			public int ID;
			public string Description;
			public string Title;
			public ushort ScenarioId;
			public ushort RewardQuestId;
			public ushort Supersedes;
			public byte OrderIndex;
			public byte Flags;
			public int Criteriatreeid;
			public int RelatedStep;
			public int VisibilityPlayerConditionId;
		}
		public class _SceneScript
		{
			public int ID;
			public ushort FirstSceneScriptId;
			public ushort NextSceneScriptId;
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
		public class _SceneScriptPackageMember
		{
			public int ID;
			public ushort SceneScriptPackageId;
			public ushort SceneScriptId;
			public ushort ChildSceneScriptPackageId;
			public byte OrderIndex;
		}
		public class _SceneScriptText
		{
			public int ID;
			public string Name;
			public string Script;
		}
		public class _ScheduledInterval
		{
			public int ID;
			public int Flags;
			public int RepeatType;
			public int DurationSecs;
			public int OffsetSecs;
			public int DateAlignmentType;
		}
		public class _ScheduledWorldState
		{
			public int ID;
			public int ScheduledWorldStateGroupId;
			public int WorldStateId;
			public int Value;
			public int DurationSecs;
			public int Weight;
			public int UniqueCategory;
			public int Flags;
			public int OrderIndex;
		}
		public class _ScheduledWorldStateGroup
		{
			public int ID;
			public int Flags;
			public int ScheduledIntervalId;
			public int SelectionType;
			public int SelectionCount;
			public int Priority;
		}
		public class _ScheduledWorldStateXUniqCat
		{
			public int ID;
			public int ScheduledUniqueCategoryId;
		}
		public class _ScreenEffect
		{
			public int ID;
			public string Name;
			public uint[] Param = new uint[4];
			public ushort LightParamsId;
			public ushort LightParamsFadeIn;
			public ushort LightParamsFadeOut;
			public ushort TimeOfDayOverride;
			public byte Effect;
			public byte EffectMask;
			public byte LightFlags;
			public int FullScreenEffectId;
			public int SoundAmbienceId;
			public int ZoneMusicId;
		}
		public class _ScreenLocation
		{
			public int ID;
			public string Name;
		}
		public class _SDReplacementModel
		{
			public int ID;
			public int SdFileDataId;
		}
		public class _SeamlessSite
		{
			public int ID;
			public int MapId;
		}
		public class _ServerMessages
		{
			public int ID;
			public string Text;
		}
		public class _ShadowyEffect
		{
			public int ID;
			public uint PrimaryColor;
			public uint SecondaryColor;
			public float Duration;
			public float Value;
			public float FadeInTime;
			public float FadeOutTime;
			public float InnerStrength;
			public float OuterStrength;
			public float InitialDelay;
			public byte AttachPos;
			public byte Flags;
			public int CurveId;
			public int Priority;
		}
		public class _SiegeableProperties
		{
			public int ID;
			public int Health;
			public int DamageSpellVisualKitId;
			public int HealingSpellVisualKitId;
			public int Flags;
		}
		public class _SkillLine
		{
			public int ID;
			public string DisplayName;
			public string AlternateVerb;
			public string Description;
			public string HordeDisplayName;
			public ushort Flags;
			public byte CategoryId;
			public byte CanLink;
			public int SpellIconFileId;
			public int ParentSkillLineId;
			public int ParentTierIndex;
		}
		public class _SkillLineAbility
		{
			public ulong RaceMask;
			public int ID;
			public int Spell;
			public int SupercedesSpell;
			public ushort SkillLine;
			public ushort TrivialSkillLineRankHigh;
			public ushort TrivialSkillLineRankLow;
			public ushort UniqueBit;
			public ushort TradeSkillCategoryId;
			public byte NumSkillUps;
			public ushort MinSkillLineRank;
			public int ClassMask;
			public byte AcquireMethod;
			public byte Flags;
			public ushort SkillupSkillLineId;
		}
		public class _SkillRaceClassInfo
		{
			public int ID;
			public ulong RaceMask;
			public ushort SkillId;
			public ushort Flags;
			public ushort SkillTierId;
			public byte Availability;
			public byte MinLevel;
			public int ClassMask;
		}
		public class _SoundAmbience
		{
			public int ID;
			public byte Flags;
			public int SoundFilterId;
			public int FlavorSoundFilterId;
			public int[] AmbienceId = new int[2];
			public int[] AmbienceStartId = new int[2];
			public int[] AmbienceStopId = new int[2];
		}
		public class _SoundAmbienceFlavor
		{
			public int ID;
			public int SoundEntriesIDDay;
			public int SoundEntriesIDNight;
		}
		public class _SoundBus
		{
			public float DefaultVolume;
			public byte Flags;
			public byte DefaultPriority;
			public byte DefaultPriorityPenalty;
			public byte DefaultPlaybackLimit;
			public byte BusEnumId;
			public int ID;
		}
		public class _SoundBusOverride
		{
			public int ID;
			public float Volume;
			public byte PlaybackLimit;
			public byte Priority;
			public byte PriorityPenalty;
			public int SoundBusId;
			public int PlayerConditionId;
		}
		public class _SoundEmitterPillPoints
		{
			public int ID;
			public float[] Position = new float[3];
			public ushort SoundEmittersId;
		}
		public class _SoundEmitters
		{
			public string Name;
			public float[] Position = new float[3];
			public float[] Direction = new float[3];
			public ushort WorldStateExpressionId;
			public ushort PhaseId;
			public byte EmitterType;
			public byte PhaseUseFlags;
			public byte Flags;
			public int ID;
			public int SoundEntriesId;
			public int PhaseGroupId;
		}
		public class _SoundEnvelope
		{
			public int ID;
			public int SoundKitId;
			public int CurveId;
			public ushort DecayIndex;
			public ushort SustainIndex;
			public ushort ReleaseIndex;
			public byte EnvelopeType;
			public int Flags;
		}
		public class _SoundFilter
		{
			public int ID;
			public string Name;
		}
		public class _SoundFilterElem
		{
			public int ID;
			public float[] Params = new float[9];
			public byte FilterType;
		}
		public class _SoundKit
		{
			public int ID;
			public float VolumeFloat;
			public float MinDistance;
			public float DistanceCutoff;
			public ushort Flags;
			public byte SoundType;
			public byte DialogType;
			public byte EAXDef;
			public int SoundKitAdvancedId;
			public float VolumeVariationPlus;
			public float VolumeVariationMinus;
			public float PitchVariationPlus;
			public float PitchVariationMinus;
			public float PitchAdjust;
			public ushort BusOverwriteId;
			public byte MaxInstances;
		}
		public class _SoundKitAdvanced
		{
			public int ID;
			public float InnerRadius2D;
			public float OuterRadius2D;
			public float DuckToSFX;
			public float DuckToMusic;
			public float InnerRadiusOfInfluence;
			public float OuterRadiusOfInfluence;
			public int TimeToDuck;
			public int TimeToUnduck;
			public byte Usage;
			public int SoundKitId;
			public int TimeA;
			public int TimeB;
			public int TimeC;
			public int TimeD;
			public int RandomOffsetRange;
			public int TimeIntervalMin;
			public int TimeIntervalMax;
			public int DelayMin;
			public int DelayMax;
			public byte VolumeSliderCategory;
			public float DuckToAmbience;
			public float DuckToDialog;
			public float DuckToSuppressors;
			public float InsideAngle;
			public float OutsideAngle;
			public float OutsideVolume;
			public byte MinRandomPosOffset;
			public ushort MaxRandomPosOffset;
			public int MsOffset;
			public int TimeCooldownMin;
			public int TimeCooldownMax;
			public byte MaxInstancesBehavior;
			public byte VolumeControlType;
			public int VolumeFadeInTimeMin;
			public int VolumeFadeInTimeMax;
			public int VolumeFadeInCurveId;
			public int VolumeFadeOutTimeMin;
			public int VolumeFadeOutTimeMax;
			public int VolumeFadeOutCurveId;
			public float ChanceToPlay;
		}
		public class _SoundKitChild
		{
			public int ID;
			public int SoundKitId;
			public int ParentSoundKitId;
		}
		public class _SoundKitEntry
		{
			public int ID;
			public int SoundKitId;
			public int FileDataId;
			public byte Frequency;
			public float Volume;
		}
		public class _SoundKitFallback
		{
			public int ID;
			public int SoundKitId;
			public int FallbackSoundKitId;
		}
		public class _SoundKitName
		{
			public int ID;
			public string Name;
		}
		public class _SoundOverride
		{
			public int ID;
			public ushort ZoneIntroMusicId;
			public ushort ZoneMusicId;
			public ushort SoundAmbienceId;
			public byte SoundProviderPreferencesId;
		}
		public class _SoundProviderPreferences
		{
			public int ID;
			public string Description;
			public float EAXDecayTime;
			public float EAX2EnvironmentSize;
			public float EAX2EnvironmentDiffusion;
			public float EAX2DecayHFRatio;
			public float EAX2ReflectionsDelay;
			public float EAX2ReverbDelay;
			public float EAX2RoomRolloff;
			public float EAX2AirAbsorption;
			public float EAX3DecayLFRatio;
			public float EAX3EchoTime;
			public float EAX3EchoDepth;
			public float EAX3ModulationTime;
			public float EAX3ModulationDepth;
			public float EAX3HFReference;
			public float EAX3LFReference;
			public ushort EAX2Room;
			public ushort EAX2RoomHF;
			public ushort EAX2Reflections;
			public ushort EAX2Reverb;
			public ushort Flags;
			public byte EAXEnvironmentSelection;
			public byte EAX3RoomLF;
		}
		public class _SourceInfo
		{
			public int ID;
			public string SourceText;
			public byte PvpFaction;
			public byte SourceTypeEnum;
		}
		public class _SpamMessages
		{
			public int ID;
			public string Text;
		}
		public class _SpecializationSpells
		{
			public string Description;
			public int SpellId;
			public int OverridesSpellId;
			public ushort SpecId;
			public byte DisplayOrder;
			public int ID;
		}
		public class _Spell
		{
			public int ID;
			public string NameSubtext;
			public string Description;
			public string AuraDescription;
		}
		public class _SpellActionBarPref
		{
			public int ID;
			public int SpellId;
			public ushort PreferredActionBarMask;
		}
		public class _SpellActivationOverlay
		{
			public int ID;
			public int SpellId;
			public int OverlayFileDataId;
			public uint Color;
			public float Scale;
			public uint[] IconHighlightSpellClassMask = new uint[4];
			public byte ScreenLocationId;
			public byte TriggerType;
			public int SoundEntriesId;
		}
		public class _SpellAuraOptions
		{
			public int ID;
			public int ProcCategoryRecovery;
			public uint ProcCharges;
			public uint[] ProcTypeMask = new uint[2];
			public ushort CumulativeAura;
			public ushort SpellProcsPerMinuteId;
			public byte DifficultyId;
			public byte ProcChance;
		}
		public class _SpellAuraRestrictions
		{
			public int ID;
			public int CasterAuraSpell;
			public int TargetAuraSpell;
			public int ExcludeCasterAuraSpell;
			public int ExcludeTargetAuraSpell;
			public byte DifficultyId;
			public byte CasterAuraState;
			public byte TargetAuraState;
			public byte ExcludeCasterAuraState;
			public byte ExcludeTargetAuraState;
		}
		public class _SpellAuraVisibility
		{
			public byte Type;
			public byte Flags;
			public int ID;
		}
		public class _SpellAuraVisXChrSpec
		{
			public int ID;
			public ushort ChrSpecializationId;
		}
		public class _SpellCastingRequirements
		{
			public int ID;
			public int SpellId;
			public ushort MinFactionId;
			public ushort RequiredAreasId;
			public ushort RequiresSpellFocus;
			public byte FacingCasterFlags;
			public byte MinReputation;
			public byte RequiredAuraVision;
		}
		public class _SpellCastTimes
		{
			public int ID;
			public uint Base;
			public uint Minimum;
			public ushort PerLevel;
		}
		public class _SpellCategories
		{
			public int ID;
			public ushort Category;
			public ushort StartRecoveryCategory;
			public ushort ChargeCategory;
			public byte DifficultyId;
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
			public int TypeMask;
		}
		public class _SpellChainEffects
		{
			public int ID;
			public float AvgSegLen;
			public float NoiseScale;
			public float TexCoordScale;
			public int SegDuration;
			public int Flags;
			public float JointOffsetRadius;
			public float MinorJointScale;
			public float MajorJointScale;
			public float JointMoveSpeed;
			public float JointSmoothness;
			public float MinDurationBetweenJointJumps;
			public float MaxDurationBetweenJointJumps;
			public float WaveHeight;
			public float WaveFreq;
			public float WaveSpeed;
			public float MinWaveAngle;
			public float MaxWaveAngle;
			public float MinWaveSpin;
			public float MaxWaveSpin;
			public float ArcHeight;
			public float MinArcAngle;
			public float MaxArcAngle;
			public float MinArcSpin;
			public float MaxArcSpin;
			public float DelayBetweenEffects;
			public float MinFlickerOnDuration;
			public float MaxFlickerOnDuration;
			public float MinFlickerOffDuration;
			public float MaxFlickerOffDuration;
			public float PulseSpeed;
			public float PulseOnLength;
			public float PulseFadeLength;
			public float WavePhase;
			public float TimePerFlipFrame;
			public float VariancePerFlipFrame;
			public float[] TextureCoordScaleU = new float[3];
			public float[] TextureCoordScaleV = new float[3];
			public float[] TextureRepeatLengthU = new float[3];
			public float[] TextureRepeatLengthV = new float[3];
			public int TextureParticleFileDataId;
			public float StartWidth;
			public float EndWidth;
			public float ParticleScaleMultiplier;
			public float ParticleEmissionRateMultiplier;
			public ushort SegDelay;
			public ushort JointCount;
			public ushort[] SpellChainEffectId = new ushort[11];
			public ushort WidthScaleCurveId;
			public byte JointsPerMinorJoint;
			public byte MinorJointsPerMajorJoint;
			public byte Alpha;
			public byte Red;
			public byte Green;
			public byte Blue;
			public byte BlendMode;
			public byte RenderLayer;
			public byte NumFlipFramesU;
			public byte NumFlipFramesV;
			public int SoundKitId;
			public int[] TextureFileDataId = new int[3];
		}
		public class _SpellClassOptions
		{
			public int ID;
			public int SpellId;
			public uint[] SpellClassMask = new uint[4];
			public byte SpellClassSet;
			public int ModalNextSpell;
		}
		public class _SpellCooldowns
		{
			public int ID;
			public int CategoryRecoveryTime;
			public int RecoveryTime;
			public int StartRecoveryTime;
			public byte DifficultyId;
		}
		public class _SpellDescriptionVariables
		{
			public int ID;
			public string Variables;
		}
		public class _SpellDispelType
		{
			public int ID;
			public string Name;
			public string InternalName;
			public byte ImmunityPossible;
			public byte Mask;
		}
		public class _SpellDuration
		{
			public int ID;
			public uint Duration;
			public uint MaxDuration;
			public uint DurationPerLevel;
		}
		public class _SpellEffect
		{
			public int ID;
			public int DifficultyId;
			public int EffectIndex;
			public int Effect;
			public float EffectAmplitude;
			public int EffectAttributes;
			public int EffectAura;
			public int EffectAuraPeriod;
			public int EffectBasePoints;
			public float EffectBonusCoefficient;
			public float EffectChainAmplitude;
			public int EffectChainTargets;
			public int EffectDieSides;
			public int EffectItemType;
			public int EffectMechanic;
			public float EffectPointsPerResource;
			public float EffectPosFacing;
			public float EffectRealPointsPerLevel;
			public int EffectTriggerSpell;
			public float BonusCoefficientFromAP;
			public float PvpMultiplier;
			public float Coefficient;
			public float Variance;
			public float ResourceCoefficient;
			public float GroupSizeBasePointsCoefficient;
			public int[] EffectMiscValue = new int[2];
			public int[] EffectRadiusIndex = new int[2];
			public int[] EffectSpellClassMask = new int[4];
			public int[] ImplicitTarget = new int[2];
		}
		public class _SpellEffectAutoDescription
		{
			public int ID;
			public string EffectDescription;
			public string AuraDescription;
			public int SpellEffectType;
			public int AuraEffectType;
			public int EffectOrderIndex;
			public int AuraOrderIndex;
			public byte PointsSign;
			public byte TargetType;
			public byte SchoolMask;
		}
		public class _SpellEffectEmission
		{
			public int ID;
			public float EmissionRate;
			public float ModelScale;
			public ushort AreaModelId;
			public byte Flags;
		}
		public class _SpellEquippedItems
		{
			public int ID;
			public int SpellId;
			public int EquippedItemInvTypes;
			public int EquippedItemSubclass;
			public byte EquippedItemClass;
		}
		public class _SpellFlyout
		{
			public int ID;
			public ulong RaceMask;
			public string Name;
			public string Description;
			public byte Flags;
			public int ClassMask;
			public int SpellIconFileId;
		}
		public class _SpellFlyoutItem
		{
			public int ID;
			public int SpellId;
			public byte Slot;
		}
		public class _SpellFocusObject
		{
			public int ID;
			public string Name;
		}
		public class _SpellInterrupts
		{
			public int ID;
			public byte DifficultyId;
			public ushort InterruptFlags;
			public int[] AuraInterruptFlags = new int[2];
			public int[] ChannelInterruptFlags = new int[2];
		}
		public class _SpellItemEnchantment
		{
			public int ID;
			public string Name;
			public string HordeName;
			public int[] EffectArg = new int[3];
			public float[] EffectScalingPoints = new float[3];
			public int TransmogCost;
			public int IconFileDataId;
			public ushort[] EffectPointsMin = new ushort[3];
			public ushort ItemVisual;
			public ushort Flags;
			public ushort RequiredSkillId;
			public ushort RequiredSkillRank;
			public ushort ItemLevel;
			public byte Charges;
			public byte[] Effect = new byte[3];
			public byte ScalingClass;
			public byte ScalingClassRestricted;
			public byte ConditionId;
			public byte MinLevel;
			public byte MaxLevel;
			public int TransmogPlayerConditionId;
		}
		public class _SpellItemEnchantmentCondition
		{
			public int ID;
			public int[] LtOperand = new int[5];
			public byte[] LtOperandType = new byte[5];
			public byte[] Operator = new byte[5];
			public byte[] RtOperandType = new byte[5];
			public byte[] RtOperand = new byte[5];
			public byte[] Logic = new byte[5];
		}
		public class _SpellKeyboundOverride
		{
			public int ID;
			public string Function;
			public int Data;
			public byte Type;
		}
		public class _SpellLabel
		{
			public int ID;
			public int LabelId;
		}
		public class _SpellLearnSpell
		{
			public int ID;
			public int SpellId;
			public int LearnSpellId;
			public int OverridesSpellId;
		}
		public class _SpellLevels
		{
			public int ID;
			public ushort BaseLevel;
			public ushort MaxLevel;
			public ushort SpellLevel;
			public byte DifficultyId;
			public byte MaxPassiveAuraLevel;
		}
		public class _SpellMechanic
		{
			public int ID;
			public string StateName;
		}
		public class _SpellMisc
		{
			public int ID;
			public byte DifficultyId;
			public ushort CastingTimeIndex;
			public ushort DurationIndex;
			public ushort RangeIndex;
			public byte SchoolMask;
			public float Speed;
			public float LaunchDelay;
			public int SpellIconFileDataId;
			public int ActiveIconFileDataId;
			public int[] Attributes = new int[14];
		}
		public class _SpellMissile
		{
			public int ID;
			public int SpellId;
			public float DefaultPitchMin;
			public float DefaultPitchMax;
			public float DefaultSpeedMin;
			public float DefaultSpeedMax;
			public float RandomizeFacingMin;
			public float RandomizeFacingMax;
			public float RandomizePitchMin;
			public float RandomizePitchMax;
			public float RandomizeSpeedMin;
			public float RandomizeSpeedMax;
			public float Gravity;
			public float MaxDuration;
			public float CollisionRadius;
			public byte Flags;
		}
		public class _SpellMissileMotion
		{
			public int ID;
			public string Name;
			public string ScriptBody;
			public byte Flags;
			public byte MissileCount;
		}
		public class _SpellName
		{
			public int ID;
			public string Name;
		}
		public class _SpellPower
		{
			public uint ManaCost;
			public float PowerCostPct;
			public float PowerCostMaxPct;
			public float PowerPctPerSecond;
			public int RequiredAuraSpellId;
			public byte OrderIndex;
			public byte PowerType;
			public int ID;
			public uint ManaCostPerLevel;
			public int ManaPerSecond;
			public int PowerDisplayId;
			public int AltPowerBarId;
			public int OptionalCost;
		}
		public class _SpellPowerDifficulty
		{
			public byte DifficultyId;
			public byte OrderIndex;
			public int ID;
		}
		public class _SpellProceduralEffect
		{
			public float[] Value = new float[4];
			public byte Type;
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
			public float[] RangeMin = new float[2];
			public float[] RangeMax = new float[2];
			public byte Flags;
		}
		public class _SpellReagents
		{
			public int ID;
			public int SpellId;
			public int[] Reagent = new int[8];
			public ushort[] ReagentCount = new ushort[8];
		}
		public class _SpellReagentsCurrency
		{
			public int ID;
			public int SpellId;
			public ushort CurrencyTypesId;
			public ushort CurrencyCount;
		}
		public class _SpellScaling
		{
			public int ID;
			public int SpellId;
			public ushort ScalesFromItemLevel;
			public int Class;
			public int MinScalingLevel;
			public int MaxScalingLevel;
		}
		public class _SpellShapeshift
		{
			public int ID;
			public int SpellId;
			public uint[] ShapeshiftExclude = new uint[2];
			public uint[] ShapeshiftMask = new uint[2];
			public byte StanceBarOrder;
		}
		public class _SpellShapeshiftForm
		{
			public int ID;
			public string Name;
			public int Flags;
			public float DamageVariance;
			public ushort CombatRoundTime;
			public ushort MountTypeId;
			public byte CreatureType;
			public byte BonusActionBar;
			public int AttackIconFileId;
			public int[] CreatureDisplayId = new int[4];
			public int[] PresetSpellId = new int[8];
		}
		public class _SpellSpecialUnitEffect
		{
			public int ID;
			public ushort SpellVisualEffectNameId;
			public int PositionerId;
		}
		public class _SpellTargetRestrictions
		{
			public int ID;
			public float ConeDegrees;
			public int Targets;
			public float Width;
			public ushort TargetCreatureType;
			public byte DifficultyId;
			public byte MaxTargets;
			public int MaxTargetLevel;
		}
		public class _SpellTotems
		{
			public int ID;
			public int SpellId;
			public int[] Totem = new int[2];
			public ushort[] RequiredTotemCategoryId = new ushort[2];
		}
		public class _SpellVisual
		{
			public int ID;
			public int Flags;
			public float[] MissileCastOffset = new float[3];
			public float[] MissileImpactOffset = new float[3];
			public ushort SpellVisualMissileSetId;
			public byte MissileAttachment;
			public byte MissileDestinationAttachment;
			public int AnimEventSoundId;
			public int MissileCastPositionerId;
			public int MissileImpactPositionerId;
			public int MissileTargetingKit;
			public int HostileSpellVisualId;
			public int CasterSpellVisualId;
			public ushort DamageNumberDelay;
			public int LowViolenceSpellVisualId;
		}
		public class _SpellVisualAnim
		{
			public int ID;
			public ushort InitialAnimId;
			public ushort LoopAnimId;
			public ushort AnimKitId;
		}
		public class _SpellVisualColorEffect
		{
			public int ID;
			public float Duration;
			public uint Color;
			public float ColorMultiplier;
			public ushort RedCurveId;
			public ushort GreenCurveId;
			public ushort BlueCurveId;
			public ushort AlphaCurveId;
			public ushort OpacityCurveId;
			public byte Flags;
			public byte Type;
			public int PositionerId;
		}
		public class _SpellVisualEffectName
		{
			public int ID;
			public int ModelFileDataId;
			public float BaseMissileSpeed;
			public float Scale;
			public float MinAllowedScale;
			public float MaxAllowedScale;
			public float Alpha;
			public int Flags;
			public int TextureFileDataId;
			public float EffectRadius;
			public int Type;
			public int GenericId;
			public int RibbonQualityId;
			public int DissolveEffectId;
			public int ModelPosition;
		}
		public class _SpellVisualEvent
		{
			public int ID;
			public int StartEvent;
			public int EndEvent;
			public int StartMinOffsetMs;
			public int StartMaxOffsetMs;
			public int EndMinOffsetMs;
			public int EndMaxOffsetMs;
			public int TargetType;
			public int SpellVisualKitId;
		}
		public class _SpellVisualKit
		{
			public int ID;
			public int Flags;
			public float FallbackPriority;
			public int FallbackSpellVisualKitId;
			public ushort DelayMin;
			public ushort DelayMax;
		}
		public class _SpellVisualKitAreaModel
		{
			public int ID;
			public int ModelFileDataId;
			public float EmissionRate;
			public float Spacing;
			public float ModelScale;
			public ushort LifeTime;
			public byte Flags;
		}
		public class _SpellVisualKitEffect
		{
			public int ID;
			public int EffectType;
			public int Effect;
		}
		public class _SpellVisualKitModelAttach
		{
			public float[] Offset = new float[3];
			public float[] OffsetVariation = new float[3];
			public int ID;
			public ushort SpellVisualEffectNameId;
			public byte AttachmentId;
			public ushort PositionerId;
			public float Yaw;
			public float Pitch;
			public float Roll;
			public float YawVariation;
			public float PitchVariation;
			public float RollVariation;
			public float Scale;
			public float ScaleVariation;
			public ushort StartAnimId;
			public ushort AnimId;
			public ushort EndAnimId;
			public ushort AnimKitId;
			public byte Flags;
			public int LowDefModelAttachId;
			public float StartDelay;
		}
		public class _SpellVisualMissile
		{
			public uint FollowGroundHeight;
			public int FollowGroundDropSpeed;
			public int Flags;
			public float[] CastOffset = new float[3];
			public float[] ImpactOffset = new float[3];
			public ushort SpellVisualEffectNameId;
			public ushort CastPositionerId;
			public ushort ImpactPositionerId;
			public ushort FollowGroundApproach;
			public ushort SpellMissileMotionId;
			public byte Attachment;
			public byte DestinationAttachment;
			public int ID;
			public int SoundEntriesId;
			public int AnimKitId;
		}
		public class _SpellXDescriptionVariables
		{
			public int ID;
			public int SpellId;
			public int SpellDescriptionVariablesId;
		}
		public class _SpellXSpellVisual
		{
			public int SpellVisualId;
			public int ID;
			public byte DifficultyId;
			public float Probability;
			public byte Flags;
			public byte Priority;
			public int SpellIconFileId;
			public int ActiveIconFileId;
			public ushort ViewerUnitConditionId;
			public int ViewerPlayerConditionId;
			public ushort CasterUnitConditionId;
			public int CasterPlayerConditionId;
		}
		public class _StartupFiles
		{
			public int ID;
			public int FileDataId;
			public int Locale;
			public int BytesRequired;
		}
		public class _Startup_Strings
		{
			public int ID;
			public string Name;
			public string Message;
		}
		public class _Stationery
		{
			public int ID;
			public byte Flags;
			public int ItemId;
			public int[] TextureFileDataId = new int[2];
		}
		public class _SummonProperties
		{
			public int ID;
			public uint Flags;
			public int Control;
			public int Faction;
			public int Title;
			public int Slot;
		}
		public class _TactKey
		{
			public int ID;
			public byte[] Key = new byte[16];
		}
		public class _TactKeyLookup
		{
			public int ID;
			public byte[] TACTId = new byte[8];
		}
		public class _Talent
		{
			public int ID;
			public string Description;
			public int SpellId;
			public int OverridesSpellId;
			public ushort SpecId;
			public byte TierId;
			public byte Flags;
			public byte ColumnIndex;
			public byte[] CategoryMask = new byte[2];
			public byte ClassId;
		}
		public class _TaxiNodes
		{
			public string Name;
			public int ID;
			public float[] Pos = new float[3];
			public int[] MountCreatureId = new int[2];
			public float[] MapOffset = new float[2];
			public float Facing;
			public float[] FlightMapOffset = new float[2];
			public ushort ContinentId;
			public ushort ConditionId;
			public ushort CharacterBitNumber;
			public byte Flags;
			public int UiTextureKitId;
			public int SpecialIconConditionId;
		}
		public class _TaxiPath
		{
			public ushort FromTaxiNode;
			public ushort ToTaxiNode;
			public int ID;
			public int Cost;
		}
		public class _TaxiPathNode
		{
			public float[] Loc = new float[3];
			public ushort PathId;
			public ushort ContinentId;
			public byte NodeIndex;
			public int ID;
			public byte Flags;
			public int Delay;
			public ushort ArrivalEventId;
			public ushort DepartureEventId;
		}
		public class _TerrainMaterial
		{
			public int ID;
			public byte Shader;
			public int EnvMapDiffuseFileId;
			public int EnvMapSpecularFileId;
		}
		public class _TerrainType
		{
			public int ID;
			public string TerrainDesc;
			public ushort FootstepSprayRun;
			public ushort FootstepSprayWalk;
			public byte SoundId;
			public byte Flags;
		}
		public class _TerrainTypeSounds
		{
			public int ID;
			public string Name;
		}
		public class _TextureBlendSet
		{
			public int ID;
			public int[] TextureFileDataId = new int[3];
			public float[] TextureScrollRateU = new float[3];
			public float[] TextureScrollRateV = new float[3];
			public float[] TextureScaleU = new float[3];
			public float[] TextureScaleV = new float[3];
			public float[] ModX = new float[4];
			public byte SwizzleRed;
			public byte SwizzleGreen;
			public byte SwizzleBlue;
			public byte SwizzleAlpha;
		}
		public class _TextureFileData
		{
			public int ID;
			public byte UsageType;
			public int MaterialResourcesId;
		}
		public class _TotemCategory
		{
			public int ID;
			public string Name;
			public uint TotemCategoryMask;
			public byte TotemCategoryType;
		}
		public class _Toy
		{
			public string SourceText;
			public int ItemId;
			public byte Flags;
			public byte SourceTypeEnum;
			public int ID;
		}
		public class _TradeSkillCategory
		{
			public string Name;
			public string HordeName;
			public int ID;
			public ushort ParentTradeSkillCategoryId;
			public ushort SkillLineId;
			public ushort OrderIndex;
			public byte Flags;
		}
		public class _TradeSkillItem
		{
			public int ID;
			public ushort ItemLevel;
			public byte RequiredLevel;
		}
		public class _TransformMatrix
		{
			public int ID;
			public float[] Pos = new float[3];
			public float Yaw;
			public float Pitch;
			public float Roll;
			public float Scale;
		}
		public class _TransmogHoliday
		{
			public int ID;
			public int RequiredTransmogHoliday;
		}
		public class _TransmogSet
		{
			public string Name;
			public ushort ParentTransmogSetId;
			public ushort UiOrder;
			public byte ExpansionId;
			public int ID;
			public int ClassMask;
			public int TrackingQuestId;
			public int Flags;
			public int TransmogSetGroupId;
			public int ItemNameDescriptionId;
		}
		public class _TransmogSetGroup
		{
			public string Name;
			public int ID;
		}
		public class _TransmogSetItem
		{
			public int ID;
			public int TransmogSetId;
			public int ItemModifiedAppearanceId;
			public int Flags;
		}
		public class _TransportAnimation
		{
			public int ID;
			public int TimeIndex;
			public float[] Pos = new float[3];
			public byte SequenceId;
		}
		public class _TransportPhysics
		{
			public int ID;
			public float WaveAmp;
			public float WaveTimeScale;
			public float RollAmp;
			public float RollTimeScale;
			public float PitchAmp;
			public float PitchTimeScale;
			public float MaxBank;
			public float MaxBankTurnSpeed;
			public float SpeedDampThresh;
			public float SpeedDamp;
		}
		public class _TransportRotation
		{
			public int ID;
			public int TimeIndex;
			public float[] Rot = new float[4];
		}
		public class _Trophy
		{
			public int ID;
			public string Name;
			public ushort GameObjectDisplayInfoId;
			public byte TrophyTypeId;
			public int PlayerConditionId;
		}
		public class _UiCamera
		{
			public int ID;
			public string Name;
			public float[] Pos = new float[3];
			public float[] LookAt = new float[3];
			public float[] Up = new float[3];
			public ushort AnimFrame;
			public byte UiCameraTypeId;
			public byte AnimVariation;
			public byte Flags;
			public int AnimId;
		}
		public class _UiCameraType
		{
			public int ID;
			public string Name;
			public int Width;
			public int Height;
		}
		public class _UiCamFbackTransmogChrRace
		{
			public int ID;
			public ushort UiCameraId;
			public byte ChrRaceId;
			public byte Gender;
			public byte InventoryType;
			public byte Variation;
		}
		public class _UiCamFbackTransmogWeapon
		{
			public int ID;
			public ushort UiCameraId;
			public byte ItemClass;
			public byte ItemSubclass;
			public byte InventoryType;
		}
		public class _UiCanvas
		{
			public int ID;
			public ushort Width;
			public ushort Height;
		}
		public class _UIExpansionDisplayInfo
		{
			public int ID;
			public int ExpansionLogo;
			public int ExpansionBanner;
			public int ExpansionLevel;
		}
		public class _UIExpansionDisplayInfoIcon
		{
			public int ID;
			public string FeatureDescription;
			public int ParentId;
			public int FeatureIcon;
		}
		public class _UiMap
		{
			public string Name;
			public int ID;
			public int ParentUiMapId;
			public int Flags;
			public int System;
			public int Type;
			public int LevelRangeMin;
			public int LevelRangeMax;
			public int BountySetId;
			public int BountyDisplayLocation;
			public int VisibilityPlayerConditionId;
			public byte HelpTextPosition;
			public int BkgFileDataId;
		}
		public class _UiMapArt
		{
			public int ID;
			public int HighlightFileDataId;
			public int UiMapArtStyleId;
		}
		public class _UiMapArtStyleLayer
		{
			public int ID;
			public float MinScale;
			public float MaxScale;
			public int AdditionalZoomSteps;
			public ushort LayerWidth;
			public ushort LayerHeight;
			public ushort TileWidth;
			public ushort TileHeight;
			public byte LayerIndex;
		}
		public class _UiMapArtTile
		{
			public int ID;
			public int FileDataId;
			public byte RowIndex;
			public byte ColIndex;
			public byte LayerIndex;
		}
		public class _UiMapAssignment
		{
			public int ID;
			public int UiMapId;
			public uint OrderIndex;
			public float[] UiMin = new float[2];
			public float[] UiMax = new float[2];
			public int MapId;
			public int AreaId;
			public int WmoDoodadPlacementId;
			public int WmoGroupId;
			public float[] Region = new float[6];
		}
		public class _UiMapFogOfWar
		{
			public int ID;
			public int UiMapId;
			public int PlayerConditionId;
			public int UiMapFogOfWarVisId;
		}
		public class _UiMapFogOfWarVisualization
		{
			public int ID;
			public float MaskScalar;
			public int BackgroundAtlasId;
			public int MaskAtlasId;
		}
		public class _UiMapGroupMember
		{
			public int ID;
			public string Name;
			public int UiMapGroupId;
			public int UiMapId;
			public int FloorIndex;
			public byte RelativeHeightIndex;
		}
		public class _UiMapPOI
		{
			public int ContinentId;
			public float[] WorldLoc = new float[3];
			public int UiTextureAtlasMemberId;
			public int Flags;
			public ushort PoiDataType;
			public ushort PoiData;
			public int ID;
		}
		public class _UiMapXMapArt
		{
			public int ID;
			public int PhaseId;
			public int UiMapArtId;
		}
		public class _UiModelScene
		{
			public int ID;
			public byte UiSystemType;
			public byte Flags;
		}
		public class _UiModelSceneActor
		{
			public string ScriptTag;
			public float[] Position = new float[3];
			public float OrientationYaw;
			public float OrientationPitch;
			public float OrientationRoll;
			public float NormalizedScale;
			public byte Flags;
			public int ID;
			public int UiModelSceneActorDisplayId;
		}
		public class _UiModelSceneActorDisplay
		{
			public int ID;
			public float Alpha;
			public float Scale;
			public float AnimSpeed;
			public int AnimationId;
			public int SequenceVariation;
		}
		public class _UiModelSceneCamera
		{
			public string ScriptTag;
			public float[] Target = new float[3];
			public float Yaw;
			public float Pitch;
			public float Roll;
			public float[] ZoomedTargetOffset = new float[3];
			public float ZoomedYawOffset;
			public float ZoomedPitchOffset;
			public float ZoomedRollOffset;
			public float ZoomDistance;
			public float MinZoomDistance;
			public float MaxZoomDistance;
			public byte Flags;
			public byte CameraType;
			public int ID;
		}
		public class _UiPartyPose
		{
			public int ID;
			public int UiWidgetSetId;
			public int UiModelSceneId;
		}
		public class _UiTextureAtlas
		{
			public int ID;
			public int FileDataId;
			public ushort AtlasWidth;
			public ushort AtlasHeight;
			public byte UiCanvasId;
		}
		public class _UiTextureAtlasElement
		{
			public string Name;
			public int ID;
		}
		public class _UiTextureAtlasMember
		{
			public string CommittedName;
			public int ID;
			public ushort UiTextureAtlasId;
			public ushort CommittedLeft;
			public ushort CommittedRight;
			public ushort CommittedTop;
			public ushort CommittedBottom;
			public ushort UiTextureAtlasElementId;
			public byte CommittedFlags;
			public byte UiCanvasId;
		}
		public class _UiTextureKit
		{
			public int ID;
			public string KitPrefix;
		}
		public class _UiWidget
		{
			public int ID;
			public int VisId;
			public uint MapId;
			public int PlayerConditionId;
			public int OrderIndex;
			public ushort ParentSetId;
		}
		public class _UiWidgetConstantSource
		{
			public int ID;
			public int Value;
			public ushort ReqId;
		}
		public class _UiWidgetDataSource
		{
			public int ID;
			public ushort ReqId;
			public ushort SourceId;
			public byte SourceType;
		}
		public class _UiWidgetStringSource
		{
			public int ID;
			public string Value;
			public ushort ReqId;
		}
		public class _UiWidgetVisualization
		{
			public int ID;
			public int TextureKit;
			public byte VisType;
		}
		public class _UnitBlood
		{
			public int ID;
			public int PlayerCritBloodSpurtId;
			public int PlayerHitBloodSpurtId;
			public int DefaultBloodSpurtId;
			public int PlayerOmniCritBloodSpurtId;
			public int PlayerOmniHitBloodSpurtId;
			public int DefaultOmniBloodSpurtId;
		}
		public class _UnitBloodLevels
		{
			public int ID;
			public byte[] Violencelevel = new byte[3];
		}
		public class _UnitCondition
		{
			public int ID;
			public int[] Value = new int[8];
			public byte Flags;
			public byte[] Variable = new byte[8];
			public byte[] Op = new byte[8];
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
			public int[] FileDataId = new int[6];
			public uint[] Color = new uint[6];
			public float StartInset;
			public float EndInset;
			public ushort StartPower;
			public ushort Flags;
			public byte CenterPower;
			public byte BarType;
			public int MinPower;
			public int MaxPower;
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
			public ushort[] SeatId = new ushort[8];
			public ushort[] PowerDisplayId = new ushort[3];
			public ushort VehicleUIIndicatorId;
			public byte FlagsB;
			public byte UiLocomotionType;
			public int MissileTargetingId;
		}
		public class _VehicleSeat
		{
			public int ID;
			public uint Flags;
			public uint FlagsB;
			public uint FlagsC;
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
			public int UiSkinFileDataId;
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
			public ushort EnterAnimStart;
			public ushort EnterAnimLoop;
			public ushort RideAnimStart;
			public ushort RideAnimLoop;
			public ushort RideUpperAnimStart;
			public ushort RideUpperAnimLoop;
			public ushort ExitAnimStart;
			public ushort ExitAnimLoop;
			public ushort ExitAnimEnd;
			public ushort VehicleEnterAnim;
			public ushort VehicleExitAnim;
			public ushort VehicleRideAnimLoop;
			public ushort EnterAnimKitId;
			public ushort RideAnimKitId;
			public ushort ExitAnimKitId;
			public ushort VehicleEnterAnimKitId;
			public ushort VehicleRideAnimKitId;
			public ushort VehicleExitAnimKitId;
			public ushort CameraModeId;
			public byte AttachmentId;
			public byte VehicleEnterAnimBone;
			public byte VehicleExitAnimBone;
			public byte VehicleRideAnimLoopBone;
			public byte PassengerAttachmentId;
			public byte VehicleAbilityDisplay;
			public int EnterUISoundId;
			public int ExitUISoundId;
		}
		public class _VehicleUIIndicator
		{
			public int ID;
			public int BackgroundTextureFileId;
		}
		public class _VehicleUIIndSeat
		{
			public int ID;
			public float XPos;
			public float YPos;
			public byte VirtualSeatIndex;
		}
		public class _Vignette
		{
			public int ID;
			public string Name;
			public float MaxHeight;
			public float MinHeight;
			public int PlayerConditionId;
			public int VisibleTrackingQuestId;
			public int QuestFeedbackEffectId;
			public int Flags;
		}
		public class _VirtualAttachment
		{
			public int ID;
			public string Name;
			public ushort PositionerId;
		}
		public class _VirtualAttachmentCustomization
		{
			public int ID;
			public int FileDataId;
			public ushort VirtualAttachmentId;
			public ushort PositionerId;
		}
		public class _VocalUISounds
		{
			public int ID;
			public byte VocalUIEnum;
			public byte RaceId;
			public byte ClassId;
			public int[] NormalSoundId = new int[2];
		}
		public class _WbAccessControlList
		{
			public int ID;
			public string URL;
			public ushort GrantFlags;
			public byte RevokeFlags;
			public byte WowEditInternal;
			public byte RegionId;
		}
		public class _WbCertWhitelist
		{
			public int ID;
			public string Domain;
			public byte GrantAccess;
			public byte RevokeAccess;
			public byte WowEditInternal;
		}
		public class _WeaponImpactSounds
		{
			public int ID;
			public byte WeaponSubClassId;
			public byte ParrySoundType;
			public byte ImpactSource;
			public int[] ImpactSoundId = new int[11];
			public int[] CritImpactSoundId = new int[11];
			public int[] PierceImpactSoundId = new int[11];
			public int[] PierceCritImpactSoundId = new int[11];
		}
		public class _WeaponSwingSounds2
		{
			public int ID;
			public byte SwingType;
			public byte Crit;
			public int SoundId;
		}
		public class _WeaponTrail
		{
			public int ID;
			public int FileDataId;
			public float Roll;
			public float Pitch;
			public float Yaw;
			public int[] TextureFileDataId = new int[3];
			public float[] TextureScrollRateU = new float[3];
			public float[] TextureScrollRateV = new float[3];
			public float[] TextureScaleU = new float[3];
			public float[] TextureScaleV = new float[3];
		}
		public class _WeaponTrailModelDef
		{
			public int ID;
			public int LowDefFileDataId;
			public ushort WeaponTrailId;
		}
		public class _WeaponTrailParam
		{
			public int ID;
			public float Duration;
			public float FadeOutTime;
			public float EdgeLifeSpan;
			public float InitialDelay;
			public float SmoothSampleAngle;
			public byte Hand;
			public byte OverrideAttachTop;
			public byte OverrideAttachBot;
			public byte Flags;
		}
		public class _Weather
		{
			public int ID;
			public float[] Intensity = new float[2];
			public float TransitionSkyBox;
			public float[] EffectColor = new float[3];
			public float Scale;
			public float Volatility;
			public float TwinkleIntensity;
			public float FallModifier;
			public float RotationalSpeed;
			public int ParticulateFileDataId;
			public float VolumeEdgeFadeStart;
			public int OverrideColor;
			public int OverrideCount;
			public float OverrideOpacity;
			public int VolumeFlags;
			public int LightningId;
			public ushort SoundAmbienceId;
			public byte Type;
			public byte EffectType;
			public byte WindSettingsId;
			public int AmbienceId;
			public int EffectTextureFileDataId;
		}
		public class _WeatherXParticulate
		{
			public int ID;
			public int FileDataId;
		}
		public class _WindSettings
		{
			public int ID;
			public float BaseMag;
			public float[] BaseDir = new float[3];
			public float VarianceMagOver;
			public float VarianceMagUnder;
			public float[] VarianceDir = new float[3];
			public float MaxStepMag;
			public float[] MaxStepDir = new float[3];
			public float Frequency;
			public float Duration;
			public byte Flags;
		}
		public class _WMOAreaTable
		{
			public string AreaName;
			public uint WmoGroupId;
			public ushort WmoId;
			public ushort AmbienceId;
			public ushort UwAmbience;
			public ushort ZoneMusic;
			public ushort IntroSound;
			public ushort UwIntroSound;
			public ushort AreaTableId;
			public byte NameSetId;
			public byte SoundProviderPref;
			public byte SoundProviderPrefUnderwater;
			public byte Flags;
			public int ID;
			public int UwZoneMusic;
		}
		public class _WmoMinimapTexture
		{
			public int ID;
			public int FileDataId;
			public ushort GroupNum;
			public byte BlockX;
			public byte BlockY;
		}
		public class _WorldBossLockout
		{
			public int ID;
			public string Name;
			public ushort TrackingQuestId;
		}
		public class _WorldChunkSounds
		{
			public int ID;
			public ushort MapId;
			public byte SoundOverrideId;
			public byte ChunkX;
			public byte ChunkY;
			public byte SubChunkX;
			public byte SubChunkY;
		}
		public class _WorldEffect
		{
			public int ID;
			public uint TargetAsset;
			public ushort CombatConditionId;
			public byte WhenToDisplay;
			public byte TargetType;
			public int QuestFeedbackEffectId;
			public int PlayerConditionId;
		}
		public class _WorldElapsedTimer
		{
			public int ID;
			public string Name;
			public byte Type;
			public byte Flags;
		}
		public class _WorldMapOverlay
		{
			public int ID;
			public ushort TextureWidth;
			public ushort TextureHeight;
			public int UiMapArtId;
			public int OffsetX;
			public int OffsetY;
			public int HitRectTop;
			public int HitRectBottom;
			public int HitRectLeft;
			public int HitRectRight;
			public int PlayerConditionId;
			public int Flags;
			public int[] AreaId = new int[4];
		}
		public class _WorldMapOverlayTile
		{
			public int ID;
			public int FileDataId;
			public byte RowIndex;
			public byte ColIndex;
			public byte LayerIndex;
		}
		public class _WorldSafeLocs
		{
			public int ID;
			public string Name;
			public float X;
			public float Y;
			public float Z;
			public float Rotation;
			public ushort MapId;
		}
		public class _WorldState
		{
			public int ID;
		}
		public class _WorldStateExpression
		{
			public int ID;
			public string Expression;
		}
		public class _WorldStateUI
		{
			public string Icon;
			public string String;
			public string Tooltip;
			public string DynamicTooltip;
			public string ExtendedUI;
			public ushort MapId;
			public ushort AreaId;
			public ushort StateVariable;
			public ushort[] ExtendedUIStateVariable = new ushort[3];
			public ushort PhaseId;
			public ushort PhaseGroupId;
			public byte Type;
			public byte OrderIndex;
			public byte PhaseUseFlags;
			public int ID;
			public int DynamicIconFileId;
			public int DynamicFlashIconFileId;
		}
		public class _WorldStateZoneSounds
		{
			public int ID;
			public int WmoAreaId;
			public ushort WorldStateId;
			public ushort WorldStateValue;
			public ushort AreaId;
			public ushort ZoneIntroMusicId;
			public ushort ZoneMusicId;
			public ushort SoundAmbienceId;
			public byte SoundProviderPreferencesId;
		}
		public class _World_PVP_Area
		{
			public int ID;
			public ushort AreaId;
			public ushort NextTimeWorldstate;
			public ushort GameTimeWorldstate;
			public ushort BattlePopulateTime;
			public ushort MapId;
			public byte MinLevel;
			public byte MaxLevel;
		}
		public class _ZoneIntroMusicTable
		{
			public int ID;
			public string Name;
			public ushort MinDelayMinutes;
			public byte Priority;
			public int SoundId;
		}
		public class _ZoneLight
		{
			public int ID;
			public string Name;
			public ushort MapId;
			public ushort LightId;
			public byte Flags;
		}
		public class _ZoneLightPoint
		{
			public int ID;
			public float[] Pos = new float[2];
			public byte PointOrder;
		}
		public class _ZoneMusic
		{
			public int ID;
			public string SetName;
			public int[] SilenceIntervalMin = new int[2];
			public int[] SilenceIntervalMax = new int[2];
			public int[] Sounds = new int[2];
		}
		public class _ZoneStory
		{
			public int ID;
			public int DisplayAchievementId;
			public int DisplayWorldMapAreaId;
			public byte PlayerFactionGroupId;
		}
	}
}
