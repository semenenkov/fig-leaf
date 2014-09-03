﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FigLeaf.Core
{
	public class Thumbnail
	{
		private readonly int _size;
		private readonly List<string> _videoExts;

		public Thumbnail(Settings settings)
		{
			if (!settings.EnableThumbnails)
				throw new ApplicationException("Trying to create thumbnail generator for disabled thumbnails option");

			_size = settings.ThumbnailSize;
			_videoExts = settings.VideoExtensions;
		}

		public string GetThumbnailFileName(string source, out bool isVideo)
		{
			string ext = Path.GetExtension(source);
			if (!string.IsNullOrEmpty(ext) && ext.StartsWith("."))
				ext = ext.Substring(1);

			isVideo = _videoExts.Any(ve => string.Equals(ve, ext, StringComparison.OrdinalIgnoreCase));
			return string.Format("{0}.jpg", Path.GetFileName(source));
		}

		public void MakeForPhoto(string source, string target)
		{
			Image thumb;
			
			try
			{
				using (var image = Image.FromFile(source))
				{
					int w = image.Width > image.Height ? _size : _size * image.Width / image.Height;
					int h = image.Height > image.Width ? _size : _size * image.Height / image.Width;
					thumb = image.GetThumbnailImage(w, h, () => false, IntPtr.Zero);
				}
			}
			catch (Exception)
			{
				// may be thrown for any non-image file (mp3, etc)
				thumb = new Bitmap(_size, _size);
				Graphics graphics = Graphics.FromImage(thumb);
				int fontSize = _size / 10;
				if (fontSize < 5) fontSize = 5;
				graphics.DrawString(Path.GetExtension(source), new Font(FontFamily.GenericSansSerif, fontSize), Brushes.GreenYellow, fontSize / 2, fontSize / 2);
			}

			thumb.Save(target, ImageFormat.Jpeg);
		}

		public void MakeForVideo(string source, string target)
		{
			var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
			string tmpFile = target + ".tmp";
			ffMpeg.GetVideoThumbnail(source, tmpFile);
			MakeForPhoto(tmpFile, target);
			File.Delete(tmpFile);
		}
	}
}
