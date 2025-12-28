using System.Collections.Generic;
using Items.Data;

namespace Items.Script
{
    public class Cuttable : ItemScript
    {
        public SO_ItemList cutResult;
        
        public void Cut()
        {
            //TODO: Spawn cutResult.
            
            Destroy(gameObject);
        }
    }
}