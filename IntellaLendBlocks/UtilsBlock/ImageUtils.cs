using ImageMagick;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using iT = iTextSharp.text;
using SDI = System.Drawing;

namespace MTSEntBlocks.UtilsBlock
{
    public class ImageUtilities
    {
        public static byte[] ConvertHTMLtoPDFByte(string template, string cssText)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.PageCount = 1;
                doc.Open();
                doc.NewPage();
                using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                {
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(template)))
                    {
                        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, htmlMemoryStream, cssMemoryStream);
                    }
                }
                doc.Close();

                writer.Dispose();
                doc.Dispose();
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public static byte[] ConvertHTMLtoPDFByteWithImage(string template, string cssText)
        {

            byte[] pdf;
            using (var memoryStream = new MemoryStream())
            {
                using (var doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                {
                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    doc.Open();
                    var html = template;

                    var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                    tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                    tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                    CssFilesImpl cssFiles = new CssFilesImpl();
                    cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                    var cssResolver = new StyleAttrCSSResolver(cssFiles);
                    cssResolver.AddCss(cssText, "utf-8", true);
                    var charset = Encoding.UTF8;
                    var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                    hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                    var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                    var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                    var worker = new XMLWorker(pipeline, true);
                    var xmlParser = new XMLParser(true, worker, charset);
                    xmlParser.Parse(new StringReader(html));
                }
                pdf = memoryStream.ToArray();
            }
            return pdf;
        }

        public static byte[] contactZipByteArray(byte[] pdfBytes, string filename)
        {
            using (var compressedFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    var zipEntry = zipArchive.CreateEntry(filename + ".pdf");

                    using (var originalFileSystem = new MemoryStream(pdfBytes))
                    {
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            originalFileSystem.CopyTo(zipEntryStream);
                        }
                    }
                }
                return compressedFileStream.ToArray();
            }

           
        }

        public static byte[] ConcatPDFByteArray(List<byte[]> pdfByteContent)
        {

            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        foreach (var p in pdfByteContent)
                        {
                            if (p.Length > 0)
                            {
                                using (var reader = new PdfReader(p))
                                {
                                    copy.AddDocument(reader);
                                }
                            }
                        }

                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        public byte[] GetJpegByteFromPDF(byte[] fileContents, Int32 pageNumber)
        {
            PdfReader reader = null;
            try
            {
                byte[] bms = null;
                reader = new PdfReader(fileContents);

                if (reader.NumberOfPages > 1)
                {
                    bms = ExtractPage(fileContents, pageNumber);
                }

                return bms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GetJpegFromPDF(byte[] fileContents, Int32 pageNumber)
        {
            MemoryStream mswrite = null;
            PdfReader reader = null;
            try
            {
                byte[] bms = null;
                reader = new PdfReader(fileContents);

                if (reader.NumberOfPages > 1)
                {
                    bms = ExtractPage(fileContents, pageNumber);
                }
                else
                {
                    bms = fileContents;
                }

                MagickReadSettings settings = new MagickReadSettings();
                settings.Density = new Density(300, 300);

                mswrite = new MemoryStream();


                using (MagickImage image = new MagickImage())
                {
                    image.Read(bms, settings);
                    image.Format = MagickFormat.Jpeg;
                    //image.SetAttribute("-quality", "40");
                    image.Write(mswrite);
                }
                SDI.Image img = SDI.Image.FromStream(mswrite);

                ImageConverter converter = new ImageConverter();

                MemoryStream returnStream = new MemoryStream((byte[])converter.ConvertTo(img, typeof(byte[])));

                return returnStream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mswrite.Close();
                reader.Close();
            }
        }

        public byte[] GetTiffFromPDF(byte[] fileContents, Int32 pageNumber)
        {
            MemoryStream mswrite = null;
            PdfReader reader = null;
            try
            {
                byte[] bms = null;
                reader = new PdfReader(fileContents);

                if (reader.NumberOfPages > 1)
                {
                    bms = ExtractPage(fileContents, pageNumber);
                }
                else
                {
                    bms = fileContents;
                }

                MagickReadSettings settings = new MagickReadSettings();
                settings.Density = new Density(300, 300);

                mswrite = new MemoryStream();


                using (MagickImage image = new MagickImage())
                {
                    image.Read(bms, settings);
                    image.Format = MagickFormat.Tif;
                    //image.SetAttribute("-quality", "40");
                    image.Write(mswrite);
                }
                SDI.Image img = SDI.Image.FromStream(mswrite);

                ImageConverter converter = new ImageConverter();

                MemoryStream returnStream = new MemoryStream((byte[])converter.ConvertTo(img, typeof(byte[])));

                return returnStream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mswrite.Close();
                reader.Close();
            }
        }


        private byte[] ExtractPage(byte[] filecontents, int pageNumber)
        {
            iT.pdf.PdfReader reader = null;
            iT.Document document = null;
            iT.pdf.PdfCopy pdfCopyProvider = null;
            iT.pdf.PdfImportedPage importedPage = null;
            try
            {

                reader = new iT.pdf.PdfReader(filecontents);

                document = new iT.Document(reader.GetPageSizeWithRotation(pageNumber));
                // Initialize an instance of the PdfCopyClass with the source
                // document and an output file stream:
                MemoryStream ms = new MemoryStream();

                pdfCopyProvider = new iT.pdf.PdfCopy(document, ms);

                document.Open();
                // Extract the desired page number:
                importedPage = pdfCopyProvider.GetImportedPage(reader, pageNumber);
                pdfCopyProvider.AddPage(importedPage);
                document.Close();
                reader.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CompressedImage ConvertTiffToJpeg(byte[] fileContent, int pageNumber, int maxWidth = 1654, int maxHeight = 2339)
        {
            using (MemoryStream ms = new MemoryStream(fileContent))
            {
                using (System.Drawing.Image tiffimage = System.Drawing.Image.FromStream(ms))
                {
                    FrameDimension frameDimensions = new FrameDimension(tiffimage.FrameDimensionsList[0]);

                    // Selects one frame at a time and save as jpeg.
                    tiffimage.SelectActiveFrame(frameDimensions, pageNumber - 1);

                    using (SDI.Bitmap bmp = new SDI.Bitmap(tiffimage))
                    {
                        return ResizeTiff(bmp, maxWidth, maxHeight);
                    }
                }

            }
        }

        public CompressedImage RezizeImage(string filename, int maxWidth = 1654, int maxHeight = 2339)
        {
            using (SDI.Bitmap bmp = new SDI.Bitmap(filename))
            {
                return ResizeTiffGrayScale(bmp, maxWidth, maxHeight);
            }
        }
        public CompressedImage ConvertTiffToJpegGrayScale(byte[] fileContent, int pageNumber, int maxWidth = 1654, int maxHeight = 2339)
        {
            using (MemoryStream ms = new MemoryStream(fileContent))
            {
                using (System.Drawing.Image tiffimage = System.Drawing.Image.FromStream(ms))
                {
                    FrameDimension frameDimensions = new FrameDimension(tiffimage.FrameDimensionsList[0]);

                    // Selects one frame at a time and save as jpeg.
                    tiffimage.SelectActiveFrame(frameDimensions, pageNumber - 1);

                    using (SDI.Bitmap bmp = new SDI.Bitmap(tiffimage))
                    {
                        return ResizeTiffGrayScale(bmp, maxWidth, maxHeight);
                    }
                }

            }
        }


        public CompressedImage ConvertJPEGtoTIFF(byte[] fileContent, int maxWidth = 1654, int maxHeight = 2339)
        {
            using (MemoryStream ms = new MemoryStream(fileContent))
            {
                using (System.Drawing.Image tiffimage = System.Drawing.Image.FromStream(ms))
                {
                    FrameDimension frameDimensions = new FrameDimension(tiffimage.FrameDimensionsList[0]);

                    // Selects one frame at a time and save as jpeg.
                    //tiffimage.SelectActiveFrame(frameDimensions, pageNumber - 1);

                    using (SDI.Bitmap bmp = new SDI.Bitmap(tiffimage))
                    {
                        return ResizeTiffCompression(bmp, maxWidth, maxHeight);
                    }
                }

            }
        }

        public Int32 GetByteDataPageCount(byte[] fileContents, string fileType)
        {
            if (fileType == "application/pdf")
            {
                PdfReader reader = null;
                try
                {
                    reader = new PdfReader(fileContents);

                    return reader.NumberOfPages;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            else if (fileType == "image/tiff")
            {
                MemoryStream ms = new MemoryStream();
                using (System.Drawing.Image tiffimage = System.Drawing.Image.FromStream(new MemoryStream(fileContents)))
                {
                    FrameDimension frameDimensions = new FrameDimension(
                        tiffimage.FrameDimensionsList[0]);

                    int frameNum;

                    // Gets the number of pages from the tiff image (if multipage)
                    return frameNum = tiffimage.GetFrameCount(frameDimensions);
                }
            }

            return 0;
        }

        public Int32 GetInvoicePageCount(byte[] fileContents, string fileType)
        {
            if (fileType == "application/pdf")
            {
                PdfReader reader = null;
                try
                {
                    reader = new PdfReader(fileContents);

                    return reader.NumberOfPages;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            else if (fileType == "image/tiff")
            {
                MemoryStream ms = new MemoryStream();
                using (System.Drawing.Image tiffimage = System.Drawing.Image.FromStream(new MemoryStream(fileContents)))
                {
                    FrameDimension frameDimensions = new FrameDimension(
                        tiffimage.FrameDimensionsList[0]);

                    int frameNum;

                    // Gets the number of pages from the tiff image (if multipage)
                    return frameNum = tiffimage.GetFrameCount(frameDimensions);
                }
            }

            return 0;
        }

        public String GetByteDataMimeType(byte[] dataByte)
        {
            string mimeType = "unknown";

            if (dataByte.Length > 0)
            {

                string getstring = BitConverter.ToString(dataByte).Substring(0, 8).Replace("-", "");

                if (thisjpg(getstring))
                {
                    mimeType = "image/jpeg";
                }
                else if (thisPDF(getstring))
                {
                    mimeType = "application/pdf";
                }
                else if (thisTiff(getstring))
                {
                    mimeType = "image/tiff";
                }
            }

            return mimeType;

        }

        public bool IsPDF(byte[] dataByte)
        {
            string mimeType = GetByteDataMimeType(dataByte);

            if (mimeType == "application/pdf")
                return true;

            return false;

        }

        public static bool thisjpg(string format)
        {
            if (format == "FFD8FF")
            {
                return true;
            }
            return false;
        }

        public static bool thisPDF(string format)
        {
            if (format == "255044")
            {
                return true;
            }
            return false;
        }

        public static bool thisTiff(string format)
        {
            if (format == "49492A" || format == "4D4D2A")
            {
                return true;
            }
            return false;
        }


        public static void GetImageSize(byte[] imageBytes, out int width, out int height)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
            width = image.Width;
            height = image.Height;
        }


        public static CompressedImage CompressImageWithSize(byte[] imageBytes)
        {
            var retImage = new CompressedImage();

            int IMAGE_THUMB_LARGE_SIZE_WIDTH = 1500;
            int IMAGE_THUMB_LARGE_SIZE_HEIGHT = 1500;

            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
            retImage.OrginalImageWidth = image.Width;
            retImage.OrginalImageHeight = image.Height;
            int thumbLargeWidth = GetWidthPropotional(image.Width, image.Height, IMAGE_THUMB_LARGE_SIZE_WIDTH, IMAGE_THUMB_LARGE_SIZE_HEIGHT);
            int thumbLargeHeight = GetHeightPropotional(image.Width, image.Height, IMAGE_THUMB_LARGE_SIZE_WIDTH, IMAGE_THUMB_LARGE_SIZE_HEIGHT);
            if (image.Width < IMAGE_THUMB_LARGE_SIZE_WIDTH && image.Height < IMAGE_THUMB_LARGE_SIZE_HEIGHT)
            {
                thumbLargeWidth = image.Width;
                thumbLargeHeight = image.Height;
            }
            MemoryStream msThmbLarge = new MemoryStream();

            retImage.CompressedImageWidth = thumbLargeWidth;
            retImage.CompressedImageHeight = thumbLargeHeight;

            using (System.Drawing.Image thumbLarge = new Bitmap(image, thumbLargeWidth, thumbLargeHeight))
            {
                thumbLarge.Save(msThmbLarge, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            msThmbLarge.Position = 0;

            retImage.Image = msThmbLarge.ToArray();

            return retImage;
        }


        public static byte[] CompressImage(byte[] imageBytes)
        {
            int IMAGE_THUMB_LARGE_SIZE_WIDTH = 1500;
            int IMAGE_THUMB_LARGE_SIZE_HEIGHT = 1500;

            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));

            int thumbLargeWidth = GetWidthPropotional(image.Width, image.Height, IMAGE_THUMB_LARGE_SIZE_WIDTH, IMAGE_THUMB_LARGE_SIZE_HEIGHT);
            int thumbLargeHeight = GetHeightPropotional(image.Width, image.Height, IMAGE_THUMB_LARGE_SIZE_WIDTH, IMAGE_THUMB_LARGE_SIZE_HEIGHT);
            if (image.Width < IMAGE_THUMB_LARGE_SIZE_WIDTH && image.Height < IMAGE_THUMB_LARGE_SIZE_HEIGHT)
            {
                thumbLargeWidth = image.Width;
                thumbLargeHeight = image.Height;
            }
            MemoryStream msThmbLarge = new MemoryStream();

            using (System.Drawing.Image thumbLarge = new Bitmap(image, thumbLargeWidth, thumbLargeHeight))
            {
                thumbLarge.Save(msThmbLarge, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            msThmbLarge.Position = 0;

            return msThmbLarge.ToArray();
        }

        private static int GetHeightPropotional(int imageWidth, int imageHeight, int targetWidth, int targetHeight)
        {
            int height = targetHeight;

            if (imageHeight > imageWidth)
            {
                height = targetHeight;
            }
            else
            {
                height = (int)((double)(targetWidth * imageHeight) / imageWidth);

            }
            return height;
        }

        private static int GetWidthPropotional(int imageWidth, int imageHeight, int targetWidth, int targetHeight)
        {
            int width = targetWidth;
            if (imageWidth > imageHeight)
            {
                width = targetWidth;
            }
            else
            {
                width = (int)((double)(targetHeight * imageWidth) / imageHeight);

            }
            return width;
        }

        #region Private Methods

        public CompressedImage ResizeTiff(Bitmap image, int maxWidth, int maxHeight, int quality = 100)
        {
            CompressedImage img = new CompressedImage();

            MemoryStream ms = new MemoryStream();
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            img.OrginalImageWidth = originalWidth;
            img.OrginalImageHeight = originalHeight;
            img.CompressedImageWidth = newWidth;
            img.CompressedImageHeight = newHeight;

            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters encoderParameters = new EncoderParameters(1);

            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(ms, imageCodecInfo, encoderParameters);
            img.Image = ms.ToArray(); ;
            return img;
        }

        public CompressedImage ResizeTiffGrayScale(Bitmap image, int maxWidth, int maxHeight)
        {
            CompressedImage img = new CompressedImage();

            MemoryStream ms = new MemoryStream();
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            img.OrginalImageWidth = originalWidth;
            img.OrginalImageHeight = originalHeight;
            img.CompressedImageWidth = newWidth;
            img.CompressedImageHeight = newHeight;

            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
                   });

                ImageAttributes attributes = new ImageAttributes();

                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(image, new SDI.Rectangle(0, 0, newWidth, newHeight), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            newBitmap.Save(ms, ImageFormat.Jpeg);
            img.Image = ms.ToArray(); ;
            return img;
        }


        public CompressedImage ResizeTiffCompression(Bitmap image, int maxWidth, int maxHeight, int quality = 100)
        {
            CompressedImage img = new CompressedImage();

            MemoryStream ms = new MemoryStream();
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            img.OrginalImageWidth = originalWidth;
            img.OrginalImageHeight = originalHeight;
            img.CompressedImageWidth = newWidth;
            img.CompressedImageHeight = newHeight;

            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Tiff);

            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters encoderParameters = new EncoderParameters(1);

            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(ms, imageCodecInfo, encoderParameters);
            img.Image = ms.ToArray(); ;
            return img;
        }


        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        #endregion
    }

    public class CompressedImage
    {
        public int OrginalImageHeight { get; set; }
        public int OrginalImageWidth { get; set; }
        public int CompressedImageHeight { get; set; }
        public int CompressedImageWidth { get; set; }
        public byte[] Image { get; set; }

    }

    public class CustomImageTagProcessor : iTextSharp.tool.xml.html.Image
    {
        public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
        {
            IDictionary<string, string> attributes = tag.Attributes;
            string src;
            if (!attributes.TryGetValue(HTML.Attribute.SRC, out src))
                return new List<IElement>(1);

            if (string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
            {
                // data:[<MIME-type>][;charset=<encoding>][;base64],<data>
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                var image = iTextSharp.text.Image.GetInstance(imagedata);

                var list = new List<IElement>();
                var htmlPipelineContext = GetHtmlPipelineContext(ctx);
                list.Add(GetCssAppliers().Apply(new Chunk((iTextSharp.text.Image)GetCssAppliers().Apply(image, tag, htmlPipelineContext), 0, 0, true), tag, htmlPipelineContext));
                return list;
            }
            else
            {
                return base.End(ctx, tag, currentContent);
            }
        }
    }



}