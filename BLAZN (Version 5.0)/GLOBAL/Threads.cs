using System;
using System.Diagnostics;
using System.Threading;
using BLAZN.UTILITIES;

namespace BLAZN.GLOBAL
{
    using static Variables;
    using static Booleans;
    using static BLAZN.UTILITIES.Vectors.Vec3;

    public class Threads
    {
        public Memory m = new Memory();
        public Offsets Offsets = new Offsets();
        public MAIN main = MAIN.form;

        public static Vector3 TPCLocation = new Vector3();
        public static Vector3 CurrentP1Loc = new Vector3();
        public static Vector3 CurrentP2Loc = new Vector3();
        public static Vector3 CurrentP3Loc = new Vector3();
        public static Vector3 CurrentP4Loc = new Vector3();
        public static Vector3 P1Location = new Vector3();
        public static Vector3 P2Location = new Vector3();
        public static Vector3 P3Location = new Vector3();
        public static Vector3 P4Location = new Vector3();

        public void PointerThread()
        {
            while (true) 
            {
                try
                {
                    if (!m.IsOpen())
                    {
                        m.AttackProcess("BlackOpsColdWar");
                    }
                    else
                    {
                        var gameProcs = Process.GetProcessesByName("BlackOpsColdWar");
                        gameProc = gameProcs[0];
                        Proc = Memory.OpenProcess((uint)Memory.ProcessAccessFlags.All, false, gameProc.Id);

                        Offsets.BaseAddress = (ulong)Memory.GetBaseAddress("BlackOpsColdWar");
                        m.AttackProcess("BlackOpsColdWar");

                        Offsets.PlayerCompPtr = m.ReadInt64(Offsets.BaseAddress + Offsets.PlayerBase);

                        Offsets.PlayerPedPtr = m.ReadInt64(Offsets.BaseAddress + Offsets.PlayerBase + 0x8);

                        Offsets.ZMGlobalBase = m.ReadInt64(Offsets.BaseAddress + Offsets.PlayerBase + 0x60);

                        Offsets.ZMBotBase = m.ReadInt64(Offsets.BaseAddress + Offsets.PlayerBase + 0x68);

                        Offsets.ZMBotListBase = m.ReadInt64(Offsets.ZMBotBase + Offsets.ZM_Bot_List_Offset);

                        Offsets.CamoPtr = m.GetPointerInt(Offsets.BaseAddress + Offsets.UnlockBase, new ulong[] { Offsets.CamoOffset }, 1);

                        Offsets.ReticlesPtr = m.GetPointer(Offsets.BaseAddress + Offsets.ReticleBase, 2);

                        Offsets.CrystalsPtr = m.GetPointer(Offsets.BaseAddress + Offsets.UnlockBase, Offsets.CrystalOffset);      
                    }
                    Thread.Sleep(1000);
                } catch { }
            }
        }

        public void NameThread()
        {
            while (true)
            {
                try
                {
                    if (GameAttached)
                    {
                        var p1 = m.ReadString(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * 0) + Offsets.PC_Name);
                        if (p1 != "" && p1 != "UnnamedPlayer")
                        {
                            main.PLAYER1.Text = p1;
                            InGame = true;
                        }
                        else
                        {
                            main.PLAYER1.Text = "OFFLINE";
                            InGame = false;
                        }

                        if (InGame)
                        {
                            var p2 = m.ReadString(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * 1) + Offsets.PC_Name);
                            if (p2 != "" && p2 != "UnnamedPlayer")
                            {
                                main.PLAYER2.Text = p2;
                            }
                            else
                            {
                                main.PLAYER2.Text = "OFFLINE";
                            }

                            var p3 = m.ReadString(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * 2) + Offsets.PC_Name);
                            if (p3 != "" && p3 != "UnnamedPlayer")
                            {
                                main.PLAYER3.Text = p3;
                            }
                            else
                            {
                                main.PLAYER3.Text = "OFFLINE";
                            }

                            var p4 = m.ReadString(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * 3) + Offsets.PC_Name);
                            if (p4 != "" && p4 != "UnnamedPlayer")
                            {
                                main.PLAYER4.Text = p4;
                            }
                            else
                            {
                                main.PLAYER4.Text = "OFFLINE";
                            }
                        }
                        else
                        {
                            main.PLAYER1.Text = "PLAYER 1";
                            main.PLAYER2.Text = "PLAYER 2";
                            main.PLAYER3.Text = "PLAYER 3";
                            main.PLAYER4.Text = "PLAYER 4";
                        }
                    }
                    else
                    {
                        main.PLAYER1.Text = "PLAYER 1";
                        main.PLAYER2.Text = "PLAYER 2";
                        main.PLAYER3.Text = "PLAYER 3";
                        main.PLAYER4.Text = "PLAYER 4";
                    }

                    Thread.Sleep(1000);
                } catch { }
            }
        }

        public void LobbyThread()
        {
            while (true)
            {
                try
                {
                    if (GameAttached)
                    {
                        if (main.INSTACheck.Checked)
                        {
                            for (int i = 0; i < 90; i++)
                            {
                                var health = m.ReadFloat(Offsets.ZMBotListBase + Offsets.ZM_Bot_Health + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i));
                                if (health > 0 && health < 50000)
                                {
                                    m.WriteInt32(Offsets.ZMBotListBase + Offsets.ZM_Bot_Health + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i), 1);
                                }
                            }
                        }

                        if (main.ALLUPCheck.Checked)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                m.WriteInt32(main.PCompPtr(i) + Offsets.PC_Points, 420420);
                            }
                        }

                        if (P1UP && !main.ALLUPCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(0) + Offsets.PC_Points, 420420);
                        }

                        if (P2UP && !main.ALLUPCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(1) + Offsets.PC_Points, 420420);
                        }

                        if (P3UP && !main.ALLUPCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(2) + Offsets.PC_Points, 420420);
                        }

                        if (P4UP && !main.ALLUPCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(3) + Offsets.PC_Points, 420420);
                        }

                        if (main.SpeedSlider.Value == 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                m.WriteFloat(main.PCompPtr(i) + Offsets.PC_RunSpeed, 1.0f);
                            }
                        }

                        if (main.JumpSlider.Value == 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                m.WriteFloat(m.GetPointer(Offsets.BaseAddress + Offsets.JumpHeight, 0x130), 45f);
                            }
                        }

                        if (main.SkipCheck.Checked)
                        {
                            skipround1 = Offsets.PlayerPedPtr + Offsets.SkipRound;
                            skipround2 = skipround1 + 0x5F8;
                            skipround3 = skipround1 + 0xBF0;
                            skipround4 = skipround1 + 0x11E8;
                            skipround5 = skipround1 + 0x17E0;
                            skipround6 = skipround1 + 0x1DD8;

                            for (int a = 0; a < Skip.Length; a++)
                            {
                                Skip[a] = skipround1 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)a);
                            }

                            for (int b = 0; b < Skip2.Length; b++)
                            {
                                Skip2[b] = skipround2 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)b);
                            }

                            for (int c = 0; c < Skip3.Length; c++)
                            {
                                Skip3[c] = skipround3 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)c);
                            }

                            for (int d = 0; d < Skip4.Length; d++)
                            {
                                Skip4[d] = skipround4 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)d);
                            }

                            for (int e = 0; e < Skip5.Length; e++)
                            {
                                Skip5[e] = skipround5 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)e);
                            }

                            for (int f = 0; f < Skip6.Length; f++)
                            {
                                Skip6[f] = skipround6 + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)f);
                            }

                            for (int k = 0; k < Skip.Length; k++)
                            {
                                m.WriteInt32(Skip[k], 0);
                            }

                            for (int g = 0; g < Skip2.Length; g++)
                            {
                                m.WriteInt32(Skip2[g], 0);
                            }

                            for (int h = 0; h < Skip3.Length; h++)
                            {
                                m.WriteInt32(Skip3[h], 0);
                            }

                            for (int j = 0; j < Skip4.Length; j++)
                            {
                                m.WriteInt32(Skip4[j], 0);
                            }

                            for (int l = 0; l < Skip5.Length; l++)
                            {
                                m.WriteInt32(Skip5[l], 0);
                            }

                            for (int n = 0; n < Skip6.Length; n++)
                            {
                                m.WriteInt32(Skip6[n], 0);
                            }
                        }
                    }
                    Thread.Sleep(100);
                } catch { }
            }
        }

        public void NoSleepThread()
        {
            while (true)
            {
                try
                {
                    if (GameAttached)
                    {

                        CurrentP1Loc = m.ReadVector3(main.PPedPtr(0) + Offsets.PP_Coords);

                        if (main.ALLGMCheck.Checked)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                m.WriteInt32(main.PCompPtr(i) + Offsets.PC_GodMode, 0xA0);
                            }
                        }                   

                        if (P1GM && !main.ALLGMCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(0) + Offsets.PC_GodMode, 0xA0);
                        }

                        if (P2GM && !main.ALLGMCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(1) + Offsets.PC_GodMode, 0xA0);
                        }

                        if (P3GM && !main.ALLGMCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(2) + Offsets.PC_GodMode, 0xA0);
                        }

                        if (P4GM && !main.ALLGMCheck.Checked)
                        {
                            m.WriteInt32(main.PCompPtr(3) + Offsets.PC_GodMode, 0xA0);
                        }

                        if (P1TP)
                        {
                            for (int i = 0; i < 90; i++)
                            {
                                m.WriteVec3(Offsets.ZMBotListBase + Offsets.ZM_Bot_Coords + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i), P1Location);
                            }
                        }

                        if (P2TP)
                        {
                            for (int i = 0; i < 90; i++)
                            {
                                m.WriteVec3(Offsets.ZMBotListBase + Offsets.ZM_Bot_Coords + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i), P2Location);
                            }
                        }

                        if (P3TP)
                        {
                            for (int i = 0; i < 90; i++)
                            {
                                m.WriteVec3(Offsets.ZMBotListBase + Offsets.ZM_Bot_Coords + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i), P3Location);
                            }
                        }

                        if (P4TP)
                        {
                            for (int i = 0; i < 90; i++)
                            {
                                m.WriteVec3(Offsets.ZMBotListBase + Offsets.ZM_Bot_Coords + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i), P4Location);
                            }
                        }

                        if (main.TPCCheck.Checked)
                        {
                            double vertical = ConvertDegreesToRadians(BitConverter.ToSingle(m.ReadBytes(main.PPedPtr(0) + Offsets.PP_Heading_XY, 4), 0));
                            double lateral = -ConvertDegreesToRadians(BitConverter.ToSingle(m.ReadBytes(main.PPedPtr(0) + Offsets.PP_Heading_Z, 4), 0));

                            Vector3 crossHairOffset = new Vector3((float)Convert.ToSingle(Math.Cos(vertical) * Math.Cos(lateral)), (float)Convert.ToSingle(Math.Sin(vertical) * Math.Cos(lateral)), (float)Convert.ToSingle(Math.Sin(lateral)));

                            for (ulong i = 0; i < 90; i++)
                            {
                                m.WriteVec3(Offsets.ZMBotListBase + (Offsets.ZM_Bot_ArraySize_Offset * i) + Offsets.ZM_Bot_Coords, CurrentP1Loc + (crossHairOffset * 100));
                            }
                        }

                        /*
                        if (main.TPCCheck.Checked)
                        {
                            byte[] playerCoords = new byte[12];
                            Memory.ReadProcessMemory(Proc, m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + 0x8, Offsets.PP_Coords), playerCoords, 12, out _);
                            var origx = BitConverter.ToSingle(playerCoords, 0);
                            var origy = BitConverter.ToSingle(playerCoords, 4);
                            var origz = BitConverter.ToSingle(playerCoords, 8);
                            TPCLocation = new Vector3((float)Math.Round(origx, 4), (float)Math.Round(origy, 4), (float)Math.Round(origz, 4));

                            byte[] enemyPosBuffer = new byte[12];

                            byte[] playerHeadingXY = new byte[4];
                            byte[] playerHeadingZ = new byte[4];
                            Memory.ReadProcessMemory(Proc, m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + 0x8, Offsets.PP_Heading_XY), playerHeadingXY, 4, out _);
                            Memory.ReadProcessMemory(Proc, m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + 0x8, Offsets.PP_Heading_Z), playerHeadingZ, 4, out _);

                            var pitch = -ConvertToRadians(BitConverter.ToSingle(playerHeadingZ, 0));
                            var yaw = ConvertToRadians(BitConverter.ToSingle(playerHeadingXY, 0));
                            var x = Convert.ToSingle(Math.Cos(yaw) * Math.Cos(pitch));
                            var y = Convert.ToSingle(Math.Sin(yaw) * Math.Cos(pitch));
                            var z = Convert.ToSingle(Math.Sin(pitch));

                            var newEnemyPos = TPCLocation + (new Vector3(x, y, z) * 150);

                            Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.X), 0, enemyPosBuffer, 0, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.Y), 0, enemyPosBuffer, 4, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.Z), 0, enemyPosBuffer, 8, 4);

                            for (int i = 0; i < 90; i++)
                            {
                                Memory.WriteProcessMemory(Proc, Offsets.ZMBotListBase + (Offsets.ZM_Bot_ArraySize_Offset * (ulong)i) + Offsets.ZM_Bot_Coords, enemyPosBuffer, 12, out _);
                            }
                        }
                        */
                    }
                    Thread.Sleep(1);
                } catch { }
            }
        }

        public void AmmoThread()
        {
            while (true)
            {
                try
                {
                    if (GameAttached)
                    {
                        if (main.ALLIACheck.Checked)
                        {
                            for (int i = 1; i < 7; i++)
                            {
                                m.WriteInt32(main.PCompPtr(0) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                                m.WriteInt32(main.PCompPtr(1) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                                m.WriteInt32(main.PCompPtr(2) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                                m.WriteInt32(main.PCompPtr(3) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                            }
                        }

                        if (P1IA && !main.ALLIACheck.Checked)
                        {
                            for (int i = 1; i < 7; i++)
                            {
                                m.WriteInt32(main.PCompPtr(0) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                            }
                        }

                        if (P2IA && !main.ALLIACheck.Checked)
                        {
                            for (int i = 1; i < 7; i++)
                            {
                                m.WriteInt32(main.PCompPtr(1) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                            }
                        }

                        if (P3IA && !main.ALLIACheck.Checked)
                        {
                            for (int i = 1; i < 7; i++)
                            {
                                m.WriteInt32(main.PCompPtr(2) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                            }
                        }

                        if (P4IA && !main.ALLIACheck.Checked)
                        {
                            for (int i = 1; i < 7; i++)
                            {
                                m.WriteInt32(main.PCompPtr(3) + Offsets.PC_Ammo + ((ulong)i * 0x4), 420);
                            }
                        }               
                    }
                    Thread.Sleep(500);
                } catch { }
            }
        }

        public static double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
