using TreeXml.Commands;

namespace TreeXml
{
    public class Command
    {
        public static ClearCommand Clear { get; } = new ClearCommand();
        public static ExitCommand Exit { get; } = new ExitCommand();
        public static HelpCommand Help { get; } = new HelpCommand();
        public static OpenCommand Open { get; } = new OpenCommand();
    }
}