using System.Collections;

namespace iterator_pattern
{
    //A demonstration of the Iterator pattern in C#
    internal class Program
    {
        static void Main(string[] args)
        {
            var collection = new WordsCollection();

            collection.AddItem("First");
            collection.AddItem("Second");
            collection.AddItem("Third");

            Console.WriteLine("Straight traversal:");

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            Console.WriteLine("\nReverse traversal:");

            collection.ReverseDirection();

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            /* OUTPUT
             * 
                Straight traversal:
                First
                Second
                Third

                Reverse traversal:
                Third
                Second
                First
            */
        }
    }

    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();

        //Returns the key of the current element
        public abstract int Key();

        //Returns the current element
        public abstract object Current();

        //Move forward to next element
        public abstract bool MoveNext();

        //Rewinds the Iterator to the first element
        public abstract void Reset();
    }

    //Returns an Iterator or another IteratorAggregate for the implementing object
    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    //Concrete Iterators implement various traversal algorithms. These classes store the current traversal position at all times.
    class AlphabeticalOrderIterator : Iterator
    {
        private WordsCollection collection;

        //Stores the current traversal position
        private int position = -1;
        private bool reverse = false;

        public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
        {
            this.collection = collection;
            this.reverse = reverse;

            if (reverse)
            {
                this.position = collection.getItems().Count;
            }
        }

        public override object Current()
        {
            return this.collection.getItems()[position];
        }

        public override int Key()
        {
            return this.position;
        }

        public override bool MoveNext()
        {
            int updatedPosition = this.position + (this.reverse ? -1 : 1);

            if (updatedPosition >= 0 && updatedPosition < this.collection.getItems().Count)
            {
                this.position = updatedPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            this.position = this.reverse ? this.collection.getItems().Count - 1 : 0;
        }
    }

    //Concrete Collections provide one or several methods for retrieving fresh iterator instances, compatible with the collection class
    class WordsCollection : IteratorAggregate
    {
        List<string> _collection = new List<string>();

        bool _direction = false;

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public List<string> getItems()
        {
            return _collection;
        }

        public void AddItem(string item)
        {
            this._collection.Add(item);
        }

        public override IEnumerator GetEnumerator()
        {
            return new AlphabeticalOrderIterator(this, _direction);
        }
    }
}
