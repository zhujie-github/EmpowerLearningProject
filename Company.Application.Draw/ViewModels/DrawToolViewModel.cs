using Company.Application.Draw.Models;
using Company.Application.Share.Mouse;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Company.Application.Draw.ViewModels
{
    internal class DrawToolViewModel : ReactiveObject
    {
        public DrawToolModel DrawToolModel { get; }
        public IMouseOperationProvider MouseOperationProvider { get; }

        public ICommand MouseOperationNoneCommand { get; }
        public ICommand DeleteAllCommand { get; }

        public DrawToolViewModel(DrawToolModel drawToolModel, IMouseOperationProvider mouseOperationProvider)
        {
            this.DrawToolModel = drawToolModel;
            this.MouseOperationProvider = mouseOperationProvider;
            MouseOperationNoneCommand = ReactiveCommand.Create(NoneCommand);
            DeleteAllCommand = ReactiveCommand.Create(DeleteAll);

        }

        private void DeleteAll()
        {
            DrawToolModel.DeleteAll();
        }

        private void NoneCommand()
        {
            MouseOperationProvider.MouseOperationType = null;
        }
    }
}
