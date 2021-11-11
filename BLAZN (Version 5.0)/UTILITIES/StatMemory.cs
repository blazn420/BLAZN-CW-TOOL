using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BLAZN.GLOBAL;

namespace BLAZN.UTILITIES
{
	using static Variables;
    internal class statmem
    {
		public IntPtr ProcessHandle;
		
		public bool IsProcessRunning(string processName)
		{
			bool result;
			try
			{
				Process process = Process.GetProcessesByName(processName)[0];
				if (process.Handle.ToInt64() == 0)
				{
					return false;
				}
				BaseAddress = process.MainModule.BaseAddress.ToInt64();
				ProcessID = process.Id;
				ProcessHandle = process.Handle;
				result = true;
			}
			catch
			{
				BaseAddress = 0;
				ProcessID = 0;
				ProcessHandle = IntPtr.Zero;
				result = false;
			}
			return result;
		}

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, ulong lpBaseAddress, [Out] byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesRead);

		public string ReadString(ulong pAddress)
		{
			try
			{
				byte[] array = new byte[1280];
				uint num = 0;
				if (ReadProcessMemory(ProcessHandle, pAddress, array, 1280, out num))
				{
					string text = "";
					int num2 = 0;
					while (array[num2] != 0)
					{
						string str = text;
						char c = (char)array[num2];
						text = str + c.ToString();
						num2++;
					}
					return text;
				}
			}
			catch (Exception)
			{
			}
			return "";
		}

		public byte[] ReadBytes(ulong pAddress, int length)
		{
			byte[] array = new byte[length];
			uint num = 0U;
			ReadProcessMemory(ProcessHandle, pAddress, array, (uint)length, out num);
			return array;
		}

		public float ReadFloat(ulong pAddress)
		{
			try
			{
				uint num = 0;
				byte[] array = new byte[4];
				if (ReadProcessMemory(ProcessHandle, pAddress, array, 4, out num))
				{
					return BitConverter.ToSingle(array, 0);
				}
			}
			catch (Exception)
			{
			}
			return 0f;
		}

		public int ReadInt(ulong pAddress)
		{
			try
			{
				uint num = 0;
				byte[] array = new byte[4];
				if (ReadProcessMemory(ProcessHandle, pAddress, array, 4, out num))
				{
					return BitConverter.ToInt32(array, 0);
				}
			}
			catch (Exception)
			{
			}
			return 0;
		}

		public ulong ReadInt64(ulong pAddress)
		{
			try
			{
				uint num = 0;
				byte[] array = new byte[8];
				if (ReadProcessMemory(ProcessHandle, pAddress, array, 8, out num))
				{
					return (ulong)BitConverter.ToInt64(array, 0);
				}
			}
			catch (Exception)
			{
			}
			return 0L;
		}

		public ulong ReadUInt(ulong pAddress)
		{
			try
			{
				uint num = 0;
				byte[] array = new byte[8];
				if (ReadProcessMemory(ProcessHandle, pAddress, array, 8, out num))
				{
					return (ulong)BitConverter.ToUInt64(array, 0);
				}
			}
			catch (Exception)
			{
			}
			return 0;
		}

		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, ulong lpBaseAddress, byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesRead);

		public void WriteString(ulong pAddress, string pString)
		{
			try
			{
				uint num = 0;
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

		public void WriteFloat(ulong pAddress, float value)
		{
			try
			{
				uint num = 0;
				WriteProcessMemory(ProcessHandle, pAddress, BitConverter.GetBytes(value), 4, out num);
			}
			catch (Exception)
			{
			}
		}

		public void WriteInt(ulong pAddress, ulong value)
		{
			try
			{
				uint num = 0;
				WriteProcessMemory(ProcessHandle, pAddress, BitConverter.GetBytes(value), 4, out num);
			}
			catch (Exception)
			{
			}
		}

		public void WriteUInt(ulong pAddress, ulong value)
		{
			try
			{
				uint num = 0;
				WriteProcessMemory(ProcessHandle, pAddress, BitConverter.GetBytes(value), 4, out num);
			}
			catch (Exception)
			{
			}
		}
	}
}