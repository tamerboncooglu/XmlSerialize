using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Settings settings = new Settings();

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            settings = DeserializeFromXML();
            foreach (var movie in settings.Movies)
            {
                listBox1.Items.Add(movie.Title);
            }

            txtEmail.Text = settings.UserInfo.Email;
            txtPassword.Text = settings.UserInfo.Password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var movie = new Movie()
                {
                    Director = txtDirector.Text,
                    Name = txtName.Text,
                    Title = txtTitle.Text,
                    Year = txtYear.Text
                };

            settings.Movies.Add(movie);

            settings.UserInfo = new UserInfo
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Text
                };

            SerializeToXML(settings);

            MessageBox.Show("Kayıt Oluşturuldu");

            txtDirector.Clear();
            txtName.Clear();
            txtTitle.Clear();
            txtYear.Clear();

            Form1_Load(null, null);
        }

        public class Settings
        {
            public List<Movie> Movies { get; set; }
            public UserInfo UserInfo { get; set; }
        }

        public class Movie
        {
            [XmlAttribute("Title")]
            public string Title { get; set; }
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("Year")]
            public string Year { get; set; }
            [XmlAttribute("Director")]
            public string Director { get; set; }
        }

        public class UserInfo
        {
            [XmlAttribute("Email")]
            public string Email { get; set; }
            [XmlAttribute("Password")]
            public string Password { get; set; }
        }

        static public void SerializeToXML(Settings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\settings.xml");
            serializer.Serialize(writer, settings);
            writer.Close();
            serializer = null;
            writer.Dispose();
            writer = null;
            GC.Collect();
        }

        static Settings DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            FileStream fs = new FileStream(Application.StartupPath + "\\settings.xml", FileMode.Open);
            Settings settings = (Settings)deserializer.Deserialize(fs);
            deserializer = null;
            fs.Dispose();
            fs = null;
            GC.Collect();
            return settings;
        }
    }
}
