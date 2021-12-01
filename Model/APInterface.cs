using System.Collections.Generic;

namespace EngUoM.Model {
    public interface APInterface {
        List<string> ListAllDimension();
        List<string> ListAllQuantity();
       
        void Loader();
       
        List<componentSupport> UOM_GClass(string GivenClass);
       
        List<componentSupport> UnitOfMeasure_GivenQuantityType(string QType);

    }
   
}