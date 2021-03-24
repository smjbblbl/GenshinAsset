using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryFileReader
{
	// Token: 0x020000DB RID: 219
	public class BinaryStream : IDisposable
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x0000F93C File Offset: 0x0000DB3C
		public BinaryStream(BinaryAccess streamModel = BinaryAccess.ReadWrite, Encoding encoding = null) : this(new MemoryStream(), streamModel, encoding)
		{
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000F94B File Offset: 0x0000DB4B
		public BinaryStream(int capacity, BinaryAccess streamModel = BinaryAccess.ReadWrite, Encoding encoding = null) : this(new MemoryStream(capacity), streamModel, encoding)
		{
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000F95B File Offset: 0x0000DB5B
		public BinaryStream(byte[] buffer, BinaryAccess streamModel = BinaryAccess.ReadWrite, Encoding encoding = null) : this(new MemoryStream(buffer), streamModel, encoding)
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0003118C File Offset: 0x0002F38C
		public BinaryStream(Stream stream, BinaryAccess streamModel = BinaryAccess.ReadWrite, Encoding encoding = null)
		{
			this._stream = stream;
			this._encoding = (encoding ?? this._encoding);
			this._reader = ((streamModel != BinaryAccess.Write) ? new BinaryReader(this._stream, this._encoding) : null);
			this._writer = ((streamModel != BinaryAccess.Read) ? new BinaryWriter(this._stream, this._encoding) : null);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0000F96B File Offset: 0x0000DB6B
		public long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0000F978 File Offset: 0x0000DB78
		public long Position
		{
			get
			{
				return this._stream.Position;
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000F985 File Offset: 0x0000DB85
		public long Seek(long offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			return this._stream.Seek(offset, seekOrigin);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00031200 File Offset: 0x0002F400
		public byte[] ToArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[4096];
				int count;
				while ((count = this._stream.Read(array, 0, array.Length)) != 0)
				{
					memoryStream.Write(array, 0, count);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000F994 File Offset: 0x0000DB94
		public bool ReadBoolean()
		{
			return this._reader.ReadBoolean();
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000F9A1 File Offset: 0x0000DBA1
		public byte ReadByte()
		{
			return this._reader.ReadByte();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000F9AE File Offset: 0x0000DBAE
		public sbyte ReadSByte()
		{
			return this._reader.ReadSByte();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000F9BB File Offset: 0x0000DBBB
		public float ReadSingle()
		{
			return this._reader.ReadSingle();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
		public double ReadDouble()
		{
			return this._reader.ReadDouble();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000F9D5 File Offset: 0x0000DBD5
		public decimal ReadDecimal()
		{
			return this._reader.ReadDecimal();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0000F9E2 File Offset: 0x0000DBE2
		public short ReadInt16(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadInt16();
			}
			return BinaryStream.Endian(this._reader.ReadInt16());
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000FA04 File Offset: 0x0000DC04
		public ushort ReadUInt16(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadUInt16();
			}
			return BinaryStream.Endian(this._reader.ReadUInt16());
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000FA26 File Offset: 0x0000DC26
		public int ReadInt32(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadInt32();
			}
			return BinaryStream.Endian(this._reader.ReadInt32());
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public uint ReadUInt32(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadUInt32();
			}
			return BinaryStream.Endian(this._reader.ReadUInt32());
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000FA6A File Offset: 0x0000DC6A
		public long ReadInt64(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadInt64();
			}
			return BinaryStream.Endian(this._reader.ReadInt64());
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		public ulong ReadUInt64(BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			if (endian != BinaryEndian.BigEndian)
			{
				return this._reader.ReadUInt64();
			}
			return BinaryStream.Endian(this._reader.ReadUInt64());
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000FAAE File Offset: 0x0000DCAE
		public string ReadString(int count, Encoding encoding = null)
		{
			return (encoding ?? this._encoding).GetString(this.ReadBytes(count));
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00031270 File Offset: 0x0002F470
		public string ReadStringToNull(Encoding encoding = null)
		{
			List<byte> list = new List<byte>();
			int num = 0;
			while (this.Position != this.Length && num < 2147483647)
			{
				byte b = this.ReadByte();
				if (b == 0)
				{
					break;
				}
				list.Add(b);
				num++;
			}
			return (encoding ?? this._encoding).GetString(list.ToArray());
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000FAC7 File Offset: 0x0000DCC7
		public byte[] ReadBytes()
		{
			return this._reader.ReadBytes((int)(this.Length - this.Position));
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000FAE2 File Offset: 0x0000DCE2
		public byte[] ReadBytes(int count)
		{
			return this._reader.ReadBytes(count);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		public byte[] ReadBytes(int offset, int count)
		{
			this.Seek((long)offset, SeekOrigin.Begin);
			return this._reader.ReadBytes(count);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public void WriteByte(byte value)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0000FB16 File Offset: 0x0000DD16
		public void WriteSByte(sbyte value)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0000FB24 File Offset: 0x0000DD24
		public void WriteSingle(float value)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0000FB32 File Offset: 0x0000DD32
		public void WriteDouble(double value)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000FB40 File Offset: 0x0000DD40
		public void WriteDecimal(decimal value)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0000FB4E File Offset: 0x0000DD4E
		public void WriteInt16(short value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public void WriteUInt16(ushort value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0000FB82 File Offset: 0x0000DD82
		public void WriteInt32(int value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public void WriteUInt32(uint value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0000FBB6 File Offset: 0x0000DDB6
		public void WriteInt64(long value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		public void WriteUInt64(ulong value, BinaryEndian endian = BinaryEndian.LittleEndian)
		{
			this._writer.Write((endian == BinaryEndian.BigEndian) ? BinaryStream.Endian(value) : value);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000FBEA File Offset: 0x0000DDEA
		public void WriteString(string value, Encoding encoding = null)
		{
			this._writer.Write((encoding ?? this._encoding).GetBytes(value));
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public void WriteStringWithNull(string value, Encoding encoding = null)
		{
			this._writer.Write((encoding ?? this._encoding).GetBytes(value + "\0"));
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000FC30 File Offset: 0x0000DE30
		public void WriteBytes(byte[] buffer)
		{
			this._writer.Write(buffer);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000FC3E File Offset: 0x0000DE3E
		public void WriteBytes(byte[] buffer, int index, int count)
		{
			this._writer.Write(buffer, index, count);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000FC4E File Offset: 0x0000DE4E
		public static short Endian(short n)
		{
			return (short)((int)(n & 255) << 8 | (n >> 8 & 255));
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0000FC64 File Offset: 0x0000DE64
		public static ushort Endian(ushort n)
		{
			return (ushort)((int)(n & 255) << 8 | (n >> 8 & 255));
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000FC7A File Offset: 0x0000DE7A
		public static int Endian(int n)
		{
			return ((int)BinaryStream.Endian((short)n) & 65535) << 16 | ((int)BinaryStream.Endian((short)(n >> 16)) & 65535);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000FC9D File Offset: 0x0000DE9D
		public static uint Endian(uint n)
		{
			return (uint)((int)(BinaryStream.Endian((ushort)n) & ushort.MaxValue) << 16 | (int)(BinaryStream.Endian((ushort)(n >> 16)) & ushort.MaxValue));
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000FCC0 File Offset: 0x0000DEC0
		public static long Endian(long n)
		{
			return ((long)BinaryStream.Endian((int)n) & (long)-1) << 32 | ((long)BinaryStream.Endian((int)(n >> 32)) & (long)-1);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000FCDF File Offset: 0x0000DEDF
		public static ulong Endian(ulong n)
		{
			return ((ulong)BinaryStream.Endian((uint)n) & 0xFFFFFFFFFFFFFFFF) << 32 | ((ulong)BinaryStream.Endian((uint)(n >> 32)) & 0xFFFFFFFFFFFFFFFF);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000312C8 File Offset: 0x0002F4C8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					BinaryReader reader = this._reader;
					if (reader != null)
					{
						reader.Close();
					}
					BinaryWriter writer = this._writer;
					if (writer != null)
					{
						writer.Close();
					}
				}
				this._reader = null;
				this._writer = null;
				this._stream = null;
				this.disposedValue = true;
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000FCFE File Offset: 0x0000DEFE
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x040005C3 RID: 1475
		private Stream _stream;

		// Token: 0x040005C4 RID: 1476
		private BinaryReader _reader;

		// Token: 0x040005C5 RID: 1477
		private BinaryWriter _writer;

		// Token: 0x040005C6 RID: 1478
		private Encoding _encoding = Encoding.Default;

		// Token: 0x040005C7 RID: 1479
		private bool disposedValue;
	}
}
