namespace AnalyzerUI
{
    using AnalyzerUI.Data;
    using Microsoft.Win32;
    using PluginAnalyzer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Annotations;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Browser Code 

        //       private void Browser_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        //       {
        //           //browser.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
        //           //browser.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        //       }
        //      // <a href = "/CRM/Org/85318e30-2b4f-4f6b-ae62-86a53f1616ee/Solutions/GetDownloadAssemblyById/dc6cf997-efaa-4d0f-9e23-65636acc92eb" class="btn btn-primary">Download DLL</a>
        //       private void CoreWebView2_DOMContentLoaded(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e)
        //       {
        //           //if (browser.CoreWebView2.DocumentTitle.Contains("Plugin Assembly"))
        //           //{

        //           //}
        //       }

        //       private void CoreWebView2_WebResourceResponseReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebResourceResponseReceivedEventArgs e)
        //       {
        //           if (e.Request.Uri.EndsWith("/Landing"))
        //           {
        //               //var r = e.Response;
        ////               browser.Source = new Uri("https://unify.services.dynamics.com/CRM/Org/d1544eef-da20-468b-8605-ad71f2daa927/Solutions/GetDownloadAssemblyById/1855e2ad-6742-4ad0-97f1-9f0d627108b7");
        //              // browser.Source = new Uri("https://unify.services.dynamics.com/"); /// CRM/Org/d1544eef-da20-468b-8605-ad71f2daa927/Solutions/GetDownloadAssemblyById/1855e2ad-6742-4ad0-97f1-9f0d627108b7");\
        //               //CRM/Org/85318e30-2b4f-4f6b-ae62-86a53f1616ee/Solutions/PluginAssembly#PluginAssemblyId=dc6cf997-efaa-4d0f-9e23-65636acc92eb
        //           }
        //       }

        //       private void Button_Click(object sender, RoutedEventArgs e)
        //       {
        //           //browser.CoreWebView2.ExecuteScriptAsync(txtScript.Text);
        //       }

        //       private void browser_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        //       {
        //           //txtScript.Text += Environment.NewLine + "================================>";
        //           //foreach (var item in e.RequestHeaders)
        //           //{
        //           //    txtScript.Text += Environment.NewLine + item.Key + "=" + item.Value;
        //           //} 

        //       }
        #endregion

        private void Window_Initialized(object sender, EventArgs e)
        {
           // browser.Source = new Uri("https://unify.services.dynamics.com/"); /// CRM/Org/d1544eef-da20-468b-8605-ad71f2daa927/Solutions/GetDownloadAssemblyById/1855e2ad-6742-4ad0-97f1-9f0d627108b7");
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SourceEditor.Document.Blocks.Clear();

            if (SourceTree.SelectedItem is SourceNode node)
            {
                var paragraph = new Paragraph();

                foreach (var line in node.Lines)
                {
                    var run = new Run(line + Environment.NewLine);
                    
                    if (AnnotationSummary.HasAnnotationMarker(line))
                    {
                        run.Text = AnnotationSummary.RemoveMarker(run.Text);
                        run.Background = Brushes.LightPink;
                    }

                    //run.Background = Brushes.Red;
                    paragraph.Inlines.Add(run);
                }

                SourceEditor.Document.Blocks.Add(paragraph);
            }
        }

        private void SelectAssembly_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Title = "Select Plugin Assembly", Filter = "Assembly Files|*.dll" };
            if (openFileDialog.ShowDialog().Value)
            {
                var analyzer = new Analyzer(openFileDialog.FileName);
                analyzer.Analyze();
                DataContext = _vm = new MainWindowViewModel(analyzer);
            }
        }

        private void Annotation_Select(object sender, MouseButtonEventArgs e)
        {
            if (AnnotationList.SelectedItem is AnnotationItem selected)
            {
                var annotation = selected.Anotation;
                
                var rootItem = SourceTree.ItemContainerGenerator.ContainerFromItem(SourceTree.Items[0]) as TreeViewItem;
                rootItem.IsExpanded = true;

                AllowEvents(() => 
                {
                    foreach (var ns in rootItem.Items.OfType<NamespaceNode>())
                    {
                        if (ns.Name == annotation.Node.GetDescriptor().Namespace)
                        {
                            var nsItem = (TreeViewItem)rootItem.ItemContainerGenerator.ContainerFromItem(ns);
                            nsItem.IsExpanded = true;

                            AllowEvents(() =>
                            {
                                foreach (var type in nsItem.Items.OfType<TypeNode>())
                                {
                                    if (type.Name == annotation.Node.GetDescriptor().TopLevelType)
                                    {
                                        var typeItem = (TreeViewItem)nsItem.ItemContainerGenerator.ContainerFromItem(type);
                                        typeItem.IsSelected = true;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
