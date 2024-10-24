using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Rectangle = iTextSharp.text.Rectangle;



namespace WindowsFormPdfGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Define fonts
        iTextSharp.text.Font fontHeader = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8, BaseColor.BLACK);
        iTextSharp.text.Font fontRegular = FontFactory.GetFont(FontFactory.TIMES, 6, BaseColor.BLACK);
        iTextSharp.text.Font fontBold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 5, BaseColor.BLACK);
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Define the dimensions of the portrait ID card
            Rectangle idCardSizePortrait = new Rectangle(54f * 2.835f, 85.6f * 2.835f);

            using (Document doc = new Document(idCardSizePortrait))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("PortraitIDCard.pdf", FileMode.Create));
                doc.Open();

                // Add the first page content
                AddFirstPageContent(doc, writer, idCardSizePortrait);

                // Add a second page
                doc.NewPage(); // Create a new page
                AddSecondPageContent(doc, writer, idCardSizePortrait);

                // Close the document
                doc.Close();
            }

            MessageBox.Show("PDF GENERATED SUCCESSFULLY!");
            GetDataFromMySQL();
        }

        private void AddFirstPageContent(Document doc, PdfWriter writer, Rectangle idCardSizePortrait)
        {
            // Add the background image and scale it to fit the ID card size
            iTextSharp.text.Image background = iTextSharp.text.Image.GetInstance("C:\\Barangay Picture\\Caloocan_City.png");
            background.ScaleToFit(idCardSizePortrait.Width, idCardSizePortrait.Height);
            background.SetAbsolutePosition(0f, 50f);

            PdfContentByte canvas = writer.DirectContent;

            // Add the image with opacity
            PdfGState gState = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.1f };
            canvas.SaveState();
            canvas.SetGState(gState);
            canvas.AddImage(background);
            canvas.RestoreState();



            // Add header text
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("Republic of the Philippines", fontHeader), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 30f, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("City of Caloocan", fontHeader), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 45f, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("OFFICE OF THE PUNONG BARANGAY", fontBold), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 60f, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("Barangay 22, Zone 2, District II", fontRegular), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 75f, 0);

            // Add full name and details text
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase($"{textBox1.Text}", fontBold), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 150f, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("FULL NAME", fontRegular), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 165f, 0);

            // Move the line under the name higher
            canvas.MoveTo(10f, idCardSizePortrait.Height - 160f); // Raised by 10 points
            canvas.LineTo(idCardSizePortrait.Width - 10f, idCardSizePortrait.Height - 160f);
            canvas.Stroke();

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase($"{textBox3.Text}", fontBold), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 195f, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("RESIDENCE ADDRESS", fontRegular), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 205f, 0);

            // Move the line under the address higher
            canvas.MoveTo(10f, idCardSizePortrait.Height - 200f); // Raised by 10 points
            canvas.LineTo(idCardSizePortrait.Width - 10f, idCardSizePortrait.Height - 200f);
            canvas.Stroke();

            // Add signature label
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("SIGNATURE", fontRegular), idCardSizePortrait.Width / 2, idCardSizePortrait.Height - 240f, 0);

            // Add profile picture
            iTextSharp.text.Image profilePic = iTextSharp.text.Image.GetInstance("C:\\Barangay Picture\\Caloocan_City.png");
            profilePic.ScaleAbsolute(40f, 40f);
            profilePic.SetAbsolutePosition(idCardSizePortrait.Width - 45f, 120f);
            PdfContentByte underCanvas = writer.DirectContentUnder;
            underCanvas.AddImage(profilePic);
        }

        private void AddSecondPageContent(Document doc, PdfWriter writer, Rectangle idCardSizePortrait)
        {
            // Add the background image and scale it to fit the ID card size
            iTextSharp.text.Image background = iTextSharp.text.Image.GetInstance("C:\\Barangay Picture\\Caloocan_City.png");
            background.ScaleToFit(idCardSizePortrait.Width, idCardSizePortrait.Height);
            background.SetAbsolutePosition(0f, 50f);

            PdfContentByte canvas = writer.DirectContent;

            // Add the image with opacity
            PdfGState gState = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.1f };
            canvas.SaveState();
            canvas.SetGState(gState);
            canvas.AddImage(background);
            canvas.RestoreState();




            // Create a smaller font for the header
            iTextSharp.text.Font fontHeaderSmall = FontFactory.GetFont(FontFactory.TIMES, 4, BaseColor.BLACK);
            iTextSharp.text.Font fontHeaderSmaller = FontFactory.GetFont(FontFactory.TIMES, 3, BaseColor.BLACK);

            // Combine the four lines into one
            string combined1 = "BIRTHDAY\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0  " +
                "SEX \u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0 " +
                "HEIGHT \u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0 WEIGHT";

            // Draw a line above the text (adjust the Y-position for the line as needed)
            float yLinePosition = idCardSizePortrait.Height - 25f; // Position the line just above the text
            float leftMargin = 10f; // Start position of the line (left side)
            float rightMargin = idCardSizePortrait.Width - 10f; // End position of the line (right side)

            // Draw the line
            canvas.MoveTo(leftMargin, yLinePosition);
            canvas.LineTo(rightMargin, yLinePosition);
            canvas.Stroke();
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase(combined1, fontHeaderSmall), idCardSizePortrait.Width / 2, yLinePosition - 10f, 0);

            // Combine the second set of text into one line
            string combined2 = "CIVIL STATUS\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0  " +
                               "BLOOD TYPE\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0 DATE ISSUED";

            // Draw the second line above combined2 text, move down from the first text
            float yLinePosition2 = yLinePosition - 35f; // Move it further down for combined2

            // Draw the line above combined2
            canvas.MoveTo(leftMargin, yLinePosition2);
            canvas.LineTo(rightMargin, yLinePosition2);
            canvas.Stroke();

            // Add the second combined header in one line
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase(combined2, fontHeaderSmall), idCardSizePortrait.Width / 2, yLinePosition2 - 10f, 0);

            // Combine the second set of text into one line
            string combined3 = "DATE OF EXPIRY\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0 " +
                               "CONTACT NUMBER\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0 PRECINCT NUMBER";

            // Draw the second line above combined2 text, move down from the first text
            float yLinePosition3 = yLinePosition2 - 35f; // Move it further down for combined2

            // Draw the line above combined2
            canvas.MoveTo(leftMargin, yLinePosition3);
            canvas.LineTo(rightMargin, yLinePosition3);
            canvas.Stroke();

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase(combined3, fontHeaderSmall), idCardSizePortrait.Width / 2, yLinePosition3 - 10f, 0);

            // Add the second combined header in one line
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase($"CONTACT PERSON IN CASE OF EMERGENCY", fontRegular), idCardSizePortrait.Width / 2, yLinePosition3 - 30f, 0);



            // Draw the second line above combined2 text, move down from the first text
            float yLinePosition4 = yLinePosition3 - 50f; // Move it further down for combined2

            // Draw the line above combined2
            canvas.MoveTo(leftMargin, yLinePosition4);
            canvas.LineTo(rightMargin, yLinePosition4);
            canvas.Stroke();

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("NAME", fontHeaderSmall), idCardSizePortrait.Width / 2, yLinePosition4 - 10f, 0);


            // Draw the second line above combined2 text, move down from the first text
            float yLinePosition5 = yLinePosition4 - 25f; // Move it further down for combined2

            // Draw the line above combined2
            canvas.MoveTo(leftMargin, yLinePosition5);
            canvas.LineTo(rightMargin, yLinePosition5);
            canvas.Stroke();

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("CONTACT NUMBER", fontHeaderSmall), idCardSizePortrait.Width / 2, yLinePosition5 - 10f, 0);





            // Define the text you want to center as a paragraph
            string certify = "This is to certify that the person whose name, photograph,\n and signature appear herein is a bonafide resident of Barangay 22, Zone 2, District II, Caloocan City." +
                             "\nIf lost and found, please return this ID to Barangay 22, Zone 2, District II, Caloocan City.";

            // Create a Paragraph to hold the text
            Paragraph paragraph = new Paragraph(certify, fontHeaderSmaller)
            {
                Alignment = Element.ALIGN_CENTER // Set paragraph alignment to center
            };

            // Define the Y-position of the paragraph
            float yPosition = yLinePosition5 - 20f;

            // Add the paragraph to the canvas, wrapped in a ColumnText object
            ColumnText column = new ColumnText(canvas);
            column.SetSimpleColumn(new Rectangle(idCardSizePortrait.Left, yPosition - 50f, idCardSizePortrait.Right, yPosition)); // Define the rectangle area where the text will fit
            column.AddElement(paragraph);
            column.Go(); // Render the paragraph

            float footpos = yLinePosition5 - 40f;
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("ISSUED BY:", fontHeaderSmall), idCardSizePortrait.Width / 2, footpos, 0);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("RONALD B. BAUTISTA", fontHeaderSmall), idCardSizePortrait.Width / 2, footpos - 13f, 0);



            // Draw the line above combined2
            canvas.MoveTo(leftMargin, footpos - 15f);
            canvas.LineTo(rightMargin, footpos - 15f);
            canvas.Stroke();
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("Punong Barangay", fontHeaderSmall), idCardSizePortrait.Width / 2, footpos - 20f, 0);




        }
        private void GetDataFromMySQL()
        {
            string connectionString = "server=localhost;uid=root;pwd=dedengtangkad;database=trial_connection";
            string query = "INSERT INTO forpdf (FirstName, LastName, Address, PhoneNumber) VALUES (@FirstName, @LastName, @Address, @PhoneNumber)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Address", textBox3.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", textBox4.Text);

                        // Open the connection
                        conn.Open();

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Close the connection
                        conn.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
