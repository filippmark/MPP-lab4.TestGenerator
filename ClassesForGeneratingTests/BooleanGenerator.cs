using System;
using System.Reflection;


namespace Generators
{
    public class ByteGenerator : Generator
    {
        public ByteGenerator(Random random) : base(random)
        {
            GeneratedType = typeof(byte);
        }

        public override object GenerateValue(Func<Type, object> generate)
        {
            return (byte)Random.Next();
        }

	public void GenerateValue1(int x)
        {
            return (byte)Random.Next();
        }

	public void GenerateValue1(float x)
        {
            return (byte)Random.Next();
        }
    }
}



namespace Generators
{
    public  class BooleanGenerator : Generator
    {
        public BooleanGenerator(Random random) : base(random)
        {
            GeneratedType = typeof(bool);
        }

        public override object GenerateValue(Func<Type, object> generate)
        {
            return true;
        }
    }
	

    public  class Blaslsl: Generator
    {
        public BooleanGenerator(Random random) : base(random)
        {
            GeneratedType = typeof(bool);
        }

        public override object GenerateValue(Func<Type, object> generate)
        {
            return true;
        }

	public override object GenerateValue1(Func<Type, object> generate)
        {
            return true;
        }
	
	public override object GenerateValue2(Func<Type, object> generate)
        {
            return true;
        }
    }
}