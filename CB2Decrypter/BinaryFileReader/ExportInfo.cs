using System;

namespace BinaryFileReader
{
	// Token: 0x020000CB RID: 203
	public static class ExportInfo
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0000F6EF File Offset: 0x0000D8EF
		public static int SuccessCount { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0000F6FE File Offset: 0x0000D8FE
		public static int ErrorCount { get; set; }

		// Token: 0x0600072B RID: 1835 RVA: 0x0000F706 File Offset: 0x0000D906
		public static void Reset()
		{
			ExportInfo.SuccessCount = 0;
			ExportInfo.ErrorCount = 0;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000F714 File Offset: 0x0000D914
		public static void AddSuccess()
		{
			ExportInfo.SuccessCount++;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000F722 File Offset: 0x0000D922
		public static void AddError()
		{
			ExportInfo.ErrorCount++;
		}
	}
}
