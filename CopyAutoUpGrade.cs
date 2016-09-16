using System;
using System.Collections;

namespace CopyAutoUpGrade
{
	/// <summary>
	/// CreateVersionFile の概要の説明です。
	/// </summary>
	//--------------------------------------------------------------------------
	// 修正履歴
	//--------------------------------------------------------------------------
	// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 
	//下記のエラー対応
	//　[2007/10/19 20:19:13]ファイルのコピーに失敗しました
	//　System.IO.IOException: プロセスはファイル "C:\Program Files\is-2\AutoUpGradeUtility.dll" に
	//　                       アクセスできません。このファイルは別のプロセスが使用中です。
	//　   at System.IO.__Error.WinIOError(Int32 errorCode, String str)
	//　   at System.IO.File.InternalCopy(String sourceFileName, String destFileName, Boolean overwrite)
	//　   at CopyAutoUpGrade.CopyAutoUpGrade.Main(String[] args)
	//--------------------------------------------------------------------------
	// MOD 2007.11.28 東都）高木 タイムスタンプの秒比較廃止
	//--------------------------------------------------------------------------
	public class CopyAutoUpGrade
	{
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
		[System.Runtime.InteropServices.DllImport("User32.dll")] 
		private static extern uint MessageBox(uint hWnd, string lpText, string lpCaption, uint uType);
		private const uint MB_OK = 0;
		private const uint MB_ICONEXCLAMATION = 0x30;
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END

		static void Main(string[] args)
		{
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 START
			string name = "";
			if(args.Length < 1)
			{
				return;
			}
			else
			{
				name = args[0];
			}
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
//			System.IO.StreamWriter fs = new System.IO.StreamWriter("./Log/CopyAutoUpGradeLog.txt", false);
			System.IO.StreamWriter fs;
			try
			{
				fs = new System.IO.StreamWriter("./Log/CopyAutoUpGradeLog.txt", false);
			}
			catch(System.IO.IOException)
			{
				MessageBox(0
					, "CopyAutoUpGradeが２重起動している可能性があります"
					, "CopyAutoUpGrade"
					, MB_OK | MB_ICONEXCLAMATION);
				return;
			}
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 END
			int iCnt = 0;
			fs_WriteLineTime(fs, name + "を開始します");
			System.Threading.Mutex mutex = new System.Threading.Mutex(false, name);
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 START
			while (mutex.WaitOne(0, false) == false)
			{
				iCnt++;
				if (iCnt > 9)
				{
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
					fs.WriteLine("[" + System.DateTime.Now.ToString() + "] 9秒待ちましたが、終了しませんでした");
					fs.Close();
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
					return;
				}
				fs_WriteLineTime(fs, name + "の終了を待ちます");
				System.Threading.Thread.Sleep(1000);
			}
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 END
// MOD 2007.11.28 東都）高木 AutoUpGradeエラー対応 START
			System.GC.Collect();
// MOD 2007.11.28 東都）高木 AutoUpGradeエラー対応 END
			System.Threading.Thread.Sleep(1000);
			string curPath = System.IO.Directory.GetCurrentDirectory();
			string appPath = System.IO.Path.Combine(curPath, "APP");
			string curFile;
			string appFile;
// DEL 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
//			string curTime;
//			string appTime;
//			long   curSize = 0;
//			long   appSize = 0;
// DEL 2007.10.25 東都）高木 AutoUpGradeエラー対応 END

			try
			{
				curFile = System.IO.Path.Combine(curPath, "AutoUpGrade.exe");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGrade.exe");
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
//				if (System.IO.File.Exists(appFile))
//				{
//					System.IO.FileStream appfs = new System.IO.FileStream(appFile, System.IO.FileMode.Open);
//					appTime = System.IO.File.GetLastWriteTime(appFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//					appSize = appfs.Length;
//					appfs.Close();
//					if (System.IO.File.Exists(curFile))
//					{
//						System.IO.FileStream curfs = new System.IO.FileStream(curFile, System.IO.FileMode.Open);
//						curTime = System.IO.File.GetLastWriteTime(curFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//						curSize = curfs.Length;
//						curfs.Close();
//						if (!appTime.Equals(curTime) || appSize != curSize)
//						{
//							System.IO.File.Copy(appFile, curFile, true);
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END

				curFile = System.IO.Path.Combine(curPath, "AutoUpGradeAdmin.exe");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGradeAdmin.exe");
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
//				if (System.IO.File.Exists(appFile))
//				{
//					System.IO.FileStream appfs = new System.IO.FileStream(appFile, System.IO.FileMode.Open);
//					appTime = System.IO.File.GetLastWriteTime(appFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//					appSize = appfs.Length;
//					appfs.Close();
//					if (System.IO.File.Exists(curFile))
//					{
//						System.IO.FileStream curfs = new System.IO.FileStream(curFile, System.IO.FileMode.Open);
//						curTime = System.IO.File.GetLastWriteTime(curFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//						curSize = curfs.Length;
//						curfs.Close();
//						if (!appTime.Equals(curTime) || appSize != curSize)
//						{
//							System.IO.File.Copy(appFile, curFile, true);
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END

				curFile = System.IO.Path.Combine(curPath, "AutoUpGradeUtility.dll");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGradeUtility.dll");
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
//				if (System.IO.File.Exists(appFile))
//				{
//					System.IO.FileStream appfs = new System.IO.FileStream(appFile, System.IO.FileMode.Open);
//					appTime = System.IO.File.GetLastWriteTime(appFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//					appSize = appfs.Length;
//					appfs.Close();
//					if (System.IO.File.Exists(curFile))
//					{
//						System.IO.FileStream curfs = new System.IO.FileStream(curFile, System.IO.FileMode.Open);
//						curTime = System.IO.File.GetLastWriteTime(curFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//						curSize = curfs.Length;
//						curfs.Close();
//						if (!appTime.Equals(curTime) || appSize != curSize)
//						{
//							System.IO.File.Copy(appFile, curFile, true);
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "をコピーしました");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
			}
			catch(Exception ex)
			{
				fs_WriteLineTime(fs, "ファイルのコピーに失敗しました");
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
				fs.WriteLine(ex.ToString());
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
			}
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
			try{
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 START
				// Mutex を開放する
				mutex.ReleaseMutex();
				mutex.Close();
// ADD 2005.05.31 東都）伊賀 AutoUpGrade終了チェック仕様変更対応 END
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
			}
			catch(ApplicationException ex)
			{
				fs_WriteLineTime(fs, ex.Message);
			}
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
			fs_WriteLineTime(fs, name + "を終了します");
			fs.Close();
		}
		
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 START
		/*********************************************************************
		 * 日時付ログ出力
		 * 引数：StreamWriter ログ出力先、sring ログ
		 * 戻値：なし
		 *
		 *********************************************************************/
		static void fs_WriteLineTime(System.IO.StreamWriter fs, string log)
		{
			fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + log);
		}

		/*********************************************************************
		 * ファイルチェックコピー
		 * 引数：StreamWriter ログ出力先、sring コピー先、string コピー元
		 * 戻値：なし
		 *
		 *********************************************************************/
		static void FileCheckCopy(System.IO.StreamWriter fs, string curFile, string appFile)
		{
			string curTime;
			string appTime;
			long   curSize = 0;
			long   appSize = 0;
			int    iErrCnt = 0;

			//アプリフォルダにコピー元がなければ終了
			if (!System.IO.File.Exists(appFile)){
				return;
			}
			//インストールフォルダにコピー先がなければコピー
			if (!System.IO.File.Exists(curFile))
			{
				System.IO.File.Copy(appFile, curFile, true);
				fs_WriteLineTime(fs, appFile + "をコピーしました");
			}

// MOD 2007.11.28 東都）高木 タイムスタンプの秒比較廃止 START
//			appTime = System.IO.File.GetLastWriteTime(appFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//			appSize = new System.IO.FileInfo(appFile).Length;
//			curTime = System.IO.File.GetLastWriteTime(curFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//			curSize = new System.IO.FileInfo(curFile).Length;
			appTime = System.IO.File.GetLastWriteTime(appFile).ToString("yyyyMMddHHmm");
			appSize = new System.IO.FileInfo(appFile).Length;
			curTime = System.IO.File.GetLastWriteTime(curFile).ToString("yyyyMMddHHmm");
			curSize = new System.IO.FileInfo(curFile).Length;
// MOD 2007.11.28 東都）高木 タイムスタンプの秒比較廃止 END

			//ファイルスタンプかサイズが異なればコピーする
			if (!appTime.Equals(curTime) || appSize != curSize)
			{
				//リトライは５回まで
				while(iErrCnt < 5)
				{
					try
					{
						System.IO.File.Copy(appFile, curFile, true);
						fs_WriteLineTime(fs, appFile + "をコピーしました");
						break;
					}
					catch(System.IO.IOException ex)
					{
						iErrCnt++;
						if(iErrCnt >= 5)
						{
							fs_WriteLineTime(fs, "ファイルのコピーに失敗しました");
							fs.WriteLine(ex.ToString());
							break;
						}
						System.Threading.Thread.Sleep(1000); // １秒
						fs_WriteLineTime(fs,"再実行(" + iErrCnt + ")");
					}
				}
			}
		}
// ADD 2007.10.25 東都）高木 AutoUpGradeエラー対応 END
	}
}
