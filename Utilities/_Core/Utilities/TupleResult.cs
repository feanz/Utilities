namespace Utilities
{
    /// <summary>
    ///   This structure can be used to hold a tuple of two different types of objects.
    /// </summary>
    /// <typeparam name="T1"> Type of first object. </typeparam>
    /// <typeparam name="T2"> Type of second object. </typeparam>
    public struct Tuple2<T1, T2>
    {
        private readonly T1 _first;

        private readonly T2 _second;

        /// <summary>
        ///   Initialize the tuple items.
        /// </summary>
        /// <param name="first"> First item of tuple. </param>
        /// <param name="second"> Second item of tuple. </param>
        public Tuple2(T1 first, T2 second)
        {
            _first = first;
            _second = second;
        }

        /// <summary>
        ///   The first item of the tuple
        /// </summary>
        public T1 First
        {
            get { return _first; }
        }

        /// <summary>
        ///   The second item of the tuple
        /// </summary>
        public T2 Second
        {
            get { return _second; }
        }
    }

    /// <summary>
    ///   This structure can be used to hold a tuple of three different types of objects.
    /// </summary>
    /// <typeparam name="T1"> Type of first object. </typeparam>
    /// <typeparam name="T2"> Type of second object. </typeparam>
    /// <typeparam name="T3"> Type of third object. </typeparam>
    public struct Tuple3<T1, T2, T3>
    {
        private readonly T1 _first;

        private readonly T2 _second;

        private readonly T3 _third;

        /// <summary>
        ///   Initialize the tuple items.
        /// </summary>
        /// <param name="first"> First type. </param>
        /// <param name="second"> Second type. </param>
        /// <param name="third"> Third type. </param>
        public Tuple3(T1 first, T2 second, T3 third)
        {
            _first = first;
            _second = second;
            _third = third;
        }

        /// <summary>
        ///   The first item of the tuple
        /// </summary>
        public T1 First
        {
            get { return _first; }
        }

        /// <summary>
        ///   The second item of the tuple
        /// </summary>
        public T2 Second
        {
            get { return _second; }
        }

        /// <summary>
        ///   The second item of the tuple
        /// </summary>
        public T3 Third
        {
            get { return _third; }
        }
    }
}