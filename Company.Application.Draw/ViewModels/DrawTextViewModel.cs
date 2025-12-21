using Company.Application.Draw.Models;
using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.Application.Draw.ViewModels
{
    internal class DrawTextViewModel : ReactiveObject
    {
        public FrameworkElement Canvas { get; private set; }
        public FrameworkElement Textbox { get; private set; }

        [Reactive]
        public Cursor Cursor { get; private set; }
        [Reactive]
        public bool IsWriting { get; private set; }
        [Reactive]
        public string InputText { get; set; }
        [Reactive]
        public double OffsetX { get; private set; }
        [Reactive]
        public double OffsetY { get; private set; }
        [Reactive]
        public Transform RenderTransform { get; private set; }
        [Reactive]
        public int FontSize { get; private set; }
        [Reactive]
        public Brush Foreground { get; private set; }

        public IMouseOperationProvider MouseOperationProvider { get; }
        public DrawToolModel DrawToolModel { get; }

        public ICommand CanvasLoadedCommand { get; private set; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; }
        public ICommand CanvasMouseMoveCommand { get; }
        public ICommand TextBoxLostFocusCommand { get; }

        public DrawTextViewModel(IMouseOperationProvider mouseOperationProvider,DrawToolModel drawToolModel)
        {
            MouseOperationProvider = mouseOperationProvider;
            DrawToolModel = drawToolModel;

            CanvasLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(CanvasLoaded);
            CanvasMouseLeftButtonDownCommand = ReactiveCommand.Create<MouseButtonEventArgs>(CanvasMouseLeftButtonDown);
            CanvasMouseLeftButtonUpCommand = ReactiveCommand.Create<MouseButtonEventArgs>(CanvasMouseLeftButtonUp);
            CanvasMouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(CanvasMouseMove);
            TextBoxLostFocusCommand = ReactiveCommand.Create<RoutedEventArgs>(TextBoxLostFocus);

            this.WhenAnyValue(p => p.DrawToolModel.PenWidthType).Subscribe(p => this.FontSize = p.GetFontSize());
            this.WhenAnyValue(p => p.DrawToolModel.PenColorType).Subscribe(p => this.Foreground = p.GetWindowsBrush());
            this.WhenAnyValue(p => p.IsWriting).Subscribe(p => this.Cursor = IsWriting ? Cursors.Arrow : Cursors.IBeam);
        }

        private void CanvasLoaded(RoutedEventArgs e)
        {
            Canvas = e.Source as FrameworkElement;
            Textbox = Canvas.Tag as TextBox;
            CanvasLoadedCommand = null;
        }

        private void CanvasMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled= true;
            if (e.Source != Canvas) return;

            System.Windows.Point point = e.GetPosition(Canvas);

            if (IsWriting)
            {
                Canvas.Focus();
            }
            else
            {
                RenderTransform = Transform.Identity;
                OffsetX = point.X - 17;
                OffsetY = point.Y - 17;
                IsWriting= true;
                Textbox.Focus();
            }

        }

        private void CanvasMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CanvasMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                e.Handled = true;//鼠标左键接下阻止事件冒泡
            }
        }

        private void TextBoxLostFocus(RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(InputText))
            {
                FrameworkElement textbox=e.Source as FrameworkElement;
                Size size = new Size(textbox.ActualWidth - 8, textbox.ActualHeight - 8);
                Point point = RenderTransform.Transform(new Point(OffsetX + 3, OffsetY + 4));//转换坐标是因为拖动后位置有变换
                DrawElementBase element = new DrawTextElement(InputText, point, size, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                DrawToolModel.DrawElements.Add(element);
            }

            InputText = string.Empty;
            IsWriting = false;
        }
    }
}
