using System;

public partial class DB2
{
    public class CardinalityAttribute : Attribute
    {
        public readonly int Count;

        public CardinalityAttribute(int count) => Count = count;
    }
}
