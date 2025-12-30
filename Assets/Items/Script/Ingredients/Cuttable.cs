using Items.Data;

namespace Items.Script.Ingredients
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