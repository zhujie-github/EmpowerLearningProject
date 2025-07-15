using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Initialize.ViewModels
{
    public class InitializeViewModel : ReactiveObject
    {
        [Reactive]
        public string? Message { get; set; }
    }
}
