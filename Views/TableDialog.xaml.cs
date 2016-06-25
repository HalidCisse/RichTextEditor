using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interop;
using RichTextEditor.Models;

namespace RichTextEditor.Views
{

    public partial class TableDialog : Window
    {
        TableObject _bindingContext;
        ReadOnlyCollection<Unit> _unitOptions;
        ReadOnlyCollection<TableHeaderOption> _headerOptions;
        ReadOnlyCollection<TableAlignment> _alignmentOptions;

        public TableDialog()
        {
            InitializeComponent();
            InitUnitOptions();
            InitHeaderOptions();
            InitAlignmentOptions();
            InitEvents();
            InitBindingContext();            
        }

        public TableObject Model
        {
            get { return _bindingContext; }
            private set
            {
                _bindingContext = value;
                DataContext = _bindingContext;
            }
        }

        void InitUnitOptions()
        {
            List<Unit> ls = new List<Unit> { Unit.Pixel, Unit.Percentage };
            _unitOptions = new ReadOnlyCollection<Unit>(ls);

            _WIDTH_UNIT_SELECTION.ItemsSource = ls;
            _HEIGHT_UNIT_SELECTION.ItemsSource = ls;
            _SPACE_UNIT_SELECTION.ItemsSource = ls;
            _PADDING_UNIT_SELECTION.ItemsSource = ls;
        }

        void InitHeaderOptions()
        {
            List<TableHeaderOption> ls = new List<TableHeaderOption>
            {
                TableHeaderOption.Default, 
                TableHeaderOption.FirstRow, 
                TableHeaderOption.FirstColumn, 
                TableHeaderOption.FirstRowAndColumn 
            };
            _headerOptions = new ReadOnlyCollection<TableHeaderOption>(ls);
            _HEADER_SELECTION.ItemsSource = _headerOptions;
        }

        void InitAlignmentOptions()
        {
            List<TableAlignment> ls = new List<TableAlignment>
            {
                TableAlignment.Default, 
                TableAlignment.Left, 
                TableAlignment.Right, 
                TableAlignment.Center
            };
            _alignmentOptions = new ReadOnlyCollection<TableAlignment>(ls);
            _ALIGNMENT_SELECTION.ItemsSource = _alignmentOptions;
        }

        void InitBindingContext()
        {
            Model = new TableObject
            {
                Columns = 5,
                Rows = 3,
                Border = 1,
                Width = 100,
                Height = 100,
                WidthUnit = Unit.Percentage,
                HeightUnit = Unit.Pixel,
                SpacingUnit = Unit.Pixel,
                PaddingUnit = Unit.Pixel,
                HeaderOption = TableHeaderOption.Default,
                Alignment = TableAlignment.Default
            };
        }

        void InitEvents()
        {
            _OKAY_BUTTON.Click += OkayButton_Click;
            _CANCEL_BUTTON.Click += CancelButton_Click;
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Close();
        }

        void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ComponentDispatcher.IsThreadModal) DialogResult = true;
            Close();
        }
    }
}
