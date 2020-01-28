using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools
{
    public abstract class AActionManagerItem
    {
        public string Name { get; }
        public AActionManagerItem(string name)
        {
            Name = name;
        }
    }

    public abstract class AActionManagerItem<T> : AActionManagerItem
    {
        public Predicate<T> MatchCondition { get; }

        public AActionManagerItem(string name, Predicate<T> predicate) : base(name)
        {
            MatchCondition = predicate;
        }
    }

}
