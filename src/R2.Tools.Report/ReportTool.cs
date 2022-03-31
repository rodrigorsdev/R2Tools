namespace R2.Tools.Report
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public abstract class ReportTool
    {
        protected static BaseColor blackColor = new BaseColor(0, 0, 0);
        protected static BaseColor darkGrayColor = new BaseColor(200, 200, 200);
        protected static BaseColor ligthGrayColor = new BaseColor(211, 211, 211);
        protected static BaseColor whiteColor = new BaseColor(255, 255, 255);
        protected static BaseColor yellowColor = new BaseColor(217, 217, 25);

        protected static Font textFont = FontFactory.GetFont("Verdana", 8, Font.NORMAL, blackColor);
        protected static Font titleFont = FontFactory.GetFont("Verdana", 8, Font.BOLD, blackColor);

        protected Document document;
        protected PdfWriter pdfWriter;
        MemoryStream outFile;
        
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public byte[] Logo { get; set; }
        public bool PrintHeader { get; set; }
        public bool PrintFooter { get; set; }
        public string FooterTitle { get; set; }
        public string FooterSubTitle { get; set; }
        public string FooterSubTitle2 { get; set; }
        public bool Portrait { get; set; }

        public ReportTool()
        {
            Start();
        }

        private void Start()
        {
            PrintHeader = true;
            PrintFooter = true;
            Title = string.Empty;
            Subtitle = string.Empty;
            FooterTitle = string.Empty;
            FooterSubTitle = string.Empty;
            FooterSubTitle2 = string.Empty;
            Portrait = false;
        }

        public virtual void RenderBody()
        {
            if (!Portrait)
                document = new Document(PageSize.A4, 20, 10, 80, 40);
            else
                document = new Document(PageSize.A4.Rotate(), 20, 10, 80, 80);

            outFile = new MemoryStream();
            pdfWriter = PdfWriter.GetInstance(document, outFile);

            document.AddAuthor("R2Studio");
            document.AddTitle(Title);
            document.AddSubject(Title);

            var footer = new Footer(Logo);
            footer.Title = Title;
            footer.Subtitle = Subtitle;
            footer.PrintHeader = PrintHeader;
            footer.PrintFooter = PrintFooter;
            footer.FooterTitle = FooterTitle;
            footer.FooterSubTitle = FooterSubTitle;
            footer.FooterSubTitle2 = FooterSubTitle2;

            pdfWriter.PageEvent = footer;

            document.Open();

            return;
        }

        public MemoryStream Get()
        {
            RenderBody();

            if (outFile == null || outFile.Length == 0)
                throw new Exception("Relatório sem dados para exibição!");

            try
            {
                pdfWriter.Flush();
                
                document.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Erro em ReportBase.Get()", e);
            }
            finally
            {
                document = null;
                pdfWriter = null;
            }

            return outFile;
        }

        protected PdfPCell GetNewCell(string text, Font font, int alignment, float spacing, int border, BaseColor borderColor, BaseColor backgroundColor)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.Padding = spacing;
            cell.Border = border;
            cell.BorderColor = borderColor;
            cell.BackgroundColor = backgroundColor;

            return cell;
        }

        protected PdfPCell GetNewCell(string text, Font font, int alignment, float spacing, int border, BaseColor borderColor)
        {
            return GetNewCell(text, font, alignment, spacing, border, borderColor, new BaseColor(255, 255, 255));
        }

        protected PdfPCell GetNewCell(string text, Font font, int alignment = 0, float spacing = 5, int border = 0)
        {
            return GetNewCell(text, font, alignment, spacing, border, new BaseColor(0, 0, 0), new BaseColor(255, 255, 255));
        }
    }

    public class Footer : PdfPageEventHelper
    {
        public Footer(byte[] logo)
        {
            Logo = logo;
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public byte[] Logo { get; set; }
        public bool PrintHeader { get; set; }
        public bool PrintFooter { get; set; }
        public string FooterTitle { get; set; }
        public string FooterSubTitle { get; set; }
        public string FooterSubTitle2 { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            base.OnStartPage(writer, doc);

            PrintDocumentHeader(writer, doc);
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            base.OnEndPage(writer, doc);

            PrintDocumentFooter(writer, doc);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

        private void PrintDocumentHeader(PdfWriter writer, Document document)
        {
            if (PrintHeader)
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, black);
                Font titleFont = FontFactory.GetFont("Verdana", 12, Font.BOLD, black);
                float[] sizes = new float[] { 1f, 3f, 1f };

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = document.PageSize.Width - (document.LeftMargin + document.RightMargin);
                table.SetWidths(sizes);

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Logo);
                logo.ScalePercent(60);

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);

                PdfPTable micro = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(Subtitle, font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                micro.AddCell(cell);
                cell = new PdfPCell(new Phrase(Title, titleFont));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                micro.AddCell(cell);

                cell = new PdfPCell(micro);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                micro = new PdfPTable(1);
                cell = new PdfPCell(new Phrase("Página: " + (document.PageNumber).ToString(), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micro.AddCell(cell);

                cell = new PdfPCell(micro);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, document.LeftMargin, (document.PageSize.Height - 10), writer.DirectContent);
            }
        }

        private void PrintDocumentFooter(PdfWriter writer, Document document)
        {
            var dateNow = Date.DateTool.ConvertDatetimeWithLocalTimeZone(DateTime.Now);

            if (PrintFooter)
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, black);
                Font bold = FontFactory.GetFont("Verdana", 8, Font.BOLD, black);
                float[] sizes = new float[] { 1.0f, 3.5f, 1f };

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = document.PageSize.Width - (document.LeftMargin + document.RightMargin);
                table.SetWidths(sizes);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Logo);
                image.ScalePercent(30);

                PdfPCell cell = new PdfPCell(image);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingLeft = 10f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                PdfPTable micro = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(FooterTitle, bold));
                cell.Border = 0;
                micro.AddCell(cell);
                cell = new PdfPCell(new Phrase(FooterSubTitle, font));
                cell.Border = 0;
                micro.AddCell(cell);
                cell = new PdfPCell(new Phrase(FooterSubTitle2, font));
                cell.Border = 0;
                micro.AddCell(cell);

                cell = new PdfPCell(micro);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                micro = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(dateNow.Value.ToString("dd/MM/yyyy"), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micro.AddCell(cell);
                cell = new PdfPCell(new Phrase(dateNow.Value.ToString("HH:mm:ss"), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micro.AddCell(cell);

                cell = new PdfPCell(micro);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, document.LeftMargin, 70, writer.DirectContent);
            }
        }
    }
}