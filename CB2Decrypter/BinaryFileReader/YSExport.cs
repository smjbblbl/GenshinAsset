using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BinaryFileReader
{
    // Token: 0x020000D0 RID: 208
    public static class YSExport
    {
        // Token: 0x06000758 RID: 1880 RVA: 0x0003045C File Offset: 0x0002E65C
        public static  void Extract(string folderPath)
        {
            YSExport.Reset();
            YSExport._folderPath = folderPath;
            FileUtils.FindFile(folderPath, "*.*", delegate (FileInfo f)
            {
                string fullName = f.FullName;
                string text = Path.GetExtension(fullName).ToLower();
                if (fullName.Contains("asset_index"))
                {
                        YSExport._blkInfo = YSAssetIndex.Read(fullName);
                        return;
                }
                if (text.Equals(".blk"))
                {
                    YSExport._unityList.Add(fullName);
                }
            });
            Console.WriteLine("Start Unpacking");
            if (YSExport._blkInfo.Count == 0)
            {
                Console.WriteLine("Error->The asset_index file was not found, please make sure that the asset_index and blocks folders are in the same directory");
                Console.WriteLine("Unpacking Failure");
            }
            else
            {
                foreach (string filePath in YSExport._unityList)
                {
                    byte[] data = FileUtils.ReadAllBytes(filePath);
                    TaskAwaiter<bool> taskAwaiter = YSExport.ExportUnityAsset(filePath, data).GetAwaiter();
                    if (!taskAwaiter.IsCompleted)
                    {
                        TaskAwaiter<bool> taskAwaiter2;
                        taskAwaiter2= taskAwaiter;
                        taskAwaiter2 = default(TaskAwaiter<bool>);
                    }
                    if (taskAwaiter.GetResult())
                    {
                        Console.WriteLine(string.Format("Export->Success {0} Files Failure {1} Files {2}", ExportInfo.SuccessCount, ExportInfo.ErrorCount, Path.GetFileName(filePath)));
                    }
                }
                List<string>.Enumerator enumerator = default(List<string>.Enumerator);
                Console.WriteLine(string.Format("Decrypt->Success {0} Files Failure {1} Files\n", ExportInfo.SuccessCount, ExportInfo.ErrorCount));
//                await Task.Delay(500);
                Console.WriteLine("Export Path->" + Path.Combine(YSExport._folderPath, "blocks_export"));
                Console.WriteLine("Decryption completed, please use AssetStudio to extract resources");
            }
        }

        // Token: 0x06000759 RID: 1881 RVA: 0x000304A4 File Offset: 0x0002E6A4
        private static async Task<bool> ExportUnityAsset(string filePath, byte[] data)
        {
            try
            {
                if (data.Length != 0)
                {
                    BinaryStream binaryStream = new BinaryStream(data, BinaryAccess.ReadWrite, null);
                    {
                        binaryStream.ReadStringToNull(null);
                        binaryStream.ReadUInt32(BinaryEndian.LittleEndian);
                        byte[] key = binaryStream.ReadBytes(32);
                        ushort size = binaryStream.ReadUInt16(BinaryEndian.LittleEndian);
                        byte[] array = binaryStream.ReadBytes();
                        byte[] array2 = YSDecrypt.InitKey(key, array, (int)size);
                        if (array2 != null)
                        {
                            int num = array2.Length;
                            for (int i = 0; i < array.Length; i++)
                            {
                                byte[] array3 = array;
                                int num2 = i;
                                array3[num2] ^= array2[i % num];
                            }
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                            string text = Path.GetDirectoryName(filePath);
                            int num3 = text.LastIndexOf("\\");
                            text = text.Substring(num3 + 1);
                            string text2 = Path.Combine(YSExport._folderPath, "blocks_export", text);
                            int key2;
                            if (int.TryParse(fileNameWithoutExtension, out key2))
                            {
                                List<int> list;
                                if (YSExport._blkInfo.TryGetValue(key2, out list) && list.Count > 1)
                                {
                                    list.Sort();
                                    for (int j = 0; j < list.Count; j++)
                                    {
                                        int num4 = list[j];
                                        int num5;
                                        if (j != list.Count - 1)
                                        {
                                            num5 = list[j + 1] - num4;
                                        }
                                        else
                                        {
                                            num5 = array.Length - num4;
                                        }
                                        byte[] DecyptedData = new byte[num5];
                                        Buffer.BlockCopy(array, num4, DecyptedData, 0, num5);
                                        FileUtils.SaveFile(Path.Combine(text2, string.Format("{0}_{1}.unity3d", fileNameWithoutExtension, num4)), DecyptedData);
                                    }
                                }
                                else
                                {
                                    text2 = Path.Combine(text2, fileNameWithoutExtension + ".unity3d");
                                    FileUtils.SaveFile(text2, array);
                                }
                                ExportInfo.AddSuccess();
                                await Task.Delay(20);
                                return true;
                            }
                            ExportInfo.AddError();
                            Console.WriteLine("\nError->" + filePath + "\n\tFailInfo->FileName Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExportInfo.AddError();
                Console.WriteLine(string.Concat(new string[]
                {
                    "\nError->",
                    filePath,
                    "\n\tFailInfo->",
                    ex.Message,
                    "\n\tStackInfo->",
                    ex.StackTrace
                }));
            }
            await Task.Delay(20);
            return false;
        }

        // Token: 0x0600075A RID: 1882 RVA: 0x0000F87C File Offset: 0x0000DA7C
        private static void Reset()
        {
            YSExport._blkInfo.Clear();
            YSExport._unityList.Clear();
            ExportInfo.Reset();
        }

        // Token: 0x04000580 RID: 1408
        private static Dictionary<int, List<int>> _blkInfo = new Dictionary<int, List<int>>();

        // Token: 0x04000581 RID: 1409
        private static List<string> _unityList = new List<string>();

        // Token: 0x04000582 RID: 1410
        private static string _folderPath = string.Empty;
    }
}
