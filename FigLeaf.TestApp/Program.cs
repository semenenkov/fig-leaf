using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FigLeaf.Core;

namespace FigLeaf.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			string source = @"d:\Patrick\FigLeaf\trunk\!_test\source\glass.jpg";
			string target = @"d:\Patrick\FigLeaf\trunk\!_test\target\glass.jp_";

			Zip.Pack(source, target, "test");

			Console.WriteLine(File.GetLastWriteTime(source).Equals(File.GetLastWriteTime(target)));

			Console.ReadLine();
			*/

			/*
			Zip.Unpack(
				@"d:\Patrick\FigLeaf\trunk\!_test\target\glass.jp_",
				@"d:\Patrick\FigLeaf\trunk\!_test\source2\glass.jpg",
				"test");
			 * */

			/*
			var th = new Thumbnail(100);
			//th.MakeForPhoto(@"d:\Patrick\FigLeaf\trunk\!_test\source\glass.jpg", @"d:\Patrick\FigLeaf\trunk\!_test\target\glass.jpg");
			th.MakeForVideo(
				@"d:\Patrick\FigLeaf\trunk\!_test\source\00002.MTS",
				@"d:\Patrick\FigLeaf\trunk\!_test\target\00002.jpg");
			 * */

			var fileProcessor = new BatchFileProcessor(new AppConfigSettings());
			//fileProcessor.Pack();
			fileProcessor.Unpack(@"d:\Patrick\FigLeaf\trunk\!_test\source2");
			Console.WriteLine("CreatedDirCount: {0}", fileProcessor.CreatedDirCount);
			Console.WriteLine("CreatedFileCount: {0}", fileProcessor.CreatedFileCount);
			Console.WriteLine("CreatedThumbnailCount: {0}", fileProcessor.CreatedThumbnailCount);
			Console.WriteLine("RemovedObsoleteFileCount: {0}", fileProcessor.RemovedObsoleteFileCount);
			Console.WriteLine("RemovedObsoleteThumbnailCount: {0}", fileProcessor.RemovedObsoleteThumbnailCount);
			Console.WriteLine("RemovedTargetWithoutSourceFileCount: {0}", fileProcessor.RemovedTargetWithoutSourceFileCount);
			Console.WriteLine("RemovedTargetWithoutSourceDirCount: {0}", fileProcessor.RemovedTargetWithoutSourceDirCount);
			Console.ReadLine();
		}
	}
}
