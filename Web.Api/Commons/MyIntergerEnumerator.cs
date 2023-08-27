using System.Collections;

namespace WebApi.Commons
{
    // When you implement IEnumerable, you must also implement IEnumerator.
    public class MyIntergerEnumerator : IEnumerator
    {
        private readonly int[] _listInt;
        private int _currentIndex = -1;

        public MyIntergerEnumerator(int[] listInt)
        {
            _listInt = listInt;
        }

        public object Current
        {
            get
            {
                return _listInt[_currentIndex];
            }
        }

        public bool MoveNext()
        {
            _currentIndex++;

            return _currentIndex < _listInt.Length;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }
}
