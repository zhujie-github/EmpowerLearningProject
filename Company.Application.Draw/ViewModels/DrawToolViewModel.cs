using Company.Application.Draw.Models;
using ReactiveUI;

namespace Company.Application.Draw.ViewModels
{
    public class DrawToolViewModel : ReactiveObject
    {
        public DrawToolModel DrawToolModel { get; private set; }

        public DrawToolViewModel(DrawToolModel drawToolModel)
        {
            DrawToolModel = drawToolModel;
        }
    }
}
