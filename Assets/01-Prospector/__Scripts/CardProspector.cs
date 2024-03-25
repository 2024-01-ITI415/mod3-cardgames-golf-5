using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public enum eCardState
=======
public enum eCardState 
>>>>>>> Stashed changes
{
    drawpile,
    tableau,
    target,
    discard
}

public class CardProspector : Card
{
    [Header("Set Dynamically: CardProspector")]
<<<<<<< Updated upstream
    public eCardState state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int layoutID;
    public SlotDef slotDef;

    override public void OnMouseUpAsButton()
=======
    public eCardState           state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int                  layoutID;
    public SlotDef              slotDef;
    
    override public void OnMouseUpAsButton() 
>>>>>>> Stashed changes
    {
        Prospector.S.CardClicked(this);
        base.OnMouseUpAsButton();
    }
}
