using System.Collections.Generic;

namespace EngUoM.Model {
  
        public class componentSupport {
            public string name { get; set; }
            public string annotation { get; set; }
            public List<string> conversionFormula { get; set; }
            public List<string> quantities { get; set; }
            public string BaseUnit { get; set; }
        }
}