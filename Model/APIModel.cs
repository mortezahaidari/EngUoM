using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EngUoM.Model {
    public class APIModel : APInterface {
         /*
         * list of dictionaries
         */
        private Dictionary<string, List<componentSupport>> _dimensionClssWithCompoDictionary; // <dimension,<baseunit,list<annotation>>
        private Dictionary<string, List<componentSupport>> _quantityCDictionary;
        public XElement ListOfElements;

        public  Dictionary<string, componentSupport> UnitOfMeasuremtnDictionary;
        //private Dictionary<string, Dictionary<string, string>> Data;

        /*
         * Implemented methods
         */
        public APIModel() {
            Loader();
            _dimensionClssWithCompoDictionary = new Dictionary<string, List<componentSupport>>();
            _quantityCDictionary = new Dictionary<string, List<componentSupport>>();
            UnitOfMeasuremtnDictionary = new Dictionary<string, componentSupport>();
            GetDataRecords();
        }


        /*
         * Loads xml data
         */
        public void Loader() => ListOfElements = XElement.Load("poscUnits22.xml");

        /*
         * parses data, and selects UOM items
         */
        public XElement[] XmlElements() {
            var elementsByParties = from item in ListOfElements.Descendants("UnitOfMeasure")
                select item;

            var xElements = elementsByParties as XElement[] ?? elementsByParties.ToArray();
            return xElements;
        }

        /*
         * Returns dictionary data
         * checks base unit if it is null, and points to catalog name. 
         */
        public IEnumerable<List<string>> GetDataRecords() {
            List<string> listOfDimension = new List<string>();
            List<string> lisOfQuantity = new List<string>();
            if (_quantityCDictionary.Count == 0 && _dimensionClssWithCompoDictionary.Count == 0) {
                var xElements = XmlElements();
                foreach (var unitOfMeasure in xElements) {
                    var nameOfUnitOfM  = unitOfMeasure.Element("Name")?.Value;
                    var UnitAnn = unitOfMeasure.Attribute("annotation")?.Value;
                    var dm =
                        unitOfMeasure.Element("DimensionalClass")?.Value; 
                    var catalogSymbol =
                        unitOfMeasure.Element("CatalogSymbol")?.Value; 
                    var baseUnit = unitOfMeasure.Element("ConversionToBaseUnit")?.Attribute("baseUnit")?.Value;
                    var dimension1 = unitOfMeasure.Element("DimensionalClass")?.Value;
                    var tquantity = unitOfMeasure.Element("QuantityType")?.Value; 
                   
                    var numerator = unitOfMeasure.Element("ConversionToBaseUnit")?.Element("Factor")
                        ?.Element("Numerator")
                        ?.Value;
                    var denominator = unitOfMeasure.Element("ConversionToBaseUnit")?.Element("Factor")
                        ?.Element("Denominator")
                        ?.Value;
                    var fraction = unitOfMeasure.Element("ConversionToBaseUnit")?.Element("Factor")?.Value;

                    // checks if key is null
                    if (dm != null) {
                        // checks if baseUnit is null
                        if (baseUnit == null) {
                            var tempData = new componentSupport() {
                                name = nameOfUnitOfM ,
                                BaseUnit = catalogSymbol,
                                conversionFormula = new List<string>() { "1", "1" },
                                annotation = UnitAnn
                            };

                            UnitOfMeasuremtnDictionary[UnitAnn] = tempData;
                            if (_dimensionClssWithCompoDictionary.ContainsKey(dm)) {
                                //if the key is in the dictionary add the 
                                _dimensionClssWithCompoDictionary[dm].Add(tempData);
                            }
                            else {
                                var tempList = new List<componentSupport>();
                                tempList.Add(tempData);
                                _dimensionClssWithCompoDictionary[dm] = tempList;
                            }
                        }

                        else {
                            if (fraction == null) {
                                var temp = new componentSupport() {
                                    name = nameOfUnitOfM ,
                                    BaseUnit = baseUnit,
                                    conversionFormula = new List<string>() { numerator, denominator },
                                    annotation = UnitAnn
                                };

                                _dimensionClssWithCompoDictionary[dm].Add(temp);
                                UnitOfMeasuremtnDictionary[UnitAnn] = temp;
                            }
                            else {
                                var temp = new componentSupport() {
                                    name = nameOfUnitOfM ,
                                    BaseUnit = baseUnit,
                                    conversionFormula = new List<string>() { fraction },
                                    annotation = UnitAnn
                                };

                                _dimensionClssWithCompoDictionary[dm].Add(temp);
                                UnitOfMeasuremtnDictionary[UnitAnn] = temp;
                            }
                        }
                    }

                    if (tquantity != null) {
                        if (UnitAnn == "gu") continue;
                        var temp = UnitOfMeasuremtnDictionary[UnitAnn];
                        // the key is the name of the quantity and the key is the dimension the quantity belongs
                        // 
                        if (_quantityCDictionary.ContainsKey(tquantity)) {
                            // if the quantity key is in the dictionary then no need to create new. just add the uom in the list.
                            _quantityCDictionary[tquantity].Add(temp);
                        }
                        else {
                            // if not found create new key and list value for the uom and add the uom.
                            var templist = new List<componentSupport>();
                            templist.Add(temp);
                            _quantityCDictionary[tquantity] = templist;
                        }
                    }
                }
            }

            foreach (var keys in _dimensionClssWithCompoDictionary.Keys) {
                listOfDimension.Add(keys);
            }

            foreach (var keys in _quantityCDictionary.Keys) {
                lisOfQuantity.Add(keys);
            }

            return new List<List<string>> {
                listOfDimension, lisOfQuantity
            };
        }

        public List<string> ListAllDimension() => GetDataRecords().First();
        public List<string> ListAllQuantity() => GetDataRecords().Last();
        public List<componentSupport> UOM_GClass(string givenClass) => _dimensionClssWithCompoDictionary[givenClass].ToList();
        public List<componentSupport> UnitOfMeasure_GivenQuantityType(string qClass) => _quantityCDictionary[qClass].ToList();
    }
}