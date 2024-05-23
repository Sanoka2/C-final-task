using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main_Window_Class : Window
    {
        const string Default_Salt = "OPrus";      //Oleksandr Prus
        const int Default_Password_Length = 2;    //here password length

        byte[] ascii_Range = new byte[75];

        byte[] target_Hash;

        public Main_Window_Class()
        {
            InitializeComponent();
            for (byte i = 0; i < 75; i++)
            {
                ascii_Range[i] = (byte)(i + (byte)48);
            }
            //passwordCharacters = asciiRange.Select(i => (byte)i).ToArray();
            /*
             I tried to make useble UI. But it throw me always two errors
             with wich I can not deal in any way
             more details in raport            
             */
            //generateButton_Click();
            //Thread.Sleep(5000);
            //crackButton_Click();


        }

        private string Create_Salt()
        {
            string salt_Input = salt_Text_Box.Text;
            if (string.IsNullOrEmpty(salt_Input))
            {
                salt_Input = Default_Salt;
            }
            return salt_Input;
        }

        private int Get_Max_Password_Length()
        {
            if (int.TryParse(password_Length_Text_Box.Text, out int length))
            {
                return length;
            }
            return Default_Password_Length;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //generateButton.IsEnabled = true;
            string salt = Create_Salt();

            Password_Creator_Class generator = new Password_Creator_Class(salt, ascii_Range, Get_Max_Password_Length());
            string generated_Password = generator.Generate_Password();
            //generatedPasswordLabel.Content = "kal";
            generated_Password_Label.Content = generated_Password;

            target_Hash = Password_Creator_Class.Compute_Hash(generated_Password);

            string doc_Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path_to_passwords = System.IO.Path.Combine(doc_Path, "passwords_list.txt");
            using (StreamWriter output_File = new StreamWriter(path_to_passwords, true))
            {
                output_File.WriteLine(Encoding.UTF8.GetString(target_Hash));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string salt = Create_Salt();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            resultLabel.Text = new Password_Cracker_Class().Find_Password(ascii_Range, Get_Max_Password_Length(), salt, target_Hash);
            stopwatch.Stop();
            used_Time_Label.Text = stopwatch.ElapsedMilliseconds.ToString() + " milliseconds";
        }
    }
}