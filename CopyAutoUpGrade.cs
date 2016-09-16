using System;
using System.Collections;

namespace CopyAutoUpGrade
{
	/// <summary>
	/// CreateVersionFile �̊T�v�̐����ł��B
	/// </summary>
	//--------------------------------------------------------------------------
	// �C������
	//--------------------------------------------------------------------------
	// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� 
	//���L�̃G���[�Ή�
	//�@[2007/10/19 20:19:13]�t�@�C���̃R�s�[�Ɏ��s���܂���
	//�@System.IO.IOException: �v���Z�X�̓t�@�C�� "C:\Program Files\is-2\AutoUpGradeUtility.dll" ��
	//�@                       �A�N�Z�X�ł��܂���B���̃t�@�C���͕ʂ̃v���Z�X���g�p���ł��B
	//�@   at System.IO.__Error.WinIOError(Int32 errorCode, String str)
	//�@   at System.IO.File.InternalCopy(String sourceFileName, String destFileName, Boolean overwrite)
	//�@   at CopyAutoUpGrade.CopyAutoUpGrade.Main(String[] args)
	//--------------------------------------------------------------------------
	// MOD 2007.11.28 ���s�j���� �^�C���X�^���v�̕b��r�p�~
	//--------------------------------------------------------------------------
	public class CopyAutoUpGrade
	{
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
		[System.Runtime.InteropServices.DllImport("User32.dll")] 
		private static extern uint MessageBox(uint hWnd, string lpText, string lpCaption, uint uType);
		private const uint MB_OK = 0;
		private const uint MB_ICONEXCLAMATION = 0x30;
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END

		static void Main(string[] args)
		{
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� START
			string name = "";
			if(args.Length < 1)
			{
				return;
			}
			else
			{
				name = args[0];
			}
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
//			System.IO.StreamWriter fs = new System.IO.StreamWriter("./Log/CopyAutoUpGradeLog.txt", false);
			System.IO.StreamWriter fs;
			try
			{
				fs = new System.IO.StreamWriter("./Log/CopyAutoUpGradeLog.txt", false);
			}
			catch(System.IO.IOException)
			{
				MessageBox(0
					, "CopyAutoUpGrade���Q�d�N�����Ă���\��������܂�"
					, "CopyAutoUpGrade"
					, MB_OK | MB_ICONEXCLAMATION);
				return;
			}
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� END
			int iCnt = 0;
			fs_WriteLineTime(fs, name + "���J�n���܂�");
			System.Threading.Mutex mutex = new System.Threading.Mutex(false, name);
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� START
			while (mutex.WaitOne(0, false) == false)
			{
				iCnt++;
				if (iCnt > 9)
				{
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
					fs.WriteLine("[" + System.DateTime.Now.ToString() + "] 9�b�҂��܂������A�I�����܂���ł���");
					fs.Close();
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
					return;
				}
				fs_WriteLineTime(fs, name + "�̏I����҂��܂�");
				System.Threading.Thread.Sleep(1000);
			}
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� END
// MOD 2007.11.28 ���s�j���� AutoUpGrade�G���[�Ή� START
			System.GC.Collect();
// MOD 2007.11.28 ���s�j���� AutoUpGrade�G���[�Ή� END
			System.Threading.Thread.Sleep(1000);
			string curPath = System.IO.Directory.GetCurrentDirectory();
			string appPath = System.IO.Path.Combine(curPath, "APP");
			string curFile;
			string appFile;
// DEL 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
//			string curTime;
//			string appTime;
//			long   curSize = 0;
//			long   appSize = 0;
// DEL 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END

			try
			{
				curFile = System.IO.Path.Combine(curPath, "AutoUpGrade.exe");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGrade.exe");
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
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
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END

				curFile = System.IO.Path.Combine(curPath, "AutoUpGradeAdmin.exe");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGradeAdmin.exe");
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
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
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END

				curFile = System.IO.Path.Combine(curPath, "AutoUpGradeUtility.dll");
				appFile = System.IO.Path.Combine(appPath, "AutoUpGradeUtility.dll");
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
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
//							fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//						}
//					}
//					else
//					{
//						System.IO.File.Copy(appFile, curFile, true);
//						fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + appFile + "���R�s�[���܂���");
//					}
//				}
				FileCheckCopy(fs, curFile, appFile);
// MOD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
			}
			catch(Exception ex)
			{
				fs_WriteLineTime(fs, "�t�@�C���̃R�s�[�Ɏ��s���܂���");
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
				fs.WriteLine(ex.ToString());
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
			}
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
			try{
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� START
				// Mutex ���J������
				mutex.ReleaseMutex();
				mutex.Close();
// ADD 2005.05.31 ���s�j�ɉ� AutoUpGrade�I���`�F�b�N�d�l�ύX�Ή� END
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
			}
			catch(ApplicationException ex)
			{
				fs_WriteLineTime(fs, ex.Message);
			}
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
			fs_WriteLineTime(fs, name + "���I�����܂�");
			fs.Close();
		}
		
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� START
		/*********************************************************************
		 * �����t���O�o��
		 * �����FStreamWriter ���O�o�͐�Asring ���O
		 * �ߒl�F�Ȃ�
		 *
		 *********************************************************************/
		static void fs_WriteLineTime(System.IO.StreamWriter fs, string log)
		{
			fs.WriteLine("[" + System.DateTime.Now.ToString() + "]" + log);
		}

		/*********************************************************************
		 * �t�@�C���`�F�b�N�R�s�[
		 * �����FStreamWriter ���O�o�͐�Asring �R�s�[��Astring �R�s�[��
		 * �ߒl�F�Ȃ�
		 *
		 *********************************************************************/
		static void FileCheckCopy(System.IO.StreamWriter fs, string curFile, string appFile)
		{
			string curTime;
			string appTime;
			long   curSize = 0;
			long   appSize = 0;
			int    iErrCnt = 0;

			//�A�v���t�H���_�ɃR�s�[�����Ȃ���ΏI��
			if (!System.IO.File.Exists(appFile)){
				return;
			}
			//�C���X�g�[���t�H���_�ɃR�s�[�悪�Ȃ���΃R�s�[
			if (!System.IO.File.Exists(curFile))
			{
				System.IO.File.Copy(appFile, curFile, true);
				fs_WriteLineTime(fs, appFile + "���R�s�[���܂���");
			}

// MOD 2007.11.28 ���s�j���� �^�C���X�^���v�̕b��r�p�~ START
//			appTime = System.IO.File.GetLastWriteTime(appFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//			appSize = new System.IO.FileInfo(appFile).Length;
//			curTime = System.IO.File.GetLastWriteTime(curFile).ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
//			curSize = new System.IO.FileInfo(curFile).Length;
			appTime = System.IO.File.GetLastWriteTime(appFile).ToString("yyyyMMddHHmm");
			appSize = new System.IO.FileInfo(appFile).Length;
			curTime = System.IO.File.GetLastWriteTime(curFile).ToString("yyyyMMddHHmm");
			curSize = new System.IO.FileInfo(curFile).Length;
// MOD 2007.11.28 ���s�j���� �^�C���X�^���v�̕b��r�p�~ END

			//�t�@�C���X�^���v���T�C�Y���قȂ�΃R�s�[����
			if (!appTime.Equals(curTime) || appSize != curSize)
			{
				//���g���C�͂T��܂�
				while(iErrCnt < 5)
				{
					try
					{
						System.IO.File.Copy(appFile, curFile, true);
						fs_WriteLineTime(fs, appFile + "���R�s�[���܂���");
						break;
					}
					catch(System.IO.IOException ex)
					{
						iErrCnt++;
						if(iErrCnt >= 5)
						{
							fs_WriteLineTime(fs, "�t�@�C���̃R�s�[�Ɏ��s���܂���");
							fs.WriteLine(ex.ToString());
							break;
						}
						System.Threading.Thread.Sleep(1000); // �P�b
						fs_WriteLineTime(fs,"�Ď��s(" + iErrCnt + ")");
					}
				}
			}
		}
// ADD 2007.10.25 ���s�j���� AutoUpGrade�G���[�Ή� END
	}
}
