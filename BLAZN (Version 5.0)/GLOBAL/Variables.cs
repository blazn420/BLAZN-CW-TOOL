using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLAZN.UTILITIES.Vectors.Vec3;

namespace BLAZN.GLOBAL
{
    public static class Variables
    {
        public static IntPtr Proc;
        public static Process gameProc;
        public static long BaseAddress = 0;
        public static int ProcessID;

        public static string TargetProcessName = "BlackOpsColdWar";

		//TOOL NAVIGATION//
		public static int userindex = -1;
        
        //TOOL FEATURES//
        public static ulong[] Skip = new ulong[50];
        public static ulong[] Skip2 = new ulong[50];
        public static ulong[] Skip3 = new ulong[50];
        public static ulong[] Skip4 = new ulong[50];
        public static ulong[] Skip5 = new ulong[50];
        public static ulong[] Skip6 = new ulong[50];
        public static ulong skipround1;
        public static ulong skipround2;
        public static ulong skipround3;
        public static ulong skipround4;
        public static ulong skipround5;
        public static ulong skipround6;

        //DISCO FEATURES//
        public static int reticle = 65;
        public static int camo = 62;

        //WEAPON CYCLE//
        public static int[] GunList = new int[] { 0, 333, 346, 315, 347, 350, 388, 395, 364, 353, 422, 409, 448, 438, 366, 450, 309, 328,
        340, 439, 349, 424, 445, 293, 323, 335, 389, 441, 344, 463, 373, 312, 341, 308, 406, 421, 465, 285, 404, 320, 311, 466, 449, 372,
        334, 431, 317, 459, 365, 458, 433, 430, 361, 301, 414, 390 };

        public static string[] GunNames = new string[] { "XM4", "AK47", "KRIG 6", "QBZ", "FFAR", "GROZA", "FARA 83", "C58", 
            "MILANO", "BULLFROG", "AK74U", "KSP45", "Mac 10", "Cnidoblaster", "TYPE 63", "M16", "DMR", "AUG", "STONER", "M60", 
            "RPD", "PELLINGTON", "TUNDRA", "M82", "DIAMATTI", "1911", "MAGNUM", "HAUER", "GALLO", "Striker", "Cigma", "RPG", 
            "M79", "knife", "Sledge hammer", "El slasher", "Ace of Spades", "Yamikirimaru", "Lance a Lot" };

        public static int cycint = 2;

        //WEAPON DROPDOWN//
        public static int p1wpn = 0;
        public static int p1wpn2 = 0;
        public static int p2wpn = 0;
        public static int p2wpn2 = 0;
        public static int p3wpn = 0;
        public static int p3wpn2 = 0;
        public static int p4wpn = 0;
        public static int p4wpn2 = 0;

        //TP DROPDOWN//
        public static int tp1 = 0;
        public static int tp2 = 0;
        public static int tp3 = 0;
        public static int tp4 = 0;

        //TELEPORT LOCATIONS//

        //*DM CHORDS*//
        public static Vector3 player1 = Vector3.Zero;
        public static Vector3 player2 = Vector3.Zero;
        public static Vector3 player3 = Vector3.Zero;
        public static Vector3 player4 = Vector3.Zero;
        public static Vector3 spawnpos = Vector3.Zero;
        public static Vector3 mbspawn = Vector3.Zero;
        public static Vector3 mbnacht = Vector3.Zero;
        public static Vector3 mbairplane = Vector3.Zero;
        public static Vector3 mbswamp = Vector3.Zero;
        public static Vector3 mbpr = Vector3.Zero;
        public static Vector3 mbl1 = Vector3.Zero;
        public static Vector3 mbl2 = Vector3.Zero;
        public static Vector3 spawn = Vector3.Zero;
        public static Vector3 airplane = Vector3.Zero;
        public static Vector3 snipers = Vector3.Zero;
        public static Vector3 swamp = Vector3.Zero;
        public static Vector3 pap = Vector3.Zero;
        public static Vector3 powerroom = Vector3.Zero;
        public static Vector3 jails = Vector3.Zero;
    }
}
