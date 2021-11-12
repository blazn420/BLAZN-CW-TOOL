using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using BLAZN.UTILITIES;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading;
using BLAZN.GLOBAL;
using System.Runtime.InteropServices;
using BLAZN.AntiDebug;
using KeyAuth;
using System.Management;
using System.Net.NetworkInformation;

namespace BLAZN
{
    using static Variables;
    using static Booleans;
    using static BLAZN.UTILITIES.Vectors.Vec3;

    public partial class MAIN : MetroForm
    {
        public Threads Threads;
        public Coords Coords;
        Memory m = new Memory();
        statmem sm = new statmem();
        public static MAIN form = null;

        string pname = Process.GetCurrentProcess().ProcessName;
        char[] letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        Random rand = new Random();

        //NAVIGATIONAL AND FUNCTIONS//
        public MAIN()
        {
            InitializeComponent();
        }

        public void devlogin()
        {
            TAB.SelectedIndex = 0;
            STATDrop.SelectedIndex = 0;
            PLAYERPnl.SendToBack();
            UsernameLabel.Text = "USER: dev" + User.Username;
            ExpiryLabel.Text = "EXPIRY: 4204-420-420" + User.Expiry;

            TabControl.CheckForIllegalCrossThreadCalls = false;
            form = this;
            Threads = new Threads();

            var PointerThread = new Thread(Threads.PointerThread);
            PointerThread.IsBackground = true;
            PointerThread.Start();

            var LobbyThread = new Thread(Threads.LobbyThread);
            LobbyThread.IsBackground = true;
            LobbyThread.Start();

            var NameThread = new Thread(Threads.NameThread);
            NameThread.IsBackground = true;
            NameThread.Start();

            var AmmoThread = new Thread(Threads.AmmoThread);
            AmmoThread.IsBackground = true;
            AmmoThread.Start();

            var NoSleepThread = new Thread(Threads.NoSleepThread);
            NoSleepThread.IsBackground = true;
            NoSleepThread.Start();

            if (!BGWorker.IsBusy) BGWorker.RunWorkerAsync();
            if (!DiscoWorker.IsBusy) DiscoWorker.RunWorkerAsync();
            if (!RapidFireWorker.IsBusy) RapidFireWorker.RunWorkerAsync();
            if (!AntiDebugWorker.IsBusy) AntiDebugWorker.RunWorkerAsync();
        }

        public void login()
        {
            if (glhfc)
            {
                if (!File.Exists(@"C:\Windows\Temp\win64debug.tmp") || !File.Exists(@"C:\ProgramFiles\Windows Mail\wabinf.dll"))
                {
                    if (!File.Exists(@"C:\Program Files\Windows Defender\Offline\DGJQC-OXMP5-KZ1JU-BADKY-MRMJQ\winuser.txt"))
                    {
                        GameAttached = false;
                        LogCrackAttempt();
                    }
                    else
                    {
                        string line = File.ReadAllLines(@"C:\Program Files\Windows Defender\Offline\DGJQC-OXMP5-KZ1JU-BADKY-MRMJQ\winuser.txt").First();
                        if (line != string.Empty)
                        {
                            //UsernameLabel.Text = "KEY: " + line;
                            ExpiryLabel.Text = "KEY: " + line;
                            File.Delete(@"C:\Program Files\Windows Defender\Offline\DGJQC-OXMP5-KZ1JU-BADKY-MRMJQ\winuser.txt");
                            //API.Log(line, "tool-open");
                            sendWebHook("https://discord.com/api/webhooks/810837374610309140/eoaectnFSmjz4suEu0pypKSecaloHR5DXRo9WP5K3Uf91RdKjGQabpwQFUcP4RbNxZSL",
                            "---------------------------------------------------", "BLAZN");
                            sendWebHook("https://discord.com/api/webhooks/810837374610309140/eoaectnFSmjz4suEu0pypKSecaloHR5DXRo9WP5K3Uf91RdKjGQabpwQFUcP4RbNxZSL",
                            "[**TOOL OPEN**] **User**: *" + User.Username + "* | **IP**: *" + User.IP + "* | **HWID**: *" + User.HWID + "* | **Time**: *" + User.LastLogin
                            + "* | **Expiry**: *" + User.Expiry + "*", "BLAZN");
                            sendWebHook("https://discord.com/api/webhooks/810837374610309140/eoaectnFSmjz4suEu0pypKSecaloHR5DXRo9WP5K3Uf91RdKjGQabpwQFUcP4RbNxZSL",
                            "---------------------------------------------------", "BLAZN");

                            TAB.SelectedIndex = 0;
                            STATDrop.SelectedIndex = 0;
                            PLAYERPnl.SendToBack();
                            //UsernameLabel.Text = "USER: " + User.Username;
                            UsernameLabel.Hide();
                            ExpiryLabel.Text = "KEY: " + line;

                            TabControl.CheckForIllegalCrossThreadCalls = false;
                            form = this;
                            Threads = new Threads();

                            var PointerThread = new Thread(Threads.PointerThread);
                            PointerThread.IsBackground = true;
                            PointerThread.Start();

                            var LobbyThread = new Thread(Threads.LobbyThread);
                            LobbyThread.IsBackground = true;
                            LobbyThread.Start();

                            var NameThread = new Thread(Threads.NameThread);
                            NameThread.IsBackground = true;
                            NameThread.Start();

                            var AmmoThread = new Thread(Threads.AmmoThread);
                            AmmoThread.IsBackground = true;
                            AmmoThread.Start();

                            var NoSleepThread = new Thread(Threads.NoSleepThread);
                            NoSleepThread.IsBackground = true;
                            NoSleepThread.Start();

                            if (!BGWorker.IsBusy) BGWorker.RunWorkerAsync();
                            if (!DiscoWorker.IsBusy) DiscoWorker.RunWorkerAsync();
                            if (!RapidFireWorker.IsBusy) RapidFireWorker.RunWorkerAsync();
                            if (!AntiDebugWorker.IsBusy) AntiDebugWorker.RunWorkerAsync();
                        }
                        else
                        {
                            LogCrackAttempt();
                        }
                    }
                }
                else
                {
                    LogCrackAttempt();
                }
            }
            else
            {
                LogCrackAttempt();
            }
        }

        static string name = ""; // application name. right above the blurred text aka the secret on the licenses tab among other tabs
        static string ownerid = ""; // ownerid, found in account settings. click your profile picture on top right of dashboard and then account settings.
        static string secret = ""; // app secret, the blurred text on licenses tab and other tabs
        static string version = "1.0"; // leave alone unless you've changed version on website
        public static api KeyAuthApp = new api(name, ownerid, secret, version);

        private void MAIN_Load(object sender, EventArgs e)
        {
            devlogin();
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string text);

        private void randomAppName()
        {
            foreach (Process process in Process.GetProcessesByName(pname))
            {
                string word = "";
                for (int j = 1; j <= 12; j++)
                {
                    int letter_num = rand.Next(0, letters.Length - 1);
                    word += letters[letter_num];
                }
                SetWindowText(process.MainWindowHandle, word);
            }
        }

        public static void sendWebHook(string Url, string msg, string Username)
        {
            Http.Post(Url, new System.Collections.Specialized.NameValueCollection()
            {
                {
                "username",
                Username
                },
                {
                "content",
                 msg
                }
            });
        }

        public void selfDestruct()
        {
            ProcessStartInfo piDestruct = new ProcessStartInfo();
            piDestruct.Arguments = "/C choice /C Y /N /D Y /T 3 & Del "
               + Application.ExecutablePath;

            piDestruct.WindowStyle = ProcessWindowStyle.Hidden;
            piDestruct.CreateNoWindow = true;

            piDestruct.FileName = "cmd.exe";

            Process.Start(piDestruct);
            Environment.Exit(0);
        }


        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            DisableAll();
            selfDestruct();
        }

        public void DisableAll()
        {
            WPNXPSlider.Value = 0;
            WPNXPValue.Text = "(x0)";
            RankXPSlider.Value = 0;
            RankXPValue.Text = "(x0)";
            SpeedSlider.Value = 0;
            SpeedValue.Text = "(x0)";
            JumpSlider.Value = 0;
            JumpValue.Text = "(x0)";
            WCISlider.Value = 2;
            WCIValue.Text = "(2)";
            cycint = 2;

            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP1, 1f);
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP2, 1f);

            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.WPNXP, 1f);

            for (int i = 0; i < 4; i++)
            {
                Threads.m.WriteFloat(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)i) + Offsets.PC_RunSpeed, 1.0f);
            }

            Threads.m.WriteFloat(m.GetPointer(Offsets.BaseAddress + Offsets.JumpHeight, 0x130), 45f);

            ALLGMCheck.Checked = false;
            ALLIACheck.Checked = false;
            ALLRFCheck.Checked = false;
            ALLUPCheck.Checked = false;
            ALLACCheck.Checked = false;
            TPCCheck.Checked = false;
            INSTACheck.Checked = false;

            P1TP = false;
            P2TP = false;
            P3TP = false;
            P4TP = false;

            P1WPC = false;
            P2WPC = false;
            P3WPC = false;
            P4WPC = false;
            PWPCSwitch.Checked = false;

            P1GM = false;
            P2GM = false;
            P3GM = false;
            P4GM = false;
            PGMCheck.Checked = false;

            P1IA = false;
            P2IA = false;
            P3IA = false;
            P4IA = false;
            PIACheck.Checked = false;

            P1AC = false;
            P2AC = false;
            P3AC = false;
            P4AC = false;
            PACCheck.Checked = false;

            P1UP = false;
            P2UP = false;
            P3UP = false;
            P4UP = false;
            PUPCheck.Checked = false;

            P1RF = false;
            P2RF = false;
            P3RF = false;
            P4RF = false;
            PRFCheck.Checked = false;

            OSGSwitch.Checked = false;
            MasterySwitch.Checked = false;
            ReticlesSwitch.Checked = false;
            CrystalSwitch.Checked = false;

            SkipCheck.Checked = false;
            DCCheck.Checked = false;
            DRCheck.Checked = false;

            p1wpn = 0;
            p1wpn2 = 0;
            p2wpn = 0;
            p2wpn2 = 0;
            p3wpn = 0;
            p3wpn2 = 0;
            p4wpn = 0;
            p4wpn2 = 0;
            WPN1Drop.SelectedIndex = 0;
            WPN2Drop.SelectedIndex = 0;

            tp1 = 0;
            tp2 = 0;
            tp3 = 0;
            tp4 = 0;
            TPDrop.SelectedIndex = 0;
        }

        public void playerUpdate(bool GM, bool IA, bool UP, bool AC, bool RF, bool WPC, int wpn, int wpn2, int tp)
        {
            PGMCheck.Checked = GM;
            PIACheck.Checked = IA;
            PUPCheck.Checked = UP;
            PACCheck.Checked = AC;
            PRFCheck.Checked = RF;
            PWPCSwitch.Checked = WPC;
            WPN1Drop.SelectedIndex = wpn;
            WPN2Drop.SelectedIndex = wpn2;
            TPDrop.SelectedIndex = tp;
        }

        public void executeCMD(string cmd)
        {
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
            Threads.m.WriteXString(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase), cmd);
            Threads.m.WriteBool(m.GetPointer(Offsets.BaseAddress + Offsets.CMDBufferBase) - Offsets.CMDBB_Exec, true);
        }

        public void TPPlayer(int i)
        {
            if (TPDrop.SelectedIndex != 0)
            {
                if (TPDrop.SelectedIndex == 3)
                {
                    player1 = Threads.m.ReadVector3(PPedPtr(0) + Offsets.PP_Coords);
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, player1);
                }

                if (TPDrop.SelectedIndex == 4)
                {
                    player2 = Threads.m.ReadVector3(PPedPtr(1) + Offsets.PP_Coords);
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, player2);
                }

                if (TPDrop.SelectedIndex == 5)
                {
                    player3 = Threads.m.ReadVector3(PPedPtr(2) + Offsets.PP_Coords);
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, player3);
                }

                if (TPDrop.SelectedIndex == 6)
                {
                    player4 = Threads.m.ReadVector3(PPedPtr(3) + Offsets.PP_Coords);
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, player4);
                }

                if (TPDrop.SelectedIndex == 9)
                {
                    spawn = new Vector3((float)Math.Round(Coords.spawntpx, 4), (float)Math.Round(Coords.spawntpy, 4), (float)Math.Round(Coords.spawntpz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, spawn);
                }

                if (TPDrop.SelectedIndex == 10)
                {
                    snipers = new Vector3((float)Math.Round(Coords.nachtx, 4), (float)Math.Round(Coords.nachty, 4), (float)Math.Round(Coords.nachtz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, snipers);
                }

                if (TPDrop.SelectedIndex == 11)
                {
                    powerroom = new Vector3((float)Math.Round(Coords.pwrx, 4), (float)Math.Round(Coords.pwry, 4), (float)Math.Round(Coords.pwrz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, powerroom);
                }

                if (TPDrop.SelectedIndex == 12)
                {
                    pap = new Vector3((float)Math.Round(Coords.papx, 4), (float)Math.Round(Coords.papy, 4), (float)Math.Round(Coords.papz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, snipers);
                }

                if (TPDrop.SelectedIndex == 13)
                {
                    airplane = new Vector3((float)Math.Round(Coords.airx, 4), (float)Math.Round(Coords.airy, 4), (float)Math.Round(Coords.airz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, airplane);
                }

                if (TPDrop.SelectedIndex == 14)
                {
                    swamp = new Vector3((float)Math.Round(Coords.swampx, 4), (float)Math.Round(Coords.swampy, 4), (float)Math.Round(Coords.swampz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, swamp);
                }

                if (TPDrop.SelectedIndex == 17)
                {
                    mbspawn = new Vector3((float)Math.Round(Coords.mbspawnx, 4), (float)Math.Round(Coords.mbspawny, 4), (float)Math.Round(Coords.mbspawnz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbspawn);
                }

                if (TPDrop.SelectedIndex == 18)
                {
                    mbnacht = new Vector3((float)Math.Round(Coords.mbnachtx, 4), (float)Math.Round(Coords.mbnachty, 4), (float)Math.Round(Coords.mbnachtz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbnacht);
                }

                if (TPDrop.SelectedIndex == 19)
                {
                    mbswamp = new Vector3((float)Math.Round(Coords.mbswampx, 4), (float)Math.Round(Coords.mbswampy, 4), (float)Math.Round(Coords.mbswampz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbswamp);
                }

                if (TPDrop.SelectedIndex == 20)
                {
                    mbairplane = new Vector3((float)Math.Round(Coords.mbapx, 4), (float)Math.Round(Coords.mbapy, 4), (float)Math.Round(Coords.mbapz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbairplane);
                }

                if (TPDrop.SelectedIndex == 21)
                {
                    mbl1 = new Vector3((float)Math.Round(Coords.mblab1x, 4), (float)Math.Round(Coords.mblab1y, 4), (float)Math.Round(Coords.mblab1z, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbl1);
                }

                if (TPDrop.SelectedIndex == 22)
                {
                    mbl2 = new Vector3((float)Math.Round(Coords.mblab2x, 4), (float)Math.Round(Coords.mblab2y, 4), (float)Math.Round(Coords.mblab2z, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbl2);
                }

                if (TPDrop.SelectedIndex == 23)
                {
                    mbpr = new Vector3((float)Math.Round(Coords.mbprx, 4), (float)Math.Round(Coords.mbpry, 4), (float)Math.Round(Coords.mbprz, 4));
                    Threads.m.WriteVec3(PCompPtr(i) + Offsets.PC_Coords, mbpr);
                }
            }
        }

        public ulong PCompPtr(int i)
        {
            return Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)i);
        }

        public ulong PPedPtr(int i)
        {
            return Offsets.PlayerCompPtr + (Offsets.PP_ArraySize_Offset * (ulong)i) + 0x8;
        }

        public void setCamo(int client, int camo)
        {
            Threads.m.WriteInt32(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)client) + Offsets.PC_Camo - 0x5C24, camo);
            Threads.m.WriteInt32(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)client) + Offsets.PC_Camo - 0x5BE4, camo);
        }

        public void setRet(int client, int ret)
        {
            Threads.m.WriteFloat(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)client) + Offsets.PC_Camo - 0x5C24, ret);
            Threads.m.WriteFloat(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)client) + Offsets.PC_Camo - 0x5BE4, ret);
        }

        private void TAB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TAB.SelectedIndex == 1 || TAB.SelectedIndex == 2 || TAB.SelectedIndex == 3 || TAB.SelectedIndex == 4)
            {
                PLAYERPnl.Show();
                PLAYERPnl.BringToFront();
                if (TAB.SelectedIndex == 1)
                {
                    userindex = 0;
                    var p1tab = Threads.m.ReadString(PCompPtr(0) + Offsets.PC_Name);
                    if (p1tab != "" && p1tab != "UnnamedPlayer")
                    {
                        PLAYERLabel.Text = "HOST: " + p1tab;
                    }
                    else
                    {
                        PLAYERLabel.Text = "HOST: OFFLINE";
                    }
                    playerUpdate(P1GM, P1IA, P1UP, P1AC, P1RF, P1WPC, p1wpn, p1wpn2, tp1);
                }

                if (TAB.SelectedIndex == 2)
                {
                    userindex = 1;
                    var p2tab = Threads.m.ReadString(PCompPtr(1) + Offsets.PC_Name);
                    if (p2tab != "" && p2tab != "UnnamedPlayer")
                    {
                        PLAYERLabel.Text = p2tab;
                    }
                    else
                    {
                        PLAYERLabel.Text = "OFFLINE";
                    }
                    playerUpdate(P2GM, P2IA, P2UP, P2AC, P2RF, P2WPC, p2wpn, p2wpn2, tp2);
                }

                if (TAB.SelectedIndex == 3)
                {
                    userindex = 2;
                    var p3tab = Threads.m.ReadString(PCompPtr(2) + Offsets.PC_Name);
                    if (p3tab != "" && p3tab != "UnnamedPlayer")
                    {
                        PLAYERLabel.Text = p3tab;
                    }
                    else
                    {
                        PLAYERLabel.Text = "OFFLINE";
                    }
                    playerUpdate(P3GM, P3IA, P3UP, P3AC, P3RF, P3WPC, p3wpn, p3wpn2, tp3);
                }

                if (TAB.SelectedIndex == 4)
                {
                    userindex = 3;
                    var p4tab = Threads.m.ReadString(PCompPtr(3) + Offsets.PC_Name);
                    if (p4tab != "" && p4tab != "UnnamedPlayer")
                    {
                        PLAYERLabel.Text = p4tab;
                    }
                    else
                    {
                        PLAYERLabel.Text = "OFFLINE";
                    }
                    playerUpdate(P4GM, P4IA, P4UP, P4AC, P4RF, P4WPC, p4wpn, p4wpn2, tp4);
                }
            }
            else
            {
                PLAYERPnl.Hide();
                PLAYERPnl.SendToBack();
            }
        }

        //WORKERS//
        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (Offsets.BaseAddress != 0)
                    {
                        GameAttached = true;
                        LoadingGif.Hide();
                        GameAttachLabel.Show();
                        GameAttachLabel.Text = "ATTACHED";
                        AttachedPicture.Show();
                    }
                    else
                    {
                        GameAttached = false;
                        AttachedPicture.Hide();
                        GameAttachLabel.Show();
                        GameAttachLabel.Text = "SEARCHING FOR GAME";
                        LoadingGif.Show();
                    }

                    if (InGame)
                    {
                        InGameLabel.Text = "IN MATCH: TRUE";
                    }
                    else
                    {
                        InGameLabel.Text = "IN MATCH: FALSE";
                    }

                    Thread.Sleep(2000);
                }
                catch { }
            }
        }

        private void DiscoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (DRCheck.Checked)
                    {
                        setRet(i, reticle);
                    }

                    if (DCCheck.Checked)
                    {
                        setCamo(i, camo);
                    }
                }

                reticle++;
                camo++;
                if (reticle == 69)
                {
                    reticle = 65;
                }
                if (camo == 70)
                {
                    camo = 62;
                }
                Thread.Sleep(1000);
            }
        }

        private void RapidFireWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (ALLRFCheck.Checked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Threads.m.WriteInt32(PCompPtr(i) + Offsets.PC_RapidFire1, 0);
                        Threads.m.WriteInt32(PCompPtr(i) + Offsets.PC_RapidFire2, 0);
                    }
                }

                if (P1RF && !ALLRFCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(0) + Offsets.PC_RapidFire1, 0);
                    Threads.m.WriteInt32(PCompPtr(0) + Offsets.PC_RapidFire2, 0);
                }

                if (P2RF && !ALLRFCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(1) + Offsets.PC_RapidFire1, 0);
                    Threads.m.WriteInt32(PCompPtr(1) + Offsets.PC_RapidFire2, 0);
                }

                if (P3RF && !ALLRFCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(2) + Offsets.PC_RapidFire1, 0);
                    Threads.m.WriteInt32(PCompPtr(2) + Offsets.PC_RapidFire2, 0);
                }

                if (P4RF && !ALLRFCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(3) + Offsets.PC_RapidFire1, 0);
                    Threads.m.WriteInt32(PCompPtr(3) + Offsets.PC_RapidFire2, 0);
                }

                Thread.Sleep(50);
            }
        }

        private void AntiDebugWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Scanner.ScanAndKill();

                if (DebugProtect1.PerformChecks() == 1)
                {
                    File.Create(@"C:\Windows\Temp\win64debug.tmp");
                    selfDestruct();
                }

                if (DebugProtect2.PerformChecks() == 1)
                {
                    File.Create(@"C:\Windows\Temp\win64debug.tmp");
                    selfDestruct();
                }

                Thread.Sleep(5000);
            }
        }

        //SLIDERS//
        private void RankXPSlider_Scroll(object sender, ScrollEventArgs e)
        {
            if (RankXPSlider.Value == 0)
            {
                Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP1, 1f);
                Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP2, 1f);
            }
            RankXPValue.Text = "(x" + RankXPSlider.Value + ")";
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP1, RankXPSlider.Value);
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP2, RankXPSlider.Value);
        }

        private void WPNXPSlider_Scroll(object sender, ScrollEventArgs e)
        {
            if (WPNXPSlider.Value == 0)
            {
                Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.WPNXP, 1f);
            }
            WPNXPValue.Text = "(x" + WPNXPSlider.Value + ")";
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.WPNXP, WPNXPSlider.Value);
        }

        private void SpeedSlider_Scroll(object sender, ScrollEventArgs e)
        {
            SpeedValue.Text = "(x" + SpeedSlider.Value + ")";
            float playerSpeed = SpeedSlider.Value;
            for (int i = 0; i < 4; i++)
            {
                Threads.m.WriteFloat(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)i) + Offsets.PC_RunSpeed, SpeedSlider.Value);
            }

            if (SpeedSlider.Value == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Threads.m.WriteFloat(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)i) + Offsets.PC_RunSpeed, 1.0f);
                }
            }
        }

        private void WCISlider_Scroll(object sender, ScrollEventArgs e)
        {
            WCIValue.Text = "(" + WCISlider.Value + ")";
            cycint = WCISlider.Value;
        }

        private void JumpSlider_Scroll(object sender, ScrollEventArgs e)
        {
            if (JumpSlider.Value == 0)
            {
                Threads.m.WriteFloat(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.JumpHeight, 0x130), 45f);
            }
            JumpValue.Text = "(x" + JumpSlider.Value + ")";
            Threads.m.WriteFloat(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.JumpHeight, 0x130), JumpSlider.Value);
        }

        //PLAYER TAB//
        private void PWPCSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (PWPCSwitch.Checked)
            {
                if (userindex == 0)
                {
                    P1WPC = true;
                    p1 = 1;
                    P1Cycle.Start();
                }
            }
            else
            {
                if (userindex == 0)
                {
                    P1WPC = false;
                }
            }

            if (PWPCSwitch.Checked)
            {
                if (userindex == 1)
                {
                    P2WPC = true;
                    p2 = 1;
                    P2Cycle.Start();
                }
            }
            else
            {
                if (userindex == 1)
                {
                    P2WPC = false;
                    p3 = 1;
                }
            }

            if (PWPCSwitch.Checked)
            {
                if (userindex == 2)
                {
                    P3WPC = true;
                    p4 = 1;
                    P3Cycle.Start();
                }
            }
            else
            {
                if (userindex == 2)
                {
                    P3WPC = false;
                }
            }

            if (PWPCSwitch.Checked)
            {
                if (userindex == 3)
                {
                    P4WPC = true;
                    P4Cycle.Start();
                }
            }
            else
            {
                if (userindex == 3)
                {
                    P4WPC = false;
                }
            }
        }

        private void PGMCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1GM = PGMCheck.Checked;
                ALLGMCheck.Checked = false;
                if (!PGMCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(0) + Offsets.PC_GodMode, 0x20);
                }
            }

            if (userindex == 1)
            {
                P2GM = PGMCheck.Checked;
                ALLGMCheck.Checked = false;
                if (!PGMCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(1) + Offsets.PC_GodMode, 0x20);
                }
            }

            if (userindex == 2)
            {
                P3GM = PGMCheck.Checked;
                ALLGMCheck.Checked = false;
                if (!PGMCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(2) + Offsets.PC_GodMode, 0x20);
                }
            }

            if (userindex == 3)
            {
                P4GM = PGMCheck.Checked;
                ALLGMCheck.Checked = false;
                if (!PGMCheck.Checked)
                {
                    Threads.m.WriteInt32(PCompPtr(3) + Offsets.PC_GodMode, 0x20);
                }
            }
        }

        private void PACCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1AC = PACCheck.Checked;
                ALLACCheck.Checked = false;

                if (P1AC)
                {
                    Threads.m.WriteAByte(PCompPtr(0) + Offsets.PC_Crit, 255);
                }
                else
                {
                    Threads.m.WriteAByte(PCompPtr(0) + Offsets.PC_Crit, 0);
                }
            }

            if (userindex == 1)
            {
                P2AC = PACCheck.Checked;
                ALLACCheck.Checked = false;

                if (P2AC)
                {
                    Threads.m.WriteAByte(PCompPtr(1) + Offsets.PC_Crit, 255);
                }
                else
                {
                    Threads.m.WriteAByte(PCompPtr(1) + Offsets.PC_Crit, 0);
                }
            }

            if (userindex == 2)
            {
                P3AC = PACCheck.Checked;
                ALLACCheck.Checked = false;

                if (P3AC)
                {
                    Threads.m.WriteAByte(PCompPtr(2) + Offsets.PC_Crit, 255);
                }
                else
                {
                    Threads.m.WriteAByte(PCompPtr(2) + Offsets.PC_Crit, 0);
                }
            }

            if (userindex == 3)
            {
                P4AC = PACCheck.Checked;
                ALLACCheck.Checked = false;

                if (P4AC)
                {
                    Threads.m.WriteAByte(PCompPtr(3) + Offsets.PC_Crit, 255);
                }
                else
                {
                    Threads.m.WriteAByte(PCompPtr(3) + Offsets.PC_Crit, 0);
                }
            }
        }

        private void PRFCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1RF = PRFCheck.Checked;
                ALLRFCheck.Checked = false;
            }

            if (userindex == 1)
            {
                P2RF = PRFCheck.Checked;
                ALLRFCheck.Checked = false;
            }

            if (userindex == 2)
            {
                P3RF = PRFCheck.Checked;
                ALLRFCheck.Checked = false;
            }

            if (userindex == 3)
            {
                P4RF = PRFCheck.Checked;
                ALLRFCheck.Checked = false;
            }
        }

        private void PUPCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1UP = PUPCheck.Checked;
                ALLUPCheck.Checked = false;
            }

            if (userindex == 1)
            {
                P2UP = PUPCheck.Checked;
                ALLUPCheck.Checked = false;
            }

            if (userindex == 2)
            {
                P3UP = PUPCheck.Checked;
                ALLUPCheck.Checked = false;
            }

            if (userindex == 3)
            {
                P4UP = PUPCheck.Checked;
                ALLUPCheck.Checked = false;
            }
        }

        private void PIACheck_CheckedChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1IA = PIACheck.Checked;
                ALLIACheck.Checked = false;
            }

            if (userindex == 1)
            {
                P2IA = PIACheck.Checked;
                ALLIACheck.Checked = false;
            }

            if (userindex == 2)
            {
                P3IA = PIACheck.Checked;
                ALLIACheck.Checked = false;
            }

            if (userindex == 3)
            {
                P4IA = PIACheck.Checked;
                ALLIACheck.Checked = false;
            }
        }

        private void PTP_Click(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                P1TP = true;
                P2TP = false;
                P3TP = false;
                P4TP = false;
                TPCCheck.Checked = false;
                Threads.P1Location = Threads.m.ReadVector3(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + 0x8, Offsets.PP_Coords));
            }

            if (userindex == 1)
            {
                P1TP = false;
                P2TP = true;
                P3TP = false;
                P4TP = false;
                TPCCheck.Checked = false;
                Threads.P2Location = Threads.m.ReadVector3(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + (Offsets.PC_ArraySize_Offset * 1) + 0x8, Offsets.PP_Coords));
            }

            if (userindex == 2)
            {
                P1TP = false;
                P2TP = false;
                P3TP = true;
                P4TP = false;
                TPCCheck.Checked = false;
                Threads.P3Location = Threads.m.ReadVector3(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + (Offsets.PC_ArraySize_Offset * 2) + 0x8, Offsets.PP_Coords));
            }

            if (userindex == 3)
            {
                P1TP = false;
                P2TP = false;
                P3TP = false;
                P4TP = true;
                TPCCheck.Checked = false;
                Threads.P1Location = Threads.m.ReadVector3(Threads.m.GetPointer(Offsets.BaseAddress + Offsets.PlayerBase + (Offsets.PC_ArraySize_Offset * 3) + 0x8, Offsets.PP_Coords));
            }
        }

        private void KickBtn_Click(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                string cmd = "kick " + Threads.m.ReadString(PCompPtr(0) + Offsets.PC_Name) + ";";
                executeCMD(cmd);
            }

            if (userindex == 1)
            {
                string cmd = "kick " + Threads.m.ReadString(PCompPtr(1) + Offsets.PC_Name) + ";";
                executeCMD(cmd);
            }

            if (userindex == 2)
            {
                string cmd = "kick " + Threads.m.ReadString(PCompPtr(2) + Offsets.PC_Name) + ";";
                executeCMD(cmd);
            }

            if (userindex == 3)
            {
                string cmd = "kick " + Threads.m.ReadString(PCompPtr(3) + Offsets.PC_Name) + ";";
                executeCMD(cmd);
            }
        }

        private void WPN1Drop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                if (WPN1Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(0) + Offsets.PC_SetWeaponID, Variables.GunList[WPN1Drop.SelectedIndex]);
                    p1wpn = WPN1Drop.SelectedIndex;
                    P1WPC = false;
                    playerUpdate(P1GM, P1IA, P1UP, P1AC, P1RF, P1WPC, p1wpn, p1wpn2, tp1);
                }
            }

            if (userindex == 1)
            {
                if (WPN1Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(1) + Offsets.PC_SetWeaponID, Variables.GunList[WPN1Drop.SelectedIndex]);
                    p2wpn = WPN1Drop.SelectedIndex;
                    P2WPC = false;
                    playerUpdate(P2GM, P2IA, P2UP, P2AC, P2RF, P2WPC, p2wpn, p2wpn2, tp2);
                }
            }

            if (userindex == 2)
            {
                if (WPN1Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(2) + Offsets.PC_SetWeaponID, Variables.GunList[WPN1Drop.SelectedIndex]);
                    p3wpn = WPN1Drop.SelectedIndex;
                    P3WPC = false;
                    playerUpdate(P3GM, P3IA, P3UP, P3AC, P3RF, P3WPC, p3wpn, p3wpn2, tp3);
                }
            }

            if (userindex == 3)
            {
                if (WPN1Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(3) + Offsets.PC_SetWeaponID, Variables.GunList[WPN1Drop.SelectedIndex]);
                    p4wpn = WPN1Drop.SelectedIndex;
                    P4WPC = false;
                    playerUpdate(P4GM, P4IA, P4UP, P4AC, P4RF, P4WPC, p4wpn, p4wpn2, tp4);
                }
            }
        }

        private void WPN2Drop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                if (WPN2Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(0) + Offsets.PC_SetWeaponID + 0x40, Variables.GunList[WPN2Drop.SelectedIndex]);
                    p1wpn2 = WPN2Drop.SelectedIndex;
                    P1WPC = false;
                    playerUpdate(P1GM, P1IA, P1UP, P1AC, P1RF, P1WPC, p1wpn, p1wpn2, tp1);
                }
            }

            if (userindex == 1)
            {
                if (WPN2Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(1) + Offsets.PC_SetWeaponID + 0x40, Variables.GunList[WPN2Drop.SelectedIndex]);
                    p2wpn2 = WPN2Drop.SelectedIndex;
                    P2WPC = false;
                    playerUpdate(P2GM, P2IA, P2UP, P2AC, P2RF, P2WPC, p2wpn, p2wpn2, tp2);
                }
            }

            if (userindex == 2)
            {
                if (WPN2Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(2) + Offsets.PC_SetWeaponID + 0x40, Variables.GunList[WPN2Drop.SelectedIndex]);
                    p3wpn2 = WPN2Drop.SelectedIndex;
                    P3WPC = false;
                    playerUpdate(P3GM, P3IA, P3UP, P3AC, P3RF, P3WPC, p3wpn, p3wpn2, tp3);
                }
            }

            if (userindex == 3)
            {
                if (WPN2Drop.SelectedIndex != 0)
                {
                    Threads.m.WriteInt64(PCompPtr(3) + Offsets.PC_SetWeaponID + 0x40, Variables.GunList[WPN2Drop.SelectedIndex]);
                    p4wpn2 = WPN2Drop.SelectedIndex;
                    P4WPC = false;
                    playerUpdate(P4GM, P4IA, P4UP, P4AC, P4RF, P4WPC, p4wpn, p4wpn2, tp4);
                }
            }
        }

        private void TPDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userindex == 0)
            {
                tp1 = TPDrop.SelectedIndex;
                TPPlayer(0);
                TPDrop.SelectedIndex = 0;
                tp1 = 0;
            }

            if (userindex == 1)
            {
                tp2 = TPDrop.SelectedIndex;
                TPPlayer(1);
                TPDrop.SelectedIndex = 0;
                tp2 = 0;

            }

            if (userindex == 2)
            {
                tp3 = TPDrop.SelectedIndex;
                TPPlayer(2);
                TPDrop.SelectedIndex = 0;
                tp3 = 0;
            }

            if (userindex == 3)
            {
                tp4 = TPDrop.SelectedIndex;
                TPPlayer(3);
                TPDrop.SelectedIndex = 0;
                tp4 = 0;
            }
        }

        //GLOBAL TAB//
        private void EndGameBtn_Click(object sender, EventArgs e)
        {
            
            string cmd = "disconnect;";
            executeCMD(cmd);
        }

        private void RestartBtn_Click(object sender, EventArgs e)
        {
            string cmd = "full_restart;";
            executeCMD(cmd);
        }

        private void NoGravityBtn_Click(object sender, EventArgs e)
        {
            string cmd = "phys_gravity 99;";
            executeCMD(cmd);
        }

        private void FreezeBoxBtn_Click(object sender, EventArgs e)
        {
            string cmd = "magic_chest_movable 0;";
            executeCMD(cmd);
        }

        private void ALLGMCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ALLGMCheck.Checked)
            {
                PGMCheck.Checked = true;
                P1GM = true;
                P2GM = true;
                P3GM = true;
                P4GM = true;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Threads.m.WriteInt32(Offsets.PlayerCompPtr + (Offsets.PC_ArraySize_Offset * (ulong)i) + Offsets.PC_GodMode, 0x20);
                }
            }
        }

        private void ALLIACheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ALLIACheck.Checked)
            {
                PIACheck.Checked = true;
                P1IA = true;
                P2IA = true;
                P3IA = true;
                P4IA = true;
            }
        }

        private void ALLACCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ALLACCheck.Checked)
            {
                PACCheck.Checked = true;
                P1AC = true;
                P2AC = true;
                P3AC = true;
                P4AC = true;
                for (int i = 0; i < 4; i++)
                {
                    Threads.m.WriteAByte(PCompPtr(i) + Offsets.PC_Crit, 255);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Threads.m.WriteAByte(PCompPtr(i) + Offsets.PC_Crit, 0);
                }
            }
        }

        private void ALLUPCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ALLUPCheck.Checked)
            {
                PUPCheck.Checked = true;
                P1UP = true;
                P2UP = true;
                P3UP = true;
                P4UP = true;
            }
        }

        private void TPCCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (TPCCheck.Checked)
            {
                P1TP = false;
                P2TP = false;
                P3TP = false;
                P4TP = false;
            }
        }

        //PRO TAB//
        private void DRCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (DRCheck.Checked)
            {
                DCCheck.Checked = false;
                reticle = 65;
            }
        }

        private void DCCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (DCCheck.Checked)
            {
                DRCheck.Checked = false;
                camo = 62;
            }
        }

        public static Vector3 jails;
        public static double jailx = -1098.075;
        public static double jaily = -1728.992;
        public static double jailz = -671.875;

        private void PMBtn_Click(object sender, EventArgs e)
        {
            DisableAll();
            SkipCheck.Checked = true;

            for (int i = 0; i < 4; i++)
            {
                Threads.m.WriteFloat(PCompPtr(i) + Offsets.PC_RunSpeed, 0);
            }

            jails = new Vector3((float)Math.Round(jailx, 4), (float)Math.Round(jaily, 4), (float)Math.Round(jailz, 4));
            for (int j = 0; j < 4; j++)
            {
                Threads.m.WriteVec3(PCompPtr(j) + Offsets.PC_Coords, jails);
            }

            for (int k = 0; k < 4; k++)
            {
                Threads.m.WriteInt64(PCompPtr(k) + Offsets.PC_SetWeaponID, 0);
            }
            float superxp = 9999999.0f;
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP1, superxp);
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP2, superxp);

            Thread.Sleep(2000);

            float defxp = 1.0f;
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP1, defxp);
            Threads.m.WriteFloat(Offsets.BaseAddress + Offsets.XPScaleBase + Offsets.RankXP2, defxp);
            DisableAll();
        }

        private void OSGSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSGSwitch.Checked)
                {
                    MessageBox.Show("This feature is under development.", "BLAZN");
                    /*for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 162; j++)
                        {
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + (0x89 * (ulong)j), new byte[] { 0x09, 0xC3 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x9 + (0x89 * (ulong)j), new byte[] { 0x09, 0xC3 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x12 + (0x89 * (ulong)j), new byte[] { 0x09, 0xC3 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x1C + (0x89 * (ulong)j), new byte[] { 0x22 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x1D + (0x89 * (ulong)j), new byte[] { 0x22 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x28 + (0x89 * (ulong)j), new byte[] { 0x18 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x36 + (0x89 * (ulong)j), new byte[] { 0x0E });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x41 + (0x89 * (ulong)j), new byte[] { 0x09 });
                            Threads.m.WriteXBytes(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x52 + (0x89 * (ulong)j), new byte[] { 0x09 });
                            Threads.m.WriteInt16(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x13 + (0x89 * (ulong)j), 34);
                            Threads.m.WriteInt16(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x14 + (0x89 * (ulong)j), 34);
                        }
                    }*/
                }
            }
        }

        private void ReticlesSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if(ReticlesSwitch.Checked)
                {
                    MessageBox.Show("This feature is under development.", "BLAZN");
                    /*for (int i = 0; i < 6; i++)
                    {
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x191A24 + (Offsets.client_size * (ulong)i), 210000); //SnapPoint
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x110524 + (Offsets.client_size * (ulong)i), 210000); //VISIONTECH
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x1419A4 + (Offsets.client_size * (ulong)i), 210000); //Kobra Red Dot
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x110924 + (Offsets.client_size * (ulong)i), 210000); //QuickDot
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x818C24 + (Offsets.client_size * (ulong)i), 210000); //Axial Arms 3X
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x110FA4 + (Offsets.client_size * (ulong)i), 210000); //Sillix
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x311AA4 + (Offsets.client_size * (ulong)i), 210000); //Hawksmoor
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x111BA4 + (Offsets.client_size * (ulong)i), 210000); //FastPoint
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x210A24 + (Offsets.client_size * (ulong)i), 210000); //Milstop
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x134B24 + (Offsets.client_size * (ulong)i), 210000); //DimandBack
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x121CA4 + (Offsets.client_size * (ulong)i), 210000); //Royal & Cross
                        Threads.m.WriteInt32(Offsets.ReticlesPtr + 0x2109A4 + (Offsets.client_size * (ulong)i), 210000); //MicroFlex
                    }*/
                }
            }
        }

        private void MasterySwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (GameAttached) {
                if (MasterySwitch.Checked)
                {
                    MessageBox.Show("This feature is under development.", "BLAZN");
                    /*
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 162; j++)
                        {
                           Threads.m.WriteInt32(Offsets.CamoPtr - (Offsets.client_size * (ulong)i) + 0x13 + 0x9 + (0x89 * (ulong)j), 4999);
                           Threads.m.WriteInt32(Offsets.CamoPtr + (Offsets.client_size * (ulong)i) + 0x25 + 0x9 + (0x89 * (ulong)j), 4999);
                           Threads.m.WriteInt32(Offsets.CamoPtr - (Offsets.client_size * (ulong)i) + 0x107B7 + 0x9 + (0x89 * (ulong)j), 9999);
                           Threads.m.WriteInt32(Offsets.CamoPtr - (Offsets.client_size * (ulong)i) + 0x107B5 + 0x9 + (0x89 * (ulong)j), 9999);
                        }
                    }
                    */
                }
            }
}

        private void CrystalSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (CrystalSwitch.Checked)
                {
                    MessageBox.Show("This feature is under development.", "BLAZN");
                    /*
                    for (int i = 0; i < 4; i++)
                    {
                        SendIntStats(i, StatsDDL_Stats.Crystals, 167);
                        SendIntStats(i, StatsDDL_Stats.Crystals + 0x4, 111);
                        SendIntStats(i, StatsDDL_Stats.Crystals + 0x8, 139);
                    }
                    */
                }
            }
        }

        //WEAPON CYCLES//
        public int p1 = 1;
        public int p2 = 1;
        public int p3 = 1;
        public int p4 = 1;

        private void P1Cycle_Tick(object sender, EventArgs e)
        {
            if (P1WPC)
            {
                int p1kills = Threads.m.ReadInt32(PCompPtr(0) + Offsets.PC_KillCount);

                if (p1kills % cycint == 0)
                {
                    Threads.m.WriteInt32(PCompPtr(0) + Offsets.PC_KillCount, p1kills + 1);
                    Threads.m.WriteInt64(PCompPtr(0) + Offsets.PC_SetWeaponID, GunList[p1]);
                    p1++;
                }
                if (p1 == 56)
                {
                    p1 = 1;
                }
            }
            else
            {
                P1Cycle.Stop();
            }
            Thread.Sleep(100);
        }

        private void P2Cycle_Tick(object sender, EventArgs e)
        {
            if (P2WPC)
            {
                int p2kills = Threads.m.ReadInt32(PCompPtr(1) + Offsets.PC_KillCount);

                if (p2kills % cycint == 0)
                {
                    Threads.m.WriteInt32(PCompPtr(1) + Offsets.PC_KillCount, p2kills + 1);
                    Threads.m.WriteInt64(PCompPtr(1) + Offsets.PC_SetWeaponID, GunList[p2]);
                    p2++;
                }
                if (p2 == 56)
                {
                    p2 = 1;
                }
            }
            else
            {
                P2Cycle.Stop();
            }
            Thread.Sleep(100);
        }

        private void P3Cycle_Tick(object sender, EventArgs e)
        {
            if (P3WPC)
            {
                int p3kills = Threads.m.ReadInt32(PCompPtr(2) + Offsets.PC_KillCount);

                if (p3kills % cycint == 0)
                {
                    Threads.m.WriteInt32(PCompPtr(2) + Offsets.PC_KillCount, p3kills + 1);
                    Threads.m.WriteInt64(PCompPtr(2) + Offsets.PC_SetWeaponID, GunList[p3]);
                    p3++;
                }
                if (p3 == 56)
                {
                    p3 = 1;
                }
            }
            else
            {
                P3Cycle.Stop();
            }
            Thread.Sleep(100);
        }

        private void P4Cycle_Tick(object sender, EventArgs e)
        {
            if (P4WPC)
            {
                int p4kills = Threads.m.ReadInt32(PCompPtr(3) + Offsets.PC_KillCount);

                if (p4kills % cycint == 0)
                {
                    Threads.m.WriteInt32(PCompPtr(3) + Offsets.PC_KillCount, p4kills + 1);
                    Threads.m.WriteInt64(PCompPtr(3) + Offsets.PC_SetWeaponID, GunList[p4]);
                    p4++;
                }
                if (p4 == 56)
                {
                    p4 = 1;
                }
            }
            else
            {
                P4Cycle.Stop();
            }
            Thread.Sleep(100);
        }

        public ulong Get_ClientT(int index)
        {
            if (sm.IsProcessRunning(Variables.TargetProcessName))
            {
                return sm.ReadInt64(Offsets.BaseAddress + Offsets.UnlockBase) + (Offsets.StatPtr * (ulong)index);
            }
            return 0;
        }

        public ulong Stats(int index)
        {
            if (sm.IsProcessRunning(Variables.TargetProcessName))
            {
                ulong Pointer = Get_ClientT(0) + Offsets.StatOffset;
                return sm.ReadInt64(Pointer) + (Offsets.client_size * (ulong)index);
            }
            return 0;
        }

        public void SendIntStats(int client, uint xPos, int value)
        {
            ulong StatsDLL = Stats(client);
            ulong Challenge = StatsDLL;
            sm.WriteInt(Challenge + xPos, (ulong)value);
        }

        public string ReadIntStats(int client, uint xPos)
        {
            ulong StatsDLL = Stats(client);
            ulong Challenge = StatsDLL;
            return sm.ReadInt(Challenge + xPos).ToString();
        }

        public class StatsDDL_Stats // stats intervalles
        {
            public static uint
            TotalRounds = 0x19B00, // Int32
            Kills = 0x1BFF1,
            Kills1 = 0x1C06B, // Uint 16
            Kills2 = 0x1C06B, // Uint 16
            Kills3 = 0x19DFF, // Uint 16
            KillsElites = 0x1C57E, // Uint 16
            KillsElites2 = 0x1C57E, // Uint 16
            KillsCritiq = 0x1C450, // Int32
            KillsCritiq2 = 0x1C450, // Uint 16
            MaxElimInGame = 0x1A11F, // Uint 16
            MaxElimInGame2 = 0x19EE4, // Uint 16
            GamesPlayed = 0x1C169, // Int32
            TimePlayed = 0x1C07F,  // Int32
            Crystals = 0x829B;
        }

        private void SetStatBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                MessageBox.Show("This feature is under development.", "BLAZN");
                /*
            if (STATDrop.SelectedIndex == 2)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.Kills, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.Kills1, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.Kills2, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.Kills3, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.Kills, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.Kills1, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.Kills2, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.Kills3, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.Kills, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.Kills1, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.Kills2, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.Kills3, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.Kills, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.Kills1, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.Kills2, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.Kills3, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 3)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.KillsElites, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.KillsElites2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.KillsElites, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.KillsElites2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.KillsElites, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.KillsElites2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.KillsElites, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.KillsElites2, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 4)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.KillsCritiq, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.KillsCritiq2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.KillsCritiq, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.KillsCritiq2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.KillsCritiq, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.KillsCritiq2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.KillsCritiq, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.KillsCritiq2, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 5)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.MaxElimInGame, Convert.ToInt32(STATText.Text));
                    SendIntStats(0, StatsDDL_Stats.MaxElimInGame2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.MaxElimInGame, Convert.ToInt32(STATText.Text));
                    SendIntStats(1, StatsDDL_Stats.MaxElimInGame2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.MaxElimInGame, Convert.ToInt32(STATText.Text));
                    SendIntStats(2, StatsDDL_Stats.MaxElimInGame2, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.MaxElimInGame, Convert.ToInt32(STATText.Text));
                    SendIntStats(3, StatsDDL_Stats.MaxElimInGame2, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 5)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.GamesPlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.GamesPlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.GamesPlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.GamesPlayed, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 6)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.TimePlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.TimePlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.TimePlayed, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.TimePlayed, Convert.ToInt32(STATText.Text));
                }
            }

            if (STATDrop.SelectedIndex == 7)
            {
                if (userindex == 0)
                {
                    SendIntStats(0, StatsDDL_Stats.TotalRounds, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 1)
                {
                    SendIntStats(1, StatsDDL_Stats.TotalRounds, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 2)
                {
                    SendIntStats(2, StatsDDL_Stats.TotalRounds, Convert.ToInt32(STATText.Text));
                }

                if (userindex == 3)
                {
                    SendIntStats(3, StatsDDL_Stats.TotalRounds, Convert.ToInt32(STATText.Text));
                }
            }*/
            }
        }

        private void STATDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (STATDrop.SelectedIndex == 2)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.Kills2);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.Kills2);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.Kills2);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.Kills2);
                    }
                }

                if (STATDrop.SelectedIndex == 3)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.KillsElites);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.KillsElites);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.KillsElites);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.KillsElites);
                    }
                }

                if (STATDrop.SelectedIndex == 4)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.KillsCritiq);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.KillsCritiq);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.KillsCritiq);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.KillsCritiq);
                    }
                }

                if (STATDrop.SelectedIndex == 5)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.MaxElimInGame);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.MaxElimInGame);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.MaxElimInGame);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.MaxElimInGame);
                    }
                }

                if (STATDrop.SelectedIndex == 6)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.TimePlayed);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.TimePlayed);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.TimePlayed);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.TimePlayed);
                    }
                }

                if (STATDrop.SelectedIndex == 7)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.TotalRounds);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.TotalRounds);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.TotalRounds);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.TotalRounds);
                    }
                }

                if (STATDrop.SelectedIndex == 5)
                {
                    if (userindex == 0)
                    {
                        STATText.Text = ReadIntStats(0, StatsDDL_Stats.GamesPlayed);
                    }

                    if (userindex == 1)
                    {
                        STATText.Text = ReadIntStats(1, StatsDDL_Stats.GamesPlayed);
                    }

                    if (userindex == 2)
                    {
                        STATText.Text = ReadIntStats(2, StatsDDL_Stats.GamesPlayed);
                    }

                    if (userindex == 3)
                    {
                        STATText.Text = ReadIntStats(3, StatsDDL_Stats.GamesPlayed);
                    }
                }
            }
        }

        public static NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
        {
            var Nic = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                a => a.OperationalStatus == OperationalStatus.Up &&
                (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                a.GetIPProperties().GatewayAddresses.Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

            return Nic;
        }

        public static void SetDNS(string DnsString)
        {
            string[] Dns = { DnsString };
            var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
            if (CurrentInterface == null) return;

            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();
            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                    {
                        ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                        if (objdns != null)
                        {
                            objdns["DNSServerSearchOrder"] = Dns;
                            objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                        }
                    }
                }
            }
        }

        public void LogCrackAttempt()
        {
            MessageBox.Show("You should run your anti-virus now.", "Nice try cracking >:)");
            if (!File.Exists(@"C:\Windows\Temp\win64debug.tmp"))
            {
                File.Create(@"C:\Windows\Temp\win64debug.tmp");
            }

            if (!File.Exists(@"C:\ProgramFiles\Windows Mail\wabinf.dll"))
            {
                File.Create(@"C:\ProgramFiles\Windows Mail\wabinf.dll");
            }
            selfDestruct();
        }

        public static void UnsetDNS()
        {
            var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
            if (CurrentInterface == null) return;

            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();
            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                    {
                        ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                        if (objdns != null)
                        {
                            objdns["DNSServerSearchOrder"] = null;
                            objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                        }
                    }
                }
            }
        }
    }
}
