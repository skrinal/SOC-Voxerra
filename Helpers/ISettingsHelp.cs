namespace Voxerra.Settings;

public interface ISettingsHelp
{
    Task<bool> DebounceValidation(int delay, string input, Func<string, Task> validationFunc)
}