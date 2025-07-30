using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using test_task.Parsers;

namespace test_task.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _inputText;
        private string _analysisResult;
        private string _selectedFilePath;
        private string _selectedFileName;

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AnalysisResult
        {
            get => _analysisResult;
            set
            {
                _analysisResult = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged(nameof(SelectedFilePath));
                }
            }
        }
        public string SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                if (_selectedFileName != value)
                {
                    _selectedFileName = value;
                    OnPropertyChanged(nameof(SelectedFileName));
                }
            }
        }

        public ICommand AnalyzeCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddFileCommand { get; }

        public MainViewModel()
        {
            AnalyzeCommand = new RelayCommand(Analyze, () => !string.IsNullOrWhiteSpace(InputText) || !string.IsNullOrWhiteSpace(SelectedFileName));
            ClearCommand = new RelayCommand(Clear);
            AddFileCommand = new RelayCommand(AddFile);
        }

        private void Analyze()
        {
            if (!string.IsNullOrWhiteSpace(SelectedFileName) && !string.IsNullOrWhiteSpace(SelectedFilePath))
            {
                try
                {
                    var lines = System.IO.File.ReadAllLines(SelectedFilePath);
                    var results = new System.Text.StringBuilder();
                    foreach (var line in lines)
                    {
                        var result = MainParser.Parse(line);
                        if(result  != string.Empty)
                            results.AppendLine(result.ToString());
                    }
                    AnalysisResult = results.ToString();
                }
                catch (Exception ex)
                {
                    AnalysisResult = $"Error reading file: {ex.Message}";
                }
            }
            else
            {
                var result = MainParser.Parse(InputText);
                if (result != string.Empty)
                    AnalysisResult = result.ToString();
            }
        }
        private void Clear()
        {
            InputText = string.Empty;
            AnalysisResult = string.Empty;
        }
        public void SetSelectedFile(string filePath)
        {
            SelectedFilePath = filePath;
            SelectedFileName = System.IO.Path.GetFileName(filePath);
        }

        private void AddFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
                SetSelectedFile(openFileDialog.FileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}