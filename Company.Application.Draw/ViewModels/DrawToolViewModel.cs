using Company.Application.Draw.Models;
using Company.Application.Share.Mouse;
using ReactiveUI;

namespace Company.Application.Draw.ViewModels
{
    public class DrawToolViewModel : ReactiveObject
    {
        public DrawToolModel DrawToolModel { get; private set; }
        public IMouseOperationProvider MouseOperationProvider { get; }

        public DrawToolViewModel(DrawToolModel drawToolModel, IMouseOperationProvider mouseOperationProvider)
        {
            DrawToolModel = drawToolModel;
            MouseOperationProvider = mouseOperationProvider;
        }
    }
}
