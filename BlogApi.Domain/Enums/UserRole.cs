using System.ComponentModel;

namespace BlogApi.Domain.Enums
{
    public enum UserRole
    {
        [Description("Public")]
        Public,
        [Description("Writer")]
        Writer,
        [Description("Editor")]
        Editor
    }
}
