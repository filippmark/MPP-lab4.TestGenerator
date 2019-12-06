namespace TestGeneratorImpl
{
    public class DegreeOfParallelism
    {
        public int AmountOfReadParallelProcess { get; set; }
        public int AmountOfGenerateParallelProcess { get; set; }
        public int AmountOfWriteParallelProcess { get; set; }
        public DegreeOfParallelism(int amountOfReadParallelProcess, int amountOfGenerateParallelProcess, int amountOfWriteParallelProcess)
        {
            this.AmountOfGenerateParallelProcess = amountOfGenerateParallelProcess;
            this.AmountOfReadParallelProcess = amountOfReadParallelProcess;
            this.AmountOfWriteParallelProcess = amountOfWriteParallelProcess;
        }
    }
}
