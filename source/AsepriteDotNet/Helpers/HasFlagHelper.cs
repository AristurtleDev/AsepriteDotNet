
namespace AsepriteDotNet;

internal static class HasFlagHelper
{
    internal static bool HasFlag(this uint value, uint flag) => (value & flag) != 0;
}
