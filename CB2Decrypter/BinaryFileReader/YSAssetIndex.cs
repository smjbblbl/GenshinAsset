using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryFileReader
{
	// Token: 0x020000CE RID: 206
	public static class YSAssetIndex
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x0002FCFC File Offset: 0x0002DEFC
		public static Dictionary<int, List<int>> Read(string filePath)
		{
			YSAssetIndex._blkInfo.Clear();
			byte[] array = FileUtils.ReadAllBytes(filePath);
			if (array == null || array.Length < 255)
			{
				return YSAssetIndex._blkInfo;
			}
			Dictionary<int, List<int>> blkInfo;
			using (BinaryStream binaryStream = new BinaryStream(array, BinaryAccess.ReadWrite, null))
			{
				binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				int num = 0;
				for (;;)
				{
					int num2 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					if (num2 > 255)
					{
						break;
					}
					binaryStream.ReadString(num2, null);
					num++;
				}
				binaryStream.Seek(binaryStream.Position - 4L, SeekOrigin.Begin);
				int num3 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int i = 0; i < num3; i++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int count = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					binaryStream.ReadString(count, null);
				}
				int num4 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int j = 0; j < num4; j++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int fileID = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int offset = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int num5 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					for (int k = 0; k < num5; k++)
					{
                        binaryStream.ReadInt32(BinaryEndian.LittleEndian);

                    }
					YSAssetIndex.AddBLKData(fileID, offset);
				}
				int num6 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int l = 0; l < num6; l++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				}
				int num7 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int m = 0; m < num7; m++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				}
				binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
				int num8 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int n = 0; n < num8; n++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				}
				for (int num9 = 0; num9 < 13; num9++)
				{
					binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int num10 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					for (int num11 = 0; num11 < num10; num11++)
					{
						binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					}
				}
				int num12 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
				for (int num13 = 0; num13 < num12; num13++)
				{
					int fileID2 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					int num14 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
					for (int num15 = 0; num15 < num14; num15++)
					{
						binaryStream.ReadInt32(BinaryEndian.LittleEndian);
						int offset2 = binaryStream.ReadInt32(BinaryEndian.LittleEndian);
						YSAssetIndex.AddBLKData(fileID2, offset2);
					}
				}
				blkInfo = YSAssetIndex._blkInfo;
			}
			return blkInfo;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002FF48 File Offset: 0x0002E148
		private static void AddBLKData(int fileID, int offset)
		{
			List<int> list;
			if (YSAssetIndex._blkInfo.TryGetValue(fileID, out list))
			{
				if (!list.Contains(offset))
				{
					YSAssetIndex._blkInfo[fileID].Add(offset);
					return;
				}
			}
			else
			{
				list = new List<int>();
				list.Add(offset);
				YSAssetIndex._blkInfo.Add(fileID, list);
			}
		}

		// Token: 0x0400057E RID: 1406
		private static Dictionary<int, List<int>> _blkInfo = new Dictionary<int, List<int>>();
	}
}
