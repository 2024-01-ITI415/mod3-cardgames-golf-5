using System.Collections.Generic;
using UnityEngine;

namespace Golf
{
    public class CardGolf : Card
    {
        [Header("Set Dynamically: CardGolf")]
        public eCardState state = eCardState.drawpile;
        public List<CardGolf> hiddenBy = new List<CardGolf>();
        public int layoutID;
        public SlotDef slotDef;

        override public void OnMouseUpAsButton()
        {
            Golf.S.CardClicked(this);
            base.OnMouseUpAsButton();
        }
    }
}
