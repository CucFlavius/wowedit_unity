public static partial class DB2
{
	public static class Definitions_Legion_735_25632
	{
		public class _Achievement
		{
			string Title;
			string Description;
			string Reward;
			uint Flags;
			ushort MapID;
			ushort Supercedes;
			ushort Category;
			ushort UIOrder;
			ushort SharesCriteria;
			byte Faction;
			byte Points;
			byte MinimumCriteria;
			int ID;
			uint IconFileDataID;
			uint CriteriaTree;
		}
		public class _Achievement_category
		{
			string Field0;
			ushort Field1;
			byte Field2;
			int ID;
		}
		public class _Adventurejournal
		{
			int ID;
			string Field01;
			string Field02;
			string Field03;
			string Field04;
			string Field05;
			int Field06;
			int Field07;
			ushort Field08;
			ushort Field09;
			ushort Field0A;
			ushort[] Field0B = new ushort[2];
			ushort Field0C;
			ushort Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
			byte Field11;
			byte Field12;
			byte[] Field13 = new byte[2];
			byte Field14;
			int Field15;
			byte Field16;
		}
		public class _Adventuremappoi
		{
			int ID;
			string Field1;
			string Field2;
			float[] Field3 = new float[2];
			int Field4;
			byte Field5;
			ushort Field6;
			int Field7;
			byte Field8;
			ushort Field9;
			byte FieldA;
			ushort FieldB;
			int FieldC;
			ushort FieldD;
		}
		public class _alliedrace
		{
			int field0;
			int ID;
			byte field2;
			ushort field3;
			int field4;
			int field5;
			int field6;
			ushort field7;
		}
		public class _Alliedrace
		{
			int Field0;
			int ID;
			byte Field2;
			ushort Field3;
			int Field4;
		}
		public class _alliedraceracialability
		{
			int ID;
			string field1;
			string field2;
			byte field3;
			int field4;
		}
		public class _Alliedraceracialability
		{
			int ID;
			string Field1;
			string Field2;
			int Field3;
		}
		public class _Animationdata
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
		}
		public class _AnimKit
		{
			int ID;
			uint OneShotDuration;
			ushort OneShotStopAnimKitID;
			ushort LowDefAnimKitID;
		}
		public class _Animkitboneset
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Animkitbonesetalias
		{
			int ID;
			byte Field1;
			byte Field2;
		}
		public class _Animkitconfig
		{
			int ID;
			int Field1;
		}
		public class _Animkitconfigboneset
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _Animkitpriority
		{
			int ID;
			byte Field1;
		}
		public class _Animkitreplacement
		{
			ushort Field0;
			ushort Field1;
			ushort Field2;
			int ID;
		}
		public class _Animkitsegment
		{
			int ID;
			int Field01;
			int Field02;
			int Field03;
			float Field04;
			int Field05;
			ushort Field06;
			ushort Field07;
			ushort Field08;
			ushort Field09;
			ushort Field0A;
			ushort Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
			byte Field11;
			int Field12;
		}
		public class _Animreplacement
		{
			ushort Field0;
			ushort Field1;
			ushort Field2;
			int ID;
		}
		public class _Animreplacementset
		{
			int ID;
			byte Field1;
		}
		public class _Areafarclipoverride
		{
			int Field0;
			float Field1;
			float Field2;
			int Field3;
			int ID;
		}
		public class _AreaGroupMember
		{
			int ID;
			ushort AreaID;
		}
		public class _Areapoi
		{
			int ID;
			string Field01;
			string Field02;
			int Field03;
			float[] Field04 = new float[3];
			int Field05;
			int Field06;
			ushort Field07;
			ushort Field08;
			ushort Field09;
			ushort Field0A;
			byte Field0B;
			byte Field0C;
			ushort Field0D;
			ushort Field0E;
			int Field0F;
			int Field10;
		}
		public class _Areapoistate
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
			int Field4;
		}
		public class _AreaTable
		{
			int ID;
			string ZoneName;
			string AreaName;
			uint[] Flags = new uint[2];
			float AmbientMultiplier;
			ushort MapID;
			ushort ParentAreaID;
			short AreaBit;
			ushort AmbienceID;
			ushort ZoneMusic;
			ushort IntroSound;
			ushort[] LiquidTypeID = new ushort[4];
			ushort UWZoneMusic;
			ushort UWAmbience;
			ushort PvPCombatWorldStateID;
			byte SoundProviderPref;
			byte SoundProviderPrefUnderwater;
			byte ExplorationLevel;
			byte FactionGroupMask;
			byte MountFlags;
			byte WildBattlePetLevelMin;
			byte WildBattlePetLevelMax;
			byte WindSettingsID;
			uint UWIntroSound;
		}
		public class _AreaTrigger
		{
			UnityEngine.Vector3 Unknown;
			float Radius;
			float BoxLength;
			float BoxWidth;
			float BoxHeight;
			float BoxYaw;
			ushort MapID;
			ushort PhaseID;
			ushort PhaseGroupID;
			ushort ShapeID;
			ushort AreaTriggerActionSetID;
			byte PhaseUseFlags;
			byte ShapeType;
			byte Flag;
			int ID;
		}
		public class _Areatriggeractionset
		{
			int ID;
			ushort Field1;
		}
		public class _Areatriggerbox
		{
			int ID;
			float[] Field1 = new float[3];
		}
		public class _Areatriggercylinder
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
		}
		public class _Areatriggersphere
		{
			int ID;
			float Field1;
		}
		public class _ArmorLocation
		{
			int ID;
			float[] Modifier = new float[5];
		}
		public class _Artifact
		{
			int ID;
			string Name;
			uint BarConnectedColor;
			uint BarDisconnectedColor;
			uint TitleColor;
			ushort ClassUiTextureKitID;
			ushort SpecID;
			byte ArtifactCategoryID;
			byte Flags;
			uint UiModelSceneID;
			uint SpellVisualKitID;
		}
		public class _ArtifactAppearance
		{
			string Name;
			uint SwatchColor;
			float ModelDesaturation;
			float ModelAlpha;
			uint ShapeshiftDisplayID;
			ushort ArtifactAppearanceSetID;
			ushort Unknown;
			byte DisplayIndex;
			byte AppearanceModID;
			byte Flags;
			byte ModifiesShapeshiftFormDisplay;
			int ID;
			uint PlayerConditionID;
			uint ItemAppearanceID;
			uint AltItemAppearanceID;
		}
		public class _ArtifactAppearanceSet
		{
			string Name;
			string Name2;
			ushort UiCameraID;
			ushort AltHandUICameraID;
			byte DisplayIndex;
			byte AttachmentPoint;
			byte Flags;
			int ID;
		}
		public class _ArtifactCategory
		{
			int ID;
			ushort ArtifactKnowledgeCurrencyID;
			ushort ArtifactKnowledgeMultiplierCurveID;
		}
		public class _ArtifactPower
		{
			float[] Pos = new float[2];
			byte ArtifactID;
			byte Flags;
			byte MaxRank;
			byte ArtifactTier;
			int ID;
			int RelicType;
		}
		public class _ArtifactPowerLink
		{
			int ID;
			ushort FromArtifactPowerID;
			ushort ToArtifactPowerID;
		}
		public class _ArtifactPowerPicker
		{
			int ID;
			uint PlayerConditionID;
		}
		public class _ArtifactPowerRank
		{
			int ID;
			uint SpellID;
			float Value;
			ushort Unknown;
			byte Rank;
		}
		public class _ArtifactQuestXP
		{
			int ID;
			uint[] Exp = new uint[10];
		}
		public class _Artifacttier
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Artifactunlock
		{
			int ID;
			ushort Field1;
			byte Field2;
			int Field3;
			ushort Field4;
		}
		public class _AuctionHouse
		{
			int ID;
			string Name;
			ushort FactionID;
			byte DepositRate;
			byte ConsignmentRate;
		}
		public class _BankBagSlotPrices
		{
			int ID;
			uint Cost;
		}
		public class _BannedAddons
		{
			int ID;
			string Name;
			string Version;
			byte Flags;
		}
		public class _BarberShopStyle
		{
			string DisplayName;
			string Description;
			float CostModifier;
			byte Type;
			byte Race;
			byte Sex;
			byte Data;
			int ID;
		}
		public class _BattlemasterList
		{
			int ID;
			string Name;
			string GameType;
			string ShortDescription;
			string LongDescription;
			uint IconFileDataID;
			ushort[] MapID = new ushort[16];
			ushort HolidayWorldState;
			ushort PlayerConditionID;
			byte InstanceType;
			byte GroupsAllowed;
			byte MaxGroupSize;
			byte MinLevel;
			byte MaxLevel;
			byte RatedPlayers;
			byte MinPlayers;
			byte MaxPlayers;
			byte Flags;
		}
		public class _Battlepetability
		{
			int ID;
			string Field1;
			string Field2;
			int Field3;
			ushort Field4;
			byte Field5;
			byte Field6;
			int Field7;
		}
		public class _Battlepetabilityeffect
		{
			ushort Field0;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			ushort[] Field4 = new ushort[6];
			byte Field5;
			int ID;
		}
		public class _Battlepetabilitystate
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _Battlepetabilityturn
		{
			ushort Field0;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			int ID;
		}
		public class _BattlePetBreedQuality
		{
			int ID;
			float Modifier;
			byte Quality;
		}
		public class _BattlePetBreedState
		{
			int ID;
			short Value;
			byte State;
		}
		public class _Battlepetdisplayoverride
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			byte Field4;
		}
		public class _Battlepeteffectproperties
		{
			int ID;
			string[] Field1 = new string[6];
			ushort Field2;
			byte[] Field3 = new byte[6];
		}
		public class _BattlePetSpecies
		{
			string SourceText;
			string Description;
			uint CreatureID;
			uint IconFileID;
			uint SummonSpellID;
			ushort Flags;
			byte PetType;
			byte Source;
			int ID;
			uint CardModelSceneID;
			uint LoadoutModelSceneID;
		}
		public class _BattlePetSpeciesState
		{
			int ID;
			int Value;
			byte State;
		}
		public class _Battlepetspeciesxability
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
		}
		public class _Battlepetstate
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Battlepetvisual
		{
			int ID;
			string Field1;
			int Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
		}
		public class _Beameffect
		{
			int ID;
			int Field1;
			float Field2;
			float Field3;
			int Field4;
			ushort Field5;
			ushort Field6;
			ushort Field7;
			ushort Field8;
			ushort Field9;
			ushort FieldA;
		}
		public class _Bonewindmodifiermodel
		{
			int ID;
			int Field1;
			int Field2;
		}
		public class _Bonewindmodifiers
		{
			int ID;
			float[] Field1 = new float[3];
			float Field2;
		}
		public class _Bounty
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
		}
		public class _Bountyset
		{
			int ID;
			ushort Field1;
			int Field2;
		}
		public class _BroadcastText
		{
			int ID;
			string MaleText;
			string FemaleText;
			ushort[] EmoteID = new ushort[3];
			ushort[] EmoteDelay = new ushort[3];
			ushort UnkEmoteID;
			byte Language;
			byte Type;
			uint PlayerConditionID;
			uint[] SoundID = new uint[2];
		}
		public class _Cameraeffect
		{
			int ID;
			byte Field1;
		}
		public class _Cameraeffectentry
		{
			int ID;
			float Field01;
			float Field02;
			float Field03;
			float Field04;
			float Field05;
			float Field06;
			float Field07;
			float Field08;
			ushort Field09;
			byte Field0A;
			byte Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			byte Field0F;
		}
		public class _Cameramode
		{
			int ID;
			float[] Field1 = new float[3];
			float[] Field2 = new float[3];
			float Field3;
			float Field4;
			float Field5;
			ushort Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			byte FieldB;
		}
		public class _Castableraidbuffs
		{
			int ID;
			ushort Field1;
		}
		public class _Celestialbody
		{
			int Field0;
			int Field1;
			int[] Field2 = new int[2];
			int Field3;
			int Field4;
			int[] Field5 = new int[2];
			float[] Field6 = new float[2];
			float[] Field7 = new float[2];
			float Field8;
			float[] Field9 = new float[2];
			float FieldA;
			float[] FieldB = new float[3];
			float FieldC;
			ushort FieldD;
			int ID;
		}
		public class _Cfg_categories
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Cfg_configs
		{
			int ID;
			float Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _Cfg_regions
		{
			int ID;
			string Field1;
			int Field2;
			float Field3;
			ushort Field4;
			byte Field5;
		}
		public class _Characterfaceboneset
		{
			int ID;
			int Field1;
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _CharacterFacialHairStyles
		{
			int ID;
			uint[] Geoset = new uint[5];
			byte RaceID;
			byte SexID;
			byte VariationID;
		}
		public class _Characterloadout
		{
			int ID;
			ulong Field1;
			byte Field2;
			byte Field3;
		}
		public class _Characterloadoutitem
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _Characterserviceinfo
		{
			int ID;
			string Field1;
			string Field2;
			string Field3;
			string Field4;
			int Field5;
			int Field6;
			string Field7;
			int Field8;
			int Field9;
			int FieldA;
			int FieldB;
		}
		public class _Charbaseinfo
		{
			int ID;
			byte Field1;
			byte Field2;
		}
		public class _CharBaseSection
		{
			int ID;
			byte Variation;
			byte ResolutionVariation;
			byte Resolution;
		}
		public class _Charcomponenttexturelayouts
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _Charcomponenttexturesections
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
		}
		public class _Charhairgeosets
		{
			int ID;
			int RootGeosetID;
			byte Race;
			byte Sex;
			byte CharSection_ID;
			byte Flag;
			byte Expansion;
			byte Field7;
			byte Field8;
			byte Field9;
			int FieldA;
		}
		public class _CharSections
		{
			int ID;
			uint[] TextureFileDataID = new uint[3];
			ushort Flags;
			byte RaceID;
			byte SexID;
			byte BaseSection;
			byte VariationIndex;
			byte ColorIndex;
		}
		public class _Charshipment
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			int Field4;
			int Field5;
			ushort Field6;
			ushort Field7;
			byte Field8;
			byte Field9;
		}
		public class _Charshipmentcontainer
		{
			int ID;
			string Field01;
			string Field02;
			int Field03;
			ushort Field04;
			ushort Field05;
			ushort Field06;
			ushort Field07;
			ushort Field08;
			ushort Field09;
			byte Field0A;
			byte Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
		}
		public class _CharStartOutfit
		{
			int ID;
			int[] ItemID = new int[24];
			uint PetDisplayID;
			byte ClassID;
			byte GenderID;
			byte OutfitID;
			byte PetFamilyID;
		}
		public class _CharTitles
		{
			int ID;
			string NameMale;
			string NameFemale;
			ushort MaskID;
			byte Flags;
		}
		public class _ChatChannels
		{
			int ID;
			string Name;
			string Shortcut;
			uint Flags;
			byte FactionGroup;
		}
		public class _Chatprofanity
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _ChrClasses
		{
			string PetNameToken;
			string Name;
			string NameFemale;
			string NameMale;
			string Filename;
			uint CreateScreenFileDataID;
			uint SelectScreenFileDataID;
			uint IconFileDataID;
			uint LowResScreenFileDataID;
			uint StartingLevel;
			ushort Flags;
			ushort CinematicSequenceID;
			ushort DefaultSpec;
			byte PowerType;
			byte SpellClassSet;
			byte AttackPowerPerStrength;
			byte AttackPowerPerAgility;
			byte RangedAttackPowerPerAgility;
			byte Unk1;
			int ID;
		}
		public class _ChrClassesXPowerTypes
		{
			int ID;
			byte PowerType;
		}
		public class _Chrclassracesex
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			int Field5;
			byte Field6;
		}
		public class _Chrclasstitle
		{
			int ID;
			string Field1;
			int Field2;
			byte Field3;
		}
		public class _Chrclassuidisplay
		{
			int ID;
			byte Field1;
			ushort Field2;
			int Field3;
		}
		public class _Chrclassvillain
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
		}
		public class _Chrcustomization
		{
			int ID;
			string Field1;
			int Field2;
			byte Field3;
			byte Field4;
			int[] Field5 = new int[3];
		}
		public class _ChrRaces
		{
			string ClientPrefix;
			string ClientFileString;
			string Name;
			string NameFemale;
			string LowercaseName;
			string LowercaseNameFemale;
			uint Flags;
			uint MaleDisplayID;
			uint FemaleDisplayID;
			uint CreateScreenFileDataID;
			uint SelectScreenFileDataID;
			float[] MaleCustomizeOffset = new float[3];
			float[] FemaleCustomizeOffset = new float[3];
			uint LowResScreenFileDataID;
			uint StartingLevel;
			uint UIDisplayOrder;
			ushort FactionID;
			ushort ResSicknessSpellID;
			ushort SplashSoundID;
			ushort CinematicSequenceID;
			byte BaseLanguage;
			byte CreatureType;
			byte TeamID;
			byte RaceRelated;
			byte UnalteredVisualRaceID;
			byte CharComponentTextureLayoutID;
			byte DefaultClassID;
			byte NeutralRaceID;
			byte ItemAppearanceFrameRaceID;
			byte CharComponentTexLayoutHiResID;
			int ID;
			uint HighResMaleDisplayID;
			uint HighResFemaleDisplayID;
			uint HeritageArmorAchievementID;
			uint MaleCorpseBonesModelFileDataID;
			uint FemaleCorpseBonesModelFileDataID;
			uint[] AlteredFormTransitionSpellVisualID = new uint[3];
			uint[] AlteredFormTransitionSpellVisualKitID = new uint[3];
		}
		public class _ChrSpecialization
		{
			string Name;
			string Name2;
			string Description;
			uint[] MasterySpellID = new uint[2];
			byte ClassID;
			byte OrderIndex;
			byte PetTalentType;
			byte Role;
			byte PrimaryStatOrder;
			int ID;
			uint IconFileDataID;
			uint Flags;
			uint AnimReplacementSetID;
		}
		public class _Chrupgradebucket
		{
			ushort Field0;
			int ID;
		}
		public class _Chrupgradebucketspell
		{
			int ID;
			int Field1;
		}
		public class _Chrupgradetier
		{
			int Field0;
			byte Field1;
			byte Field2;
			int ID;
		}
		public class _CinematicCamera
		{
			int ID;
			uint SoundID;
			float[] Origin = new float[3];
			float OriginFacing;
			uint ModelFileDataID;
		}
		public class _CinematicSequences
		{
			int ID;
			uint SoundID;
			ushort[] Camera = new ushort[8];
		}
		public class _Cloakdampening
		{
			int ID;
			float[] Field1 = new float[5];
			float[] Field2 = new float[5];
			int[] Field3 = new int[2];
			float[] Field4 = new float[2];
			float Field5;
			float Field6;
			float Field7;
		}
		public class _Combatcondition
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			ushort[] Field4 = new ushort[2];
			ushort[] Field5 = new ushort[2];
			byte[] Field6 = new byte[2];
			byte[] Field7 = new byte[2];
			byte Field8;
			byte[] Field9 = new byte[2];
			byte[] FieldA = new byte[2];
			byte FieldB;
		}
		public class _Commentatorstartlocation
		{
			int ID;
			float[] Field1 = new float[3];
			ushort Field2;
		}
		public class _Commentatortrackedcooldown
		{
			int ID;
			byte Field1;
			byte Field2;
			int Field3;
		}
		public class _Componentmodelfiledata
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _Componenttexturefiledata
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
		}
		public class _Configurationwarning
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _Contribution
		{
			string Field0;
			string Field1;
			int ID;
			int Field3;
			int[] Field4 = new int[4];
			byte Field5;
		}
		public class _ConversationLine
		{
			int ID;
			uint BroadcastTextID;
			uint SpellVisualKitID;
			uint Duration;
			ushort NextLineID;
			ushort Unk1;
			byte Yell;
			byte Unk2;
			byte Unk3;
		}
		public class _Creature
		{
			int ID;
			string Field1;
			string Field2;
			string Field3;
			string Field4;
			int[] Field5 = new int[3];
			int Field6;
			int[] Field7 = new int[4];
			float[] Field8 = new float[4];
			byte Field9;
			byte FieldA;
			byte FieldB;
			byte FieldC;
		}
		public class _Creaturedifficulty
		{
			int ID;
			int[] Field1 = new int[7];
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _CreatureDisplayInfo
		{
			int ID;
			float CreatureModelScale;
			ushort ModelID;
			ushort NPCSoundID;
			byte SizeClass;
			byte Flags;
			byte Gender;
			uint ExtendedDisplayInfoID;
			uint PortraitTextureFileDataID;
			byte CreatureModelAlpha;
			ushort SoundID;
			float PlayerModelScale;
			uint PortraitCreatureDisplayInfoID;
			byte BloodID;
			ushort ParticleColorID;
			uint CreatureGeosetData;
			ushort ObjectEffectPackageID;
			ushort AnimReplacementSetID;
			byte UnarmedWeaponSubclass;
			uint StateSpellVisualKitID;
			float InstanceOtherPlayerPetScale;
			uint MountSpellVisualKitID;
			uint[] TextureVariation = new uint[3];
		}
		public class _SDReplacementModel
		{
			int ID;
			int SdFileDataId;
		}
		public class _Creaturedisplayinfocond
		{
			int ID;
			ulong Field1;
			int[] Field2 = new int[2];
			int[] Field3 = new int[2];
			int[] Field4 = new int[2];
			byte Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			ushort Field9;
			byte FieldA;
			byte FieldB;
			byte FieldC;
			ushort FieldD;
			int[] FieldE = new int[3];
		}
		public class _Creaturedisplayinfoevt
		{
			int ID;
			int Field1;
			int Field2;
			byte Field3;
		}
		public class _CreatureDisplayInfoExtra
		{
			int ID;
			uint FileDataID;
			uint HDFileDataID;
			byte DisplayRaceID;
			byte DisplaySexID;
			byte DisplayClassID;
			byte SkinID;
			byte FaceID;
			byte HairStyleID;
			byte HairColorID;
			byte FacialHairID;
			byte[] CustomDisplayOption = new byte[3];
			byte Flags;
		}
		public class _Creaturedisplayinfotrn
		{
			int ID;
			int Field1;
			float Field2;
			int Field3;
			uint Field4;
			int Field5;
		}
		public class _Creaturedispxuicamera
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _CreatureFamily
		{
			int ID;
			string Name;
			float MinScale;
			float MaxScale;
			uint IconFileDataID;
			ushort[] SkillLine = new ushort[2];
			ushort PetFoodMask;
			byte MinScaleLevel;
			byte MaxScaleLevel;
			byte PetTalentType;
		}
		public class _Creatureimmunities
		{
			int ID;
			int[] Field1 = new int[2];
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
			int[] Field8 = new int[8];
			int[] Field9 = new int[16];
		}
		public class _CreatureModelData
		{
			int ID;
			float ModelScale;
			float FootprintTextureLength;
			float FootprintTextureWidth;
			float FootprintParticleScale;
			float CollisionWidth;
			float CollisionHeight;
			float MountHeight;
			float[] GeoBoxMin = new float[3];
			float[] GeoBoxMax = new float[3];
			float WorldEffectScale;
			float AttachedEffectScale;
			float MissileCollisionRadius;
			float MissileCollisionPush;
			float MissileCollisionRaise;
			float OverrideLootEffectScale;
			float OverrideNameScale;
			float OverrideSelectionRadius;
			float TamedPetBaseScale;
			float HoverHeight;
			uint Flags;
			uint FileDataID;
			uint SizeClass;
			uint BloodID;
			uint FootprintTextureID;
			uint FoleyMaterialID;
			uint FootstepEffectID;
			uint DeathThudEffectID;
			uint SoundID;
			uint CreatureGeosetDataID;
		}
		public class _Creaturemovementinfo
		{
			int ID;
			float Field1;
		}
		public class _Creaturesounddata
		{
			int ID;
			float Field01;
			float Field02;
			byte Field03;
			uint Field04;
			int Field05;
			uint Field06;
			uint Field07;
			int Field08;
			uint Field09;
			uint Field0A;
			uint Field0B;
			uint Field0C;
			uint Field0D;
			uint Field0E;
			uint Field0F;
			uint Field10;
			uint Field11;
			uint Field12;
			uint Field13;
			uint Field14;
			uint Field15;
			uint Field16;
			uint Field17;
			uint Field18;
			uint Field19;
			uint Field1A;
			uint Field1B;
			uint Field1C;
			uint Field1D;
			uint Field1E;
			uint Field1F;
			uint Field20;
			uint Field21;
			uint Field22;
			uint Field23;
			uint[] Field24 = new uint[5];
			uint[] Field25 = new uint[4];
		}
		public class _CreatureType
		{
			int ID;
			string Name;
			byte Flags;
		}
		public class _Creaturexcontribution
		{
			int ID;
			int Field1;
		}
		public class _Criteria
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			int Field4;
			ushort Field5;
			ushort Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			byte FieldB;
		}
		public class _CriteriaTree
		{
			int ID;
			string Description;
			uint Amount;
			ushort Flags;
			byte Operator;
			uint CriteriaID;
			uint Parent;
			int OrderIndex;
		}
		public class _Criteriatreexeffect
		{
			int ID;
			ushort Field1;
		}
		public class _Currencycategory
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
		}
		public class _CurrencyTypes
		{
			int ID;
			string Name;
			string Description;
			uint MaxQty;
			uint MaxEarnablePerWeek;
			uint Flags;
			byte CategoryID;
			byte SpellCategory;
			byte Quality;
			uint InventoryIconFileDataID;
			uint SpellWeight;
		}
		public class _Curve
		{
			int ID;
			byte Type;
			byte Unused;
		}
		public class _CurvePoint
		{
			int ID;
			float X;
			float Y;
			ushort CurveID;
			byte Index;
		}
		public class _DBCache
		{
			int ID;
		}
		public class _Deaththudlookups
		{
			int ID;
			byte Field1;
			byte Field2;
			ushort Field3;
			int Field4;
		}
		public class _Decalproperties
		{
			int ID;
			int Field01;
			float Field02;
			float Field03;
			float Field04;
			float Field05;
			float Field06;
			float Field07;
			float Field08;
			float Field09;
			byte Field0A;
			byte Field0B;
			ushort Field0C;
			ushort Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
		}
		public class _DestructibleModelData
		{
			int ID;
			ushort StateDamagedDisplayID;
			ushort StateDestroyedDisplayID;
			ushort StateRebuildingDisplayID;
			ushort StateSmokeDisplayID;
			ushort HealEffectSpeed;
			byte StateDamagedImpactEffectDoodadSet;
			byte StateDamagedAmbientDoodadSet;
			byte StateDamagedNameSet;
			byte StateDestroyedDestructionDoodadSet;
			byte StateDestroyedImpactEffectDoodadSet;
			byte StateDestroyedAmbientDoodadSet;
			byte StateDestroyedNameSet;
			byte StateRebuildingDestructionDoodadSet;
			byte StateRebuildingImpactEffectDoodadSet;
			byte StateRebuildingAmbientDoodadSet;
			byte StateRebuildingNameSet;
			byte StateSmokeInitDoodadSet;
			byte StateSmokeAmbientDoodadSet;
			byte StateSmokeNameSet;
			byte EjectDirection;
			byte DoNotHighlight;
			byte HealEffect;
		}
		public class _Deviceblacklist
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _Devicedefaultsettings
		{
			int ID;
			ushort Field1;
			ushort Field2;
			byte Field3;
		}
		public class _Difficulty
		{
			int ID;
			string Name;
			ushort GroupSizeHealthCurveID;
			ushort GroupSizeDmgCurveID;
			ushort GroupSizeSpellPointsCurveID;
			byte FallbackDifficultyID;
			byte InstanceType;
			byte MinPlayers;
			byte MaxPlayers;
			byte OldEnumValue;
			byte Flags;
			byte ToggleDifficultyID;
			byte ItemBonusTreeModID;
			byte OrderIndex;
		}
		public class _Dissolveeffect
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			byte Field9;
			byte FieldA;
			ushort FieldB;
			int FieldC;
			int FieldD;
			byte FieldE;
		}
		public class _Driverblacklist
		{
			int ID;
			int Field1;
			int Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _DungeonEncounter
		{
			string Name;
			uint CreatureDisplayID;
			ushort MapID;
			byte DifficultyID;
			byte Bit;
			byte Flags;
			int ID;
			int OrderIndex;
			uint TextureFileDataID;
		}
		public class _Dungeonmap
		{
			float[] Field0 = new float[2];
			float[] Field1 = new float[2];
			ushort Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			int ID;
		}
		public class _Dungeonmapchunk
		{
			int ID;
			float Field1;
			int Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
		}
		public class _DurabilityCosts
		{
			int ID;
			ushort[] WeaponSubClassCost = new ushort[21];
			ushort[] ArmorSubClassCost = new ushort[8];
		}
		public class _DurabilityQuality
		{
			int ID;
			float QualityMod;
		}
		public class _Edgegloweffect
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			float Field9;
			float FieldA;
			byte FieldB;
			ushort FieldC;
			int FieldD;
		}
		public class _Emotes
		{
			int ID;
			long RaceMask;
			string EmoteSlashCommand;
			uint SpellVisualKitID;
			uint EmoteFlags;
			ushort AnimID;
			byte EmoteSpecProc;
			uint EmoteSpecProcParam;
			uint EmoteSoundID;
			int ClassMask;
		}
		public class _EmotesText
		{
			int ID;
			string Name;
			ushort EmoteID;
		}
		public class _Emotestextdata
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _EmotesTextSound
		{
			int ID;
			byte RaceId;
			byte SexId;
			byte ClassId;
			uint SoundId;
		}
		public class _Environmentaldamage
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _Exhaustion
		{
			string Field0;
			string Field1;
			int Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			int ID;
		}
		public class _Faction
		{
			ulong[] ReputationRaceMask = new ulong[4];
			string Name;
			string Description;
			int ID;
			int[] ReputationBase = new int[4];
			float ParentFactionModIn;
			float ParentFactionModOut;
			uint[] ReputationMax = new uint[4];
			short ReputationIndex;
			ushort[] ReputationClassMask = new ushort[4];
			ushort[] ReputationFlags = new ushort[4];
			ushort ParentFactionID;
			ushort ParagonFactionID;
			byte ParentFactionCapIn;
			byte ParentFactionCapOut;
			byte Expansion;
			byte Flags;
			byte FriendshipRepID;
		}
		public class _Factiongroup
		{
			string Field0;
			string Field1;
			int ID;
			byte Field3;
			int Field4;
			int Field5;
		}
		public class _FactionTemplate
		{
			int ID;
			ushort Faction;
			ushort Flags;
			ushort[] Enemies = new ushort[4];
			ushort[] Friends = new ushort[4];
			byte Mask;
			byte FriendMask;
			byte EnemyMask;
		}
		public class _Footprinttextures
		{
			int ID;
			ushort Field1;
			byte Field2;
			int Field3;
		}
		public class _Footstepterrainlookup
		{
			int ID;
			ushort Field1;
			byte Field2;
			uint Field3;
			uint Field4;
		}
		public class _Friendshiprepreaction
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
		}
		public class _Friendshipreputation
		{
			string Field0;
			int Field1;
			ushort Field2;
			int ID;
		}
		public class _Fullscreeneffect
		{
			int ID;
			float Field01;
			float Field02;
			float Field03;
			float Field04;
			int Field05;
			float Field06;
			float Field07;
			float Field08;
			float Field09;
			float Field0A;
			int Field0B;
			float Field0C;
			float Field0D;
			float Field0E;
			float Field0F;
			float Field10;
			int Field11;
			float Field12;
			float Field13;
			int Field14;
			int Field15;
			float Field16;
			float Field17;
			byte Field18;
			ushort Field19;
			ushort Field1A;
			int Field1B;
		}
		public class _Gameobjectartkit
		{
			int ID;
			int Field1;
			int[] Field2 = new int[3];
		}
		public class _Gameobjectdiffanimmap
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
		}
		public class _GameObjectDisplayInfo
		{
			int ID;
			uint FileDataID;
			float[] GeoBoxMin = new float[3];
			float[] GeoBoxMax = new float[3];
			float OverrideLootEffectScale;
			float OverrideNameScale;
			ushort ObjectEffectPackageID;
		}
		public class _Gameobjectdisplayinfoxsoundkit
		{
			int ID;
			byte Field1;
			uint Field2;
		}
		public class _GameObjects
		{
			string Name;
			float[] Position = new float[3];
			float RotationX;
			float RotationY;
			float RotationZ;
			float RotationW;
			float Size;
			int[] Data = new int[8];
			ushort MapID;
			ushort DisplayID;
			ushort PhaseID;
			ushort PhaseGroupID;
			byte PhaseUseFlags;
			byte Type;
			int ID;
		}
		public class _Gametips
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
		}
		public class _GarrAbility
		{
			string Name;
			string Description;
			uint IconFileDataID;
			ushort Flags;
			ushort OtherFactionGarrAbilityID;
			byte GarrAbilityCategoryID;
			byte FollowerTypeID;
			int ID;
		}
		public class _Garrabilitycategory
		{
			int ID;
			string Field1;
		}
		public class _Garrabilityeffect
		{
			float Field0;
			float Field1;
			float Field2;
			int Field3;
			ushort Field4;
			byte Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			int ID;
		}
		public class _GarrBuilding
		{
			int ID;
			string NameAlliance;
			string NameHorde;
			string Description;
			string Tooltip;
			uint HordeGameObjectID;
			uint AllianceGameObjectID;
			uint IconFileDataID;
			ushort CostCurrencyID;
			ushort HordeTexPrefixKitID;
			ushort AllianceTexPrefixKitID;
			ushort AllianceActivationScenePackageID;
			ushort HordeActivationScenePackageID;
			ushort FollowerRequiredGarrAbilityID;
			ushort FollowerGarrAbilityEffectID;
			short CostMoney;
			byte Unknown;
			byte Type;
			byte Level;
			byte Flags;
			byte MaxShipments;
			byte GarrTypeID;
			int BuildDuration;
			int CostCurrencyAmount;
			int BonusAmount;
		}
		public class _Garrbuildingdoodadset
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _GarrBuildingPlotInst
		{
			float[] LandmarkOffset = new float[2];
			ushort UiTextureAtlasMemberID;
			ushort GarrSiteLevelPlotInstID;
			byte GarrBuildingID;
			int ID;
		}
		public class _GarrClassSpec
		{
			string NameMale;
			string NameFemale;
			string NameGenderless;
			ushort ClassAtlasID;
			ushort GarrFollItemSetID;
			byte Limit;
			byte Flags;
			int ID;
		}
		public class _Garrclassspecplayercond
		{
			int ID;
			string Field1;
			int Field2;
			byte Field3;
			byte Field4;
			int Field5;
			byte Field6;
		}
		public class _Garrencounter
		{
			string Field0;
			int Field1;
			float Field2;
			float Field3;
			int Field4;
			int ID;
			byte Field6;
		}
		public class _Garrencountersetxencounter
		{
			int ID;
			ushort Field1;
		}
		public class _Garrencounterxmechanic
		{
			int ID;
			byte Field1;
			byte Field2;
		}
		public class _Garrfollitemsetmember
		{
			int ID;
			int Field1;
			ushort Field2;
			byte Field3;
		}
		public class _GarrFollower
		{
			string HordeSourceText;
			string AllianceSourceText;
			string Name;
			uint HordeCreatureID;
			uint AllianceCreatureID;
			uint HordePortraitIconID;
			uint AlliancePortraitIconID;
			uint HordeAddedBroadcastTextID;
			uint AllianceAddedBroadcastTextID;
			ushort HordeGarrFollItemSetID;
			ushort AllianceGarrFollItemSetID;
			ushort ItemLevelWeapon;
			ushort ItemLevelArmor;
			ushort HordeListPortraitTextureKitID;
			ushort AllianceListPortraitTextureKitID;
			byte FollowerTypeID;
			byte HordeUiAnimRaceInfoID;
			byte AllianceUiAnimRaceInfoID;
			byte Quality;
			byte HordeGarrClassSpecID;
			byte AllianceGarrClassSpecID;
			byte Level;
			byte Unknown1;
			byte Flags;
			byte Unknown2;
			byte Unknown3;
			byte GarrTypeID;
			byte MaxDurability;
			byte Class;
			byte HordeFlavorTextGarrStringID;
			byte AllianceFlavorTextGarrStringID;
			int ID;
		}
		public class _Garrfollowerlevelxp
		{
			int ID;
			ushort Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _Garrfollowerquality
		{
			int ID;
			int Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _Garrfollowertype
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _Garrfolloweruicreature
		{
			int ID;
			int Field1;
			float Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _GarrFollowerXAbility
		{
			int ID;
			ushort GarrAbilityID;
			byte FactionIndex;
		}
		public class _Garrfollsupportspell
		{
			int ID;
			int Field1;
			int Field2;
			byte Field3;
		}
		public class _Garritemlevelupgradedata
		{
			int ID;
			byte Field1;
			int Field2;
			ushort Field3;
			int Field4;
		}
		public class _Garrmechanic
		{
			int ID;
			float Field1;
			byte Field2;
			ushort Field3;
		}
		public class _Garrmechanicsetxmechanic
		{
			byte Field0;
			int ID;
		}
		public class _Garrmechanictype
		{
			string Field0;
			string Field1;
			int Field2;
			byte Field3;
			int ID;
		}
		public class _Garrmission
		{
			string Field00;
			string Field01;
			string Field02;
			int Field03;
			int Field04;
			float[] Field05 = new float[2];
			float[] Field06 = new float[2];
			ushort Field07;
			ushort Field08;
			ushort Field09;
			byte Field0A;
			byte Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
			byte Field11;
			byte Field12;
			int ID;
			uint Field14;
			uint Field15;
			uint Field16;
			uint Field17;
			uint Field18;
			uint Field19;
			uint Field1A;
			uint Field1B;
		}
		public class _Garrmissiontexture
		{
			int ID;
			float[] Field1 = new float[2];
			ushort Field2;
		}
		public class _Garrmissiontype
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Garrmissionxencounter
		{
			byte Field0;
			int ID;
			ushort Field2;
			byte Field3;
		}
		public class _Garrmissionxfollower
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _Garrmssnbonusability
		{
			int ID;
			float Field1;
			int Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
		}
		public class _GarrPlot
		{
			int ID;
			string Name;
			uint AllianceConstructionGameObjectID;
			uint HordeConstructionGameObjectID;
			byte GarrPlotUICategoryID;
			byte PlotType;
			byte Flags;
			uint MinCount;
			uint MaxCount;
		}
		public class _GarrPlotBuilding
		{
			int ID;
			byte GarrPlotID;
			byte GarrBuildingID;
		}
		public class _GarrPlotInstance
		{
			int ID;
			string Name;
			byte GarrPlotID;
		}
		public class _Garrplotuicategory
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _GarrSiteLevel
		{
			int ID;
			float[] TownHall = new float[2];
			ushort MapID;
			ushort SiteID;
			ushort MovieID;
			ushort UpgradeResourceCost;
			ushort UpgradeMoneyCost;
			byte Level;
			byte UITextureKitID;
			byte Level2;
		}
		public class _GarrSiteLevelPlotInst
		{
			int ID;
			float[] Landmark = new float[2];
			ushort GarrSiteLevelID;
			byte GarrPlotInstanceID;
			byte Unknown;
		}
		public class _Garrspecialization
		{
			int ID;
			string Field1;
			string Field2;
			int Field3;
			float[] Field4 = new float[2];
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _Garrstring
		{
			int ID;
			string Field1;
		}
		public class _Garrtalent
		{
			string Field00;
			string Field01;
			int Field02;
			int Field03;
			byte Field04;
			byte Field05;
			byte Field06;
			int ID;
			int Field08;
			int Field09;
			int Field0A;
			int Field0B;
			int Field0C;
			int Field0D;
			int Field0E;
			int Field0F;
			int Field10;
			int Field11;
			int Field12;
			int Field13;
		}
		public class _Garrtalenttree
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Garrtype
		{
			int ID;
			byte Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
			int[] Field5 = new int[2];
		}
		public class _Garruianimclassinfo
		{
			int ID;
			float Field1;
			byte Field2;
			byte Field3;
			int Field4;
			int Field5;
			int Field6;
		}
		public class _Garruianimraceinfo
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			float Field9;
			float FieldA;
			float FieldB;
			float FieldC;
			byte FieldD;
		}
		public class _GemProperties
		{
			int ID;
			uint Type;
			ushort EnchantID;
			ushort MinItemLevel;
		}
		public class _Globalstrings
		{
			int ID;
			string Field1;
			string Field2;
			byte Field3;
		}
		public class _GlyphBindableSpell
		{
			int ID;
			uint SpellID;
		}
		public class _Glyphexclusivecategory
		{
			int ID;
			string Field1;
		}
		public class _GlyphProperties
		{
			int ID;
			uint SpellID;
			ushort SpellIconID;
			byte Type;
			byte GlyphExclusiveCategoryID;
		}
		public class _GlyphRequiredSpec
		{
			int ID;
			ushort ChrSpecializationID;
		}
		public class _Gmsurveyanswers
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _Gmsurveycurrentsurvey
		{
			int ID;
			byte Field1;
		}
		public class _Gmsurveyquestions
		{
			int ID;
			string Field1;
		}
		public class _Gmsurveysurveys
		{
			int ID;
			byte[] Field1 = new byte[15];
		}
		public class _Groundeffectdoodad
		{
			int ID;
			float Field1;
			float Field2;
			byte Field3;
			int Field4;
		}
		public class _Groundeffecttexture
		{
			int ID;
			ushort[] Field1 = new ushort[4];
			byte[] Field2 = new byte[4];
			byte Field3;
			byte Field4;
		}
		public class _Groupfinderactivity
		{
			int ID;
			string Field1;
			string Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			byte FieldB;
			byte FieldC;
			byte FieldD;
			byte FieldE;
		}
		public class _Groupfinderactivitygrp
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _Groupfindercategory
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
		}
		public class _GuildColorBackground
		{
			int ID;
			byte Red;
			byte Green;
			byte Blue;
		}
		public class _GuildColorBorder
		{
			int ID;
			byte Red;
			byte Green;
			byte Blue;
		}
		public class _GuildColorEmblem
		{
			int ID;
			byte Red;
			byte Green;
			byte Blue;
		}
		public class _GuildPerkSpells
		{
			int ID;
			uint SpellID;
		}
		public class _Heirloom
		{
			string SourceText;
			uint ItemID;
			uint[] OldItem = new uint[2];
			uint NextDifficultyItemID;
			uint[] UpgradeItemID = new uint[3];
			ushort[] ItemBonusListID = new ushort[3];
			byte Flags;
			byte Source;
			int ID;
		}
		public class _Helmetanimscaling
		{
			int ID;
			float Field1;
			int Field2;
		}
		public class _Helmetgeosetvisdata
		{
			int ID;
			int[] Field1 = new int[9];
		}
		public class _Highlightcolor
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			byte Field4;
			byte Field5;
		}
		public class _Holidaydescriptions
		{
			int ID;
			string Field1;
		}
		public class _Holidaynames
		{
			int ID;
			string Field1;
		}
		public class _Holidays
		{
			int ID;
			uint[] Date = new uint[16];
			ushort[] Duration = new ushort[10];
			ushort Region;
			byte Looping;
			byte[] CalendarFlags = new byte[10];
			byte Priority;
			byte CalendarFilterType;
			byte Flags;
			uint HolidayNameID;
			uint HolidayDescriptionID;
			int[] TextureFileDataID = new int[3];
		}
		public class _ImportPriceArmor
		{
			int ID;
			float ClothFactor;
			float LeatherFactor;
			float MailFactor;
			float PlateFactor;
		}
		public class _ImportPriceQuality
		{
			int ID;
			float Factor;
		}
		public class _ImportPriceShield
		{
			int ID;
			float Factor;
		}
		public class _ImportPriceWeapon
		{
			int ID;
			float Factor;
		}
		public class _Invasionclientdata
		{
			string Field0;
			float[] Field1 = new float[2];
			int ID;
			ushort Field3;
			int Field4;
			int Field5;
			ushort Field6;
			byte Field7;
			ushort Field8;
		}
		public class _Item
		{
			int ID;
			uint FileDataID;
			byte Class;
			byte SubClass;
			byte SoundOverrideSubclass;
			byte Material;
			byte InventoryType;
			byte Sheath;
			byte GroupSoundsID;
		}
		public class _ItemAppearance
		{
			int ID;
			uint DisplayID;
			uint IconFileDataID;
			uint UIOrder;
			byte ObjectComponentSlot;
		}
		public class _Itemappearancexuicamera
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _ItemArmorQuality
		{
			int ID;
			float[] QualityMod = new float[7];
			ushort ItemLevel;
		}
		public class _ItemArmorShield
		{
			int ID;
			float[] Quality = new float[7];
			ushort ItemLevel;
		}
		public class _ItemArmorTotal
		{
			int ID;
			float[] Value = new float[4];
			ushort ItemLevel;
		}
		public class _ItemBagFamily
		{
			int ID;
			string Name;
		}
		public class _ItemBonus
		{
			int ID;
			int[] Value = new int[3];
			ushort BonusListID;
			byte Type;
			byte Index;
		}
		public class _ItemBonusListLevelDelta
		{
			short Delta;
			int ID;
		}
		public class _ItemBonusTreeNode
		{
			int ID;
			ushort SubTreeID;
			ushort BonusListID;
			ushort ItemLevelSelectorID;
			byte BonusTreeModID;
		}
		public class _ItemChildEquipment
		{
			int ID;
			uint AltItemID;
			byte AltEquipmentSlot;
		}
		public class _ItemClass
		{
			int ID;
			string Name;
			float PriceMod;
			byte OldEnumValue;
			byte Flags;
		}
		public class _Itemcontextpickerentry
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			int Field5;
		}
		public class _ItemCurrencyCost
		{
			int ID;
			uint ItemId;
		}
		public class _ItemDamageAmmo
		{
			int ID;
			float[] DPS = new float[7];
			ushort ItemLevel;
		}
		public class _ItemDamageOneHand
		{
			int ID;
			float[] DPS = new float[7];
			ushort ItemLevel;
		}
		public class _ItemDamageOneHandCaster
		{
			int ID;
			float[] DPS = new float[7];
			ushort ItemLevel;
		}
		public class _ItemDamageTwoHand
		{
			int ID;
			float[] DPS = new float[7];
			ushort ItemLevel;
		}
		public class _ItemDamageTwoHandCaster
		{
			int ID;
			float[] DPS = new float[7];
			ushort ItemLevel;
		}
		public class _ItemDisenchantLoot
		{
			int ID;
			ushort MinItemLevel;
			ushort MaxItemLevel;
			ushort RequiredDisenchantSkill;
			byte ItemSubClass;
			byte ItemQuality;
			byte Expansion;
		}
		public class _Itemdisplayinfo
		{
			int ID;
			int Field01;
			int Field02;
			int ItemVisualID;
			int Field04;
			int Field05;
			int Field06;
			int Field07;
			int Field08;
			int Field09;
			int Field0A;
			int[] Modelfiledata = new int[2];
			int[] Texturefiledata = new int[2];
			int[] Field0D = new int[4];
			int[] Field0E = new int[4];
			int[] GeosetFlag = new int[2];
		}
		public class _Itemdisplayinfomaterialres
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _Itemdisplayxuicamera
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _ItemEffect
		{
			int ID;
			uint SpellID;
			int Cooldown;
			int CategoryCooldown;
			short Charges;
			ushort Category;
			ushort ChrSpecializationID;
			byte OrderIndex;
			byte Trigger;
		}
		public class _ItemExtendedCost
		{
			int ID;
			uint[] RequiredItem = new uint[5];
			uint[] RequiredCurrencyCount = new uint[5];
			ushort[] RequiredItemCount = new ushort[5];
			ushort RequiredPersonalArenaRating;
			ushort[] RequiredCurrency = new ushort[5];
			byte RequiredArenaSlot;
			byte RequiredFactionId;
			byte RequiredFactionStanding;
			byte RequirementFlags;
			byte RequiredAchievement;
		}
		public class _Itemgroupsounds
		{
			int ID;
			int[] Field1 = new int[4];
		}
		public class _ItemLevelSelector
		{
			int ID;
			ushort ItemLevel;
			ushort ItemLevelSelectorQualitySetID;
		}
		public class _ItemLevelSelectorQuality
		{
			int ID;
			uint ItemBonusListID;
			byte Quality;
		}
		public class _ItemLevelSelectorQualitySet
		{
			int ID;
			ushort ItemLevelMin;
			ushort ItemLevelMax;
		}
		public class _ItemLimitCategory
		{
			int ID;
			string Name;
			byte Quantity;
			byte Flags;
		}
		public class _Itemlimitcategorycondition
		{
			int ID;
			byte Field1;
			ushort Field2;
		}
		public class _ItemModifiedAppearance
		{
			uint ItemID;
			int ID;
			byte AppearanceModID;
			ushort AppearanceID;
			byte Index;
			byte SourceType;
		}
		public class _Itemmodifiedappearanceextra
		{
			int ID;
			int Field1;
			int Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Itemnamedescription
		{
			int ID;
			string Field1;
			int Field2;
		}
		public class _Itempetfood
		{
			int ID;
			string Field1;
		}
		public class _ItemPriceBase
		{
			int ID;
			float ArmorFactor;
			float WeaponFactor;
			ushort ItemLevel;
		}
		public class _ItemRandomProperties
		{
			int ID;
			string Name;
			ushort[] Enchantment = new ushort[5];
		}
		public class _ItemRandomSuffix
		{
			int ID;
			string Name;
			ushort[] Enchantment = new ushort[5];
			ushort[] AllocationPct = new ushort[5];
		}
		public class _Itemrangeddisplayinfo
		{
			int ID;
			int Field1;
			byte Field2;
			ushort Field3;
			int Field4;
		}
		public class _ItemSearchName
		{
			ulong AllowableRace;
			string Name;
			int ID;
			uint[] Flags = new uint[3];
			ushort ItemLevel;
			byte Quality;
			byte RequiredExpansion;
			byte RequiredLevel;
			ushort RequiredReputationFaction;
			byte RequiredReputationRank;
			int AllowableClass;
			ushort RequiredSkill;
			ushort RequiredSkillRank;
			uint RequiredSpell;
		}
		public class _ItemSet
		{
			int ID;
			string Name;
			uint[] ItemID = new uint[17];
			ushort RequiredSkillRank;
			uint RequiredSkill;
			uint Flags;
		}
		public class _ItemSetSpell
		{
			int ID;
			uint SpellID;
			ushort ChrSpecID;
			byte Threshold;
		}
		public class _ItemSparse
		{
			int ID;
			long AllowableRace;
			string Name;
			string Name2;
			string Name3;
			string Name4;
			string Description;
			uint[] Flags = new uint[4];
			float Unk1;
			float Unk2;
			uint BuyCount;
			uint BuyPrice;
			uint SellPrice;
			uint RequiredSpell;
			uint MaxCount;
			uint Stackable;
			int[] ItemStatAllocation = new int[10];
			float[] ItemStatSocketCostMultiplier = new float[10];
			float RangedModRange;
			uint BagFamily;
			float ArmorDamageModifier;
			uint Duration;
			float StatScalingFactor;
			short AllowableClass;
			ushort ItemLevel;
			ushort RequiredSkill;
			ushort RequiredSkillRank;
			ushort RequiredReputationFaction;
			short[] ItemStatValue = new short[10];
			ushort ScalingStatDistribution;
			ushort Delay;
			ushort PageText;
			ushort StartQuest;
			ushort LockID;
			ushort RandomProperty;
			ushort RandomSuffix;
			ushort ItemSet;
			ushort Area;
			ushort Map;
			ushort TotemCategory;
			ushort SocketBonus;
			ushort GemProperties;
			ushort ItemLimitCategory;
			ushort HolidayID;
			ushort RequiredTransmogHolidayID;
			ushort ItemNameDescriptionID;
			byte Quality;
			byte InventoryType;
			byte RequiredLevel;
			byte RequiredHonorRank;
			byte RequiredCityRank;
			byte RequiredReputationRank;
			byte ContainerSlots;
			byte[] ItemStatType = new byte[10];
			byte DamageType;
			byte Bonding;
			byte LanguageID;
			byte PageMaterial;
			byte Material;
			byte Sheath;
			byte[] SocketColor = new byte[3];
			byte CurrencySubstitutionID;
			byte CurrencySubstitutionCount;
			byte ArtifactID;
			byte RequiredExpansion;
		}
		public class _ItemSpec
		{
			int ID;
			ushort SpecID;
			byte MinLevel;
			byte MaxLevel;
			byte ItemType;
			byte PrimaryStat;
			byte SecondaryStat;
		}
		public class _ItemSpecOverride
		{
			int ID;
			ushort SpecID;
		}
		public class _Itemsubclass
		{
			int ID;
			string Field1;
			string Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
		}
		public class _Itemsubclassmask
		{
			int ID;
			string Field1;
			int Field2;
			byte Field3;
		}
		public class _ItemUpgrade
		{
			int ID;
			uint CurrencyCost;
			ushort PrevItemUpgradeID;
			ushort CurrencyID;
			byte ItemUpgradePathID;
			byte ItemLevelBonus;
		}
		public class _Itemvisuals
		{
			int ID;
			int[] Field1 = new int[5];
		}
		public class _ItemXBonusTree
		{
			int ID;
			ushort BonusTreeID;
		}
		public class _Journalencounter
		{
			int ID;
			string Field1;
			string Field2;
			float[] Field3 = new float[2];
			ushort Field4;
			ushort Field5;
			ushort Field6;
			ushort Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			int FieldB;
		}
		public class _Journalencountercreature
		{
			string Field0;
			string Field1;
			int Field2;
			int Field3;
			int Field4;
			ushort Field5;
			byte Field6;
			int ID;
		}
		public class _Journalencounteritem
		{
			int Field0;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			int ID;
		}
		public class _Journalencountersection
		{
			int ID;
			string Field1;
			string Field2;
			int Field3;
			int Field4;
			int Field5;
			int Field6;
			ushort Field7;
			ushort Field8;
			ushort Field9;
			ushort FieldA;
			ushort FieldB;
			ushort FieldC;
			byte FieldD;
			byte FieldE;
			byte FieldF;
		}
		public class _Journalencounterxdifficulty
		{
			int ID;
			byte Field1;
		}
		public class _Journalencounterxmaploc
		{
			int ID;
			float[] Field1 = new float[2];
			byte Field2;
			ushort Field3;
			int Field4;
			int Field5;
		}
		public class _Journalinstance
		{
			string Field0;
			string Field1;
			int Field2;
			int Field3;
			int Field4;
			int Field5;
			ushort Field6;
			ushort Field7;
			byte Field8;
			byte Field9;
			int ID;
		}
		public class _Journalitemxdifficulty
		{
			int ID;
			byte Field1;
		}
		public class _Journalsectionxdifficulty
		{
			int ID;
			byte Field1;
		}
		public class _Journaltier
		{
			int ID;
			string Field1;
		}
		public class _Journaltierxinstance
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _Keychain
		{
			int ID;
			byte[] Key = new byte[32];
		}
		public class _Keystoneaffix
		{
			int ID;
			string Field1;
			string Field2;
			int Field3;
		}
		public class _Languages
		{
			string Field0;
			int ID;
		}
		public class _Languagewords
		{
			int ID;
			string Field1;
			byte Field2;
		}
		public class _Lfgdungeonexpansion
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			int Field5;
			byte Field6;
		}
		public class _Lfgdungeongroup
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _LFGDungeons
		{
			int ID;
			string Name;
			string Description;
			uint Flags;
			float MinItemLevel;
			ushort MaxLevel;
			ushort TargetLevelMax;
			ushort MapID;
			ushort RandomID;
			ushort ScenarioID;
			ushort LastBossJournalEncounterID;
			ushort BonusReputationAmount;
			ushort MentorItemLevel;
			ushort PlayerConditionID;
			byte MinLevel;
			byte TargetLevel;
			byte TargetLevelMin;
			byte DifficultyID;
			byte Type;
			byte Faction;
			byte Expansion;
			byte OrderIndex;
			byte GroupID;
			byte CountTank;
			byte CountHealer;
			byte CountDamage;
			byte MinCountTank;
			byte MinCountHealer;
			byte MinCountDamage;
			byte SubType;
			byte MentorCharLevel;
			int TextureFileDataID;
			int RewardIconFileDataID;
			int ProposalTextureFileDataID;
		}
		public class _Lfgdungeonsgroupingmap
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _Lfgrolerequirement
		{
			int ID;
			byte Field1;
			int Field2;
		}
		public class _Light
		{
			int ID;
			float[] Pos = new float[3];
			float FalloffStart;
			float FalloffEnd;
			ushort MapID;
			ushort[] LightParamsID = new ushort[8];
		}
		public class _Lightdata
		{
			int ID;
			int Field01;
			int Field02;
			int Field03;
			int Field04;
			int Field05;
			int Field06;
			int Field07;
			int Field08;
			int Field09;
			int Field0A;
			int Field0B;
			int Field0C;
			int Field0D;
			int Field0E;
			int Field0F;
			int Field10;
			int Field11;
			int Field12;
			float Field13;
			float Field14;
			float Field15;
			float Field16;
			float Field17;
			float Field18;
			float Field19;
			float Field1A;
			float Field1B;
			int Field1C;
			int Field1D;
			int Field1E;
			int Field1F;
			int Field20;
			int Field21;
			ushort Field22;
		}
		public class _Lightparams
		{
			float Field00;
			float Field04;
			float Field08;
			float Field0C;
			float Field10;
			int[] Field14 = new int[3];
			ushort LightSkyboxID;
			byte Field22;
			byte Field23;
			byte Field24;
			int ID;
		}
		public class _Lightskybox
		{
			int ID;
			string SkyboxPathName;
			int Field2;
			int Field3;
			byte Field4;
		}
		public class _Liquidmaterial
		{
			int ID;
			byte Field1;
			byte Field2;
		}
		public class _Liquidobject
		{
			int ID;
			float Field1;
			float Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
		}
		public class _LiquidType
		{
			int ID;
			string Name;
			string[] Texture = new string[6];
			uint SpellID;
			float MaxDarkenDepth;
			float FogDarkenIntensity;
			float AmbDarkenIntensity;
			float DirDarkenIntensity;
			float ParticleScale;
			uint[] Color = new uint[2];
			float[] Float = new float[18];
			uint[] Int = new uint[4];
			ushort Flags;
			ushort LightID;
			byte Type;
			byte ParticleMovement;
			byte ParticleTexSlots;
			byte MaterialID;
			byte[] DepthTexCount = new byte[6];
			uint SoundID;
		}
		public class _Loadingscreens
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
		}
		public class _Loadingscreentaxisplines
		{
			int ID;
			float[] Field1 = new float[10];
			float[] Field2 = new float[10];
			ushort Field3;
			ushort Field4;
			byte Field5;
		}
		public class _Locale
		{
			int ID;
			int Field1;
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _Location
		{
			int ID;
			float[] Field1 = new float[3];
			float[] Field2 = new float[3];
		}
		public class _Lock
		{
			int ID;
			uint[] Index = new uint[8];
			ushort[] Skill = new ushort[8];
			byte[] Type = new byte[8];
			byte[] Action = new byte[8];
		}
		public class _Locktype
		{
			string Field0;
			string Field1;
			string Field2;
			string Field3;
			int ID;
		}
		public class _Lookatcontroller
		{
			int ID;
			float Field01;
			float Field02;
			float Field03;
			float Field04;
			ushort Field05;
			ushort Field06;
			ushort Field07;
			ushort Field08;
			byte Field09;
			byte Field0A;
			byte Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			ushort Field0F;
			int Field10;
			byte Field11;
			int Field12;
		}
		public class _MailTemplate
		{
			int ID;
			string Body;
		}
		public class _Managedworldstate
		{
			int Field0;
			int Field1;
			int Field2;
			int Field3;
			int Field4;
			int Field5;
			int Field6;
			int Field7;
			int Field8;
			int ID;
		}
		public class _Managedworldstatebuff
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
		}
		public class _Managedworldstateinput
		{
			int ID;
			byte Field1;
			int Field2;
			int Field3;
		}
		public class _Manifestinterfaceactionicon
		{
			int ID;
		}
		public class _Manifestinterfacedata
		{
			int ID;
			string Field1;
			string Field2;
		}
		public class _Manifestinterfaceitemicon
		{
			int ID;
		}
		public class _Manifestinterfacetocdata
		{
			int ID;
			string Field1;
		}
		public class _Manifestmp3
		{
			int ID;
		}
		public class _Map
		{
			int ID;
			string Directory;
			string MapName;
			string MapDescription0;
			string MapDescription1;
			string ShortDescription;
			string LongDescription;
			uint[] Flags = new uint[2];
			float MinimapIconScale;
			float[] CorpsePos = new float[2];
			ushort AreaTableID;
			ushort LoadingScreenID;
			ushort CorpseMapID;
			ushort TimeOfDayOverride;
			ushort ParentMapID;
			ushort CosmeticParentMapID;
			ushort WindSettingsID;
			byte InstanceType;
			byte unk5;
			byte ExpansionID;
			byte MaxPlayers;
			byte TimeOffset;
		}
		public class _Mapcelestialbody
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _Mapchallengemode
		{
			string Field0;
			int ID;
			ushort Field2;
			ushort[] Field3 = new ushort[3];
			byte Field4;
		}
		public class _MapDifficulty
		{
			int ID;
			string Message_lang;
			byte DifficultyID;
			byte RaidDurationType;
			byte MaxPlayers;
			byte LockID;
			byte Flags;
			byte ItemBonusTreeModID;
			uint Context;
		}
		public class _Mapdifficultyxcondition
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
		}
		public class _Maploadingscreen
		{
			int ID;
			float[] Field1 = new float[2];
			float[] Field2 = new float[2];
			ushort Field3;
			byte Field4;
		}
		public class _Marketingpromotionsxlocale
		{
			int ID;
			string Field1;
			int Field2;
			int Field3;
			int Field4;
			int Field5;
			byte Field6;
			byte Field7;
		}
		public class _Material
		{
			int ID;
			byte Field1;
			ushort Field2;
			int Field3;
			int Field4;
		}
		public class _Minortalent
		{
			int ID;
			int Field1;
			int Field2;
		}
		public class _Missiletargeting
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float[] Field8 = new float[2];
			float Field9;
			int FieldA;
			int FieldB;
			int[] FieldC = new int[2];
		}
		public class _Modelanimcloakdampening
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _Modelfiledata
		{
			byte Field0;
			int ID;
			ushort Field2;
		}
		public class _Modelribbonquality
		{
			int ID;
			byte Field1;
		}
		public class _ModifierTree
		{
			int ID;
			uint[] Asset = new uint[2];
			uint Parent;
			byte Type;
			byte Unk700;
			byte Operator;
			byte Amount;
		}
		public class _Mount
		{
			string Name;
			string Description;
			string SourceDescription;
			uint SpellId;
			float CameraPivotMultiplier;
			ushort MountTypeId;
			ushort Flags;
			byte Source;
			int ID;
			uint PlayerConditionId;
			int UiModelSceneID;
		}
		public class _MountCapability
		{
			uint RequiredSpell;
			uint SpeedModSpell;
			ushort RequiredRidingSkill;
			ushort RequiredArea;
			short RequiredMap;
			byte Flags;
			int ID;
			uint RequiredAura;
		}
		public class _MountTypeXCapability
		{
			int ID;
			ushort MountTypeID;
			ushort MountCapabilityID;
			byte OrderIndex;
		}
		public class _MountXDisplay
		{
			int ID;
			uint DisplayID;
			uint PlayerConditionID;
		}
		public class _Movie
		{
			int ID;
			uint AudioFileDataID;
			uint SubtitleFileDataID;
			byte Volume;
			byte KeyID;
		}
		public class _Moviefiledata
		{
			int ID;
			ushort Field1;
		}
		public class _Movievariation
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _NameGen
		{
			int ID;
			string Name;
			byte Race;
			byte Sex;
		}
		public class _NamesProfanity
		{
			int ID;
			string Name;
			byte Language;
		}
		public class _NamesReserved
		{
			int ID;
			string Name;
		}
		public class _NamesReservedLocale
		{
			int ID;
			string Name;
			byte LocaleMask;
		}
		public class _Npcmodelitemslotdisplayinfo
		{
			int ID;
			int DisplayID;
			byte Slot;
		}
		public class _Npcsounds
		{
			int ID;
			int[] Field1 = new int[4];
		}
		public class _Objecteffect
		{
			int ID;
			float[] Field1 = new float[3];
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			int Field7;
			int Field8;
		}
		public class _Objecteffectmodifier
		{
			int ID;
			float[] Field1 = new float[4];
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _Objecteffectpackageelem
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Outlineeffect
		{
			int ID;
			float Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			int Field5;
			int Field6;
		}
		public class _OverrideSpellData
		{
			int ID;
			uint[] SpellID = new uint[10];
			uint PlayerActionbarFileDataID;
			byte Flags;
		}
		public class _Pagetextmaterial
		{
			int ID;
			string Field1;
		}
		public class _Paperdollitemframe
		{
			int ID;
			string Field1;
			byte Field2;
			int Field3;
		}
		public class _Paragonreputation
		{
			int ID;
			int Field1;
			int Field2;
			ushort Field3;
		}
		public class _Particlecolor
		{
			int ID;
			int[] Field1 = new int[3];
			int[] Field2 = new int[3];
			int[] Field3 = new int[3];
		}
		public class _Path
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _Pathnode
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Pathnodeproperty
		{
			ushort Field0;
			ushort Field1;
			byte Field2;
			int ID;
			int Field4;
		}
		public class _Pathproperty
		{
			int Field0;
			ushort Field1;
			byte Field2;
			int ID;
		}
		public class _Phase
		{
			int ID;
			ushort Flags;
		}
		public class _Phaseshiftzonesounds
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			ushort FieldA;
			ushort FieldB;
			byte FieldC;
			byte FieldD;
		}
		public class _PhaseXPhaseGroup
		{
			int ID;
			ushort PhaseID;
		}
		public class _PlayerCondition
		{
			long RaceMask;
			string FailureDescription;
			int ID;
			byte Flags;
			ushort MinLevel;
			ushort MaxLevel;
			int ClassMask;
			byte Gender;
			byte NativeGender;
			uint SkillLogic;
			byte LanguageID;
			byte MinLanguage;
			int MaxLanguage;
			ushort MaxFactionID;
			byte MaxReputation;
			uint ReputationLogic;
			byte Unknown1;
			byte MinPVPRank;
			byte MaxPVPRank;
			byte PvpMedal;
			uint PrevQuestLogic;
			uint CurrQuestLogic;
			uint CurrentCompletedQuestLogic;
			uint SpellLogic;
			uint ItemLogic;
			byte ItemFlags;
			uint AuraSpellLogic;
			ushort WorldStateExpressionID;
			byte WeatherID;
			byte PartyStatus;
			byte LifetimeMaxPVPRank;
			uint AchievementLogic;
			uint LfgLogic;
			uint AreaLogic;
			uint CurrencyLogic;
			ushort QuestKillID;
			uint QuestKillLogic;
			byte MinExpansionLevel;
			byte MaxExpansionLevel;
			byte MinExpansionTier;
			byte MaxExpansionTier;
			byte MinGuildLevel;
			byte MaxGuildLevel;
			byte PhaseUseFlags;
			ushort PhaseID;
			uint PhaseGroupID;
			int MinAvgItemLevel;
			int MaxAvgItemLevel;
			ushort MinAvgEquippedItemLevel;
			ushort MaxAvgEquippedItemLevel;
			byte ChrSpecializationIndex;
			byte ChrSpecializationRole;
			byte PowerType;
			byte PowerTypeComp;
			byte PowerTypeValue;
			uint ModifierTreeID;
			int MainHandItemSubclassMask;
			ushort[] SkillID = new ushort[4];
			ushort[] MinSkill = new ushort[4];
			ushort[] MaxSkill = new ushort[4];
			uint[] MinFactionID = new uint[3];
			byte[] MinReputation = new byte[3];
			ushort[] PrevQuestID = new ushort[4];
			ushort[] CurrQuestID = new ushort[4];
			ushort[] CurrentCompletedQuestID = new ushort[4];
			int[] SpellID = new int[4];
			int[] ItemID = new int[4];
			uint[] ItemCount = new uint[4];
			ushort[] Explored = new ushort[2];
			uint[] Time = new uint[2];
			uint[] AuraSpellID = new uint[4];
			byte[] AuraCount = new byte[4];
			ushort[] Achievement = new ushort[4];
			byte[] LfgStatus = new byte[4];
			byte[] LfgCompare = new byte[4];
			uint[] LfgValue = new uint[4];
			ushort[] AreaID = new ushort[4];
			uint[] CurrencyID = new uint[4];
			uint[] CurrencyCount = new uint[4];
			uint[] QuestKillMonster = new uint[6];
			int[] MovementFlags = new int[2];
		}
		public class _Positioner
		{
			int ID;
			float Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _Positionerstate
		{
			int ID;
			float Field1;
			byte Field2;
			ushort Field3;
			byte Field4;
			ushort Field5;
			ushort Field6;
			ushort Field7;
			int Field8;
		}
		public class _Positionerstateentry
		{
			int ID;
			float Field1;
			float Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			ushort Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			ushort FieldB;
		}
		public class _PowerDisplay
		{
			int ID;
			string GlobalStringBaseTag;
			byte PowerType;
			byte Red;
			byte Green;
			byte Blue;
		}
		public class _PowerType
		{
			int ID;
			string PowerTypeToken;
			string PowerCostToken;
			float RegenerationPeace;
			float RegenerationCombat;
			short MaxPower;
			ushort RegenerationDelay;
			ushort Flags;
			byte PowerTypeEnum;
			byte RegenerationMin;
			byte RegenerationCenter;
			byte RegenerationMax;
			byte UIModifier;
		}
		public class _PrestigeLevelInfo
		{
			int ID;
			string PrestigeText;
			uint IconID;
			byte PrestigeLevel;
			byte Flags;
		}
		public class _Pvpbrackettypes
		{
			int ID;
			byte Field1;
			int[] Field2 = new int[4];
		}
		public class _PVPDifficulty
		{
			int ID;
			byte BracketID;
			byte MinLevel;
			byte MaxLevel;
		}
		public class _Pvpitem
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _PvpReward
		{
			int ID;
			uint HonorLevel;
			uint Prestige;
			uint RewardPackID;
		}
		public class _Pvpscalingeffect
		{
			int ID;
			float Field1;
			byte Field2;
			ushort Field3;
		}
		public class _Pvpscalingeffecttype
		{
			int ID;
			string Field1;
		}
		public class _Pvptalent
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			int Field4;
			int Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			ushort Field9;
			byte FieldA;
		}
		public class _Pvptalentunlock
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
		}
		public class _QuestFactionReward
		{
			int ID;
			short[] QuestRewFactionValue = new short[10];
		}
		public class _Questfeedbackeffect
		{
			int ID;
			int Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
		}
		public class _Questinfo
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _Questline
		{
			int ID;
			string Field1;
		}
		public class _Questlinexquest
		{
			int ID;
			ushort Field1;
			ushort Field2;
			byte Field3;
		}
		public class _QuestMoneyReward
		{
			int ID;
			uint[] Money = new uint[10];
		}
		public class _Questobjective
		{
			int ID;
			string Field1;
			int Field2;
			int Field3;
			byte Field4;
			byte Field5;
			byte Field6;
			byte Field7;
		}
		public class _QuestPackageItem
		{
			int ID;
			uint ItemID;
			ushort QuestPackageID;
			byte FilterType;
			uint ItemCount;
		}
		public class _Questpoiblob
		{
			int ID;
			ushort Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			int Field5;
			ushort Field6;
			byte Field7;
		}
		public class _Questpoipoint
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _QuestSort
		{
			int ID;
			string SortName;
			byte SortOrder;
		}
		public class _QuestV2
		{
			int ID;
			ushort UniqueBitFlag;
		}
		public class _Questv2clitask
		{
			ulong Field00;
			string Field01;
			string Field02;
			int Field03;
			ushort Field04;
			ushort Field05;
			ushort Field06;
			ushort[] Field07 = new ushort[3];
			ushort Field08;
			ushort Field09;
			byte Field0A;
			byte Field0B;
			byte Field0C;
			byte Field0D;
			byte Field0E;
			byte Field0F;
			byte Field10;
			byte Field11;
			byte Field12;
			byte Field13;
			int ID;
			int Field15;
			int Field16;
			int Field17;
		}
		public class _Questxgroupactivity
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _QuestXP
		{
			int ID;
			ushort[] Exp = new ushort[10];
		}
		public class _RandPropPoints
		{
			int ID;
			uint[] EpicPropertiesPoints = new uint[5];
			uint[] RarePropertiesPoints = new uint[5];
			uint[] UncommonPropertiesPoints = new uint[5];
		}
		public class _Relicslottierrequirement
		{
			int ID;
			int Field1;
			byte Field2;
			byte Field3;
		}
		public class _Relictalent
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			int Field4;
			byte Field5;
		}
		public class _Researchbranch
		{
			int ID;
			string Field1;
			int Field2;
			ushort Field3;
			byte Field4;
			int Field5;
			int Field6;
		}
		public class _Researchfield
		{
			string Field0;
			byte Field1;
			int ID;
		}
		public class _Researchproject
		{
			string Field0;
			string Field1;
			int Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
			int ID;
			int Field7;
			byte Field8;
		}
		public class _Researchsite
		{
			int ID;
			string Field1;
			int Field2;
			ushort Field3;
			byte Field4;
		}
		public class _Resistances
		{
			int ID;
			string Field1;
			byte Field2;
			ushort Field3;
		}
		public class _RewardPack
		{
			int ID;
			uint Money;
			float ArtifactXPMultiplier;
			byte ArtifactXPDifficulty;
			byte ArtifactCategoryID;
			uint TitleID;
			uint Unused;
		}
		public class _Rewardpackxcurrencytype
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _RewardPackXItem
		{
			int ID;
			uint ItemID;
			uint Amount;
		}
		public class _Ribbonquality
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			byte Field4;
			byte Field5;
		}
		public class _RulesetItemUpgrade
		{
			int ID;
			uint ItemID;
			ushort ItemUpgradeID;
		}
		public class _SandboxScaling
		{
			int ID;
			uint MinLevel;
			uint MaxLevel;
			uint Flags;
		}
		public class _ScalingStatDistribution
		{
			int ID;
			ushort ItemLevelCurveID;
			uint MinLevel;
			uint MaxLevel;
		}
		public class _Scenario
		{
			int ID;
			string Name;
			ushort Data;
			byte Flags;
			byte Type;
		}
		public class _Scenarioevententry
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _ScenarioStep
		{
			int ID;
			string Description;
			string Name;
			ushort ScenarioID;
			ushort PreviousStepID;
			ushort QuestRewardID;
			byte Step;
			byte Flags;
			uint CriteriaTreeID;
			uint BonusRequiredStepID;
		}
		public class _SceneScript
		{
			int ID;
			ushort PrevScriptId;
			ushort NextScriptId;
		}
		public class _SceneScriptGlobalText
		{
			int ID;
			string Name;
			string Script;
		}
		public class _SceneScriptPackage
		{
			int ID;
			string Name;
		}
		public class _Scenescriptpackagemember
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
		}
		public class _SceneScriptText
		{
			int ID;
			string Name;
			string Script;
		}
		public class _Scheduledinterval
		{
			int ID;
			int Field1;
			byte Field2;
			int Field3;
			int Field4;
			byte Field5;
		}
		public class _Scheduledworldstate
		{
			int ID;
			ushort Field1;
			ushort Field2;
			int Field3;
			int Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
			byte Field8;
		}
		public class _Scheduledworldstategroup
		{
			int ID;
			int Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			ushort Field5;
		}
		public class _Scheduledworldstatexuniqcat
		{
			int ID;
			ushort Field1;
		}
		public class _Screeneffect
		{
			int ID;
			string Field1;
			int[] Field2 = new int[4];
			ushort Field3;
			ushort Field4;
			ushort Field5;
			ushort Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			int FieldB;
			ushort FieldC;
		}
		public class _Screenlocation
		{
			int ID;
			string Field1;
		}
		public class _Seamlesssite
		{
			int ID;
			ushort Field1;
		}
		public class _Servermessages
		{
			int ID;
			string Field1;
		}
		public class _Shadowyeffect
		{
			int ID;
			int Field1;
			int Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			int Field9;
			byte FieldA;
			byte FieldB;
			ushort FieldC;
			byte FieldD;
		}
		public class _SkillLine
		{
			int ID;
			string DisplayName;
			string Description;
			string AlternateVerb;
			ushort Flags;
			byte CategoryID;
			byte CanLink;
			uint IconFileDataID;
			uint ParentSkillLineID;
		}
		public class _SkillLineAbility
		{
			ulong RaceMask;
			int ID;
			uint SpellID;
			uint SupercedesSpell;
			ushort SkillLine;
			ushort TrivialSkillLineRankHigh;
			ushort TrivialSkillLineRankLow;
			ushort UniqueBit;
			ushort TradeSkillCategoryID;
			byte NumSkillUps;
			int ClassMask;
			ushort MinSkillLineRank;
			byte AcquireMethod;
			byte Flags;
		}
		public class _SkillRaceClassInfo
		{
			int ID;
			long RaceMask;
			ushort SkillID;
			ushort Flags;
			ushort SkillTierID;
			byte Availability;
			byte MinLevel;
			int ClassMask;
		}
		public class _Soundambience
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			int[] Field4 = new int[2];
		}
		public class _Soundambienceflavor
		{
			int ID;
			int Field1;
			uint Field2;
		}
		public class _Soundbus
		{
			float Field0;
			byte Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			int ID;
		}
		public class _Soundbusoverride
		{
			int ID;
			float Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			ushort Field5;
			ushort Field6;
		}
		public class _Soundemitterpillpoints
		{
			int ID;
			float[] Field1 = new float[3];
			ushort Field2;
		}
		public class _Soundemitters
		{
			string Field0;
			float[] Field1 = new float[3];
			float[] Field2 = new float[3];
			ushort Field3;
			ushort Field4;
			byte Field5;
			byte Field6;
			byte Field7;
			int ID;
			int Field9;
			byte FieldA;
		}
		public class _Soundenvelope
		{
			int ID;
			int Field1;
			int Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
		}
		public class _Soundfilter
		{
			int ID;
			string Field1;
		}
		public class _Soundfilterelem
		{
			int ID;
			float[] Field1 = new float[9];
			byte Field2;
		}
		public class _SoundKit
		{
			int ID;
			float VolumeFloat;
			float MinDistance;
			float DistanceCutoff;
			ushort Flags;
			ushort SoundEntriesAdvancedID;
			byte SoundType;
			byte DialogType;
			byte EAXDef;
			float VolumeVariationPlus;
			float VolumeVariationMinus;
			float PitchVariationPlus;
			float PitchVariationMinus;
			float PitchAdjust;
			ushort BusOverwriteID;
			byte Unk700;
		}
		public class _Soundkitadvanced
		{
			int ID;
			float Field01;
			float Field02;
			float Field03;
			float Field04;
			float Field05;
			int Field06;
			int Field07;
			float Field08;
			byte Field09;
			uint Field0A;
			int Field0B;
			int Field0C;
			int Field0D;
			int Field0E;
			int Field0F;
			int Field10;
			int Field11;
			int Field12;
			int Field13;
			int Field14;
			float Field15;
			float Field16;
			float Field17;
			float Field18;
			float Field19;
			float Field1A;
			float Field1B;
			float Field1C;
			int Field1D;
			int Field1E;
			int Field1F;
			int Field20;
			int Field21;
			int Field22;
			int Field23;
			int Field24;
			int Field25;
			int Field26;
			int Field27;
		}
		public class _Soundkitchild
		{
			int ID;
			uint Field1;
			int Field2;
		}
		public class _Soundkitentry
		{
			int ID;
			uint Field1;
			int Field2;
			byte Field3;
			float Field4;
		}
		public class _Soundkitfallback
		{
			int ID;
			ushort Field1;
			ushort Field2;
		}
		public class _Soundkitname
		{
			int ID;
			string Name;
		}
		public class _Soundoverride
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
		}
		public class _Soundproviderpreferences
		{
			int ID;
			string Field01;
			float Field02;
			float Field03;
			float Field04;
			float Field05;
			float Field06;
			float Field07;
			float Field08;
			float Field09;
			float Field0A;
			float Field0B;
			float Field0C;
			float Field0D;
			float Field0E;
			float Field0F;
			float Field10;
			ushort Field11;
			ushort Field12;
			ushort Field13;
			ushort Field14;
			ushort Field15;
			byte Field16;
			byte Field17;
		}
		public class _Sourceinfo
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
		}
		public class _Spammessages
		{
			int ID;
			string Field1;
		}
		public class _SpecializationSpells
		{
			string Description;
			uint SpellID;
			uint OverridesSpellID;
			ushort SpecID;
			byte OrderIndex;
			int ID;
		}
		public class _Spell
		{
			int ID;
			string Name;
			string NameSubtext;
			string Description;
			string AuraDescription;
		}
		public class _Spellactionbarpref
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _Spellactivationoverlay
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
			float Field4;
			int[] Field5 = new int[4];
			byte Field6;
			byte Field7;
			ushort Field8;
		}
		public class _SpellAuraOptions
		{
			int ID;
			uint ProcCharges;
			uint ProcTypeMask;
			uint ProcCategoryRecovery;
			ushort CumulativeAura;
			ushort SpellProcsPerMinuteID;
			byte DifficultyID;
			byte ProcChance;
		}
		public class _SpellAuraRestrictions
		{
			int ID;
			uint CasterAuraSpell;
			uint TargetAuraSpell;
			uint ExcludeCasterAuraSpell;
			uint ExcludeTargetAuraSpell;
			byte DifficultyID;
			byte CasterAuraState;
			byte TargetAuraState;
			byte ExcludeCasterAuraState;
			byte ExcludeTargetAuraState;
		}
		public class _Spellauravisibility
		{
			byte Field0;
			byte Field1;
			int ID;
		}
		public class _Spellauravisxchrspec
		{
			int ID;
			ushort Field1;
		}
		public class _SpellCastingRequirements
		{
			int ID;
			uint SpellID;
			ushort MinFactionID;
			ushort RequiredAreasID;
			ushort RequiresSpellFocus;
			byte FacingCasterFlags;
			byte MinReputation;
			byte RequiredAuraVision;
		}
		public class _SpellCastTimes
		{
			int ID;
			int CastTime;
			int MinCastTime;
			short CastTimePerLevel;
		}
		public class _SpellCategories
		{
			int ID;
			ushort Category;
			ushort StartRecoveryCategory;
			ushort ChargeCategory;
			byte DifficultyID;
			byte DefenseType;
			byte DispelType;
			byte Mechanic;
			byte PreventionType;
		}
		public class _SpellCategory
		{
			int ID;
			string Name;
			int ChargeRecoveryTime;
			byte Flags;
			byte UsesPerWeek;
			byte MaxCharges;
			uint ChargeCategoryType;
		}
		public class _Spellchaineffects
		{
			int ID;
			float Field01;
			float Field02;
			float Field03;
			int Field04;
			int Field05;
			float Field06;
			float Field07;
			float Field08;
			float Field09;
			float Field0A;
			float Field0B;
			float Field0C;
			float Field0D;
			float Field0E;
			float Field0F;
			float Field10;
			float Field11;
			float Field12;
			float Field13;
			float Field14;
			float Field15;
			float Field16;
			float Field17;
			float Field18;
			float Field19;
			float Field1A;
			float Field1B;
			float Field1C;
			float Field1D;
			float Field1E;
			float Field1F;
			float Field20;
			float Field21;
			float Field22;
			float Field23;
			float[] Field24 = new float[3];
			float[] Field25 = new float[3];
			float[] Field26 = new float[3];
			float[] Field27 = new float[3];
			int Field28;
			float Field29;
			float Field2A;
			float Field2B;
			float Field2C;
			ushort Field2D;
			ushort Field2E;
			ushort[] Field2F = new ushort[11];
			ushort Field30;
			byte Field31;
			byte Field32;
			byte Field33;
			byte Field34;
			byte Field35;
			byte Field36;
			byte Field37;
			byte Field38;
			byte Field39;
			byte Field3A;
			int Field3B;
			int[] Field3C = new int[3];
		}
		public class _SpellClassOptions
		{
			int ID;
			uint SpellID;
			int[] SpellClassMask = new int[4];
			byte SpellClassSet;
			uint ModalNextSpell;
		}
		public class _SpellCooldowns
		{
			int ID;
			uint CategoryRecoveryTime;
			uint RecoveryTime;
			uint StartRecoveryTime;
			byte DifficultyID;
		}
		public class _Spelldescriptionvariables
		{
			int ID;
			string Field1;
		}
		public class _Spelldispeltype
		{
			int ID;
			string Field1;
			string Field2;
			byte Field3;
			byte Field4;
		}
		public class _SpellDuration
		{
			int ID;
			int Duration;
			int MaxDuration;
			int DurationPerLevel;
		}
		public class _SpellEffect
		{
			int ID;
			uint Effect;
			int EffectBasePoints;
			uint EffectIndex;
			uint EffectAura;
			uint DifficultyID;
			float EffectAmplitude;
			uint EffectAuraPeriod;
			float EffectBonusCoefficient;
			float EffectChainAmplitude;
			uint EffectChainTargets;
			int EffectDieSides;
			uint EffectItemType;
			uint EffectMechanic;
			float EffectPointsPerResource;
			float EffectRealPointsPerLevel;
			uint EffectTriggerSpell;
			float EffectPosFacing;
			uint EffectAttributes;
			float BonusCoefficientFromAP;
			float PvPMultiplier;
			float Coefficient;
			float Variance;
			float ResourceCoefficient;
			float GroupSizeCoefficient;
			int[] EffectSpellClassMask = new int[4];
			int EffectMiscValue;
			int EffectMiscValueB;
			uint EffectRadiusIndex;
			uint EffectRadiusMaxIndex;
			uint[] ImplicitTarget = new uint[2];
		}
		public class _Spelleffectemission
		{
			int ID;
			float Field1;
			float Field2;
			ushort Field3;
			byte Field4;
		}
		public class _SpellEquippedItems
		{
			int ID;
			uint SpellID;
			int EquippedItemInventoryTypeMask;
			int EquippedItemSubClassMask;
			byte EquippedItemClass;
		}
		public class _Spellflyout
		{
			int ID;
			ulong Field1;
			string Field2;
			string Field3;
			byte Field4;
			ushort Field5;
			int Field6;
		}
		public class _Spellflyoutitem
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _SpellFocusObject
		{
			int ID;
			string Name;
		}
		public class _SpellInterrupts
		{
			int ID;
			byte DifficultyID;
			ushort InterruptFlags;
			uint[] AuraInterruptFlags = new uint[2];
			uint[] ChannelInterruptFlags = new uint[2];
		}
		public class _SpellItemEnchantment
		{
			int ID;
			string Name;
			uint[] EffectSpellID = new uint[3];
			float[] EffectScalingPoints = new float[3];
			uint TransmogCost;
			uint TextureFileDataID;
			ushort[] EffectPointsMin = new ushort[3];
			ushort ItemVisual;
			ushort Flags;
			ushort RequiredSkillID;
			ushort RequiredSkillRank;
			ushort ItemLevel;
			byte Charges;
			byte[] Effect = new byte[3];
			byte ConditionID;
			byte MinLevel;
			byte MaxLevel;
			byte ScalingClass;
			byte ScalingClassRestricted;
			uint PlayerConditionID;
		}
		public class _SpellItemEnchantmentCondition
		{
			int ID;
			uint[] LTOperand = new uint[5];
			byte[] LTOperandType = new byte[5];
			byte[] Operator = new byte[5];
			byte[] RTOperandType = new byte[5];
			byte[] RTOperand = new byte[5];
			byte[] Logic = new byte[5];
		}
		public class _Spellkeyboundoverride
		{
			int ID;
			string Field1;
			int Field2;
			byte Field3;
		}
		public class _Spelllabel
		{
			int ID;
			int Field1;
		}
		public class _SpellLearnSpell
		{
			int ID;
			uint LearnSpellID;
			uint SpellID;
			uint OverridesSpellID;
		}
		public class _SpellLevels
		{
			int ID;
			ushort BaseLevel;
			ushort MaxLevel;
			ushort SpellLevel;
			byte DifficultyID;
			byte MaxUsableLevel;
		}
		public class _Spellmechanic
		{
			int ID;
			string Field1;
		}
		public class _SpellMisc
		{
			int ID;
			ushort CastingTimeIndex;
			ushort DurationIndex;
			ushort RangeIndex;
			byte SchoolMask;
			uint IconFileDataID;
			float Speed;
			uint ActiveIconFileDataID;
			float MultistrikeSpeedMod;
			byte DifficultyID;
			uint Attributes;
			uint AttributesEx;
			uint AttributesExB;
			uint AttributesExC;
			uint AttributesExD;
			uint AttributesExE;
			uint AttributesExF;
			uint AttributesExG;
			uint AttributesExH;
			uint AttributesExI;
			uint AttributesExJ;
			uint AttributesExK;
			uint AttributesExL;
			uint AttributesExM;
		}
		public class _Spellmissile
		{
			int ID;
			int Field01;
			float Field02;
			float Field03;
			float Field04;
			float Field05;
			float Field06;
			float Field07;
			int Field08;
			float Field09;
			int Field0A;
			float Field0B;
			float Field0C;
			float Field0D;
			float Field0E;
			byte Field0F;
		}
		public class _Spellmissilemotion
		{
			int ID;
			string Field1;
			string Field2;
			byte Field3;
			byte Field4;
		}
		public class _SpellPower
		{
			int ManaCost;
			float ManaCostPercentage;
			float ManaCostPercentagePerSecond;
			uint RequiredAura;
			float HealthCostPercentage;
			byte PowerIndex;
			byte PowerType;
			int ID;
			int ManaCostPerLevel;
			int ManaCostPerSecond;
			int ManaCostAdditional;
			uint PowerDisplayID;
			uint UnitPowerBarID;
		}
		public class _SpellPowerDifficulty
		{
			byte DifficultyID;
			byte PowerIndex;
			int ID;
		}
		public class _Spellproceduraleffect
		{
			int[] Field0 = new int[4];
			byte Field1;
			int ID;
		}
		public class _SpellProcsPerMinute
		{
			int ID;
			float BaseProcRate;
			byte Flags;
		}
		public class _SpellProcsPerMinuteMod
		{
			int ID;
			float Coeff;
			ushort Param;
			byte Type;
		}
		public class _SpellRadius
		{
			int ID;
			float Radius;
			float RadiusPerLevel;
			float RadiusMin;
			float RadiusMax;
		}
		public class _SpellRange
		{
			int ID;
			string DisplayName;
			string DisplayNameShort;
			float MinRangeHostile;
			float MinRangeFriend;
			float MaxRangeHostile;
			float MaxRangeFriend;
			byte Flags;
		}
		public class _SpellReagents
		{
			int ID;
			uint SpellID;
			int[] Reagent = new int[8];
			ushort[] ReagentCount = new ushort[8];
		}
		public class _Spellreagentscurrency
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _SpellScaling
		{
			int ID;
			uint SpellID;
			ushort ScalesFromItemLevel;
			int ScalingClass;
			uint MinScalingLevel;
			uint MaxScalingLevel;
		}
		public class _SpellShapeshift
		{
			int ID;
			uint SpellID;
			uint[] ShapeshiftExclude = new uint[2];
			uint[] ShapeshiftMask = new uint[2];
			byte StanceBarOrder;
		}
		public class _SpellShapeshiftForm
		{
			int ID;
			string Name;
			float WeaponDamageVariance;
			uint Flags;
			ushort CombatRoundTime;
			ushort MountTypeID;
			byte CreatureType;
			byte BonusActionBar;
			uint AttackIconFileDataID;
			uint[] CreatureDisplayID = new uint[4];
			uint[] PresetSpellID = new uint[8];
		}
		public class _Spellspecialuniteffect
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _SpellTargetRestrictions
		{
			int ID;
			float ConeAngle;
			float Width;
			uint Targets;
			ushort TargetCreatureType;
			byte DifficultyID;
			byte MaxAffectedTargets;
			uint MaxTargetLevel;
		}
		public class _SpellTotems
		{
			int ID;
			uint SpellID;
			uint[] Totem = new uint[2];
			ushort[] RequiredTotemCategoryID = new ushort[2];
		}
		public class _Spellvisual
		{
			int ID;
			float[] Field1 = new float[3];
			float[] Field2 = new float[3];
			int Field3;
			ushort Field4;
			byte Field5;
			byte Field6;
			int Field7;
			int Field8;
			int Field9;
			int FieldA;
			int FieldB;
			int FieldC;
			int FieldD;
			int FieldE;
		}
		public class _Spellvisualanim
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Spellvisualcoloreffect
		{
			int ID;
			float Field1;
			int Field2;
			float Field3;
			ushort Field4;
			ushort Field5;
			ushort Field6;
			ushort Field7;
			ushort Field8;
			byte Field9;
			byte FieldA;
			ushort FieldB;
		}
		public class _Spellvisualeffectname
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			int Field7;
			int Field8;
			int Field9;
			byte FieldA;
			int FieldB;
			int FieldC;
			ushort FieldD;
		}
		public class _Spellvisualevent
		{
			int ID;
			int Field1;
			ushort Field2;
			int Field3;
			byte Field4;
			int Field5;
			int Field6;
			byte Field7;
			int Field8;
		}
		public class _Spellvisualkit
		{
			int ID;
			int Field1;
			float Field2;
			byte Field3;
			ushort Field4;
			int Field5;
		}
		public class _Spellvisualkitareamodel
		{
			int ID;
			int Field1;
			float Field2;
			float Field3;
			float Field4;
			ushort Field5;
			byte Field6;
		}
		public class _Spellvisualkiteffect
		{
			int ID;
			int Field1;
			int Field2;
		}
		public class _Spellvisualkitmodelattach
		{
			float[] Field00 = new float[3];
			float[] Field01 = new float[3];
			int ID;
			ushort Field03;
			byte Field04;
			byte Field05;
			ushort Field06;
			float Field07;
			float Field08;
			float Field09;
			float Field0A;
			float Field0B;
			float Field0C;
			float Field0D;
			float Field0E;
			int Field0F;
			int Field10;
			int Field11;
			float Field12;
			int Field13;
			float Field14;
		}
		public class _Spellvisualmissile
		{
			int Field0;
			int Field1;
			int Field2;
			float[] Field3 = new float[3];
			float[] Field4 = new float[3];
			ushort Field5;
			ushort Field6;
			ushort Field7;
			ushort Field8;
			ushort Field9;
			byte FieldA;
			byte FieldB;
			int ID;
			int FieldD;
			int FieldE;
		}
		public class _Spellxdescriptionvariables
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _SpellXSpellVisual
		{
			uint SpellVisualID;
			int ID;
			float Chance;
			ushort CasterPlayerConditionID;
			ushort CasterUnitConditionID;
			ushort PlayerConditionID;
			ushort UnitConditionID;
			uint IconFileDataID;
			uint ActiveIconFileDataID;
			byte Flags;
			byte DifficultyID;
			byte Priority;
		}
		public class _Startup_strings
		{
			int ID;
			string Field1;
			string Field2;
		}
		public class _Startupfiles
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
		}
		public class _Stationery
		{
			int ID;
			byte Field1;
			ushort Field2;
			int[] Field3 = new int[2];
		}
		public class _SummonProperties
		{
			int ID;
			uint Flags;
			uint Category;
			uint Faction;
			int Type;
			int Slot;
		}
		public class _TactKey
		{
			int ID;
			byte[] Key = new byte[16];
		}
		public class _Tactkeylookup
		{
			int ID;
			byte[] Field1 = new byte[8];
		}
		public class _Talent
		{
			int ID;
			string Description;
			uint SpellID;
			uint OverridesSpellID;
			ushort SpecID;
			byte TierID;
			byte ColumnIndex;
			byte Flags;
			byte[] CategoryMask = new byte[2];
			byte ClassID;
		}
		public class _TaxiNodes
		{
			int ID;
			string Name;
			float[] Pos = new float[3];
			uint[] MountCreatureID = new uint[2];
			float[] MapOffset = new float[2];
			float Unk730;
			float[] FlightMapOffset = new float[2];
			ushort MapID;
			ushort ConditionID;
			ushort LearnableIndex;
			byte Flags;
			int UiTextureKitPrefixID;
			uint SpecialAtlasIconPlayerConditionID;
		}
		public class _TaxiPath
		{
			ushort From;
			ushort To;
			int ID;
			uint Cost;
		}
		public class _TaxiPathNode
		{
			float[] Loc = new float[3];
			ushort PathID;
			ushort MapID;
			byte NodeIndex;
			int ID;
			byte Flags;
			uint Delay;
			ushort ArrivalEventID;
			ushort DepartureEventID;
		}
		public class _Terrainmaterial
		{
			int ID;
			byte Field1;
			int Field2;
			int Field3;
		}
		public class _Terraintype
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
			byte Field4;
			byte Field5;
		}
		public class _Terraintypesounds
		{
			int ID;
			string Field1;
		}
		public class _Textureblendset
		{
			int ID;
			int[] Field1 = new int[3];
			float[] Field2 = new float[3];
			float[] Field3 = new float[3];
			float[] Field4 = new float[3];
			float[] Field5 = new float[3];
			float[] Field6 = new float[4];
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
		}
		public class _Texturefiledata
		{
			int ID;
			int Field1;
			byte Field2;
		}
		public class _TotemCategory
		{
			int ID;
			string Name;
			uint CategoryMask;
			byte CategoryType;
		}
		public class _Toy
		{
			string Description;
			uint ItemID;
			byte Flags;
			byte CategoryFilter;
			int ID;
		}
		public class _Tradeskillcategory
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			byte Field5;
		}
		public class _Tradeskillitem
		{
			int ID;
			ushort Field1;
			byte Field2;
		}
		public class _Transformmatrix
		{
			int ID;
			float[] Field1 = new float[3];
			int Field2;
			float Field3;
			float Field4;
			float Field5;
		}
		public class _TransmogHoliday
		{
			int ID;
			int HolidayID;
		}
		public class _TransmogSet
		{
			string Name;
			ushort BaseSetID;
			ushort UIOrder;
			byte ExpansionID;
			int ID;
			int Flags;
			int QuestID;
			int ClassMask;
			int ItemNameDescriptionID;
			uint TransmogSetGroupID;
		}
		public class _TransmogSetGroup
		{
			string Label;
			int ID;
		}
		public class _TransmogSetItem
		{
			int ID;
			uint TransmogSetID;
			uint ItemModifiedAppearanceID;
			int Flags;
		}
		public class _TransportAnimation
		{
			int ID;
			uint TimeIndex;
			float[] Pos = new float[3];
			byte SequenceID;
		}
		public class _Transportphysics
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			float Field9;
			float FieldA;
		}
		public class _TransportRotation
		{
			int ID;
			uint TimeIndex;
			float X;
			float Y;
			float Z;
			float W;
		}
		public class _Trophy
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			int Field4;
		}
		public class _Uicamera
		{
			int ID;
			string Field1;
			float[] Field2 = new float[3];
			float[] Field3 = new float[3];
			float[] Field4 = new float[3];
			ushort Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
		}
		public class _Uicameratype
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Uicamfbacktransmogchrrace
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Uicamfbacktransmogweapon
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _Uiexpansiondisplayinfo
		{
			int ID;
			int Field1;
			int Field2;
			int Field3;
		}
		public class _Uiexpansiondisplayinfoicon
		{
			int ID;
			string Field1;
			int Field2;
			int Field3;
		}
		public class _Uimappoi
		{
			int Field0;
			float[] Field1 = new float[3];
			int Field2;
			int Field3;
			ushort Field4;
			ushort Field5;
			int ID;
		}
		public class _Uimodelscene
		{
			int ID;
			byte Field1;
			byte Field2;
		}
		public class _Uimodelsceneactor
		{
			string Field0;
			float[] Field1 = new float[3];
			float Field2;
			int Field3;
			int Field4;
			float Field5;
			byte Field6;
			int ID;
			byte Field8;
		}
		public class _Uimodelsceneactordisplay
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			int Field4;
			byte Field5;
		}
		public class _Uimodelscenecamera
		{
			string Field0;
			float[] Field1 = new float[3];
			int[] Field2 = new int[3];
			float Field3;
			float Field4;
			int Field5;
			float Field6;
			int Field7;
			int Field8;
			float Field9;
			float FieldA;
			float FieldB;
			byte FieldC;
			byte FieldD;
			int ID;
		}
		public class _Uitextureatlas
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Uitextureatlasmember
		{
			string Field0;
			int ID;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			ushort Field6;
			byte Field7;
		}
		public class _Uitexturekit
		{
			int ID;
			string Field1;
		}
		public class _Unitblood
		{
			int ID;
			ushort Field1;
			int Field2;
			int Field3;
			int Field4;
			uint Field5;
			int Field6;
		}
		public class _Unitbloodlevels
		{
			int ID;
			byte[] Field1 = new byte[3];
		}
		public class _Unitcondition
		{
			int ID;
			int[] Field1 = new int[8];
			byte Field2;
			byte[] Field3 = new byte[8];
			byte[] Field4 = new byte[8];
		}
		public class _UnitPowerBar
		{
			int ID;
			string Name;
			string Cost;
			string OutOfError;
			string ToolTip;
			float RegenerationPeace;
			float RegenerationCombat;
			uint[] FileDataID = new uint[6];
			uint[] Color = new uint[6];
			float StartInset;
			float EndInset;
			ushort StartPower;
			ushort Flags;
			byte CenterPower;
			byte BarType;
			uint MinPower;
			uint MaxPower;
		}
		public class _Vehicle
		{
			int ID;
			uint Flags;
			float TurnSpeed;
			float PitchSpeed;
			float PitchMin;
			float PitchMax;
			float MouseLookOffsetPitch;
			float CameraFadeDistScalarMin;
			float CameraFadeDistScalarMax;
			float CameraPitchOffset;
			float FacingLimitRight;
			float FacingLimitLeft;
			float CameraYawOffset;
			ushort[] SeatID = new ushort[8];
			ushort VehicleUIIndicatorID;
			ushort[] PowerDisplayID = new ushort[3];
			byte FlagsB;
			byte UILocomotionType;
			int MissileTargetingID;
		}
		public class _VehicleSeat
		{
			int ID;
			uint[] Flags = new uint[3];
			float[] AttachmentOffset = new float[3];
			float EnterPreDelay;
			float EnterSpeed;
			float EnterGravity;
			float EnterMinDuration;
			float EnterMaxDuration;
			float EnterMinArcHeight;
			float EnterMaxArcHeight;
			float ExitPreDelay;
			float ExitSpeed;
			float ExitGravity;
			float ExitMinDuration;
			float ExitMaxDuration;
			float ExitMinArcHeight;
			float ExitMaxArcHeight;
			float PassengerYaw;
			float PassengerPitch;
			float PassengerRoll;
			float VehicleEnterAnimDelay;
			float VehicleExitAnimDelay;
			float CameraEnteringDelay;
			float CameraEnteringDuration;
			float CameraExitingDelay;
			float CameraExitingDuration;
			float[] CameraOffset = new float[3];
			float CameraPosChaseRate;
			float CameraFacingChaseRate;
			float CameraEnteringZoom;
			float CameraSeatZoomMin;
			float CameraSeatZoomMax;
			uint UISkinFileDataID;
			short EnterAnimStart;
			short EnterAnimLoop;
			short RideAnimStart;
			short RideAnimLoop;
			short RideUpperAnimStart;
			short RideUpperAnimLoop;
			short ExitAnimStart;
			short ExitAnimLoop;
			short ExitAnimEnd;
			short VehicleEnterAnim;
			short VehicleExitAnim;
			short VehicleRideAnimLoop;
			ushort EnterAnimKitID;
			ushort RideAnimKitID;
			ushort ExitAnimKitID;
			ushort VehicleEnterAnimKitID;
			ushort VehicleRideAnimKitID;
			ushort VehicleExitAnimKitID;
			ushort CameraModeID;
			byte AttachmentID;
			byte PassengerAttachmentID;
			byte VehicleEnterAnimBone;
			byte VehicleExitAnimBone;
			byte VehicleRideAnimLoopBone;
			byte VehicleAbilityDisplay;
			uint EnterUISoundID;
			uint ExitUISoundID;
		}
		public class _Vehicleuiindicator
		{
			int ID;
			int Field1;
		}
		public class _Vehicleuiindseat
		{
			int ID;
			float Field1;
			float Field2;
			byte Field3;
		}
		public class _Vignette
		{
			int ID;
			string Field1;
			float Field2;
			float Field3;
			int Field4;
			byte Field5;
			ushort Field6;
			ushort Field7;
		}
		public class _Virtualattachment
		{
			int ID;
			string Field1;
			ushort Field2;
		}
		public class _Virtualattachmentcustomization
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Vocaluisounds
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			int[] Field4 = new int[2];
		}
		public class _Wbaccesscontrollist
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
			byte Field5;
		}
		public class _Wbcertwhitelist
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
			byte Field4;
		}
		public class _Weaponimpactsounds
		{
			int ID;
			byte Field1;
			byte Field2;
			byte Field3;
			int[] Field4 = new int[11];
			int[] Field5 = new int[11];
			int[] Field6 = new int[11];
			int[] Field7 = new int[11];
		}
		public class _Weaponswingsounds2
		{
			int ID;
			byte Field1;
			byte Field2;
			int Field3;
		}
		public class _Weapontrail
		{
			int ID;
			int Field1;
			float Field2;
			int Field3;
			int Field4;
			int[] Field5 = new int[3];
			float[] Field6 = new float[3];
			float[] Field7 = new float[3];
			float[] Field8 = new float[3];
			float[] Field9 = new float[3];
		}
		public class _Weapontrailmodeldef
		{
			int ID;
			int Field1;
			ushort Field2;
		}
		public class _Weapontrailparam
		{
			int ID;
			float Field1;
			float Field2;
			float Field3;
			float Field4;
			float Field5;
			byte Field6;
			byte Field7;
			byte Field8;
			byte Field9;
		}
		public class _Weather
		{
			int ID;
			float[] Field1 = new float[2];
			float Field2;
			float[] Field3 = new float[3];
			float Field4;
			float Field5;
			float Field6;
			float Field7;
			float Field8;
			ushort Field9;
			byte FieldA;
			byte FieldB;
			byte FieldC;
			ushort FieldD;
			int FieldE;
		}
		public class _Windsettings
		{
			int ID;
			float Field1;
			float[] Field2 = new float[3];
			float Field3;
			float Field4;
			float[] Field5 = new float[3];
			float Field6;
			float[] Field7 = new float[3];
			int Field8;
			int Field9;
			byte FieldA;
		}
		public class _WMOAreaTable
		{
			string AreaName;
			int WMOGroupID;
			ushort AmbienceID;
			ushort ZoneMusic;
			ushort IntroSound;
			ushort AreaTableID;
			ushort UWIntroSound;
			ushort UWAmbience;
			byte NameSet;
			byte SoundProviderPref;
			byte SoundProviderPrefUnderwater;
			byte Flags;
			int ID;
			uint UWZoneMusic;
		}
		public class _Wmominimaptexture
		{
			int ID;
			int Field1;
			ushort Field2;
			byte Field3;
			byte Field4;
		}
		public class _World_pvp_area
		{
			int ID;
			ushort Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			byte Field6;
			byte Field7;
		}
		public class _Worldbosslockout
		{
			int ID;
			string Field1;
			ushort Field2;
		}
		public class _Worldchunksounds
		{
			int ID;
			ushort Field1;
			byte Field2;
			byte Field3;
			byte Field4;
			byte Field5;
			byte Field6;
		}
		public class _WorldEffect
		{
			int ID;
			uint TargetAsset;
			ushort CombatConditionID;
			byte TargetType;
			byte WhenToDisplay;
			uint QuestFeedbackEffectID;
			uint PlayerConditionID;
		}
		public class _Worldelapsedtimer
		{
			int ID;
			string Field1;
			byte Field2;
			byte Field3;
		}
		public class _WorldMapArea
		{
			string AreaName;
			float LocLeft;
			float LocRight;
			float LocTop;
			float LocBottom;
			uint Flags;
			ushort MapID;
			ushort AreaID;
			ushort DisplayMapID;
			short DefaultDungeonFloor;
			ushort ParentWorldMapID;
			byte LevelRangeMin;
			byte LevelRangeMax;
			byte BountySetID;
			byte BountyBoardLocation;
			int ID;
			uint PlayerConditionID;
		}
		public class _Worldmapcontinent
		{
			int ID;
			float[] Field1 = new float[2];
			float Field2;
			float[] Field3 = new float[2];
			float[] Field4 = new float[2];
			ushort Field5;
			ushort Field6;
			byte Field7;
			byte Field8;
			byte Field9;
			byte FieldA;
			byte FieldB;
		}
		public class _WorldMapOverlay
		{
			string TextureName;
			int ID;
			ushort TextureWidth;
			ushort TextureHeight;
			uint MapAreaID;
			uint[] AreaID = new uint[4];
			int OffsetX;
			int OffsetY;
			int HitRectTop;
			int HitRectLeft;
			int HitRectBottom;
			int HitRectRight;
			uint PlayerConditionID;
			uint Flags;
		}
		public class _WorldMapTransforms
		{
			int ID;
			float[] RegionMin = new float[3];
			float[] RegionMax = new float[3];
			float[] RegionOffset = new float[2];
			float RegionScale;
			ushort MapID;
			ushort AreaID;
			ushort NewMapID;
			ushort NewDungeonMapID;
			ushort NewAreaID;
			byte Flags;
			int Priority;
		}
		public class _WorldSafeLocs
		{
			int ID;
			string AreaName;
			float[] Loc = new float[3];
			float Facing;
			ushort MapID;
		}
		public class _Worldstate
		{
			int ID;
		}
		public class _Worldstateexpression
		{
			int ID;
			string Field1;
		}
		public class _Worldstateui
		{
			string Field00;
			string Field01;
			string Field02;
			string Field03;
			string Field04;
			ushort Field05;
			ushort Field06;
			ushort Field07;
			ushort Field08;
			ushort Field09;
			ushort[] Field0A = new ushort[3];
			byte Field0B;
			byte Field0C;
			byte Field0D;
			int ID;
			int Field0F;
			int Field10;
		}
		public class _Worldstatezonesounds
		{
			int ID;
			int Field1;
			ushort Field2;
			ushort Field3;
			ushort Field4;
			ushort Field5;
			ushort Field6;
			ushort Field7;
			byte Field8;
		}
		public class _Zoneintromusictable
		{
			int ID;
			string Field1;
			ushort Field2;
			byte Field3;
			uint Field4;
		}
		public class _Zonelight
		{
			int ID;
			string Field1;
			ushort Field2;
			ushort Field3;
		}
		public class _Zonelightpoint
		{
			int ID;
			float[] Field1 = new float[2];
			byte Field2;
		}
		public class _Zonemusic
		{
			int ID;
			string Field1;
			int[] Field2 = new int[2];
			int[] Field3 = new int[2];
			int[] Field4 = new int[2];
		}
		public class _Zonestory
		{
			int ID;
			int Field1;
			int Field2;
			byte Field3;
		}
	}
}
