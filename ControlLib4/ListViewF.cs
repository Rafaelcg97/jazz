using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlLib4
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ControlLib4"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ControlLib4;assembly=ControlLib4"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class ListViewF : ListView
    {
        public static readonly DependencyProperty FooterObjProperty = DependencyProperty.Register(
             "FooterObj",
             typeof(object),
             typeof(ListViewF),
             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));




        static ListViewF()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewF), new FrameworkPropertyMetadata(typeof(ListViewF)));
        }

        public object FooterObj
        {

            get { return GetValue(FooterObjProperty); }
            set { SetValue(FooterObjProperty, value); }
        }



        Style footerStyle;
        public Style FooterStyle
        {
            get
            {
                if (footerStyle == null)
                {
                    ComponentResourceKey key = new ComponentResourceKey(typeof(ListViewF), "DefaultFooterStyle");
                    footerStyle = (Style)(TryFindResource(key));
                    Style sCopy = new Style(typeof(ScrollViewer), footerStyle);

                    Setter fontSizeSetter = new Setter(ScrollViewer.FontSizeProperty, this.FontSize + 2);
                    //footerStyle.Setters.Add(fontSizeSetter);
                    sCopy.Setters.Add(fontSizeSetter);
                    footerStyle = sCopy;
                }
                return footerStyle;
            }

            set { footerStyle = value; }
        }



        public void InitFooter()
        {
            ParserContext pc = new ParserContext();
            pc.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            pc.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            for (int i = 0; i < ((GridView)(this.View)).Columns.Count; i++)
            {

                GridViewColumn col = ((GridView)(this.View)).Columns[i];

                DataTemplate template = col.CellTemplate;
                if (template != null)
                {
                    FrameworkElementFactory fefBorder = new FrameworkElementFactory(typeof(Border));
                    fefBorder.SetValue(Border.MarginProperty, new Thickness(-6, 0, -6, 0));
                    fefBorder.SetValue(Border.PaddingProperty, new Thickness(6, 2, 6, 2));
                    fefBorder.SetValue(Border.BorderThicknessProperty, new Thickness(1, 0, 1, 0));
                    fefBorder.SetValue(Border.BorderBrushProperty, new SolidColorBrush(Colors.Gray));

                    FrameworkElementFactory fef = new FrameworkElementFactory(typeof(ContentPresenter));
                    fef.SetValue(ContentPresenter.ContentTemplateProperty, template);
                    fefBorder.AppendChild(fef);
                    col.CellTemplateSelector = new CellTemplateSelector(col.CellTemplate, new DataTemplate() { VisualTree = fefBorder });

                    //clear template to stop it being used ahead of selector
                    col.CellTemplate = null;

                }
                else if (col.DisplayMemberBinding != null)
                {
                    string fTemplate = @"<DataTemplate>
                                            <Border BorderThickness=""1,0,1,0"" BorderBrush=""Gray"" Margin=""-6,0,-6,0"" Padding=""6,2,6,2"" >
                                                <TextBlock Text=""{Binding DisplayMemberBinding}"" /> 
                                            </Border>
                                         </DataTemplate>";

                    fTemplate = fTemplate.Replace("DisplayMemberBinding", (col.DisplayMemberBinding as Binding).Path.Path);
                    MemoryStream sr = new MemoryStream(Encoding.ASCII.GetBytes(fTemplate));
                    DataTemplate footerTemplate = (DataTemplate)XamlReader.Load(sr, pc);

                    string dTemplate = @"<DataTemplate>
                                            <TextBlock Margin=""-6,0,-6,0"" Padding=""6,2,6,2"" Text=""{Binding DisplayMemberBinding}"" /> 
                                         </DataTemplate>";

                    dTemplate = dTemplate.Replace("DisplayMemberBinding", (col.DisplayMemberBinding as Binding).Path.Path);
                    sr = new MemoryStream(Encoding.ASCII.GetBytes(dTemplate));
                    DataTemplate regularTemplate = (DataTemplate)XamlReader.Load(sr, pc);


                    //clear binding to stop it being used ahead of selector
                    col.DisplayMemberBinding = null;
                    col.CellTemplateSelector = new CellTemplateSelector(regularTemplate, footerTemplate);
                }

            }
        }


        class CellTemplateSelector : DataTemplateSelector
        {
            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if (container is ContentPresenter)
                {
                    (container as ContentPresenter).HorizontalAlignment = HorizontalAlignment.Stretch;
                }

                for (int i = 5; i >= 0; i--)
                {
                    container = VisualTreeHelper.GetParent(container);
                    if (container is ListViewItem)
                        return defaultTemplate;
                }
                return footerTemplate;
            }

            public CellTemplateSelector(DataTemplate template1, DataTemplate template2)
            {
                defaultTemplate = template1;
                footerTemplate = template2;
            }

            readonly DataTemplate footerTemplate;
            readonly DataTemplate defaultTemplate;
        }


        public ListViewF()
        {
        }

    }


    public class MyGridView : GridView
    {

        protected override object DefaultStyleKey
        {
            get
            {
                return typeof(ListViewF);
            }
        }
    }
}
