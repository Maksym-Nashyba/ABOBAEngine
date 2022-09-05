namespace ABOBAEngine;

public class Model
{
    public float[] Vertices => _data[_vericesKey];
    public float[] AlbedoUVs => _data[_abbedoUVs];
    
    private Dictionary<string, float[]> _data;
    private const string _vericesKey = "verices";
    private const string _abbedoUVs = "albedoUVs";

    private Model()
    {
        _data = new Dictionary<string, float[]>();
    }

    public static ModelBuilder FromVertexArray(float[] _vertices)
    {
        return new ModelBuilder(_vertices);
    }

    public class ModelBuilder
    {
        private Model _model;

        public ModelBuilder(float[] _vertices)
        {
            _model = new Model();
        }
    
        public Model Build()
        {
            return _model;
        }
        
        public ModelBuilder WithAlbedoUVs(float[] albedoUVs)
        {
            if (albedoUVs.Length % 2 != 0)
                throw new ArgumentException("Odd number of coordinates. Correct format: x,y,x,y,x,y...");
            
            AddOrReplaceData(_abbedoUVs, albedoUVs);
            return this;
        }

        private void SetVertices(float[] vertices)
        {
            AddOrReplaceData(_vericesKey, vertices);
        }
        
        private void AddOrReplaceData(string key, float[] data)
        {
            if (_model._data.ContainsKey(key))
            {
                _model._data[key] = data;
            }
            else
            {
                _model._data.Add(key, data);
            }
        }
    }
}