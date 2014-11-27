using System;
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
				ext = ext.Substring(1).ToLower();

			isVideo = _videoExts.Any(ve => string.Equals(ve, ext, StringComparison.OrdinalIgnoreCase));
			if (!isVideo && !IsSupportedPhotoFormat(ext))
				return null;

			return string.Format("{0}.jpg", Path.GetFileName(source));
		}

		public bool MakeForPhoto(string source, string target)
		{
			try
			{
				using (var image = Image.FromFile(source))
				{
					int w = image.Width > image.Height ? _size : _size * image.Width / image.Height;
					int h = image.Height > image.Width ? _size : _size * image.Height / image.Width;
					using (Image thumb = image.GetThumbnailImage(w, h, () => false, IntPtr.Zero))
						thumb.Save(target, ImageFormat.Jpeg);
					return true;
				}
			}
			catch
			{
				// may be thrown for bad image or not supported format
				return false;
			}
		}

		public bool MakeForVideo(string source, string target)
		{
			var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
			string tmpFile = target + ".tmp";
			try
			{
				ffMpeg.GetVideoThumbnail(source, tmpFile);
				bool result = MakeForPhoto(tmpFile, target);
				File.Delete(tmpFile);
				return result;
			}
			catch
			{
				// may be thrown for bad image or not supported format
				return false;
			}
		}

		private bool IsSupportedPhotoFormat(string ext)
		{
			return ext == "bmp" 
				|| ext == "gif" 
				|| ext == "jpg" 
				|| ext == "jpeg" 
				|| ext == "png" 
				|| ext == "tiff";
		}
	}
}
