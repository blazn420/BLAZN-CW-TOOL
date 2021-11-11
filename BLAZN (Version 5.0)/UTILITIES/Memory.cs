using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLAZN.UTILITIES
{
    public class Memory
    {

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        public const uint PROCESS_VM_READ = 0x10;
        public const uint PROCESS_VM_WRITE = 0x20;
        public const uint PROCESS_VM_OPERATION = 8;
        public const uint PAGE_READWRITE = 4;
        private Process CurProcess;
        private IntPtr ProcessHandle;
        private string ProcessName;
        private int ProcessID;
        public IntPtr BaseModule;

        public bool AttackProcess(string _ProcessName)
        {
            Process[] processesByName = Process.GetProcessesByName(_ProcessName);
            if (processesByName.Length == 0)
            {
                return false;
            }
            BaseModule = processesByName[0].MainModule.BaseAddress;
            CurProcess = processesByName[0];
            ProcessID = processesByName[0].Id;
            ProcessName = _ProcessName;
            ProcessHandle = OpenProcess(0x38, false, ProcessID);
            return (ProcessHandle != IntPtr.Zero);
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
        ~Memory()
        {
            if (ProcessHandle != IntPtr.Zero)
            {
                CloseHandle(ProcessHandle);
            }
        }

        public static IntPtr GetModuleBaseAddress(Process proc, string modName)
        {
            IntPtr addr = IntPtr.Zero;

            foreach (ProcessModule m in proc.Modules)
            {
                if (m.ModuleName == modName)
                {
                    addr = m.BaseAddress;
                    break; ;
                }
            }
            return addr;
        }

        internal static IntPtr GetBaseAddress(string ProcessName)
        {
            try
            {
                return Process.GetProcessesByName(ProcessName)[0].MainModule.BaseAddress;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        public bool IsOpen() =>
            (ProcessName != string.Empty) ? AttackProcess(ProcessName) : false;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwAccess, bool inherit, int pid);
        public float ReadFloat(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 4UL, out ptr);
            return BitConverter.ToSingle(lpBuffer, 0);
        }

        public Int32 ReadInt32(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 4UL, out ptr);
            return BitConverter.ToInt32(lpBuffer, 0);
        }

        public Int32 ReadXInt32(ulong _lpBaseAddress, byte[] array)
        {
            IntPtr ptr;
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, array, 4UL, out ptr);
            return BitConverter.ToInt32(array, 0);
        }

        public ulong ReadInt64(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[8];
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 8UL, out ptr);
            return BitConverter.ToUInt64(lpBuffer, 0);
        }

        public ulong ReadPointerInt(ulong add, ulong[] offsets, int level)
        {
            ulong num = add;
            for (int i = 0; i < level; i++)
            {
                num = ReadInt64(num) + offsets[i];
            }
            return ReadInt64(num);
        }

        public ulong GetPointer(params ulong[] args)
        {
            ulong CurrentAddr = 0x0;
            for (int i = 0; i <= args.Length - 1; i++)
            {
                if (i != args.Length - 1)
                {
                    CurrentAddr = ReadInt64(CurrentAddr + args[i]);
                }
                else
                {
                    CurrentAddr += args[i];
                }
            }
            return CurrentAddr;
        }

        public ulong GetPointerInt(ulong add, ulong[] offsets, int level)
        {
            ulong num = add;

            for (int i = 0; i < level; i++)
            {
                num = ReadInt64(num) + offsets[i];
            }
            return num;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, ulong lpBaseAddress, [In, Out] byte[] lpBuffer, ulong dwSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        protected static extern bool ReadProcessMemory2(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);

        public string ReadString(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[0x500];
            if (ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 0x500, out ptr))
            {
                string str;
                string str1 = "";
                int index = 0;
                while (true)
                {
                    if (lpBuffer[index] == 0)
                    {
                        str = str1;
                        break;
                    }
                    str1 = str1 + ((char)lpBuffer[index]).ToString();
                    index++;
                }
                return str;
            }
            return "";
        }

        public uint ReadUInt32(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[4];
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 4UL, out ptr);
            return BitConverter.ToUInt32(lpBuffer, 0);
        }

        public  byte[] ReadBytes(ulong _lpBaseAddress, int Length)
        {
            byte[] lpBuffer = new byte[Length];
            IntPtr ptr;
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 12UL, out ptr);
            return lpBuffer;
        }

        public Vectors.Vec3.Vector3 ReadVector3(ulong _lpBaseAddress)
        {
            byte[] buffer = ReadBytes(_lpBaseAddress, 0x4 * 3);
            var floatArray = new float[buffer.Length / 4];
            Buffer.BlockCopy(buffer, 0, floatArray, 0, buffer.Length);
            return new Vectors.Vec3.Vector3(floatArray[0], floatArray[1], floatArray[2]);
        }

        public ulong ReadUInt64(ulong _lpBaseAddress)
        {
            IntPtr ptr;
            byte[] lpBuffer = new byte[8];
            ReadProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, 8UL, out ptr);
            return BitConverter.ToUInt64(lpBuffer, 0);
        }

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);


        public void WriteByte(ulong _lpBaseAddress, byte _Value)
        {
            byte[] bytes = BitConverter.GetBytes((short)_Value);
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(ProcessHandle, _lpBaseAddress, bytes, (ulong)bytes.Length , out zero);
        }

        public void WriteAByte(ulong _lpBaseAddress, byte _Value)
        {
            byte[] bytes = BitConverter.GetBytes((byte)_Value);
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(ProcessHandle, _lpBaseAddress, bytes, (ulong)bytes.Length, out zero);
        }

        public void WriteBytes(ulong _lpBaseAddress, byte[] buffer)
        {
            IntPtr intPtr;
            WriteProcessMemory(ProcessHandle, _lpBaseAddress, buffer, (ulong)buffer.Length, out intPtr);
        }

        public void WriteVec3(ulong Address, Vectors.Vec3.Vector3 Value)
        {
            byte[] buffer = new byte[Vectors.Vec3.Vector3.SizeInBytes];
            Buffer.BlockCopy(BitConverter.GetBytes(Value.X), 0, buffer, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Value.Y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Value.Z), 0, buffer, 8, 4);
            WriteBytes(Address, buffer);
        }

        public void WriteFloat(ulong _lpBaseAddress, float _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteMemory(_lpBaseAddress, bytes);
        }

        public void WriteInt16(ulong _lpBaseAddress, short _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteMemory(_lpBaseAddress, bytes);
        }

        public void WriteInt32(ulong _lpBaseAddress, int _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteMemory(_lpBaseAddress, bytes);
        }

    
        public void WriteInt64(ulong _lpBaseAddress, long _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteMemory(_lpBaseAddress, bytes);
        }

        public void WriteMemory(ulong MemoryAddress, byte[] Buffer)
        {
            uint num;
            IntPtr ptr;
            VirtualProtectEx(ProcessHandle, (IntPtr)MemoryAddress, (uint)Buffer.Length, 4, out num);
            WriteProcessMemory(ProcessHandle, MemoryAddress, Buffer, (ulong)Buffer.Length, out ptr);
        }

        public void WriteNOP(ulong Address)
        {
            byte[] lpBuffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(ProcessHandle, Address, lpBuffer, (ulong)lpBuffer.Length, out zero);
        }

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, ulong lpBaseAddress, [In, Out] byte[] lpBuffer, ulong dwSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory2(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, [Out] int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory3(IntPtr hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.AsAny)] object lpBuffer, Int64 nSize, out IntPtr lpNumberOfBytesWritten);

        public void WriteString(ulong Address, string Text)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(Text);
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(ProcessHandle, Address, bytes, (ulong)ReadString(Address).Length, out zero);
        }

        public void WriteXString(ulong pAddress, string pString)
        {
            try
            {
                IntPtr num = IntPtr.Zero;
                if (WriteProcessMemory(ProcessHandle, pAddress, Encoding.UTF8.GetBytes(pString), (uint)pString.Length, out num))
                {
                    byte[] lpBuffer = new byte[1];
                    WriteProcessMemory(ProcessHandle, pAddress + (ulong)pString.Length, lpBuffer, 1, out num);
                }
            }
            catch (Exception)
            {
            }
        }

        public void WriteBool(ulong pAddress, bool value)
        {
            try
            {
                byte[] buff = new byte[] { value ? ((byte)1) : ((byte)0) };
                WriteProcessMemory(ProcessHandle, pAddress, buff, (uint)buff.Length, out IntPtr ptr);
            }
            catch (Exception)
            {
            }
        }

        public void WriteUInt32(ulong _lpBaseAddress, uint _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteMemory(_lpBaseAddress, bytes);
        }

        public void WriteXBytes(ulong _lpBaseAddress, byte[] _Value)
        {
            byte[] lpBuffer = _Value;
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(ProcessHandle, _lpBaseAddress, lpBuffer, (ulong)lpBuffer.Length, out zero);
        }
    }

    #region Vectors
    public partial class Vectors
    {
        public partial class Vec2
        {

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
#pragma warning disable CS0660 // 'Vectors.Vec2.Vector2' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // 'Vectors.Vec2.Vector2' defines operator == or operator != but does not override Object.GetHashCode()
            public struct Vector2
#pragma warning restore CS0661 // 'Vectors.Vec2.Vector2' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // 'Vectors.Vec2.Vector2' defines operator == or operator != but does not override Object.Equals(object o)
            {
                public float X;
                public float Y;

                public Vector2(float value)
                {
                    X = value;
                    Y = value;
                }

                public Vector2(float x, float y)
                {
                    X = x;
                    Y = y;
                }


                /// <summary>
                /// The size of the <see cref="Vector2"/> type, in bytes.
                /// </summary>
                public static readonly int SizeInBytes = Marshal.SizeOf<Vector2>();

                /// <summary>
                /// A <see cref="Vector2"/> with all of its components set to zero.
                /// </summary>
                public static readonly Vector2 Zero = new Vector2(0);

                /// <summary>
                /// The X unit <see cref="Vector2"/> (1, 0).
                /// </summary>
                public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

                /// <summary>
                /// The Y unit <see cref="Vector2"/> (0, 1).
                /// </summary>
                public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

                /// <summary>
                /// A <see cref="Vector2"/> with all of its components set to one.
                /// </summary>
                public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

                public bool IsZero
                {
                    get { return X == 0 && Y == 0; }
                }

                public float Length()
                {
                    return (float)Math.Sqrt((X * X) + (Y * Y));
                }

                public float LengthSquared()
                {
                    return (X * X) + (Y * Y);
                }

                public void Normalize()
                {
                    float length = Length();
                    if (length != 0)
                    {
                        float inv = 1.0f / length;
                        X *= inv;
                        Y *= inv;
                    }
                }

                public float[] ToArray()
                {
                    return new float[] { X, Y };
                }

                public static Vector2 Add(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X + right.X, left.Y + right.Y);
                }

                public static Vector2 Subtract(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X - right.X, left.Y - right.Y);
                }

                public static Vector2 Multiply(Vector2 value, float scale)
                {
                    return new Vector2(value.X * scale, value.Y * scale);
                }

                public static Vector2 Multiply(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X * right.X, left.Y * right.Y);
                }

                public static Vector2 Divide(Vector2 value, float scale)
                {
                    return new Vector2(value.X / scale, value.Y / scale);
                }

                public static Vector2 Divide(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X / right.X, left.Y / right.Y);
                }

                public static Vector2 Negate(Vector2 value)
                {
                    return new Vector2(-value.X, -value.Y);
                }

                public static Vector2 Abs(Vector2 value)
                {
                    return new Vector2(
                        value.X > 0.0f ? value.X : -value.X,
                        value.Y > 0.0f ? value.Y : -value.Y);
                }


                public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
                {
                    float x = value.X;
                    x = (x > max.X) ? max.X : x;
                    x = (x < min.X) ? min.X : x;

                    float y = value.Y;
                    y = (y > max.Y) ? max.Y : y;
                    y = (y < min.Y) ? min.Y : y;

                    return new Vector2(x, y);
                }

                public static float Distance(Vector2 value1, Vector2 value2)
                {
                    float x = value1.X - value2.X;
                    float y = value1.Y - value2.Y;

                    return (float)Math.Sqrt((x * x) + (y * y));
                }

                public static float Dot(Vector2 left, Vector2 right)
                {
                    return (left.X * right.X) + (left.Y * right.Y);
                }


                public static Vector2 operator +(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X + right.X, left.Y + right.Y);
                }

                public static Vector2 operator *(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X * right.X, left.Y * right.Y);
                }

                public static Vector2 operator +(Vector2 value)
                {
                    return value;
                }

                public static Vector2 operator -(Vector2 left, Vector2 right)
                {
                    return new Vector2(left.X - right.X, left.Y - right.Y);
                }

                public static Vector2 operator -(Vector2 value)
                {
                    return new Vector2(-value.X, -value.Y);
                }

                public static Vector2 operator *(float scale, Vector2 value)
                {
                    return new Vector2(value.X * scale, value.Y * scale);
                }

                public static Vector2 operator *(Vector2 value, float scale)
                {
                    return new Vector2(value.X * scale, value.Y * scale);
                }

                public static Vector2 operator /(Vector2 value, float scale)
                {
                    return new Vector2(value.X / scale, value.Y / scale);
                }

                public static Vector2 operator /(float scale, Vector2 value)
                {
                    return new Vector2(scale / value.X, scale / value.Y);
                }

                public static Vector2 operator /(Vector2 value, Vector2 scale)
                {
                    return new Vector2(value.X / scale.X, value.Y / scale.Y);
                }

                public static Vector2 operator +(Vector2 value, float scalar)
                {
                    return new Vector2(value.X + scalar, value.Y + scalar);
                }

                public static Vector2 operator +(float scalar, Vector2 value)
                {
                    return new Vector2(scalar + value.X, scalar + value.Y);
                }

                public static Vector2 operator -(Vector2 value, float scalar)
                {
                    return new Vector2(value.X - scalar, value.Y - scalar);
                }

                public static Vector2 operator -(float scalar, Vector2 value)
                {
                    return new Vector2(scalar - value.X, scalar - value.Y);
                }

                public static bool operator ==(Vector2 v1, Vector2 v2)
                {
                    return v1.X == v2.X && v1.Y == v2.Y;
                }

                public static bool operator !=(Vector2 v1, Vector2 v2)
                {
                    return v1.X != v2.X || v1.Y != v2.Y;
                }


                public override string ToString()
                {
                    return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X, Y);
                }
            }
        }

        public partial class Vec3
        {
            [StructLayout(LayoutKind.Sequential, Pack = 4)]
#pragma warning disable CS0661 // 'Vectors.Vec3.Vector3' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // 'Vectors.Vec3.Vector3' defines operator == or operator != but does not override Object.Equals(object o)
            public struct Vector3
#pragma warning restore CS0660 // 'Vectors.Vec3.Vector3' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // 'Vectors.Vec3.Vector3' defines operator == or operator != but does not override Object.GetHashCode()
            {
                public float X;
                public float Y;
                public float Z;

                public Vector3(float value)
                {
                    X = value;
                    Y = value;
                    Z = value;
                }

                public Vector3(float x, float y, float z)
                {
                    X = x;
                    Y = y;
                    Z = z;
                }

                [System.Runtime.CompilerServices.IndexerName("Component")]
                public unsafe float this[int index]
                {
                    get
                    {
                        if ((index | 0x3) != 0x3) //index < 0 || index > 3
                            throw new ArgumentOutOfRangeException("index");
                        fixed (float* v = &X)
                        {
                            return *(v + index);
                        }
                    }
                    set
                    {
                        if ((index | 0x3) != 0x3) //index < 0 || index > 3
                            throw new ArgumentOutOfRangeException("index");
                        fixed (float* v = &X)
                        {
                            *(v + index) = value;
                        }
                    }
                }

                /// <summary>
                /// The size of the <see cref="Vector3"/> type, in bytes.
                /// </summary>
                public static readonly int SizeInBytes = Marshal.SizeOf<Vector3>();

                /// <summary>
                /// A <see cref="Vector3"/> with all of its components set to zero.
                /// </summary>
                public static readonly Vector3 Zero = new Vector3(0);

                /// <summary>
                /// The X unit <see cref="Vector3"/> (1, 0, 0).
                /// </summary>
                public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);

                /// <summary>
                /// The Y unit <see cref="Vector3"/> (0, 1, 0).
                /// </summary>
                public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);

                /// <summary>
                /// The Z unit <see cref="Vector3"/> (0, 0, 1).
                /// </summary>
                public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);

                /// <summary>
                /// A <see cref="Vector3"/> with all of its components set to one.
                /// </summary>
                public static readonly Vector3 One = new Vector3(1.0f, 1.0f, 1.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating up (0, 1, 0).
                /// </summary>
                public static readonly Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating down (0, -1, 0).
                /// </summary>
                public static readonly Vector3 Down = new Vector3(0.0f, -1.0f, 0.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating left (-1, 0, 0).
                /// </summary>
                public static readonly Vector3 Left = new Vector3(-1.0f, 0.0f, 0.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating right (1, 0, 0).
                /// </summary>
                public static readonly Vector3 Right = new Vector3(1.0f, 0.0f, 0.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating forward in a right-handed coordinate system (0, 0, -1).
                /// </summary>
                public static readonly Vector3 ForwardRH = new Vector3(0.0f, 0.0f, -1.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating forward in a left-handed coordinate system (0, 0, 1).
                /// </summary>
                public static readonly Vector3 ForwardLH = new Vector3(0.0f, 0.0f, 1.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating backward in a right-handed coordinate system (0, 0, 1).
                /// </summary>
                public static readonly Vector3 BackwardRH = new Vector3(0.0f, 0.0f, 1.0f);

                /// <summary>
                /// A unit <see cref="Vector3"/> designating backward in a left-handed coordinate system (0, 0, -1).
                /// </summary>
                public static readonly Vector3 BackwardLH = new Vector3(0.0f, 0.0f, -1.0f);

                public bool IsZero
                {
                    get { return X == 0 && Y == 0 && Z == 0; }
                }

                public float Length()
                {
                    return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
                }

                public float LengthSquared()
                {
                    return (X * X) + (Y * Y) + (Z * Z);
                }

                public void Normalize()
                {
                    float length = Length();
                    if (length != 0)
                    {
                        float inv = 1.0f / length;
                        X *= inv;
                        Y *= inv;
                        Z *= inv;
                    }
                }

                public float[] ToArray()
                {
                    return new float[] { X, Y, Z };
                }

                public static Vector3 Add(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
                }

                public static Vector3 Subtract(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
                }

                public static Vector3 Multiply(Vector3 value, float scale)
                {
                    return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
                }

                public static Vector3 Multiply(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
                }

                public static Vector3 Divide(Vector3 value, float scale)
                {
                    return new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
                }

                public static Vector3 Divide(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
                }

                public static Vector3 Negate(Vector3 value)
                {
                    return new Vector3(-value.X, -value.Y, -value.Z);
                }

                public static Vector3 Abs(Vector3 value)
                {
                    return new Vector3(
                        value.X > 0.0f ? value.X : -value.X,
                        value.Y > 0.0f ? value.Y : -value.Y,
                        value.Z > 0.0f ? value.Z : -value.Z);
                }


                public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
                {
                    float x = value.X;
                    x = (x > max.X) ? max.X : x;
                    x = (x < min.X) ? min.X : x;

                    float y = value.Y;
                    y = (y > max.Y) ? max.Y : y;
                    y = (y < min.Y) ? min.Y : y;

                    float z = value.Z;
                    z = (z > max.Z) ? max.Z : z;
                    z = (z < min.Z) ? min.Z : z;

                    return new Vector3(x, y, z);
                }

                public static float Distance(Vector3 value1, Vector3 value2)
                {
                    float x = value1.X - value2.X;
                    float y = value1.Y - value2.Y;
                    float z = value1.Z - value2.Z;

                    return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
                }

                public static float Dot(Vector3 left, Vector3 right)
                {
                    return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
                }


                public static Vector3 operator +(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
                }

                public static Vector3 operator *(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
                }

                public static Vector3 operator +(Vector3 value)
                {
                    return value;
                }

                public static Vector3 operator -(Vector3 left, Vector3 right)
                {
                    return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
                }

                public static Vector3 operator -(Vector3 value)
                {
                    return new Vector3(-value.X, -value.Y, -value.Z);
                }

                public static Vector3 operator *(float scale, Vector3 value)
                {
                    return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
                }

                public static Vector3 operator *(Vector3 value, float scale)
                {
                    return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
                }

                public static Vector3 operator /(Vector3 value, float scale)
                {
                    return new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
                }

                public static Vector3 operator /(float scale, Vector3 value)
                {
                    return new Vector3(scale / value.X, scale / value.Y, scale / value.Z);
                }

                public static Vector3 operator /(Vector3 value, Vector3 scale)
                {
                    return new Vector3(value.X / scale.X, value.Y / scale.Y, value.Z / scale.Z);
                }

                public static Vector3 operator +(Vector3 value, float scalar)
                {
                    return new Vector3(value.X + scalar, value.Y + scalar, value.Z + scalar);
                }

                public static Vector3 operator +(float scalar, Vector3 value)
                {
                    return new Vector3(scalar + value.X, scalar + value.Y, scalar + value.Z);
                }

                public static Vector3 operator -(Vector3 value, float scalar)
                {
                    return new Vector3(value.X - scalar, value.Y - scalar, value.Z - scalar);
                }

                public static Vector3 operator -(float scalar, Vector3 value)
                {
                    return new Vector3(scalar - value.X, scalar - value.Y, scalar - value.Z);
                }

                public static bool operator ==(Vector3 v1, Vector3 v2)
                {
                    return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
                }

                public static bool operator !=(Vector3 v1, Vector3 v2)
                {
                    return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
                }


                public override string ToString()
                {
                    return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", X, Y, Z);
                }
            }
        }

        public partial class Vec4
        {
            [StructLayout(LayoutKind.Sequential, Pack = 4)]
#pragma warning disable CS0661 // 'Vectors.Vec4.Vector4' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // 'Vectors.Vec4.Vector4' defines operator == or operator != but does not override Object.Equals(object o)
            public struct Vector4
#pragma warning restore CS0660 // 'Vectors.Vec4.Vector4' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // 'Vectors.Vec4.Vector4' defines operator == or operator != but does not override Object.GetHashCode()
            {
                public float X;
                public float Y;
                public float Z;
                public float W;

                public Vector4(float value)
                {
                    X = value;
                    Y = value;
                    Z = value;
                    W = value;
                }

                public Vector4(float x, float y, float z, float w)
                {
                    X = x;
                    Y = y;
                    Z = z;
                    W = w;
                }


                /// <summary>
                /// The size of the <see cref="Vector4"/> type, in bytes.
                /// </summary>
                public static readonly int SizeInBytes = Marshal.SizeOf<Vector4>();

                /// <summary>
                /// A <see cref="Vector4"/> with all of its components set to zero.
                /// </summary>
                public static readonly Vector4 Zero = new Vector4();

                /// <summary>
                /// The X unit <see cref="Vector4"/> (1, 0, 0, 0).
                /// </summary>
                public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

                /// <summary>
                /// The Y unit <see cref="Vector4"/> (0, 1, 0, 0).
                /// </summary>
                public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

                /// <summary>
                /// The Z unit <see cref="Vector4"/> (0, 0, 1, 0).
                /// </summary>
                public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

                /// <summary>
                /// The W unit <see cref="Vector4"/> (0, 0, 0, 1).
                /// </summary>
                public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

                /// <summary>
                /// A <see cref="Vector4"/> with all of its components set to one.
                /// </summary>
                public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

                public bool IsZero
                {
                    get { return X == 0 && Y == 0 && Z == 0 && W == 0; }
                }

                public float Length()
                {
                    return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
                }

                public float LengthSquared()
                {
                    return (X * X) + (Y * Y) + (Z * Z) + (W * W);
                }

                public void Normalize()
                {
                    float length = Length();
                    if (length != 0)
                    {
                        float inv = 1.0f / length;
                        X *= inv;
                        Y *= inv;
                        Z *= inv;
                        W *= inv;
                    }
                }

                public float[] ToArray()
                {
                    return new float[] { X, Y, Z, W };
                }

                public static Vector4 Add(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
                }

                public static Vector4 Subtract(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
                }

                public static Vector4 Multiply(Vector4 value, float scale)
                {
                    return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
                }

                public static Vector4 Multiply(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
                }

                public static Vector4 Divide(Vector4 value, float scale)
                {
                    return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
                }

                public static Vector4 Divide(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
                }

                public static Vector4 Negate(Vector4 value)
                {
                    return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
                }

                public static Vector4 Abs(Vector4 value)
                {
                    return new Vector4(
                        value.X > 0.0f ? value.X : -value.X,
                        value.Y > 0.0f ? value.Y : -value.Y,
                        value.Z > 0.0f ? value.Z : -value.Z,
                        value.W > 0.0f ? value.W : -value.W);
                }


                public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
                {
                    float x = value.X;
                    x = (x > max.X) ? max.X : x;
                    x = (x < min.X) ? min.X : x;

                    float y = value.Y;
                    y = (y > max.Y) ? max.Y : y;
                    y = (y < min.Y) ? min.Y : y;

                    float z = value.Z;
                    z = (z > max.Z) ? max.Z : z;
                    z = (z < min.Z) ? min.Z : z;

                    float w = value.W;
                    w = (w > max.W) ? max.W : w;
                    w = (w < min.W) ? min.W : w;

                    return new Vector4(x, y, z, w);
                }

                public static float Distance(Vector4 value1, Vector4 value2)
                {
                    float x = value1.X - value2.X;
                    float y = value1.Y - value2.Y;
                    float z = value1.Z - value2.Z;
                    float w = value1.W - value2.W;

                    return (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
                }

                public static float Dot(Vector4 left, Vector4 right)
                {
                    return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
                }


                public static Vector4 operator +(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
                }

                public static Vector4 operator *(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
                }

                public static Vector4 operator +(Vector4 value)
                {
                    return value;
                }

                public static Vector4 operator -(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
                }

                public static Vector4 operator -(Vector4 value)
                {
                    return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
                }

                public static Vector4 operator *(float scale, Vector4 value)
                {
                    return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
                }

                public static Vector4 operator *(Vector4 value, float scale)
                {
                    return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
                }

                public static Vector4 operator /(Vector4 value, float scale)
                {
                    return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
                }

                public static Vector4 operator /(float scale, Vector4 value)
                {
                    return new Vector4(scale / value.X, scale / value.Y, scale / value.Z, scale / value.W);
                }

                public static Vector4 operator /(Vector4 value, Vector4 scale)
                {
                    return new Vector4(value.X / scale.X, value.Y / scale.Y, value.Z / scale.Z, value.W / scale.W);
                }

                public static Vector4 operator +(Vector4 value, float scalar)
                {
                    return new Vector4(value.X + scalar, value.Y + scalar, value.Z + scalar, value.W + scalar);
                }

                public static Vector4 operator +(float scalar, Vector4 value)
                {
                    return new Vector4(scalar + value.X, scalar + value.Y, scalar + value.Z, scalar + value.W);
                }

                public static Vector4 operator -(Vector4 value, float scalar)
                {
                    return new Vector4(value.X - scalar, value.Y - scalar, value.Z - scalar, value.W - scalar);
                }

                public static Vector4 operator -(float scalar, Vector4 value)
                {
                    return new Vector4(scalar - value.X, scalar - value.Y, scalar - value.Z, scalar - value.W);
                }

                public static bool operator ==(Vector4 v1, Vector4 v2)
                {
                    return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W;
                }

                public static bool operator !=(Vector4 v1, Vector4 v2)
                {
                    return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z || v1.W != v2.W;
                }


                public override string ToString()
                {
                    return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
                }
            }
        }

        public partial class View
        {

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct Matrix
            {
                public float M11;
                public float M12;
                public float M13;
                public float M14;
                public float M21;
                public float M22;
                public float M23;
                public float M24;
                public float M31;
                public float M32;
                public float M33;
                public float M34;
                public float M41;
                public float M42;
                public float M43;
                public float M44;

                public Matrix(float value)
                {
                    M11 = value;
                    M12 = value;
                    M13 = value;
                    M14 = value;
                    M21 = value;
                    M22 = value;
                    M23 = value;
                    M24 = value;
                    M31 = value;
                    M32 = value;
                    M33 = value;
                    M34 = value;
                    M41 = value;
                    M42 = value;
                    M43 = value;
                    M44 = value;
                }

                public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
                {
                    M11 = m11;
                    M12 = m12;
                    M13 = m13;
                    M14 = m14;
                    M21 = m21;
                    M22 = m22;
                    M23 = m23;
                    M24 = m24;
                    M31 = m31;
                    M32 = m32;
                    M33 = m33;
                    M34 = m34;
                    M41 = m41;
                    M42 = m42;
                    M43 = m43;
                    M44 = m44;
                }

                public static Matrix Transpose(Matrix value)
                {
                    Matrix temp = new Matrix
                    {
                        M11 = value.M11,
                        M12 = value.M21,
                        M13 = value.M31,
                        M14 = value.M41,
                        M21 = value.M12,
                        M22 = value.M22,
                        M23 = value.M32,
                        M24 = value.M42,
                        M31 = value.M13,
                        M32 = value.M23,
                        M33 = value.M33,
                        M34 = value.M43,
                        M41 = value.M14,
                        M42 = value.M24,
                        M43 = value.M34,
                        M44 = value.M44
                    };
                    return temp;
                }


                public override string ToString()
                {
                    return string.Format(CultureInfo.CurrentCulture, "M11:{0} M12:{1} M13:{2} M14:{3} M21:{4} M22:{5} M23:{6} M24:{7} M31:{8} M32:{9} M33:{10} M34:{11} M41:{12} M42:{13} M43:{14} M44:{15}", M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
                }
            }
        }


    }
    #endregion
}
