﻿/**************************************************************
 * Copyright (c) 2017. Shenggang Hu.
 * All rights reserved.
 **************************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using BatchRenamer.RenameTools;

namespace BatchRenamer
{
   
    public partial class SettingsWindow : Window
    {
        private NameComponentList componentList;
        private bool isCancelled = true;

        public SettingsWindow(NameComponentList list,ExtensionMapperList mappers, bool[] boxSelected)
        {
            InitializeComponent();
            componentList = list;
            maps = mappers;
            nameCB.IsChecked = boxSelected[0];
            extensionCB.IsChecked = boxSelected[1];
            CreateItems();
            CreateExtensionMaps();
        }

        public NameComponentList ComponentList
        {
            get { return componentList; }
        }

        public bool[] BoxSelected
        {
            get { return new bool[] { (bool)nameCB.IsChecked, (bool)extensionCB.IsChecked }; }
        }

        public bool IsCancelled
        {
            get { return isCancelled; }
        }

        private void CreateItems()
        {
            foreach(NameComponent c in componentList.getList())
            {
                if (c is StringComponent)
                {
                    addString(c);
                }
                if (c is CounterComponent)
                {
                    addCounter(c);
                }
                if (c is RotorComponent)
                {
                    addRotor(c);
                }
            }
        }

        private Canvas createNameComponent(int index)
        {
            Canvas canvas = new Canvas();
            Border border = addInBorder(480, 35);
            canvas.Width = 480;
            canvas.Height = 40;
            Canvas.SetTop(canvas, 5 + index * 40);
            Canvas.SetTop(border, 0);
            canvas.Children.Add(border);
            return canvas;
        }

        private Border addInBorder(double width, double height)
        {
            Border border = new Border();
            border.Width = width;
            border.Height = height;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(0, 0, 0, 1);
            return border;
        }

        private void initialiseTextBox(TextBox tb, string text)
        {
            tb.Text = text;
            tb.AcceptsReturn = false;
            tb.AcceptsTab = false;
        }

        private void addString(NameComponent component)
        {
            StringComponent com = (StringComponent)component;
            Canvas canvas = createNameComponent(componentList.getList().IndexOf(com));
            TextBox textBox = new TextBox();
            Label label = new Label();
            AddComponentsToCanvas(canvas, textBox, label);
            label.Content = "String:";

            textBox.TextChanged += StringTextChanged;
            textBox.Width = 300;
            Canvas.SetLeft(label, 5);
            Canvas.SetLeft(textBox, 100);
            nameCanvas.Children.Add(canvas);
            initialiseTextBox(textBox, com.Content);
            EnlargeCanvas(nameCanvas, 40);
        }

        private void addCounter(NameComponent component)
        {
            CounterComponent com = (CounterComponent)component;
            Canvas canvas = createNameComponent(componentList.getList().IndexOf(com));
            Label startLabel = new Label();
            Label stepLabel = new Label();
            TextBox startBox = new TextBox();
            TextBox stepBox = new TextBox();

            AddComponentsToCanvas(canvas, startBox, startLabel);
            AddComponentsToCanvas(canvas, stepBox, stepLabel);

            startBox.PreviewTextInput += handleNonNurmeric;
            stepBox.PreviewTextInput += handleNonNurmeric;
            startBox.TextChanged += StartNumberChanged;
            stepBox.TextChanged += StepNumberChanged;

            startBox.Width = 100;
            stepBox.Width = 70;
            startLabel.Content = "Start at: ";
            stepLabel.Content = "With step size: ";

            Canvas.SetLeft(startLabel, 5);
            Canvas.SetLeft(startBox, 100);
            Canvas.SetLeft(stepLabel, 220);
            Canvas.SetLeft(stepBox, 340);
            nameCanvas.Children.Add(canvas);
            initialiseTextBox(startBox, com.StartNumber.ToString());
            initialiseTextBox(stepBox, com.Step.ToString());
            EnlargeCanvas(nameCanvas, 40);
        }

        private void addRotor(NameComponent component)
        {
            RotorComponent com = (RotorComponent)component;
            Canvas canvas = createNameComponent(componentList.getList().IndexOf(com));
            Label label = new Label();
            label.Content = "String set: ";
            TextBox stringSetBox = new TextBox();
            AddComponentsToCanvas(canvas, stringSetBox, label);

            stringSetBox.Width = 350;
            stringSetBox.TextChanged += StringSetChanged;

            Canvas.SetLeft(label, 5);
            Canvas.SetLeft(stringSetBox, 85);

            nameCanvas.Children.Add(canvas);
            initialiseTextBox(stringSetBox, com.StringSet);
            EnlargeCanvas(nameCanvas, 40);
        }

        private void StringSetChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            int position = tb.SelectionStart;
            RotorComponent component = (RotorComponent)componentList.getList()[index];
            component.StringSet = tb.Text;
            if (!component.StringSet.Equals(tb.Text))
            {
                tb.Text = component.StringSet;
                e.Handled = true;
                tb.SelectionStart = position - 1;
            }
        }

        private void StepNumberChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            CounterComponent component = (CounterComponent)componentList.getList()[index];
            if (!tb.Text.Equals(""))
            {
                component.Step = int.Parse(tb.Text);
            }
        }

        private void StartNumberChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            CounterComponent component = (CounterComponent)componentList.getList()[index];
            if (!tb.Text.Equals(""))
            {
                component.StartNumber = int.Parse(tb.Text);
            }
        }

        private void handleNonNurmeric(object sender, TextCompositionEventArgs e)
        {
            string s = e.Text;
            foreach (char c in s)
            {
                if (!char.IsNumber(c))
                {
                    e.Handled = true;
                }
            }
        }

        private void StringTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            int position = tb.SelectionStart;
            StringComponent component = (StringComponent)componentList.getList()[index];
            component.Content = tb.Text;
            if (!component.Content.Equals(tb.Text))
            {
                tb.Text = component.Content;
                e.Handled = true;
                tb.SelectionStart = position-1;
            }
        }

        private void ReverseRenameBlock(object sender, RoutedEventArgs e)
        {
            bool isEnabled = (bool)nameCB.IsChecked;
            nameSV.IsEnabled = isEnabled;
            stringButton.IsEnabled = isEnabled;
            counterButton.IsEnabled = isEnabled;
            rotorButton.IsEnabled = isEnabled;
            deleteCompButton.IsEnabled = isEnabled;
        }

        private void ReverseExtensionBlock(object sender, RoutedEventArgs e)
        {
            bool isEnabled = (bool)extensionCB.IsChecked;
            extensionSV.IsEnabled = isEnabled;
            mappingButton.IsEnabled = isEnabled;
            deleteMappingButton.IsEnabled = isEnabled;
        }

        private void EnlargeCanvas(Canvas canvas, double length)
        {
            canvas.Height = canvas.Height + length;
        }

        private void addStringComponent(object sender, RoutedEventArgs e)
        {
            StringComponent c = new StringComponent("");
            componentList.Add(c);
            addString(c);
        }

        private void addCounterComponent(object sender, RoutedEventArgs e)
        {
            CounterComponent c = new CounterComponent(0, 1);
            componentList.Add(c);
            addCounter(c);
        }

        private void addRotorComponent(object sender, RoutedEventArgs e)
        {
            RotorComponent c = new RotorComponent(new string[] { "" });
            componentList.Add(c);
            addRotor(c);
        }

        private void DeleteNameComponent(object sender, RoutedEventArgs e)
        {
            componentList.removeTheLast();
            nameCanvas.Children.RemoveAt(nameCanvas.Children.Count - 1);
            EnlargeCanvas(nameCanvas, -40);
        }

        private void SaveDetailsAndClose(object sender, RoutedEventArgs e)
        {
            isCancelled = false;
            this.Close();
        }

        private void CancelChangeAndClose(object sender, RoutedEventArgs e)
        {
            isCancelled = true;
            this.Close();
        }
    }
}
