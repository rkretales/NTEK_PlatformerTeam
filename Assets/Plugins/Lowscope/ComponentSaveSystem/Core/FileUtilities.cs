using System;
using System.IO;
using System.Text;

namespace Lowscope.Saving.Utils
{
	public class FileUtilities
	{
		public static string GetOldestFilePath(string pathOne, string pathTwo)
		{
			bool pathOneExists = File.Exists(pathOne);
			bool pathTwoExists = File.Exists(pathTwo);

			if (!pathOneExists)
				return pathOne;

			if (!pathTwoExists)
				return pathTwo;

			DateTime pathOneWriteTime = File.GetLastWriteTime(pathOne);
			DateTime pathTwoWriteTime = File.GetLastWriteTime(pathTwo);

			return pathOneWriteTime < pathTwoWriteTime ? pathOne : pathTwo;
		}

		public static string GetNewestFilePath(string pathOne, string pathTwo)
		{
			bool pathOneExists = File.Exists(pathOne);
			bool pathTwoExists = File.Exists(pathTwo);

			if (!pathOneExists && !pathTwoExists)
				return "";

			if (pathOneExists && !pathTwoExists)
				return pathOne;

			if (!pathOneExists)
				return pathTwo;

			DateTime pathOneWriteTime = File.GetLastWriteTime(pathOne);
			DateTime pathTwoWriteTime = File.GetLastWriteTime(pathTwo);

			return pathOneWriteTime > pathTwoWriteTime ? pathOne : pathTwo;
		}

		public static void ArchiveFileAsCorrupted(string path)
		{
			string pathDirectory = Path.GetDirectoryName(path);
			string pathFileName = Path.GetFileName(path);
			string subFolder = Path.Combine(pathDirectory, "Corrupted Saves");

			// Create a folder for the save game
			if (!Directory.Exists(subFolder))
				Directory.CreateDirectory(subFolder);

			string targetPathFileName = new StringBuilder()
				.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"))
				.Append("_")
				.Append(pathFileName).ToString();
			
			File.Move(path, Path.Combine(subFolder, targetPathFileName));
			File.Delete(path);
		}

		public static string GetAlternativeFilePath(string path, string extensionName)
		{
			return new StringBuilder()
				.Append(path)
				.Append(extensionName)
				.ToString();
		}
	}	
}