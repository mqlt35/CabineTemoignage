using System.Device.Gpio;

public static class Program
{
    const int PinCombine = 21;
    public static Etat etat { get; set; } = Etat.Initialisation;
    public static Task TaskEnregistrement { get; set; }

    public static async Task Main(string[] args) {
        using var controller = new GpioController();
        controller.OpenPin(PinCombine, PinMode.InputPullUp);

        Console.WriteLine("Initialisation de la cabine de témoignage....");

        controller.RegisterCallbackForPinValueChangedEvent(
            PinCombine,
            PinEventTypes.Falling,
            DecrochageCombine);

        controller.RegisterCallbackForPinValueChangedEvent(
            PinCombine,
            PinEventTypes.Rising,
            RaccorchageCombine);

        etat = Etat.Prete;

        Console.WriteLine("La cabine de témoignage est disponible.");

        await Task.Delay(Timeout.Infinite);
    }

    static void DecrochageCombine(object sender, PinValueChangedEventArgs args)
    {
        Console.WriteLine("Decrochage Combiné");
        if (etat == Etat.Initialisation)
        {
            //TODO Annonce Cabine en cours d'initialisation
        }
        else
        {
            etat = Etat.Annonce;
            //TODO Annonce pré-enregistrement
            TaskEnregistrement = Task.Run(() => Enregistrement());
        }
    }

    static void RaccorchageCombine(object sender, PinValueChangedEventArgs args)
    {
        Console.WriteLine("Raccrochage Combiné");
        if(etat == Etat.Initialisation || etat == Etat.Prete)
            return;
        if(etat == Etat.Enregistrement)
        {
            etat = Etat.ArretEnregistrement;
            TaskEnregistrement.Wait();
        }
        if(etat == Etat.Annonce)
        {
            //TODO stopper l'annonce
        }
        etat = Etat.Prete;
    }

    static void Enregistrement()
    {
        etat = Etat.Enregistrement;
        while(etat == Etat.Enregistrement)
        {
            //TODO faire l'engistrement
            Console.WriteLine("Enregistrement en cours");
            Task.Delay(1000);
        }
        Console.WriteLine("Fin enregsitrement");

        //TODO sauvegarder l'engistrement
    }

    public enum Etat
    {
        Initialisation,
        Prete,
        Annonce,
        Enregistrement,
        ArretEnregistrement
    }
}