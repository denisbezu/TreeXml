using System;

namespace TreeWPF.Interfaces
{
    public interface IClosableViewModel
    {
        event EventHandler Close;
    }
}