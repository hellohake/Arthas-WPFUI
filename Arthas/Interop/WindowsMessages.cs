using System.Diagnostics.CodeAnalysis;

namespace Arthas.Interop
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum WindowsMessages
    {
        NCLBUTTONDOWN = 0x000000A1,
        NCCALCSIZE = 0x00000083,
    }
}