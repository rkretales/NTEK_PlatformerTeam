using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lowscope.Saving.Data;
using Lowscope.Saving.Enums;
using Lowscope.Saving.Encryption;
using System;
using System.Text;
using Lowscope.Saving.Utils;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace Lowscope.Saving.Core
{
	public class SaveFileUtility
	{
		// Required if you use no domain reloading.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ResetVariable()
		{
			encryption = null;
		}

		// Saving with WebGL requires a seperate DLL, which is included in the plugin.
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        [DllImport("__Internal")]
        private static extern void WindowAlert(string message);
#endif

		public static string fileExtentionName
		{
			get { return SaveSettings.Get().fileExtensionName; }
		}

		public static string gameFileName
		{
			get { return SaveSettings.Get().fileName; }
		}

		public static string metaExtentionName
		{
			get { return SaveSettings.Get().metaDataExtentionName; }
		}

		private static bool debugMode
		{
			get { return SaveSettings.Get().showSaveFileUtilityLog; }
		}

		private static string backupExtensionName
		{
			get { return SaveSettings.Get().backupExtensionName; }
		}

		private static bool ArchiveSaveFileOnFullCorruption
		{
			get { return SaveSettings.Get().archiveSaveFileOnFullCorruption; }
		}

		private static AESEncryption encryption;

		/// <summary>
		/// Directory which you can find the save folder in.
		/// </summary>
		private static string saveDirectoryPath
		{
			get
			{
				return SaveSettings.Get().saveDirectory == SaveSettings.SaveDirectory.UnityPersistentDataDirectory
					? Application.persistentDataPath
					: Directory.GetParent(Application.dataPath).ToString();
			}
		}

		public static string TempFolderPath
		{
			get { return Path.Combine(saveDirectoryPath, SaveSettings.Get().temporaryFolderName); }
		}

		/// <summary>
		/// Path to the save folder itsself.
		/// </summary>
		public static string SaveFolderPath
		{
			get
			{
				return string.Format("{0}/{1}",
					saveDirectoryPath,
					SaveSettings.Get().fileFolderName);
			}
		}

		private static void Log(string text)
		{
			if (debugMode)
			{
				Debug.Log(text);
			}
		}

		private static Dictionary<int, string> cachedSlotSaveFileNames;

		public static Dictionary<int, string> ObtainSlotSaveFileNames()
		{
			if (cachedSlotSaveFileNames != null)
				return cachedSlotSaveFileNames;

			Dictionary<int, string> slotSavePaths = new Dictionary<int, string>();

			// Create a directory if it doesn't exist yet
			if (!Directory.Exists(SaveFolderPath))
				Directory.CreateDirectory(SaveFolderPath);

			string[] filePaths = Directory.GetFiles(SaveFolderPath);

			string[] savePaths = filePaths.Where(path => path.EndsWith(fileExtentionName)).ToArray();

			int pathCount = savePaths.Length;

			for (int i = 0; i < pathCount; i++)
			{
				Log(string.Format("Found slot save file at: {0}", savePaths[i]));

				int getSlotNumber;

				string slotName = savePaths[i].Substring(SaveFolderPath.Length + gameFileName.Length + 1);

				if (int.TryParse(slotName.Substring(0, slotName.LastIndexOf(".")), out getSlotNumber))
				{
					string name = string.Format("{0}{1}", gameFileName, slotName);
					slotSavePaths.Add(getSlotNumber, name);
				}
			}

			cachedSlotSaveFileNames = slotSavePaths;

			return slotSavePaths;
		}

		public static string GetSaveFilePath(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				return "";

			return string.Format("{0}{1}", Path.Combine(SaveFolderPath, fileName), fileExtentionName);
		}

		public static void EncryptSaveFile(string savePath)
		{
			if (encryption == null)
			{
				encryption = new AESEncryption(SaveSettings.Get().encryptionKey, SaveSettings.Get().encryptionIV);
			}

			try
			{
				string data = encryption.Encrypt(Convert.ToBase64String(File.ReadAllBytes(savePath)));
				File.WriteAllBytes(savePath, Convert.FromBase64String(data));
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
		}

		public static void DecryptSaveFile(string savePath)
		{
			if (encryption == null)
			{
				encryption = new AESEncryption(SaveSettings.Get().encryptionKey, SaveSettings.Get().encryptionIV);
			}

			try
			{
				string data = encryption.Decrypt(Convert.ToBase64String(File.ReadAllBytes(savePath)));
				File.WriteAllBytes(savePath, Convert.FromBase64String(data));
			}
			catch (Exception e)
			{
				Debug.Log(string.Format("Decrypt failed. File is potentially already unencrypted. Error: {0}", e));
			}
		}

		private static bool GetFileStorageType(string savePath, out StorageType storageType)
		{
			byte[] bytes = new byte[17];
			using (FileStream fs =
			       new FileStream(savePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				fs.Read(bytes, 0, 16);
			}

			string chkStr = Encoding.ASCII.GetString(bytes);

			if (chkStr.Contains("SQLite format"))
			{
				storageType = StorageType.SQLiteExperimental;
				return true;
			}
			else if (chkStr.Contains("binary"))
			{
				storageType = StorageType.Binary;
				return true;
			}
			else if (chkStr.Contains('{'))
			{
				storageType = StorageType.JSON;
				return true;
			}

			storageType = StorageType.Binary;

			return false;
		}

		internal static SaveGame CreateSaveGameInstance(StorageType storageType)
		{
			switch (storageType)
			{
				case StorageType.JSON: return new SaveGameJSON();
				case StorageType.Binary: return new SaveGameBinary();
				case StorageType.SQLiteExperimental: return new SaveGameSqlite();
			}

			return null;
		}

		private static SaveGame LoadSave(string filePath, string fileName, int slot = -1,
			bool loadEmptyIfCorrupt = false)
		{
			SaveGame getSave = null;

			StorageType storageType = SaveSettings.Get().storageType;
			SaveFileValidation validation = SaveSettings.Get().fileValidation;
			EncryptionType encryptionType = SaveSettings.Get().encryptionType;

			bool doesFileExist = Directory.Exists(SaveFolderPath) && File.Exists(filePath);

			Log(string.Format("Loading save file: {0}", filePath));

			if (doesFileExist)
			{
				if (encryptionType == EncryptionType.AES && storageType != StorageType.SQLiteExperimental)
				{
					DecryptSaveFile(filePath);
				}

				if (validation != SaveFileValidation.DontCheck)
				{
					StorageType getType;
					if (GetFileStorageType(filePath, out getType) && getType != storageType)
					{
						switch (validation)
						{
							case SaveFileValidation.GiveError:

								Debug.Log(string.Format("Storage type of file: ({0}) " +
								                        "did not match file from settings: ({1}). Click to read more. \n" +
								                        "Set the File Validation mode in settings to convert it," +
								                        " or change the storage type to matching type.", getType,
									storageType));

								if (!loadEmptyIfCorrupt)
									return null;

								break;
							case SaveFileValidation.ConvertToType:

								if (getType != storageType)
								{
									Log(string.Format("Converting save file from type: {0} to type: {1}", getType,
										storageType));

									getSave = CreateSaveGameInstance(getType);

									if (!getSave.ReadSaveFromPath(filePath))
									{
										if (!loadEmptyIfCorrupt)
											return null;
									}
									else
									{
										getSave = (getSave as IConvertSaveGame).ConvertTo(storageType, filePath);

										if (getSave == null)
										{
											Log(string.Format("No converter available for type: {0} to type {1}",
												getType, storageType));

											if (!loadEmptyIfCorrupt)
												return null;
										}
									}
								}

								break;
							case SaveFileValidation.Replace:

								getSave = CreateSaveGameInstance(storageType);

								if (!getSave.ReadSaveFromPath(filePath))
								{
									if (!loadEmptyIfCorrupt)
										return null;
								}
								else
								{
									if (slot != -1)
									{
										WriteSave(getSave, slot);
									}

									return getSave;
								}

								break;
						}
					}
					else
					{
						Log("Failed to get storage type of save file. " +
						    "Will attempt to load it anyway.");
					}
				}
			}

			if (getSave == null)
			{
				getSave = CreateSaveGameInstance(storageType);

				if (doesFileExist)
				{
					bool tryLoad = getSave.ReadSaveFromPath(filePath);

					if (!tryLoad)
					{
						Log(string.Format("File corrupted {0}", filePath));

						if (!loadEmptyIfCorrupt)
							return null;
					}
				}
			}

			getSave.SetFileName(Path.GetFileNameWithoutExtension(fileName));

			if (doesFileExist && encryptionType == EncryptionType.AES && storageType != StorageType.SQLiteExperimental)
			{
				EncryptSaveFile(filePath);
			}

			getSave.OnAfterLoad();
			return getSave;
		}

		public static int[] GetUsedSlots()
		{
			int[] saves = new int[ObtainSlotSaveFileNames().Count];

			int counter = 0;

			foreach (int item in ObtainSlotSaveFileNames().Keys)
			{
				saves[counter] = item;
				counter++;
			}

			return saves;
		}

		public static int GetSaveSlotCount()
		{
			return ObtainSlotSaveFileNames().Count;
		}

		public static SaveGame LoadSave(int slot, bool createIfEmpty = false, Action isCorrupted = null, bool overwriteIfCorrupt = false)
		{
			if (slot < 0 && slot != -2)
			{
				Debug.LogWarning("Attempted to load negative slot");
				return null;
			}

			SaveMaster.OnLoadingFromDiskBegin(slot);

#if UNITY_WEBGL && !UNITY_EDITOR
            SyncFiles();
#endif

			string fileName;

			if (ObtainSlotSaveFileNames().TryGetValue(slot, out fileName))
			{
				SaveGame saveGame;

				string savePath = Path.Combine(SaveFolderPath, fileName);

				string fileWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
				string metaPath = Path.Combine(SaveFolderPath,
					string.Format("{0}{1}", fileWithoutExtension, metaExtentionName));
				string altPath = FileUtilities.GetAlternativeFilePath(savePath, backupExtensionName);
				string newestPath = FileUtilities.GetNewestFilePath(savePath, altPath);

				// If null or empty. Means there is no file. So it will just create a new file
				if (string.IsNullOrEmpty(newestPath))
				{
					saveGame = LoadSave(savePath, fileName, slot, true);
				}
				else
				{
					// Try to load newest path first
					saveGame = LoadSave(newestPath, fileName, slot);
					string usedPath = newestPath;
					string otherPath = newestPath == savePath ? altPath : savePath;

					bool secondSaveExists = File.Exists(otherPath);

					bool archiveNewestPath = false;
					bool archiveOtherPath = false;
					
					if (saveGame == null)
						archiveNewestPath = true;

					// If that fails, try to load the other save file
					if (saveGame == null && secondSaveExists)
					{
						saveGame = LoadSave(otherPath, fileName, slot);
						usedPath = otherPath;

						// If other save is also corrupt
						if (saveGame == null)
							archiveOtherPath = true;
					}

					bool bothSavesCorrupt = archiveNewestPath && archiveOtherPath;
					
					if (bothSavesCorrupt)
					{
						if (ArchiveSaveFileOnFullCorruption)
						{
							Log(string.Format("Archiving corrupted file: {0}", newestPath));
							Log(string.Format("Archiving corrupted file: {0}", otherPath));

							FileUtilities.ArchiveFileAsCorrupted(newestPath);
							FileUtilities.ArchiveFileAsCorrupted(otherPath);
						}
					}
					else
					{
						if (secondSaveExists || ArchiveSaveFileOnFullCorruption)
						{
							if (archiveNewestPath)
							{
								Log(string.Format("Archiving corrupted file: {0}", newestPath));
								FileUtilities.ArchiveFileAsCorrupted(newestPath);
							}

							if (archiveOtherPath)
							{
								Log(string.Format("Archiving corrupted file: {0}", otherPath));
								FileUtilities.ArchiveFileAsCorrupted(otherPath);
							}
						}

						// In case the main file has been archived. The backup file gets copied to the
						// Regular file to keep it listed.
						if ((archiveNewestPath || archiveOtherPath) && Path.GetExtension(usedPath) != fileExtentionName)
						{
							File.Copy(usedPath, usedPath == savePath ? altPath : savePath);
						}
					}
				}

				if (saveGame == null)
				{
					Log(string.Format("Save file(s) are corrupted at slot: {0}", slot));

					if (ArchiveSaveFileOnFullCorruption)
					{
						ObtainSlotSaveFileNames().Remove(slot);
						
						// Also archive meta files. Not necessarily corrupt.
						FileUtilities.ArchiveFileAsCorrupted(metaPath);
						FileUtilities.ArchiveFileAsCorrupted(string.Format("{0}{1}", metaPath, backupExtensionName));
					}

					if (isCorrupted != null)
						isCorrupted();

					return !overwriteIfCorrupt ? null : LoadSave(savePath, fileName, slot, true);
				}

				Log(string.Format("Successful load at slot: {0}", slot));

				return saveGame;
			}

			if (!createIfEmpty)
			{
				Log(string.Format("Could not load game at slot {0}", slot));
			}
			else
			{
				Log(string.Format("Creating save at slot {0}", slot));

				SaveGame saveGame = null;

				switch (SaveSettings.Get().storageType)
				{
					case StorageType.JSON:
						saveGame = new SaveGameJSON();
						break;
					case StorageType.Binary:
						saveGame = new SaveGameBinary();
						break;
					case StorageType.SQLiteExperimental:
						saveGame = new SaveGameSqlite();
						break;
				}

				return saveGame;
			}

			return null;
		}

		public static void WriteSave(SaveGame saveGame, int saveSlot = -1, string fileName = "")
		{
			string savePath = "";

			if (saveSlot != -1)
			{
				savePath = string.Format("{0}/{1}{2}{3}", SaveFolderPath, gameFileName, saveSlot.ToString(),
					fileExtentionName);
			}
			else
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					savePath = string.Format("{0}/{1}{2}", SaveFolderPath, fileName, fileExtentionName);
				}
				else
				{
					Debug.LogError("Specified file name is empty");
					return;
				}
			}

			// Check if we can save the file to the destination.
			var directoryName = Path.GetDirectoryName(savePath);
			if (!Directory.Exists(directoryName))
				Directory.CreateDirectory(directoryName);

			Dictionary<int, string> getFileNames = ObtainSlotSaveFileNames();
			if (!getFileNames.ContainsKey(saveSlot))
				getFileNames.Add(saveSlot, savePath);

			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(savePath);

			string altPath = FileUtilities.GetAlternativeFilePath(savePath, backupExtensionName);
			savePath = FileUtilities.GetOldestFilePath(savePath, altPath);

			StorageType storageType = SaveSettings.Get().storageType;

			Log(string.Format("Saving game slot {0} to : {1}. Using storage type: {2}",
				saveSlot.ToString(), savePath, storageType));

			saveGame.SetFileName(fileNameWithoutExtension);

			saveGame.OnBeforeWrite();

			saveGame.WriteSaveFile(saveGame, savePath);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
#endif

			if (SaveSettings.Get().encryptionType == EncryptionType.AES &&
			    storageType != StorageType.SQLiteExperimental)
			{
				EncryptSaveFile(savePath);
			}
		}

		public static void DeleteSave(int slot)
		{
			string filePath = string.Format("{0}/{1}{2}{3}", SaveFolderPath, gameFileName, slot, fileExtentionName);
			string metaDataFilePath =
				string.Format("{0}/{1}{2}{3}", SaveFolderPath, gameFileName, slot, metaExtentionName);

			string alternativeFilePath = FileUtilities.GetAlternativeFilePath(filePath, backupExtensionName);
			string alternativeMetaPath = FileUtilities.GetAlternativeFilePath(metaDataFilePath, backupExtensionName);

			File.Delete(alternativeFilePath);
			File.Delete(alternativeMetaPath);

			File.Delete(filePath);
			File.Delete(metaDataFilePath);

			if (ObtainSlotSaveFileNames().ContainsKey(slot))
				ObtainSlotSaveFileNames().Remove(slot);

			Log(string.Format("Removed file at {0}", filePath));

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
#endif
		}

		public static void DeleteAllSaveFiles()
		{
			if (!Directory.Exists(SaveFolderPath))
				return;

			string[] filePaths = Directory.GetFiles(SaveFolderPath);

			foreach (string path in filePaths.Where(path => path.EndsWith(fileExtentionName)).ToArray())
				File.Delete(path);

			foreach (string path in filePaths.Where(path => path.EndsWith(metaExtentionName)).ToArray())
				File.Delete(path);

			foreach (string path in filePaths.Where(path => path.EndsWith(backupExtensionName)).ToArray())
				File.Delete(path);

			Log("Save Master: Successfully removed all save files & metadata");

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
#endif
		}

		public static bool IsSlotUsed(int index)
		{
			return ObtainSlotSaveFileNames().ContainsKey(index);
		}

		public static bool IsSaveFileNameUsed(string fileName)
		{
			string filePath = string.Format("{0}/{1}{2}", SaveFolderPath, fileName, fileExtentionName);
			return File.Exists(filePath);
		}

		public static int GetAvailableSaveSlot()
		{
			int slotCount = SaveSettings.Get().maxSaveSlotCount;

			for (int i = 0; i < slotCount; i++)
				if (!ObtainSlotSaveFileNames().ContainsKey(i))
					return i;

			return -1;
		}

		public static string ObtainSlotFileName(int slot)
		{
			string fileName = "";
			ObtainSlotSaveFileNames().TryGetValue(slot, out fileName);

			if (!string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileNameWithoutExtension(fileName);
			}
			else
			{
				return string.Format("{0}{1}", gameFileName, slot);
			}

			return fileName;
		}
	}
}