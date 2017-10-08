using System;

namespace TreeWPF.Interfaces
{
    public interface IOpenSaveDialog
    {
        void OpenFile(Action<string> onOpenAction);
        void SaveFile(Action<string> onSaveAction);
    }
}