using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.Devices;

namespace BinaryFileReader
{
	// Token: 0x02000111 RID: 273
	public static class FileUtils
	{
		// Token: 0x0600086E RID: 2158 RVA: 0x000100B2 File Offset: 0x0000E2B2
		public static byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00035D80 File Offset: 0x00033F80
		public static void SaveFile(string path, byte[] bytes)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000100BA File Offset: 0x0000E2BA
		public static void RenameFile(string fileName, string newName)
		{
			if (!Path.GetFileName(fileName).Equals(newName))
			{
				FileUtils._computer.FileSystem.RenameFile(fileName, newName);
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000100DB File Offset: 0x0000E2DB
		public static void FindFile(string dirPath, Action<FileInfo> doWork)
		{
			FileUtils.FindFile(dirPath, "*.*", doWork);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00035DAC File Offset: 0x00033FAC
		public static void FindFile(string dirPath, string searchPattern, Action<FileInfo> doWork)
		{
			foreach (FileInfo obj in new DirectoryInfo(dirPath).GetFiles(searchPattern, SearchOption.AllDirectories))
			{
				if (doWork != null)
				{
					doWork(obj);
				}
			}
		}
		public static ValueTuple<bool, byte[], long> ReadBytes(string path, int offset, int count)
		{
			ValueTuple<bool, byte[], long> result;
			using (FileStream fileStream = File.OpenRead(path))
			{
				if (fileStream.Length < (long)count)
				{
					result = new ValueTuple<bool, byte[], long>(false, null, fileStream.Length);
				}
				else
				{
					byte[] array = new byte[count];
					int num;
					while ((num = fileStream.Read(array, offset, count)) > 0)
					{
						offset += num;
						count -= num;
					}
					result = new ValueTuple<bool, byte[], long>(true, array, fileStream.Length);
				}
			}
			return result;
		}

		// Token: 0x0400081A RID: 2074
		private static Computer _computer = new Computer();
	}
}
