using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Collections {
  public class TFixedSizeQueue<T> {

    private int _Size;
    private Queue<T> _Items;
    private object _Lock = new object();

    public TFixedSizeQueue(int size) {
      _Size = size;
      _Items = new Queue<T>(_Size);
    }

    public void Enqueue(T item) {
      lock (_Lock) {
        if (_Items.Count == _Size) {
          _Items.Dequeue();
        }
        _Items.Enqueue(item);
      }
    }

    public T Dequeue() {
      lock (_Lock) {
        if (_Items.Any()) {
          return _Items.Dequeue();
        }
      }
      throw new ApplicationException("Unable to dequeue items : queue is empty");
    }

    public void Clear() {
      lock (_Lock) {
        _Items.Clear();
      }
    }

    public T[] ToArray() {
      lock (_Lock) {
        return _Items.ToArray();
      }
    }

    public bool SequenceEqual(T[] other) {
      return _Items.ToArray().SequenceEqual(other);
    }
  }
}
