

using System;

public class Gem
{
    public Slot currentSlot = null;
    public GemView gemView;
    public GemInfo gemInfo;
    
    public Action<Slot> onSlotChanged;

    public Slot CurrentSlot
    {
        get => currentSlot;
        set
        {
            currentSlot = value;
            onSlotChanged(currentSlot);

        }
    }
}