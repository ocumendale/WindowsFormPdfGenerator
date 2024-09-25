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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a PDF document
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("example.pdf", FileMode.Create));

            // Open the document to write content
            doc.Open();

            /* Add image
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("D:\\Programs c#\\PdfGenerator\\pictures\\scene.jpg");
            doc.Add(img);*/

            iTextSharp.text.Image background = iTextSharp.text.Image.GetInstance("D:\\Barangay Picture\\Caloocan_City.png");

            float imageWidth = 500f;
            float imageHeight = 300f;
            background.ScaleAbsolute(imageWidth, imageHeight);

            // Get the page size
            Rectangle pageSize = doc.PageSize;
            float pageWidth = pageSize.Width;
            float pageHeight = pageSize.Height;

            // Calculate the center position
            float xPosition = (pageWidth - imageWidth) / 2;
            float yPosition = (pageHeight - imageHeight) / 2;

            // Set the absolute position to center the image
            background.SetAbsolutePosition(xPosition, yPosition);

            // Create a PdfGState to control the opacity
            PdfGState gState = new PdfGState();
            gState.FillOpacity = 0.1f; // Set opacity (0.0f to 1.0f, where 1.0 is fully opaque)

            // Add the background image with opacity
            PdfContentByte canvas = writer.DirectContent;
            canvas.SaveState();
            canvas.SetGState(gState);
            canvas.AddImage(background);
            canvas.RestoreState();


            background.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
            background.SetAbsolutePosition(0, 0); // Position it at the bottom-left corner
            // Add the image to the document

            // Header of the document
            iTextSharp.text.Font font1bold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font font1 = FontFactory.GetFont(FontFactory.TIMES, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font font = FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontTitle = FontFactory.GetFont(FontFactory.TIMES_BOLD, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontRight = FontFactory.GetFont(FontFactory.TIMES_BOLD, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            Paragraph headerClearance = new Paragraph($"Republic City of the Philippines", font1bold);          
            Paragraph headerClearance1 = new Paragraph($"City of Caloocan",font);            
            Paragraph headerClearance2 = new Paragraph($"OFFICE OF THE PUNONG BARANGAY",font1);
            Paragraph headerClearance3 = new Paragraph($"Barangay 22, Zone 2, District II", font);
            Paragraph title = new Paragraph($"\n\nBARANGAY CERTIFICATION", fontTitle);
            Paragraph bodyIndigency = new Paragraph($"\n\n\t\t\tThis is to Certify that {textBox1.Text} {textBox2.Text}., A resident of {textBox3.Text} will use this Barangay Certification.\n" +
                $"\n\t\tThis certification is issued for whatever legal purpose or purposes this may serve.\n" +
                $"\n\t\tSigned this on the (Date Today) at BARANGAY 22 ZONE 2 DISTRICT II , CALOOCAN CITY, NATIONAL CAPITAL REGION, PHILIPPINES.\n" +
                $"\n\t\tThis certification is valid only for 1 year from the issuance.\n");

            Paragraph signature = new Paragraph($"\n\nSIGNATURE", fontTitle);
            Paragraph punongBarangay = new Paragraph($"Punong Barangay", font);
            Paragraph chairMan = new Paragraph($"RONALDO B. BAUTISTA", fontRight);
            Paragraph dateRight = new Paragraph($"DATE TODAY", font);

            Paragraph witness = new Paragraph($"\n\nWitnessed by:", font);
            Paragraph wSignature = new Paragraph($"SIGNATURE", fontTitle);
            Paragraph witnessMan = new Paragraph($"ANTHONY S. MULAWIN", fontRight);
            Paragraph pos = new Paragraph($"Secretary", font);
            

            headerClearance.Alignment = Element.ALIGN_CENTER;
            headerClearance1.Alignment = Element.ALIGN_CENTER;
            headerClearance2.Alignment = Element.ALIGN_CENTER;
            headerClearance3.Alignment = Element.ALIGN_CENTER;
            title.Alignment = Element.ALIGN_CENTER;

            signature.Alignment = Element.ALIGN_RIGHT;
            punongBarangay.Alignment = Element.ALIGN_RIGHT;
            chairMan.Alignment = Element.ALIGN_RIGHT;
            dateRight.Alignment = Element.ALIGN_RIGHT;
            witness.Alignment = Element.ALIGN_RIGHT;
            chairMan.Alignment = Element.ALIGN_RIGHT;
            dateRight.Alignment = Element.ALIGN_RIGHT;
            witness.Alignment = Element.ALIGN_RIGHT;
            wSignature.Alignment = Element.ALIGN_RIGHT;
            witnessMan.Alignment = Element.ALIGN_RIGHT;
            pos.Alignment = Element.ALIGN_RIGHT;

            doc.Add(headerClearance);
            doc.Add(headerClearance1);
            doc.Add(headerClearance2);
            doc.Add(headerClearance3);
            doc.Add(title);
            doc.Add(bodyIndigency);

            doc.Add(signature);
            doc.Add(chairMan);
            doc.Add(punongBarangay);
            doc.Add(dateRight);

            doc.Add(witness);
            doc.Add(wSignature);
            doc.Add(witnessMan);
            doc.Add(pos);
            doc.Add(dateRight);



            // ---------------------------------------------------------



            Paragraph address = new Paragraph($"");
            Paragraph number = new Paragraph($"");
            
            

            // Close the document
            doc.Close();

            MessageBox.Show("PDF GENERATED SUCCESSFULLY!");
            GetDataFromMySQL();
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
