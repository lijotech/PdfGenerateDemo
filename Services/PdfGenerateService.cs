using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.IO.Source;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using PdfGenerate.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PdfGenerate.Services
{
    public class PdfGenerateService : IPdfGenerateService
    {

        private static Color DEFAULT_FONT_COLOR = new DeviceRgb(0, 0, 0);
        private static Color CELL_BORDER_COLOR = new DeviceRgb(215, 222, 232);
        private static Color CELL_BACKGROUND_COLOR = new DeviceRgb(238, 232, 213);
        private PdfFont DEFAULT_FONT_TYPE;
        public static readonly string LOGOIMGPATH = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "assets", "logo.jpg");
        public static readonly string BACKGROUNDIMGPATH = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "assets", "background.png");
        public static readonly string CUSTOM_FONT_URL = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "assets", "StarellaTattoo.ttf");
       
        
        public Task<FileDownloadDto> GeneratePdf(DataTable dataTable)
        {
            DEFAULT_FONT_TYPE = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var CUSTOM_FONT = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(CUSTOM_FONT_URL), PdfEncodings.WINANSI,
                                         PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            FileDownloadDto resultFile = new FileDownloadDto
            {
                MimeType = "application/pdf"
            };
            int noOfColumns = dataTable.Columns.Count;
            int fixedColumnWidthPercentage = 2;

            var percentageArray = Enumerable.Repeat<float>(fixedColumnWidthPercentage, noOfColumns).ToArray();



            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            Document doc = new Document(pdfDoc);

            //code to add background Image
            Image imagebackground = new Image(ImageDataFactory.Create(BACKGROUNDIMGPATH));
            imagebackground.SetFixedPosition(1, 0, 60);
            imagebackground.ScaleAbsolute(530, 600);
            doc.Add(imagebackground);

            //define the table and columns for header 
            Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3, 2 })).UseAllAvailableWidth();

            //adding logo Title and other fields in head section
            headerTable.AddCell(CreateImageCell(LOGOIMGPATH, VerticalAlignment.BOTTOM, 4, 0, 50));
            headerTable.AddCell(CreateTextCellCustom("Main Title with rowspan", TextAlignment.CENTER, VerticalAlignment.BOTTOM, 10, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, isBold: true, isCellBorder: false, isCellBackgroundColor: false, rowSpan: 4));
            headerTable.AddCell(CreateTextCellCustom($"Date :{System.DateTime.Today.ToString("dd-MMM-yyyy")}", TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, multipliedLeading: 1.5f));
            headerTable.AddCell(CreateTextCellCustom($"Time : {System.DateTime.Now.ToString("HH:mm")}", TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, multipliedLeading: 1.5f));
            headerTable.AddCell(CreateTextCellCustom("Report generated by : UserName", TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, multipliedLeading: 1.5f));
            doc.Add(headerTable);

            //define table columns for the data
            Table table = new Table(UnitValue.CreatePercentArray(percentageArray)).UseAllAvailableWidth();

            table.AddCell(CreateTextCellCustom("Sub title with colspan", TextAlignment.CENTER, VerticalAlignment.BOTTOM, 9, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, false, 0, 7, 5, 2f));
            //Adding column headers
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                table.AddCell(CreateTextCellCustom(dataColumn.ColumnName, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, true, true, commonCellPadding: 5));
            }

            //Adding data in each cell.
            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    table.AddCell(CreateTextCellCustom(dataRow[dataColumn.ColumnName].ToString(), TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, false, true, commonCellPadding: 5));
                }
            }

            doc.Add(table);
            doc.Add(new Paragraph());
            doc.Add(new Paragraph());
            doc.Add(new Paragraph());


            Table tableNext = new Table(UnitValue.CreatePercentArray(new float[] { 10 }));
            tableNext.SetWidth(UnitValue.CreatePercentValue(100));
            tableNext.SetFixedLayout();

            tableNext.AddCell(CreateTextCellCustom("Sub title with Custom Font", TextAlignment.CENTER, VerticalAlignment.BOTTOM, 20, DEFAULT_FONT_COLOR, CUSTOM_FONT, true, false, false, 0, 0, 5,2f));
            Cell mainCell = FormattedEmptyCell(CELL_BACKGROUND_COLOR, CELL_BORDER_COLOR,opacity:0.5f);
                Table innerTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 7,2 }));
                innerTable.SetWidth(UnitValue.CreatePercentValue(100));
                innerTable.SetFixedLayout();


                //table header styles
                innerTable.AddCell(CreateTextCellCustom("MAIN ITEM",
                TextAlignment.LEFT, VerticalAlignment.BOTTOM, 9, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));

                innerTable.AddCell(CreateTextCellCustom("DESCRIPTION",
                TextAlignment.LEFT, VerticalAlignment.BOTTOM, 9, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));

                innerTable.AddCell(CreateTextCellCustom("AMOUNT",
                TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 9, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));

                //table data values
                innerTable.AddCell(CreateTextCellCustom("Item name",
                TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));

                innerTable.AddCell(CreateTextCellCustom("Item description in detail",
                TextAlignment.LEFT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));

                innerTable.AddCell(CreateTextCellCustom("23.00",
                TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 8, DEFAULT_FONT_COLOR, DEFAULT_FONT_TYPE, true, false, commonCellPadding: 5));


            mainCell.Add(innerTable);
            tableNext.AddCell(mainCell);
            doc.Add(tableNext);

            PdfPage page = pdfDoc.GetPage(1);
            float width = pdfDoc.GetDefaultPageSize().GetWidth();
            float height = pdfDoc.GetDefaultPageSize().GetHeight();
            Rectangle pageRect = new Rectangle(20, 20, width - 40, height - 40);
            new PdfCanvas(page).SetStrokeColor(CELL_BORDER_COLOR).SetLineWidth(0.6f).Rectangle(pageRect).Stroke();          

            doc.Close();

            resultFile.Attachment = baos.ToArray();
            resultFile.FileName = string.Format("Test_Data_PDF_{0}.pdf", System.DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            return Task.FromResult(resultFile);
        }


        private static Cell CreateTextCellCustom(
        string text,
        TextAlignment textAlignment,
        VerticalAlignment cellVerticalAlignment,
        float fontsize,
        Color fontColor,
        PdfFont fontType,
        bool isBold = false,
        bool isCellBorder = false,
        bool isCellBackgroundColor = false,
        int rowSpan = 0,
        int colSpan = 0,
        float commonCellPadding = 0,
        float multipliedLeading = 1)
        {

            Cell cell = new Cell(rowSpan, colSpan);
            Paragraph p = new Paragraph();
            p.SetTextAlignment(textAlignment);

            p.Add(new Text(text).SetFont(fontType));
            p.SetFontColor(fontColor).SetFontSize(fontsize);
            p.SetMultipliedLeading(multipliedLeading);
            if (isBold)
                p.SetBold();
            cell.Add(p).SetVerticalAlignment(cellVerticalAlignment).SetPadding(commonCellPadding);
            if (!isCellBorder)
                cell.SetBorder(Border.NO_BORDER);
            if (isCellBackgroundColor)
            {
                Color customColor = new DeviceRgb(221, 235, 247);
                cell.SetBackgroundColor(customColor);
            }
            return cell;
        }

        private static Cell CreateImageCell(
        string path,
        VerticalAlignment cellVerticalAlignment,
        int rowSpan = 0,
        int colSpan = 0,
        float percentageImageWidth = 100)
        {
            Image img = new Image(ImageDataFactory.Create(path));
            img.SetWidth(UnitValue.CreatePercentValue(percentageImageWidth));
            Cell cell = new Cell(rowSpan, colSpan).Add(img).SetVerticalAlignment(cellVerticalAlignment);
            cell.SetBorder(Border.NO_BORDER);
            return cell;
        }

        public static Cell FormattedEmptyCell(
        Color cellBackgroudColor,
        Color cellBorderColor,
        float cellBorderWidth = 0.6f,
        int rowSpan = 0,
        int colSpan = 0,
        float opacity = 1f)
        {

            Cell cellFirst = new Cell(rowSpan, colSpan);
            cellFirst.SetBorder(Border.NO_BORDER);
            cellFirst.SetBorderLeft(new SolidBorder(cellBorderColor, cellBorderWidth));
            cellFirst.SetBorderBottom(new SolidBorder(cellBorderColor, cellBorderWidth));
            cellFirst.SetBorderTop(new SolidBorder(cellBorderColor, cellBorderWidth));
            cellFirst.SetBorderRight(new SolidBorder(cellBorderColor, cellBorderWidth));
            cellFirst.SetBackgroundColor(cellBackgroudColor, opacity);
            return cellFirst;
        }

    }
}
