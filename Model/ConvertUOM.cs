using System.Collections.Generic;
using System.Globalization;

namespace EngUoM.Model {
    public class ConvertUOM {
        private APIModel _model;

        public ConvertUOM() {
            _model = new APIModel();
        }
        
    public List<string> Conversion(double inputValue, string fromUnitOfMeasure, string toUnitOfMeasure) {
        var fromUnitOfM = _model.UnitOfMeasuremtnDictionary[fromUnitOfMeasure];
        var toUnitOfM =_model. UnitOfMeasuremtnDictionary[toUnitOfMeasure];
        var resultUomAnnotation = toUnitOfM.annotation;
        var resultUomName = toUnitOfM.name;
        var fromConvFormula = fromUnitOfM.conversionFormula;
        var toConvFormula = toUnitOfM.conversionFormula;
        double B1, C1, B2, C2, z = 0.0;


        if (_model.UnitOfMeasuremtnDictionary[fromUnitOfMeasure] == null ||_model. UnitOfMeasuremtnDictionary
                [toUnitOfMeasure] == null)
            //return "Conversion between the units is impossible";
            return new List<string> { "empty list" };

        if (fromConvFormula.Count == 2) {
            var inputNumber = fromConvFormula[0];
            B1 = double.Parse(inputNumber);

            C1 = double.Parse(fromConvFormula[1]);
        }
        else {
            B1 = double.Parse(fromConvFormula[0], CultureInfo.InvariantCulture);
            C1 = 1.0;
        }

        if (fromConvFormula.Count == 2) {
            B2 = double.Parse(toConvFormula[0], CultureInfo.InvariantCulture);
            C2 = double.Parse(toConvFormula[0], CultureInfo.InvariantCulture);
        }
        else {
            B2 = double.Parse(toConvFormula[0], CultureInfo.InvariantCulture);
            C2 = 1.0;
        }

        if (fromUnitOfM.BaseUnit.Equals(toUnitOfM.BaseUnit))
            z = B1 / C1 / (B2 / C2) * inputValue;
        else
            return new List<string> { "empty list" };

        var convertResult = new {
            result = z,
            uom = resultUomName,
            annotation = resultUomAnnotation
        };

        return new List<string> {
            convertResult.annotation,
            convertResult.uom,
            convertResult.result.ToString()
        };
        }
    }
}