using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using Microsoft.Win32;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Student> ListaStudentow { get; set; }
        private string saveLocation = "data.xml";


        public MainWindow()
        {
            ListaStudentow = new List<Student>();

            var student2 = new Student("Jan", "Kowalski", 1234, "KIS");

            ListaStudentow.Add(student2);




            InitializeComponent();

            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Imię", Binding = new Binding("imie") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Nazwikso", Binding = new Binding("nazwisko") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "NR INdeksu", Binding = new Binding("NrIndeksu") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Wydział", Binding = new Binding("wydzial") });


            dgStudent.AutoGenerateColumns = false;
            dgStudent.ItemsSource = ListaStudentow;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ADD_Student_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StudentWindow();
            if (dialog.ShowDialog() == true)
            {
                ListaStudentow.Add(dialog.student);
                dgStudent.Items.Refresh();
            }
        }

        private void R_Student_Click(object sender, RoutedEventArgs e)
        {
            if (dgStudent.SelectedItem is Student)
            {
                ListaStudentow.Remove((Student)dgStudent.SelectedItem);
                dgStudent.Items.Refresh();
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream("data.txt", FileMode.Create);
                sw = new StreamWriter(fs);
                foreach (Student student in ListaStudentow)
                {
                    Save(student, sw);
                }

                MessageBox.Show("Saved to file");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save file");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }


        private void Save<T>(T ob, StreamWriter sw)
        {
            Type t = ob.GetType();
            sw.WriteLine($"[[{t.FullName}]]");
            foreach (var prop in t.GetProperties())
            {
                sw.WriteLine($"[{prop.Name}]");
                sw.WriteLine($"{prop.GetValue(ob)}");
            }

            sw.WriteLine("[[]]");
        }

        private T Load<T>(StreamReader sr) where T : new()
        {
            T obj = default(T);
            Type objType = null;
            PropertyInfo propertyInfo = null;

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line == "[[]]")
                {
                    return obj;
                }
                else if (line.StartsWith("[["))
                {
                    objType = Type.GetType(line.Trim('[', ']'));
                    if (typeof(T).IsAssignableFrom(objType))
                    {
                        obj = (T)Activator.CreateInstance(objType);
                    }
                }
                else if (line.StartsWith("[") && obj != null)
                {
                    propertyInfo = objType.GetProperty(line.Trim('[', ']'));
                }
                else if (obj != null)
                {
                    propertyInfo.SetValue(obj, Convert.ChangeType(line, propertyInfo.PropertyType));
                }
            }

            return default(T);
        }

        private void saveLocationChangeButton_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                saveLocation = dialog.FileName;
                saveLocationLabel.Content = dialog.FileName;
                saveLocationLabel.Visibility = Visibility.Visible;
            }
        }

        private void xmlSave_Click_1(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(saveLocation, FileMode.Create);
                sw = new StreamWriter(fs);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                serializer.Serialize(fs, ListaStudentow);
                MessageBox.Show("Saved to xml file");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save xml file");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        private void xmlLoad_Click_1(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(saveLocation, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                List<Student> data = (List<Student>)serializer.Deserialize(fs);
                ListaStudentow.Clear();
                foreach (Student student in data)
                {
                    ListaStudentow.Add(student);
                }

                dgStudent.Items.Refresh();
                MessageBox.Show("Loaded data from xml file");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load data from xml file");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private void loadButton_Click_1(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                fs = new FileStream("data.txt", FileMode.Open);
                sr = new StreamReader(fs);
                ListaStudentow.Clear();
                ListaStudentow.Add(Load<Student>(sr));
                dgStudent.Items.Refresh();
                MessageBox.Show("Loaded data from file");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load data from file");
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
    }
}

