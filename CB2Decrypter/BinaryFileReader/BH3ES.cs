using System;

namespace BinaryFileReader
{
	// Token: 0x020000C1 RID: 193
	public static class BH3ES
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x0002E6B8 File Offset: 0x0002C8B8
		public static byte[] BH3ESDecrypt(byte[] keyData)
		{
			if (keyData.Length != 16)
			{
				throw new Exception("BH3ESDecrypt keyData Length is not 128bit");
			}
			byte[] array = new byte[256];
			int i = 1;
			BH3ES.TransformMatrix(ref keyData, ref array);
			BH3ES.XorMatrix(ref array);
			while (i < 10)
			{
				BH3ES.XorRoundKey(ref keyData, ref array);
				BH3ES.TransformMatrix(ref keyData, ref array);
				BH3ES.XorMatrix(ref array, i << 8);
				i++;
			}
			BH3ES.MatrixToColumnKey(ref keyData, ref array);
			BH3ES.TransformMatrix(ref keyData, ref array);
			BH3ES.XorMatrix(ref array, i << 8);
			BH3ES.MatrixToKey(ref keyData, ref array);
			return keyData;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002E744 File Offset: 0x0002C944
		private static uint GetDynamicKey()
		{
			int num = (BH3ES.DynamicKeyIndex + 1) % 624;
			uint num2 = BitConverter.ToUInt32(BH3Key.DynamicKey, num * 4);
			uint num3 = (~BitConverter.ToUInt32(BH3Key.DynamicKey, BH3ES.DynamicKeyIndex * 4) | 2147483647U) & 3477330189U;
			uint num4 = BitConverter.ToUInt32(BH3Key.DynamicKey, (BH3ES.DynamicKeyIndex + 397) % 624 * 4);
			num4 ^= ((~num2 | 2147483649U) ^ 817637106U ^ num3) >> 1;
			num4 ^= 2567483615U * (num2 & 1U);
			Buffer.BlockCopy(BitConverter.GetBytes(num4), 0, BH3Key.DynamicKey, BH3ES.DynamicKeyIndex * 4, 4);
			num4 ^= num4 >> 11;
			num4 ^= (num4 << 7 & 2636928640U);
			num4 = ((~(num4 << 15) | 272236543U) ^ ~num4);
			num4 ^= num4 >> 18;
			BH3ES.DynamicKeyIndex = num;
			return num4;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002E814 File Offset: 0x0002CA14
		private static void TransformMatrix(ref byte[] keyData, ref byte[] matrixData)
		{
			for (int i = 0; i < keyData.Length; i++)
			{
				uint num = 31U * (BH3ES.GetDynamicKey() % 31U) + 3U + (uint)((byte)(BH3ES.GetDynamicKey() & 63U));
				for (int j = 0; j < 16; j++)
				{
					matrixData[i * 16 + j] = BH3Key.MatrixKey[(int)(checked((IntPtr)(unchecked((long)(2 * j) + (long)((ulong)num)))))];
				}
				byte b = keyData[i];
				for (int k = 1; k < 16; k++)
				{
					b ^= matrixData[i * 16 + k];
				}
				matrixData[i * 16] = b;
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002E898 File Offset: 0x0002CA98
		private static void XorMatrix(ref byte[] matrixData)
		{
			for (int i = 0; i < matrixData.Length; i++)
			{
				byte[] array = matrixData;
				int num = i;
				array[num] ^= BH3Key.XorBoxKey1[i];
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002E8C8 File Offset: 0x0002CAC8
		private static void XorMatrix(ref byte[] matrixData, int offset)
		{
			for (int i = 0; i < 16; i++)
			{
				ulong num = BitConverter.ToUInt64(matrixData, i * 16);
				ulong num2 = BitConverter.ToUInt64(matrixData, i * 16 + 8);
				ulong num3 = BitConverter.ToUInt64(BH3Key.XorBoxKey2, i * 16 + offset);
				ulong num4 = BitConverter.ToUInt64(BH3Key.XorBoxKey2, i * 16 + offset + 8);
				ulong num5 = BitConverter.ToUInt64(BH3Key.XorBoxKey3, i * 16 + offset);
				ulong num6 = BitConverter.ToUInt64(BH3Key.XorBoxKey3, i * 16 + offset + 8);
				ulong value = num ^ (num3 ^ num5);
				num2 ^= (num4 ^ num6);
				Buffer.BlockCopy(BitConverter.GetBytes(value), 0, matrixData, i * 16, 8);
				Buffer.BlockCopy(BitConverter.GetBytes(num2), 0, matrixData, i * 16 + 8, 8);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002E980 File Offset: 0x0002CB80
		private static void MatrixToColumnKey(ref byte[] keyData, ref byte[] matrixData)
		{
			for (int i = 0; i < 16; i++)
			{
				byte b = 0;
				for (int j = 0; j < 16; j++)
				{
					b ^= matrixData[(int)(BH3Key.ColumnOrder[i] * 16) + j];
				}
				keyData[i] = (byte)~(BH3Key.XorColumnKey[(int)b] ^ b);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002E9CC File Offset: 0x0002CBCC
		private static void MatrixToKey(ref byte[] keyData, ref byte[] matrixData)
		{
			for (int i = 0; i < keyData.Length; i++)
			{
				byte b = 0;
				for (int j = 0; j < 16; j++)
				{
					b ^= matrixData[i * 16 + j];
				}
				keyData[i] = b;
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002EA0C File Offset: 0x0002CC0C
		private static void XorRoundKey(ref byte[] keyData, ref byte[] matrixData)
		{
			uint num = 0U;
			int num2 = 0;
			for (int i = 0; i < 16; i++)
			{
				int num3 = i % 4;
				if (num3 == 0)
				{
					num = 0U;
				}
				int num4 = 0;
				for (int j = 0; j < 16; j++)
				{
					num4 ^= (int)matrixData[(int)(BH3Key.ColumnOrder[i] * 16) + j];
				}
				num ^= BitConverter.ToUInt32(BH3Key.XorRoundKey, num3 * 1024 + 4 * num4);
				if (num3 == 3)
				{
					Buffer.BlockCopy(BitConverter.GetBytes(num), 0, keyData, num2, 4);
					num2 += 4;
				}
			}
		}

		// Token: 0x04000544 RID: 1348
		private static int DynamicKeyIndex;
	}
}
