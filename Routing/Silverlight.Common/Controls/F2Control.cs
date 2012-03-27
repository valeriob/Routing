using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using Silverlight.Common.DynamicSearch;
using System.Collections.Generic;

namespace Silverlight.Common.Controls
{
    [TemplatePart(Name = F2Control.TextBoxF2, Type = typeof(TextBox))]
    [TemplatePart(Name = F2Control.TextBlockF2, Type = typeof(TextBlock))]
    [TemplatePart(Name = F2Control.ButtonF2, Type = typeof(Button))]
    public class F2Control : ContentControl
    {
        private const string TextBoxF2 = "TextBoxF2";
        private const string TextBlockF2 = "TextBlockF2";
        private const string ButtonF2 = "ButtonF2";
        
        private TextBlock _textBlock;
        private TextBox _textBox;
        private Button  _button;

        protected string LastSearch { get; set; }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(F2Control), null);


        public bool IsTextChanged
        {
            get { return (bool)GetValue(IsTextChangedProperty); }
            set { SetValue(IsTextChangedProperty, value); }
        }
        public static readonly DependencyProperty IsTextChangedProperty = DependencyProperty.Register("IsTextChanged", typeof(bool), typeof(F2Control), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(F2Control),null );


        public string TextPath
        {
            get { return (string)GetValue(TextPathProperty); }
            set { SetValue(TextPathProperty, value); }
        }
        public static readonly DependencyProperty TextPathProperty = DependencyProperty.Register("TextPath", typeof(string), typeof(F2Control), new PropertyMetadata(TextPathChangedCallback));

        private static void TextPathChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as F2Control;
            var textPath = e.NewValue as string;

            if (textPath != null && control._textBox != null)
            {
                var binding = new Binding(textPath);
                binding.Source = control.DataContext;
                control._textBox.SetBinding(TextBox.TextProperty, binding);
            }
        }


        public event EventHandler<TextChangedEventArgs> TextChanged;
        
        public F2Control()
        {
            DefaultStyleKey = typeof(F2Control);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBlock = GetTemplateChild(F2Control.TextBlockF2) as TextBlock;
            _textBox = GetTemplateChild(F2Control.TextBoxF2) as TextBox;
            _button = GetTemplateChild(F2Control.ButtonF2) as Button;

            if (_textBlock != null)
            {

            }

            if (_textBox != null)
            {
                var binding = new Binding("Text");
                binding.Mode = BindingMode.TwoWay;
                binding.Source = this;

                if (TextPath != null)
                {
                    binding = new Binding(TextPath);
                    binding.Source = DataContext;


                    //var value = GetValue(DataContext, TextPath);
                }


                _textBox.SetBinding(TextBox.TextProperty, binding);
                
               

                //if (TextBinding != null)
                //    _textBox.SetBinding(TextBox.TextProperty, TextBinding);

                _textBox.LostFocus += (sender, e) =>
                {
                    //if (LastSearch == _textBox.Text)
                    //    return;
                    OnTextChanged(_textBox.Text, true);
                    LastSearch = _textBox.Text;
                };
                _textBox.KeyUp += (sender, e) =>
                {
                    if (e.Key == Key.F2)
                    {
                        OnTextChanged(_textBox.Text, false);
                        LastSearch = _textBox.Text;
                    }
                    //if (e.Key == Key.Enter)
                    //    OnTextChanged(_textBlock.Text, true);
                };
               
            }

            if (_button != null)
            {
                _button.Click += (sender, e) =>
                {
                    OnTextChanged(_textBox.Text, false);
                    LastSearch = _textBox.Text;
                };
            }
        }


        protected void OnTextChanged(string text, bool silent)
        {
            if (TextChanged != null)
                TextChanged(this, new TextChangedEventArgs { Text = text, IsSilent = silent });
        }

        public static object GetValue(object source, string path)
        {
            object result = source;
            if (string.IsNullOrWhiteSpace(path))
                return source;

            var queue = new Queue<string>(path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
            while (queue.Any())
            {
                var relativePath = queue.Dequeue();

                var pi = result.GetType().GetProperty(relativePath);
                if (pi == null)
                    throw new Exception("Property not found");

                result = pi.GetValue(result, null);
            }
            return result;
        }
    }

    public class TextChangedEventArgs : EventArgs
    {
        public string Text { get; set; }

        public bool IsSilent { get; set; }

    }
}
