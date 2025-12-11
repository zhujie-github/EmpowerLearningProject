using Company.Application.Draw.Models;
using Company.Application.Share.Mouse;
using ReactiveUI;

namespace Company.Application.Draw.ViewModels
{
    public class DrawToolViewModel(DrawToolModel drawToolModel, IMouseOperationProvider mouseOperationProvider)
        : ReactiveObject
    {
        public DrawToolModel DrawToolModel { get; private set; } = drawToolModel;
        public IMouseOperationProvider MouseOperationProvider { get; } = mouseOperationProvider;
    }
}
