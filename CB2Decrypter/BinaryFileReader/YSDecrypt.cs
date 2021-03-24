using System;
using System.IO;

namespace BinaryFileReader
{
	// Token: 0x020000CF RID: 207
	public class YSDecrypt
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0002FF98 File Offset: 0x0002E198
		public static byte[] InitKey(byte[] key, byte[] data, int size)
		{
			YSDecrypt._dynamicKeyIndex = 0;
			int num = (size >= 2048) ? 256 : (size >> 3);
			uint num2 = (uint)((num ^ 3) & num);
			byte[] array = new byte[16];
			Buffer.BlockCopy(key, 0, array, 0, 16);
			byte[] array2 = BH3ES.BH3ESDecrypt(array);
			for (int i = 0; i < 16; i++)
			{
				array2[i] ^= YSKey.XorKey[i];
			}
			ulong num3 = ulong.MaxValue;
			ulong num4 = 0UL;
			ulong num5 = 0UL;
			ulong num6 = 0UL;
			byte[] result;
			using (BinaryStream binaryStream = new BinaryStream(data, BinaryAccess.ReadWrite, null))
			{
				int num7 = 0;
				while ((long)num7 < (long)((ulong)num2))
				{
					num3 ^= binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
					num4 ^= binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
					num5 ^= binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
					num6 ^= binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
					num7 += 4;
				}
				num3 ^= (num4 ^ num5 ^ num6);
				if ((ulong)num2 != (ulong)((long)num))
				{
					long num8 = (long)num - (long)((ulong)num2);
					int num9 = 0;
					while ((long)num9 < num8)
					{
						num3 ^= binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
						num9++;
					}
				}
				ulong num10 = BitConverter.ToUInt64(array2, 0);
				ulong num11 = BitConverter.ToUInt64(array2, 8);
				using (BinaryStream binaryStream2 = YSDecrypt.InitKeyData(num10 ^ num11 ^ num3 ^ 7936877569879298522UL ^ 11567864270080435830UL))
				{
					using (BinaryStream binaryStream3 = new BinaryStream(new byte[4096], BinaryAccess.ReadWrite, null))
					{
						for (int j = 0; j < 512; j++)
						{
							ulong dynamicKey = YSDecrypt.GetDynamicKey(binaryStream2);
							binaryStream3.WriteUInt64(dynamicKey, BinaryEndian.LittleEndian);
						}
						result = binaryStream3.ToArray();
					}
				}
			}
			return result;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00030184 File Offset: 0x0002E384
		private static BinaryStream InitKeyData(ulong key)
		{
			BinaryStream binaryStream = new BinaryStream(new byte[2496], BinaryAccess.ReadWrite, null);
			binaryStream.WriteUInt64(key, BinaryEndian.LittleEndian);
			for (int i = 1; i < 312; i++)
			{
				binaryStream.Seek((long)((i - 1) * 8), SeekOrigin.Begin);
				ulong num = binaryStream.ReadUInt64(BinaryEndian.LittleEndian);
				ulong value = (ulong)((long)i + (long)(6364136223846793005UL * (num ^ num >> 62)));
				binaryStream.WriteUInt64(value, BinaryEndian.LittleEndian);
			}
			return binaryStream;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000301F0 File Offset: 0x0002E3F0
		private static ulong GetDynamicKey(BinaryStream keyStream)
		{
			if (YSDecrypt._dynamicKeyIndex % 312 == 0)
			{
				keyStream.Seek(0L, SeekOrigin.Begin);
				for (int i = 0; i < 312; i++)
				{
					if (i < 156)
					{
						ulong num = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						ulong num2 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						ulong num3 = (num ^ 2147483647UL) & num;
						ulong num4 = num2 & 2147483646UL;
						ulong num5 = (num3 & num4) | (num3 ^ num4);
						keyStream.Seek((long)((i + 156) * 8), SeekOrigin.Begin);
						ulong num6 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						num6 ^= num5 >> 1;
						ulong num7 = BitConverter.ToUInt64(YSKey.XorDataKey, (int)(num2 & 1UL) * 8);
						num7 ^= num6;
						keyStream.Seek((long)(i * 8), SeekOrigin.Begin);
						keyStream.WriteUInt64(num7, BinaryEndian.LittleEndian);
					}
					else if (i >= 156 && i < 311)
					{
						ulong num8 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						ulong num9 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						ulong num10 = (num8 ^ 2147483647UL) & num8;
						ulong num11 = num9 ^ 18446744071562067969UL;
						ulong num12 = (num11 & num10 & num9) | ((num11 & num9) ^ num10);
						keyStream.Seek((long)((i - 156) * 8), SeekOrigin.Begin);
						ulong num13 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						num13 ^= num12 >> 1;
						ulong num14 = BitConverter.ToUInt64(YSKey.XorDataKey, (int)(num9 & 1UL) * 8);
						num14 ^= num13;
						keyStream.Seek((long)(i * 8), SeekOrigin.Begin);
						keyStream.WriteUInt64(num14, BinaryEndian.LittleEndian);
					}
					else
					{
						ulong num15 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						keyStream.Seek(0L, SeekOrigin.Begin);
						ulong num16 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						ulong num17 = (num15 ^ 2147483647UL) & num15;
						ulong num18 = num16 & 2147483646UL;
						ulong num19 = (num17 & num18) | (num17 ^ num18);
						keyStream.Seek(1240L, SeekOrigin.Begin);
						ulong num20 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
						num20 ^= num19 >> 1;
						ulong num21 = BitConverter.ToUInt64(YSKey.XorDataKey, (int)(num16 & 1UL) * 8);
						num21 ^= num20;
						keyStream.Seek((long)(i * 8), SeekOrigin.Begin);
						keyStream.WriteUInt64(num21, BinaryEndian.LittleEndian);
					}
				}
				YSDecrypt._dynamicKeyIndex = 0;
			}
			keyStream.Seek((long)(YSDecrypt._dynamicKeyIndex * 8), SeekOrigin.Begin);
			ulong num22 = keyStream.ReadUInt64(BinaryEndian.LittleEndian);
			ulong num23 = num22 ^ (num22 >> 29 & 22906492245UL);
			ulong num24 = num23 ^ (num23 << 17 & 8202884508482404352UL);
			ulong num25 = num24 ^ (num24 << 37 & 18444473444759240704UL);
			ulong result = num25 ^ num25 >> 43;
			YSDecrypt._dynamicKeyIndex++;
			return result;
		}

		// Token: 0x0400057F RID: 1407
		private static int _dynamicKeyIndex;
	}
}
