using System;

namespace BLAZN.GLOBAL
{
    public class Offsets
    {
        public static ulong BaseAddress = 0x0;

        //*BASES (NEEDS SIGS)*//
        public static ulong PlayerBase = 0x112931C8;
        public static ulong XPScaleBase = 0x112C31B8;
        public static ulong TimeScaleBase = 0xF89D91C;
        public static ulong CMDBufferBase = 0xD81A8B0;
        public static ulong JumpHeight = 0x11391F90;
        public static ulong UnlockBase = 0x1186C580;
        public static ulong ReticleBase = 0x15639830;
        public static ulong SessionState = 0x1937C4B8;
        public static ulong PlayerCompPtr, PlayerPedPtr, ZMGlobalBase, ZMBotBase, ZMBotListBase, CMDBufferPtr, CamoPtr, CrystalsPtr, ReticlesPtr;
        public static ulong SkipRound = 0x333D8;
        public static ulong CMDBB_Exec = 0x1B;

        //*UNLOCK OFFSETS*//
        public static ulong CamoOffset = 0x112735;
        public static ulong ReticleOffset = 0xCFA8;
        public static ulong CrystalOffset = 0x829B;
        public static ulong client_size = 0x114D8;

        //*XP OFFSETS*//        
        public static ulong RankXP1 = 0x18;
        public static ulong RankXP2 = 0x28;
        public static ulong WPNXP = 0x30;

        //*STAT OFFSETS*//
        public static ulong StatOffset = 0xCFA8;
        public static ulong StatPtr = 0x112D0;

        //*PLAYERCOMPPTR OFFSETS*//
        public static ulong PC_ArraySize_Offset = 0xB970;
        public static ulong PC_CurrentUsedWeaponID = 0x28;
        public static ulong PC_SetWeaponID = 0xB0; // +(1-5 * 0x40 for WP2 to WP6)
        public static ulong PC_SetWeaponID2 = 0xF0;
        public static ulong PC_InfraredVision = 0xE66; // (byte) On=0x10|Off=0x0
        public static ulong PC_GodMode = 0xE67; // (byte) On=0xA0|Off=0x20
        public static ulong PC_Coords = 0xDE8;
        public static ulong PC_RapidFire1 = 0xE6C;
        public static ulong PC_RapidFire2 = 0xE80;
        public static ulong PC_MaxAmmo = 0x1360; // +(1-5 * 0x8 for WP1 to WP6)
        public static ulong PC_Ammo = 0x13D4; // +(1-5 * 0x4 for WP1 to WP6)
        public static ulong PC_Points = 0x5D24;
        public static ulong PC_Name = 0x605C;
        public static ulong PC_RunSpeed = 0x5C70;
        public static ulong PC_ClanTags = 0x605C;
        public static ulong PC_ReadyState1 = 0xE8;
        public static ulong PC_TeamID = 0x220;
        public static ulong PC_NumShots = 0xFE4;
        public static ulong PC_KillCount = 0x5D48;
        public static ulong PC_Camo = 0x5CE4;
        public static ulong PC_Crit = 0x10DA;

        //*SKIPROUNDPTR OFFSETS*//
        public static ulong SkipRoundOne = 0x5E8;
        public static ulong SkipRoundTwo = 0xBD0;
        public static ulong SkipRoundThree = 0x11B8;
        public static ulong SkipRoundFour = 0x17A0;
        public static ulong SkipRoundFive = 0x1D88;
        public static ulong SkipRoundSix = 0x2370;

        //*PLAYERPEDPTR OFFSETS*//
        public static ulong PP_ArraySize_Offset = 0x5E8;
        public static ulong PP_Health = 0x398;
        public static ulong PP_MaxHealth = 0x39C;
        public static ulong PP_Coords = 0x2D4;
        public static ulong PP_Heading_Z = 0x34;
        public static ulong PP_Heading_XY = 0x38;

        //*ZMGLOBAL & BOTBASE OFFSETS*//
        public static ulong ZM_Global_ZombiesIgnoreAll = 0x14;
        public static ulong ZM_Global_ZMLeftCount = 0x3C;
        public static ulong ZM_Bot_List_Offset = 0x8;
        public static ulong ZM_Bot_ArraySize_Offset = 0x5F8;

        public static ulong ZM_Bot_Health = 0x398;
        public static ulong ZM_Bot_MaxHealth = 0x39C;
        public static ulong ZM_Bot_Coords = 0x2D4;
    }
}
