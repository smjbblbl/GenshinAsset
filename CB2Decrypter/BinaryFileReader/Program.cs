using System;
using System.Threading.Tasks;

namespace BinaryFileReader
{
	// Token: 0x020000E8 RID: 232
	internal static class Program
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x0000FE62 File Offset: 0x0000E062
		[STAThread]
		private static void Main()
        {
            string cb2 = "Z:\\YuanShen_Data\\StreamingAssets\\AssetBundles";
            string dev12 = "D:\\Works\\GenshinDev\\GenshinImpact_Data\\StreamingAssets\\AssetBundles";
            string dev15 = "D:\\Works\\GenshinDevelopment\\GenshinImpact_Data\\StreamingAssets\\AssetBundles";
            YSExport.Extract(dev15);
        }
	}
}
