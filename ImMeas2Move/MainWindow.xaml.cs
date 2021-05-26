using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ImMeas2Move
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "MOD file (*.mod)|*.mod",
                Multiselect = false,
                CheckFileExists = true,
                RestoreDirectory = true
            };

            if(dialog.ShowDialog() ?? false)
            {
                textBox.Text = dialog.FileName;
                progressBar.Value = 0;
            }
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(textBox.Text)) return;

            var dialog = new SaveFileDialog
            {
                Filter = "MOD file (*.mod)|*.mod",
                RestoreDirectory = true,
                OverwritePrompt = true
            };

            if (dialog.ShowDialog() ?? false)
            {
                await Converter.Convert(textBox.Text, dialog.FileName, new Progress<ProgressUpdate>(p =>
                {
                    progressBar.Maximum = p.Max;
                    progressBar.Value = p.Value;
                }));
            }
        }
    }
}
