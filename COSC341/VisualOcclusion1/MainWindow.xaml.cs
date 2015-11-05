using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VisualOcclusion1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        public List<String> Exerpts = new List<string> {
            "There was once a girl who was lazy and would not spin, and her mother could not persuade her to it, do what she would. At last the mother became angry and out of patience, and gave her a good beating, so that she cried out loudly. At that moment the Queen was going by; as she heard the crying, she stopped; and, going into the house, she asked the mother why she was beating her daughter, so that every one outside in the street could hear her cries. The woman was ashamed of her daughter's laziness, she said, I cannot stop her from spinning.",
            "There lived a king and queen, who said to each other every day of their lives, \"Would that we had a child!\" and yet they had none. But it happened once that when the queen was bathing, there came a frog out of the water, and he squatted on the ground, and said to her: \"Thy wish shall be fulfilled; before a year has gone by, thou shalt bring a daughter into the world.\" And as the frog foretold, so it happened; and the queen bore a daughter so beautiful that the king could not contain himself for joy, and he ordained a great feast.",
            "There was once a rich man whose wife lay sick, and when she felt her end drawing near she called to her only daughter to come near her bed, and said, \"Dear child, be good, and God will take care of you, and I will look down upon you from heaven.\" And then she closed her eyes forever. The maiden went daily to her mother's grave and wept, and was always pious and good. When the winter came, snow covered the grave with a white covering, when the sun came in the early spring, it melted away, the man took to himself another wife."
        };

        private Int32 mCurrentChar = -1;
        private Int32 mTriggerChar = 0;
        private Random mRandom = new Random();

        private List<Int64> mData = new List<Int64>();
        private Stopwatch mTimer = new Stopwatch();

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender,RoutedEventArgs e) {
            story.Selection.Text = Exerpts[new Random().Next(0,Exerpts.Count)];
            textBox.Focus();
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.MaxLength = story.Selection.Text.Length;
            mTimer.Start();
        }

        private void textBox_TextChanged(object sender,TextChangedEventArgs e) {
            story.SelectAll();
            if(textBox.MaxLength == textBox.Text.Length) { ExportData(); Application.Current.Shutdown(); }

            foreach(TextChange i in e.Changes) {
                if(textBox.Text.Substring(i.Offset,i.AddedLength) == " ") {
                    mCurrentChar = -1;
                    Int32 length = story.Selection.Text.Substring(i.Offset + 1,story.Selection.Text.Substring(i.Offset + 1).IndexOf(' ')).Length;
                    mTriggerChar = mRandom.Next(0,length);
                    mData.Add(mTimer.ElapsedMilliseconds);
                    mTimer.Restart();
                }
            }

            story.Selection.ApplyPropertyValue(RichTextBox.ForegroundProperty,Brushes.Black);
            string textToPos = textBox.Text;
            string textToRetypeToPos = story.Selection.Text.Substring(0,textToPos.Length);
            story.Selection.Select(story.Selection.Start,story.Selection.Start.GetPositionAtOffset(textToPos.Length));
            if(textToPos == textToRetypeToPos) story.Selection.ApplyPropertyValue(RichTextBox.ForegroundProperty,Brushes.Green);
            else story.Selection.ApplyPropertyValue(RichTextBox.ForegroundProperty,Brushes.Red);

            if(mCurrentChar == mTriggerChar) {

                popup_image.Height = popup_image.Width = mRandom.Next(100,351);
                popup_image.Source = (BitmapImage)Resources[mRandom.Next(1,6).ToString()];
                Rect rect = textBox.GetRectFromCharacterIndex(textBox.CaretIndex,true);
                Point coords = textBox.TransformToAncestor(this).Transform(rect.Location);
                popup.Margin = new Thickness(coords.X - popup_image.RenderSize.Width / 2,coords.Y - popup_image.RenderSize.Height / 2 + textBox.FontSize / 2,0,0);
                popup.Visibility = Visibility.Visible;
            }
            ++mCurrentChar;
        }

        private void ClosePopup(object sender,MouseButtonEventArgs e) {
            popup.Visibility = Visibility.Hidden;
        }

        private String SerializeList() {
            String s = "WordIndex, Time(ms)\r\n";
            for(int i = 0; i < mData.Count; ++i) s += i.ToString() + "," + mData[i].ToString() + "\r\n";
            return s;
        }

        protected void ExportData() {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Comma Separated List (.csv)|*.csv";
            sfd.ShowDialog();
            string filename = sfd.FileName;
            string contents = SerializeList();
            if(String.IsNullOrWhiteSpace(filename)) {
                MessageBoxResult m = MessageBox.Show("Are you sure you would like to quit without saving?","Are you sure?",MessageBoxButton.YesNo);
                if(m == MessageBoxResult.No) {
                    ExportData();
                }
                return;
            }
            try {
                File.WriteAllText(filename,contents);
            } catch(IOException) {
                MessageBox.Show("The file could not be saved. If the file is being used in another program, please create a new file.");
                ExportData();
            }
        }
    }
}
