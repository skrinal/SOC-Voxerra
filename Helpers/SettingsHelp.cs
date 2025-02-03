namespace Voxerra.Helpers;

public class SettingsHelp : ISettingsHelp
{
    
    private CancellationTokenSource _cts;

    public async Task<bool> DebounceValidation(int delay, string input, Func<string, Task> validationFunc)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(delay, _cts.Token);
            
            var result = await validationFunc(input);
            
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (TaskCanceledException)
        {
            // netreba nic lebo task bol cancelnuty
        }
    }
}