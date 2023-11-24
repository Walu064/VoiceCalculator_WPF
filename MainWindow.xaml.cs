using System.Windows;
using System.Windows.Media;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Data;
using NAudio.CoreAudioApi;
using System.Text.RegularExpressions;
using Humanizer;

namespace VoiceCalculator
{
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognitionEngine recognizer;
        private readonly Choices words;
        private readonly DataTable dataTable;
        private readonly MMDeviceEnumerator enumerator;
        private readonly SpeechSynthesizer speechSynthesizer;
        private readonly GrammarBuilder? grammarBuilder;
        private readonly Grammar? grammar;

        public MainWindow()
        {
            InitializeComponent();
            recognizer = new SpeechRecognitionEngine();
            words = new Choices("0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "plus", "dodać", "dodaj", "razy", "pomnóż", "mnożyć", "podziel", "podzielić",
                "przez", "dziel", "odjąć", "odejmij", "minus", "wykonaj", "wyczyść");

            grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(words);

            enumerator = new MMDeviceEnumerator();
            grammar = new Grammar(grammarBuilder);
            dataTable = new DataTable();
            dataTable.Columns.Add("expression", typeof(string));
            recognizer.LoadGrammar(grammar);
            printEquationLabel.Content = "";
            recognizer.SpeechRecognized += (s, e) =>
            {
                recordingStatus.Fill = Brushes.Green;
                if (e.Result != null && e.Result.Confidence > 0.5)
                {
                    string recognizedText = e.Result.Text;
                    UpdateLabel(recognizedText);
                }
            };
            recordingStatus.Fill = Brushes.Red;
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            
            var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            detectedMicrophoneLabel.Content += defaultDevice.FriendlyName;

            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
        }

        private void ShowHelpDialog(object sender, RoutedEventArgs e)
        {
            HelpDialogWindow helpDialogWindow = new();
            helpDialogWindow.ShowDialog();
        }

        private void ShowCreditsDIalog(object sender, RoutedEventArgs e)
        {
            CreditsDialogWindow creditsDialogWindow = new();
            creditsDialogWindow.ShowDialog();
        }

        private void UpdateLabel(string text)
        {
            if (Regex.IsMatch(text, "^[0-9]$"))
            {
                printEquationLabel.Content += text;
            }
            else 
            {
                switch (text)
                {
                    case "dodaj":
                    case "dodać":
                    case "plus":
                        printEquationLabel.Content += " + ";
                        break;

                    case "odejmij":
                    case "odjąć":
                    case "minus":
                        printEquationLabel.Content += " - ";
                        break;

                    case "pomnóż":
                    case "razy":
                    case "mnożyć":
                        printEquationLabel.Content += " * ";
                        break;

                    case "podziel":
                    case "podzielić":
                    case "dziel":
                    case "przez":
                        printEquationLabel.Content += " / ";
                        break;

                    case "wyczyść":
                        printEquationLabel.Content = "";
                        break;

                    case "wykonaj":
                        string equation = printEquationLabel.Content.ToString();
                        string[] parts = equation.Split(' ');

                        if (parts.Length != 3)
                        {
                            printEquationLabel.Content = "Błędne równanie";
                            speechSynthesizer.SpeakAsync(printEquationLabel.Content.ToString());
                            break;
                        }

                        int leftOperand, rightOperand;

                        if (!int.TryParse(parts[0], out leftOperand) || !int.TryParse(parts[2], out rightOperand))
                        {
                            printEquationLabel.Content = "Błędne równanie";
                            speechSynthesizer.SpeakAsync(printEquationLabel.Content.ToString());
                            break;
                        }

                        string resultAsString = "", leftOperandAsString = "", operatorAsString = "", rightOperandAsString = "";

                        int result = 0;
                        switch (parts[1])
                        {
                            case "+":
                                result = leftOperand + rightOperand;
                                resultAsString = result.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                leftOperandAsString = leftOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                operatorAsString = "dodać";
                                rightOperandAsString = rightOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                break;
                            case "-":
                                result = leftOperand - rightOperand;
                                resultAsString = result.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                leftOperandAsString = leftOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                operatorAsString = "odjąć";
                                rightOperandAsString = rightOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                break;
                            case "*":
                                result = leftOperand * rightOperand;
                                resultAsString = result.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                leftOperandAsString = leftOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                operatorAsString = "razy";
                                rightOperandAsString = rightOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                break;
                            case "/":
                                if (rightOperand == 0)
                                {
                                    printEquationLabel.Content = "Błąd";
                                    return;
                                }
                                result = leftOperand / rightOperand;
                                resultAsString = result.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                leftOperandAsString = leftOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                operatorAsString = "podzielić przez";
                                rightOperandAsString = rightOperand.ToWords(new System.Globalization.CultureInfo("pl-PL"));
                                break;
                        }
                        string resultToPrint = printEquationLabel.Content + " = " + result.ToString();
                        printEquationLabel.Content = resultToPrint;
                        string resultToSpeech = "Wynik działania "+leftOperandAsString+" "+operatorAsString+" "+rightOperandAsString+" to "+resultAsString;
                        speechSynthesizer.SpeakAsync(resultToSpeech);
                        break;
                }
            }
        }
    }
}

