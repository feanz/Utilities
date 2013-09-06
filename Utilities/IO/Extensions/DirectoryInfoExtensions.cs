using System;
using System.IO;
using System.Linq;
using Utilities.IO.Extensions.Enums;

namespace Utilities.IO.Extensions
{
	/// <summary>
	///     Extension methods for <see cref="System.IO.DirectoryInfo" />
	/// </summary>
	public static class DirectoryInfoExtensions
	{
		/// <summary>
		///     Copies a directory to another location
		/// </summary>
		/// <param name="source"> Source directory </param>
		/// <param name="destination"> Destination directory </param>
		/// <param name="recursive"> Should the copy be recursive </param>
		/// <param name="options"> Options used in copying </param>
		/// <returns> The DirectoryInfo for the destination info </returns>
		public static DirectoryInfo CopyTo(this DirectoryInfo source, string destination, bool recursive = true,
			CopyOptions options = CopyOptions.CopyAlways)
		{
			if (source == null)
				throw new ArgumentException("source");

			if (!source.Exists)
				throw new DirectoryNotFoundException("Source directory " + source.FullName + " not found.");

			if (destination == null)
				throw new ArgumentException("destination");

			var destinationInfo = new DirectoryInfo(destination);
			destinationInfo.Create();
			foreach (var tempFile in source.EnumerateFiles())
			{
				if (options == CopyOptions.CopyAlways)
				{
					tempFile.CopyTo(Path.Combine(destinationInfo.FullName, tempFile.Name), true);
				}
				else if (options == CopyOptions.CopyIfNewer)
				{
					if (File.Exists(Path.Combine(destinationInfo.FullName, tempFile.Name)))
					{
						var FileInfo = new FileInfo(Path.Combine(destinationInfo.FullName, tempFile.Name));
						if (FileInfo.LastWriteTime.CompareTo(tempFile.LastWriteTime) < 0)
							tempFile.CopyTo(Path.Combine(destinationInfo.FullName, tempFile.Name), true);
					}
					else
					{
						tempFile.CopyTo(Path.Combine(destinationInfo.FullName, tempFile.Name), true);
					}
				}
				else if (options == CopyOptions.DoNotOverwrite)
				{
					tempFile.CopyTo(Path.Combine(destinationInfo.FullName, tempFile.Name), false);
				}
			}
			if (recursive)
			{
				foreach (var subDirectory in source.EnumerateDirectories())
					subDirectory.CopyTo(Path.Combine(destinationInfo.FullName, subDirectory.Name), true, options);
			}
			return new DirectoryInfo(destination);
		}

		/// <summary>
		///     Deletes directory and all content found within it
		/// </summary>
		/// <param name="directory"> Directory info object </param>
		public static void DeleteAll(this DirectoryInfo directory)
		{
			if (!directory.Exists)
				return;

			directory.DeleteFiles();

			foreach (var d in directory.EnumerateDirectories())
			{
				d.DeleteAll();
			}

			directory.Delete(false);
		}

		/// <summary>
		///     Deletes files from a directory
		/// </summary>
		/// <param name="directory"> Directory to delete the files from </param>
		/// <param name="recursive"> Should this be recursive? </param>
		/// <returns> The directory that is sent in </returns>
		public static DirectoryInfo DeleteFiles(this DirectoryInfo directory, bool recursive = false)
		{
			if (!directory.Exists)
				throw new DirectoryNotFoundException("Directory");

			foreach (var file in  directory.EnumerateFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
				file.Delete();
			}

			return directory;
		}

		/// <summary>
		///     Deletes files newer than the specified date
		/// </summary>
		/// <param name="directory"> Directory to look within </param>
		/// <param name="compareDate"> The date to compare to </param>
		/// <param name="recursive"> Is this a recursive call </param>
		/// <returns> Returns the directory object </returns>
		public static DirectoryInfo DeleteFilesNewerThan(this DirectoryInfo directory, DateTime compareDate,
			bool recursive = false)
		{
			if (!directory.Exists)
				throw new DirectoryNotFoundException("Directory");

			foreach (var file in directory.EnumerateFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
				.Where(x => x.LastWriteTime > compareDate))
			{
				file.Delete();
			}

			return directory;
		}

		/// <summary>
		///     Deletes files older than the specified date
		/// </summary>
		/// <param name="directory"> Directory to look within </param>
		/// <param name="compareDate"> The date to compare to </param>
		/// <param name="recursive"> Is this a recursive call </param>
		/// <returns> Returns the directory object </returns>
		public static DirectoryInfo DeleteFilesOlderThan(this DirectoryInfo directory, DateTime compareDate,
			bool recursive = false)
		{
			if (!directory.Exists)
				throw new DirectoryNotFoundException("Directory");

			foreach (var file in directory.EnumerateFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
				.Where(x => x.LastWriteTime < compareDate))
			{
				file.Delete();
			}

			return directory;
		}

		/// <summary>
		///     Sets a directory's attributes
		/// </summary>
		/// <param name="directory"> Directory </param>
		/// <param name="attributes"> Attributes to set </param>
		/// <param name="recursive"> Determines if this is a recursive call </param>
		/// <returns> The directory object </returns>
		public static DirectoryInfo SetAttributes(this DirectoryInfo directory, FileAttributes attributes,
			bool recursive = false)
		{
			foreach (var file in directory.EnumerateFiles())
			{
				file.SetAttributes(attributes);
			}

			if (recursive)
			{
				foreach (var subDirectory in directory.EnumerateDirectories())
				{
					subDirectory.SetAttributes(attributes, true);
				}
			}

			return directory;
		}

		/// <summary>
		///     Gets the size of all files within a directory
		/// </summary>
		/// <param name="directory"> Directory </param>
		/// <param name="searchPattern"> Search pattern used to tell what files to include (defaults to all) </param>
		/// <param name="recursive"> determines if this is a recursive call or not </param>
		/// <returns> The directory size </returns>
		public static long Size(this DirectoryInfo directory, string searchPattern = "*", bool recursive = false)
		{
			if (directory == null)
				throw new ArgumentException("directory");

			if (!directory.Exists)
				throw new DirectoryNotFoundException("Directory");
			return directory.EnumerateFiles(searchPattern,
				recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
				.Sum(x => x.Length);
		}
	}
}