namespace MeshDeformation.JobSystemDeformer
{
    public static class JobSystemModel
    {
        public static int BatchSize
        {
            get => _batchSize == 0 ? 64 : _batchSize;
            set => _batchSize = value;
        }

        private static int _batchSize;
    }
}