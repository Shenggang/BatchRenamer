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
using System.Windows.Shapes;
using BatchRenamer.RenameTools;

namespace BatchRenamer
{
    /* To myself:
     * The code for components is generally lack of tidiness, please just carry on with it.
     * Rename rule part all done. Now move to extension mapping.
     * Execution should be easy with help of the design of NameComponent.
     */
    public partial class SettingsWindow : Window
    {
        private List<NameComponent> componentList = new List<NameComponent>();
        private bool isCancelled = true;

        public SettingsWindow(List<NameComponent> list,ExtensionMapperList mappers, bool[] boxSelected)
        {
            InitializeComponent();
            componentList = list;
            maps = mappers;
            nameCB.IsChecked = boxSelected[0];
            extensionCB.IsChecked = boxSelected[1];
            CreateItems();
            CreateExtensionMaps();
        }

        public List<NameComponent> ComponentList
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
            foreach(NameComponent c in componentList)
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
            Border border = new Border();
            canvas.Width = 480;
            canvas.Height = 30;
            Canvas.SetLeft(border, 5);
            Canvas.SetTop(canvas, 10 + index * 40);
            return canvas;
        }

        private void addString(NameComponent component)
        {
            StringComponent com = (StringComponent)component;
            Canvas canvas = createNameComponent(componentList.IndexOf(com));
            TextBox textBox = new TextBox();
            Label label = new Label();
            label.Content = "String:";
            canvas.Children.Add(textBox);
            canvas.Children.Add(label);
            textBox.TextChanged += StringTextChanged;
            textBox.Width = 300;
            Canvas.SetLeft(label, 5);
            Canvas.SetLeft(textBox, 100);
            Canvas.SetTop(textBox, 5);
            nameCanvas.Children.Add(canvas);
            textBox.Text = com.Content;
            EnlargeCanvas(nameCanvas, 40);
        }

        private void addCounter(NameComponent component)
        {
            CounterComponent com = (CounterComponent)component;
            Canvas canvas = createNameComponent(componentList.IndexOf(com));
            Label startLabel = new Label();
            Label stepLabel = new Label();
            TextBox startBox = new TextBox();
            TextBox stepBox = new TextBox();

            canvas.Children.Add(startLabel);
            canvas.Children.Add(stepLabel);
            canvas.Children.Add(startBox);
            canvas.Children.Add(stepBox);

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
            Canvas.SetTop(startBox, 5);
            Canvas.SetTop(stepBox, 5);
            nameCanvas.Children.Add(canvas);
            startBox.Text = com.StartNumber.ToString();
            stepBox.Text = com.Step.ToString();
            EnlargeCanvas(nameCanvas, 40);
        }

        private void addRotor(NameComponent component)
        {
            RotorComponent com = (RotorComponent)component;
            Canvas canvas = createNameComponent(componentList.IndexOf(com));
            Label label = new Label();
            label.Content = "String set: ";
            TextBox stringSetBox = new TextBox();

            canvas.Children.Add(label);
            canvas.Children.Add(stringSetBox);

            stringSetBox.Width = 350;
            stringSetBox.TextChanged += StringSetChanged;

            Canvas.SetLeft(label, 5);
            Canvas.SetLeft(stringSetBox, 85);
            Canvas.SetTop(stringSetBox, 5);

            nameCanvas.Children.Add(canvas);
            stringSetBox.Text = com.StringSet;
            EnlargeCanvas(nameCanvas, 40);
        }

        private void StringSetChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            RotorComponent component = (RotorComponent)componentList[index];
            component.StringSet = tb.Text;
        }

        private void StepNumberChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            CounterComponent component = (CounterComponent)componentList[index];
            component.Step = int.Parse(tb.Text);
        }

        private void StartNumberChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = nameCanvas.Children.IndexOf((UIElement)tb.Parent);
            CounterComponent component = (CounterComponent)componentList[index];
            component.StartNumber = int.Parse(tb.Text);
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
            StringComponent component = (StringComponent)componentList[index];
            component.Content = tb.Text;
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
            componentList.Add(new StringComponent(""));
            addString(componentList.Last());
        }

        private void addCounterComponent(object sender, RoutedEventArgs e)
        {
            componentList.Add(new CounterComponent(0, 1));
            addCounter(componentList.Last());
        }

        private void addRotorComponent(object sender, RoutedEventArgs e)
        {
            componentList.Add(new RotorComponent(new string[] { "" }));
            addRotor(componentList.Last());
        }

        private void DeleteNameComponent(object sender, RoutedEventArgs e)
        {
            int index = componentList.Count - 1;
            componentList.RemoveAt(index);
            nameCanvas.Children.RemoveAt(index);
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
