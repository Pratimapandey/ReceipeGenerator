//using Microsoft.ML;
//using Microsoft.ML.Data;
//using ReceipeGenerator.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.ML.Data;


//namespace ReceipeGenerator.ML
//{
//    public class RecipeVectorizer
//    {
//        private readonly MLContext _mlContext;

//        public RecipeVectorizer()
//        {
//            _mlContext = new MLContext();
//        }

//        public List<RecipeModel> VectorizeRecipes(List<RecipeData> recipes)
//        {
//            // Define the input data schema
//            var inputSchema = SchemaDefinition.Create(typeof(RecipeData));

//            // Load the input data
//            var dataView = _mlContext.Data.LoadFromEnumerable(recipes, inputSchema);

//            // Define the data preprocessing pipeline
//            var dataPipeline = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "IngredientsTokens", inputColumnName: nameof(RecipeData.Ingredients))
//                .Append(_mlContext.Transforms.Text.TokenizeIntoWords(outputColumnName: "IngredientsTokens"));

//            // Fit the data preprocessing pipeline
//            var transformedData = dataPipeline.Fit(dataView).Transform(dataView);

//            // Get the SlotNames column
//            var slotNamesColumn = transformedData.Schema["IngredientsTokens"].Annotations.Schema["SlotNames"];

//            // Extract the slot names
//            VBuffer<ReadOnlyMemory<char>> slotNames = default;
//            if (!slotNamesColumn.Annotations.Schema.TryGetSlotNames(ref slotNames))
//            {
//                throw new InvalidOperationException("SlotNames column is not of the expected type.");
//            }

//            // Convert the slot names to a dictionary
//            var slotNamesDict = slotNames.DenseValues().Select((name, index) => (index, name)).ToDictionary(kv => kv.index, kv => kv.name.ToString());

//            var featureColumns = slotNamesDict[0]; // Selecting the first slot name assuming only one slot for now

//            // Vectorize the recipes
//            var recipeModels = _mlContext.Data.CreateEnumerable<RecipeModel>(transformedData, reuseRowObject: false).ToList();

//            return recipeModels;
//        }
//    }
//}
