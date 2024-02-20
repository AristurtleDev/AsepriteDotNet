
namespace AsepriteDotNet;

internal static class HasFlagHelper
{
    internal static bool HasFlag(this uint value, uint flag) => (value & flag) != 0;
    internal static bool DoesNotHaveFlag(this uint value, uint flag) => !HasFlag(value, flag);
}
