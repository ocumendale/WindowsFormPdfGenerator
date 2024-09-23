using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.IO;

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

            // Add image
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("D:\\Programs c#\\PdfGenerator\\pictures\\scene.jpg");
            doc.Add(img);

            // Add a paragraph
            Paragraph name = new Paragraph($"Name: {textBox1.Text} {textBox2.Text}");
            Paragraph address = new Paragraph($"Address: {textBox3.Text}");
            Paragraph number = new Paragraph($"Phone Number: {textBox4.Text}");
            doc.Add(name);
            doc.Add(address);
            doc.Add(number);

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
