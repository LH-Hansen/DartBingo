using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Dart_Bingo
{
    public class PdfService
    {
        public string CreatePDF(string targetDir, int pages)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            Document document = new Document
            {
                Info = { Title = "Bingo" }
            };

            Section section = document.AddSection();

            section.PageSetup.Orientation = Orientation.Landscape;

            for (int i = 0; i < pages; i++)
            {
                AddPage(section);

                if (i != pages - 1)
                    section.AddPageBreak();
            }

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            pdfRenderer.RenderDocument();

            string fileName = $"Bingo_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string fullPath = Path.Combine(targetDir, fileName);

            pdfRenderer.Save(fullPath);

            return fullPath;
        }

        public void AddPage(Section section)
        {
            Table parentTable = section.AddTable();

            for (int i = 0; i < 2; i++)
            {
                parentTable.AddColumn("15cm");
            }

            for (int i = 0; i < 2; i++)
            {
                parentTable.AddRow();
                parentTable.Rows[i].TopPadding = Unit.FromCentimeter(0.5);
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    parentTable.Rows[i].Cells[j].Elements.Add(CreateTable());
                }
            }
        }

        private Table CreateTable()
        {
            Table table = new Table();

            table.Borders.Width = 0.75;
            table.Borders.Color = Colors.Black;

            for (int i = 0; i < 5; i++)
            {
                table.AddColumn("2cm");
            }

            #region Header

            Row header = table.AddRow();

            header.Cells[0].MergeRight = 4;
            header.Height = Unit.FromCentimeter(0.75);
            header.VerticalAlignment = VerticalAlignment.Center;
            header.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            Paragraph title = header.Cells[0].AddParagraph("Bingo");

            title.Format.Font.Size = 20;

            Row name = table.AddRow();

            name.Cells[0].MergeRight = 4;
            name.Height = Unit.FromCentimeter(0.75);
            name.VerticalAlignment = VerticalAlignment.Center;
            name.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            name.Cells[0].AddParagraph("Name:");
            name.Format.Font.Size = 13;

            #endregion

            for (int i = 0; i < 3; i++)
            {
                table.AddRow().Height = Unit.FromCentimeter(2);
            }

            FillTable(table);

            return table;
        }

        private List<string> FillListSingle()
        {
            List<string> singleList = new List<string>();

            for (int i = 1; i <= 20; i++)
            {
                singleList.Add(i.ToString());
            }

            return singleList;
        }

        private List<string> FillListDouble()
        {
            List<string> doubleList = new List<string>();

            for (int i = 1; i <= 20; i++)
            {
                doubleList.Add("D" + i.ToString());
            }

            doubleList.Add("25");

            return doubleList;
        }

        private List<string> FillListTriple()
        {
            List<string> tripleList = new List<string>();

            for (int i = 1; i <= 20; i++)
            {
                tripleList.Add("T" + i.ToString());
            }

            tripleList.Add("50");

            return tripleList;
        }

        private void FillTable(Table table)
        {
            List<string> singleList = FillListSingle();
            List<string> doubleList = FillListDouble();
            List<string> tripleList = FillListTriple();

            Random random = new Random();

            int index;

            for (int i = 2; i <= 4; i++)
            {
                List<int> avialblePositions = new List<int> { 0, 1, 2, 3, 4 };

                table.Rows[i].VerticalAlignment = VerticalAlignment.Center;
                table.Rows[i].Format.Alignment = ParagraphAlignment.Center;

                table.Rows[i].Format.Font.Size = 16;
                table.Rows[i].Format.Font.Bold = true;

                if (random.Next(0, 2) == 0)
                    PlaceGuaranteedValue(table.Rows[i], doubleList, avialblePositions, random);
                else
                    PlaceGuaranteedValue(table.Rows[i], tripleList, avialblePositions, random);

                foreach (int position in avialblePositions)
                {
                    index = random.Next(0, singleList.Count);

                    table.Rows[i].Cells[position].AddParagraph(singleList[index]);

                    singleList.RemoveRange(index, 1);
                }
            }
        }

        private void PlaceGuaranteedValue(Row row, List<string> value, List<int> avilablePositions, Random random)
        {
            int positionIndex = random.Next(0, avilablePositions.Count);
            int valueIndex = random.Next(0, value.Count);

            row.Cells[positionIndex].AddParagraph(value[valueIndex]);

            avilablePositions.Remove(avilablePositions[positionIndex]);
            value.Remove(value[valueIndex]);
        }
    }

}
