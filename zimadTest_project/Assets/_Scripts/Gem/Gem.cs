

using System;

public class Gem
{
    public Slot currentSlot = null;
    public GemView gemView;
    public GemInfo gemInfo;
    
    public Action<Slot> onSlotChanged = delegate(Slot slot) {  };
    
    public IGemBehaviour gemBehaviour = new BehaviourRegularGem();

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