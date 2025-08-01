using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using test_task.Parsers;
using test_task.Utils;

namespace test_task.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _inputText;
        private string _analysisResult;
        private string _selectedFilePath;
        private string _selectedFileName;
        public bool Analyzing { get; private set; }
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged("InputText");
            }
        }

        public string AnalysisResult
        {
            get => _analysisResult;
            set
            {
                _analysisResult = value;
                OnPropertyChanged("AnalysisResult");
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
                    OnPropertyChanged("SelectedFilePath");
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
                    OnPropertyChanged("SelectedFileName");
                }
            }
        }
        public ICommand AnalyzeCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddFileCommand { get; }



        public bool CanAnalyze()
        {
            return !Analyzing;
        }

        public MainViewModel()
        {
            Analyzing = false;
            AnalyzeCommand = new RelayCommand(Analyze, CanAnalyze);
            ClearCommand = new RelayCommand(Clear);
            AddFileCommand = new RelayCommand(AddFile);
        }

        public static int IndexOfSubstring(string content, string substring, int startIndex = 0)
        {
            if (content == null || content == "")
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - передана пустая строка");
                return -1;
            }

            if (substring == null || substring == "")
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - передана пустая подстрока");
                return -1;
            }

            if (startIndex < 0 || startIndex > content.Length)
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - стартовый индекс за границей входной строки");
                return -1;
            }

            if (startIndex + substring.Length > content.Length)
            {
                Logger.LogError("Ошиибка поиска в IndexOfSubstring - стартовый индекс + длина подстроки выходит за границы входной строки");
                return -1;
            }

            for (int i = startIndex; i < content.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < substring.Length; j++)
                {
                    if (i + j > content.Length - 1 || content[i + j] != substring[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    Logger.LogInfo($"IndexOfSubstring нашёл подстроку \"{substring}\" в строке на индексе {i}");
                    return i;
                }
            }
            Logger.LogInfo($"IndexOfSubstring не нашёл подстроку \"{substring}\" в строке, анализ с индекса {startIndex}");
            return -1;
        }

        private List<(string preamble, string payload)> ExtractMessages(string content)
        {
            Logger.LogInfo("Начат поиск сообщений");
            var messages = new List<(string preamble, string payload)>();
            var preambles = MainParser.GetPreambles();

            foreach (var preamble in preambles)
            {
                int currentIndex = 0;
                while ((currentIndex = IndexOfSubstring(content, preamble, currentIndex)) != -1)
                {
                    Logger.LogInfo($"Найдена преамбула \"{preamble}\" по индексу \"{currentIndex}\"");

                    int payloadStart = currentIndex + preamble.Length;
                    int endOfPayload = IndexOfSubstring(content, "\r\n", payloadStart);
                    if (endOfPayload == -1)
                    {
                        Logger.LogError($"Не найден конец полезной информации для преамбулы \"{preamble}\"");
                        break;
                    }

                    string payload = content.Substring(payloadStart, endOfPayload - payloadStart);

                    currentIndex = payloadStart + 2; // сдвиг на \r\n

                    if (payload == null || payload == "")
                    {
                        Logger.LogInfo($"Полезная информация для преамбулы \"{preamble}\" пустая");

                        continue;
                    }

                    Logger.LogInfo($"Успешно найдена полезная информация для \"{preamble}\"");

                    messages.Add((preamble, payload));
                }
            }
            Logger.LogInfo("Закончен поиск сообщений во входных данных");
            return messages;
        }

        private void Analyze()
        {
            Analyzing = true;
            Logger.LogInfo("Вызван анализ");

            // Анализ, если есть файл
            if (SelectedFilePath != null && SelectedFilePath != "")
            {
                Logger.LogInfo($"Есть выбранный файл \"{SelectedFileName}\"  - читаем данные с него");

                string file_one_string = System.IO.File.ReadAllText(SelectedFilePath);

                List<(string preamble, string payload)> extractedMessages = ExtractMessages(file_one_string);

                if (extractedMessages.Count == 0)
                {
                    Logger.LogInfo("Не найдено сообщений для анализа в файле");
                }
                else
                {
                    foreach (var (preamble, payload) in extractedMessages)
                    {
                        Logger.LogInfo($"Попытка разобрать сообщение: \"{preamble}\" / \"{payload}\"");
                        if (MainParser.Parse(preamble, payload, out var parsedResult))
                        {
                            if (!string.IsNullOrEmpty(parsedResult))
                                AnalysisResult += parsedResult + "\n";
                        }
                    }
                }

            }
            else
            {
                Logger.LogError("Нет выбранного файла для анализа");
            }

            // Анализ, если есть введённое в строку значение
            if (InputText != null && InputText != "")
            {
                Logger.LogError("Есть входная строка");

                List<(string preamble, string payload)> extractedMessages = ExtractMessages(InputText);

                if (extractedMessages.Count == 0)
                {
                    Logger.LogInfo("Не найдено сообщений для анализа в строке");
                }
                else
                {
                    foreach (var (preamble, payload) in extractedMessages)
                    {
                        Logger.LogInfo($"Попытка разобрать сообщение: \"{preamble}\" \"{payload}\"");
                        if (MainParser.Parse(preamble, payload, out var parsedResult))
                        {
                            if (!string.IsNullOrEmpty(parsedResult))
                                AnalysisResult += parsedResult + "\n";
                        }
                    }
                }
            }
            else
            {
                Logger.LogError("Нет входных данных для анализа строки");
            }

            if (AnalysisResult == string.Empty || AnalysisResult == null)
            {
                AnalysisResult = "Ошибка обработки, ничего обработать удалось:(\n";
            }
            Analyzing = false;
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
