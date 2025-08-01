using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using test_task.Parsers;
using test_task.Utils;

namespace test_task
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string InputText { get; private set; }

        public string AnalysisResult { get; private set; }


        public string SelectedFileName { get; private set; }
        public string SelectedFilePath { get; private set; }

        public bool Analyzing { get; private set; }

        private void Analyze(object sender, RoutedEventArgs e)
        {
            AnalyzeButton.IsEnabled = false;
            Logger.LogInfo("Вызван анализ");

            // Анализ, если есть файл
            if (SelectedFilePath != null && SelectedFilePath != "")
            {
                Logger.LogInfo($"Есть выбранный файл \"{SelectedFileName}\"  - читаем данные с него");

                string file_one_string = System.IO.File.ReadAllText(SelectedFilePath);

                AnalysisResult = StringAnalyzer.Analyze(file_one_string);
            }
            else
            {
                Logger.LogError("Нет выбранного файла для анализа");
            }

            // Анализ, если есть введённое в строку значение
            InputText = InputTextBox.Text;
            if (InputText != null && InputText != "")
            {
                Logger.LogError("Есть входная строка");

                AnalysisResult = StringAnalyzer.Analyze(InputText);
            }
            else
            {
                Logger.LogError("Нет входных данных для анализа");
            }

            if(AnalysisResult ==  string.Empty || AnalysisResult == null)
            {
                AnalysisResult = "Ошибка обработки, ничего обработать удалось:(\n";
            }

            AnalysisResultTextBox.Text = AnalysisResult;

            AnalyzeButton.IsEnabled = true;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Logger.LogInfo("Вызвана очистка полей ввода");
            InputText = string.Empty;
            AnalysisResult = string.Empty;
            AnalysisResultTextBox.Text = string.Empty;
            InputTextBox.Text = string.Empty;
        }

        public void SetSelectedFile(string filePath)
        {
            SelectedFilePath = filePath;
            SelectedFileName = System.IO.Path.GetFileName(filePath);
            FileNameTextBlock.Text = SelectedFileName;
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
                SetSelectedFile(openFileDialog.FileName);
        }
    }
}


