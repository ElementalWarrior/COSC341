using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace CircleMenu_Toolbar {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        Random mRandom = new Random();
        Stopwatch mTimer = new Stopwatch();
        private Byte mCurrentTask = 1;
        private List<Datum> mData = new List<Datum>();

        private enum Buttons {
            DRAW,
            ERASE,
            BLACK,
            RED,
            BLUE
        };

        private struct Datum {
            public Buttons button;
            public Int64 millseconds;
        };

        public MainWindow() {
            InitializeComponent();

            this.MouseRightButtonUp += RightClick;
            this.MouseLeftButtonUp += CloseToolbar;
            this.toolbar.MouseRightButtonUp += CloseToolbar;
            this.draw_button.Click += DrawButton;
            this.erase_button.Click += EraseButton;
            this.red_button.Click += RedButton;
            this.blue_button.Click += BlueButton;
            this.black_button.Click += BlackButton;
            this.submit_button.Click += SubmitButton;

            instructions.Text = GenerateInstructions();
        }

        private void RightClick(object sender,MouseButtonEventArgs e) {
            var thing = e.GetPosition(this);
            toolbar.Margin = new Thickness(thing.X,thing.Y,0,0);
            toolbar.Visibility = Visibility.Visible;

            mTimer.Restart();
        }

        private void CloseToolbar(object sender,MouseButtonEventArgs e) {
            toolbar.Visibility = Visibility.Hidden;
            e.Handled = true;
        }

        private void DrawButton(object sender,RoutedEventArgs e) {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.Cursor = Cursors.Pen;
            toolbar.Visibility = Visibility.Hidden;
            mData.Add(new Datum { button = Buttons.DRAW,millseconds = mTimer.ElapsedMilliseconds });
        }

        private void EraseButton(object sender,RoutedEventArgs e) {
            //inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            inkCanvas.EraserShape = new EllipseStylusShape(40,40);

            inkCanvas.Cursor = ((TextBlock)Resources["eraser"]).Cursor;

            toolbar.Visibility = Visibility.Hidden;
            mData.Add(new Datum { button = Buttons.ERASE,millseconds = mTimer.ElapsedMilliseconds });
        }

        private void RedButton(object sender,RoutedEventArgs e) {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
            toolbar.Visibility = Visibility.Hidden;
            mData.Add(new Datum { button = Buttons.RED,millseconds = mTimer.ElapsedMilliseconds });
        }

        private void BlueButton(object sender,RoutedEventArgs e) {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
            toolbar.Visibility = Visibility.Hidden;
            mData.Add(new Datum { button = Buttons.BLUE,millseconds = mTimer.ElapsedMilliseconds });
        }

        private void BlackButton(object sender,RoutedEventArgs e) {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
            toolbar.Visibility = Visibility.Hidden;
            mData.Add(new Datum { button = Buttons.BLACK,millseconds = mTimer.ElapsedMilliseconds });
        }

        private void SubmitButton(object sender,RoutedEventArgs e) {
            if(mCurrentTask == 30) {
                ExportData();
                Application.Current.Shutdown();
            } else {
                ++mCurrentTask;
                String text;
                do text = GenerateInstructions();
                while(text == instructions.Text);
                instructions.Text = text;
            }
        }

        private String GenerateInstructions() {
            String text = "Draw a ";
            switch(mRandom.Next(0,2)) {
                case 0:
                    text += "black ";
                    break;
                case 1:
                    text += "red ";
                    break;
                case 2:
                    text += "blue ";
                    break;
            }
            switch(mRandom.Next(0,2)) {
                case 0:
                    text += "triangle";
                    break;
                case 1:
                    text += "square";
                    break;
                case 2:
                    text += "circle";
                    break;
            }
            return text;
        }

        private String SerializeList() {
            String s = "Button, Time(ms)\r\n";
            foreach(Datum i in mData) s += i.button + "," + i.millseconds.ToString() + "\r\n";
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
