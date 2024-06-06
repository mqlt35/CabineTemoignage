using System.Device.Gpio;

const int PinCombine = 21;

using var controller = new GpioController();
controller.OpenPin(PinCombine, PinMode.InputPullUp);

Console.WriteLine(
    $"Initialisation de la cabine de témoignage....");

controller.RegisterCallbackForPinValueChangedEvent(
    PinCombine,
    PinEventTypes.Falling,
    DecrochageCombine);

controller.RegisterCallbackForPinValueChangedEvent(
    PinCombine,
    PinEventTypes.Rising,
    RaccorchageCombine);


Console.WriteLine(
    $"La cabine de témoignage est disponible.");

await Task.Delay(Timeout.Infinite);

static void DecrochageCombine(object sender, PinValueChangedEventArgs args)
{
    Console.WriteLine("Decrochage Combiné");
}

static void RaccorchageCombine(object sender, PinValueChangedEventArgs args)
{
    Console.WriteLine("Raccrochage Combiné");
}