using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Collections.Generic;

namespace Silverlight.Common.Controls
{
    [TemplatePart(Name = CustomDataPager.PageSizeComboBox, Type = typeof(ComboBox))]
    [TemplatePart(Name = CustomDataPager.TotalCountTextBlock, Type = typeof(TextBlock))]
    public class CustomDataPager : DataPager
    {
        private const string PageSizeComboBox = "PageSizeComboBox";
        private const string TotalCountTextBlock = "TotalCountTextBlock2";

        private ComboBox _pageSizeComboBox;
        private TextBlock _totalCountTextBlock;

        public CustomDataPager()
        {
            DefaultStyleKey = typeof(CustomDataPager);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            _pageSizeComboBox = GetTemplateChild(CustomDataPager.PageSizeComboBox) as ComboBox;
            _totalCountTextBlock = GetTemplateChild(CustomDataPager.TotalCountTextBlock) as TextBlock;

            if (_pageSizeComboBox != null)
            {
                _pageSizeComboBox.ItemsSource = new List<int>(new int[] { 1, 5, 10, 20, 30, 50, 100 });
                var binding = new Binding("Source.PageSize");
                binding.Mode = BindingMode.TwoWay;
                binding.Source = this;
                _pageSizeComboBox.SetBinding(ComboBox.SelectedValueProperty, binding);
            }

            if (_totalCountTextBlock != null)
            {

                var binding = new Binding("Source.TotalItemCount");
                binding.Source = this;
                _totalCountTextBlock.SetBinding(TextBlock.TextProperty, binding);
            }
        }
    }
}
