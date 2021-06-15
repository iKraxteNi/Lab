using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public Student student;



        public StudentWindow(Student student = null)
        {
            InitializeComponent();
            if (student != null)
            {
                tbImie.Text = student.imie;
                tbNazwisko.Text = student.nazwisko;
                tbNr.Text = student.NrIndeksu.ToString();
                tbWydzial.Text = student.wydzial;
            }
            this.student = student ?? new Student();
        }

        private void bok_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(tbImie.Text, @"^\p{Lu}\p{Ll}{1,12}$") ||
                !Regex.IsMatch(tbNazwisko.Text, @"^\p{Lu}\p{Ll}{1,12}$") ||
                !Regex.IsMatch(tbWydzial.Text, @"^\p{Lu}{1,12}$") ||
                !Regex.IsMatch(tbNr.Text, @"^[0-9]{4,10}$")
                )
            {
                MessageBox.Show("Podano błedne dane.");
                return;

            }
            student.imie = tbImie.Text;
            student.nazwisko = tbNazwisko.Text;
            student.NrIndeksu = int.Parse(tbNr.Text);
            student.wydzial = tbWydzial.Text;

            this.DialogResult = true;

        }
    }
}
