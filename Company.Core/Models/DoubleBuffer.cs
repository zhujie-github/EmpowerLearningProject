using Company.Core.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Company.Core.Models
{
    public enum BufferType
    {
        Buffer1,
        Buffer2
    }

    /// <summary>
    /// 双缓冲区的非托管二维数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DoubleBuffer<T> : INotifyPropertyChanged, IDisposable where T : struct
    {
        private UnmanagedArray2D<T>? Buffer1 { get; set; }

        private UnmanagedArray2D<T>? Buffer2 { get; set; }

        private UnmanagedArray2D<T>? _current;
        /// <summary>
        /// 当前图像数据
        /// </summary>
        public UnmanagedArray2D<T>? Current
        {
            get => _current;
            set
            {
                if (_current != value)
                {
                    _current = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// 当前缓冲区
        /// </summary>
        public BufferType BufferType { get; set; } = BufferType.Buffer1;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Write(UnmanagedArray2D<T> buffer)
        {
            if (Buffer1 == null)
            {
                Current = null;
                return;
            }

            Write(buffer.Header, buffer.Width, buffer.Height);
        }

        public void Write(IntPtr header, int width, int height)
        {
            UnmanagedArray2D<T>? result = null;
            var next = BufferType.Buffer1;
            switch(BufferType)
            {
                case BufferType.Buffer1:
                    Buffer2 ??= new UnmanagedArray2D<T>(width, height);
                    result = Buffer2;
                    next = BufferType.Buffer2;
                    break;
                case BufferType.Buffer2:
                    Buffer1 ??= new UnmanagedArray2D<T>(width, height);
                    result = Buffer1;
                    next = BufferType.Buffer1;
                    break;
                default:
                    break;
            }
            if (result == null) return;
            MemoryHelper.CopyMemory(result.Header, header, result.Length);
            Current = result;
            BufferType = next;
        }

        public void Dispose()
        {
            Buffer1?.Dispose();
            Buffer2?.Dispose();
        }
    }
}
